using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Globalization;
using System.Threading;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.IO;

namespace HTTP
{
	public class HTTPException : Exception
	{
		public HTTPException (string message) : base(message)
		{
		}
	}

	public enum RequestState
	{
		Waiting,
		Reading,
		Done
	}

	public class Request
	{
		public string method = "GET";
		public string protocol = "HTTP/1.1";
		public byte[] bytes;
		public Uri uri;
		public static byte[] EOL = { (byte)'\r', (byte)'\n' };
		public Response response = null;
		public bool isDone = false;
		public int maximumRedirects = 8;
		public bool acceptGzip = true;
		public bool useCache = false;
		public Exception exception = null;
		public RequestState state = RequestState.Waiting;
		public System.Action<Uri> OnRedirect = null;
		static Dictionary<string, List<ActiveConnection>> activeConnections = new Dictionary<string, List<ActiveConnection>> ();
		Dictionary<string, List<string>> headers = new Dictionary<string, List<string>> ();
		static Dictionary<string, string> etags = new Dictionary<string, string> ();
		
		public ActiveConnection upgradedConnection;
		
		public float Progress {
			get {
				if (response == null)
					return 0f;
				return response.progress;
			}
		}
		
		public Coroutine Wait() {
			return SimpleWWW.Instance.StartCoroutine(_Wait());	
		}
		
		IEnumerator _Wait() {
			while(!isDone) yield return null;	
		}

		public Request (string method, string uri)
		{
			this.method = method;
			this.uri = new Uri (uri);
		}

		public Request (string method, string uri, bool useCache)
		{
			this.method = method;
			this.uri = new Uri (uri);
			this.useCache = useCache;
			if (useCache) {
				LoadEtags ();
			}
		}

		public Request (string uri, WWWForm form)
		{
			this.method = "POST";
			this.uri = new Uri (uri);
			this.bytes = form.data;
			foreach (string k in form.headers.Keys)
				SetHeader (k, (string)form.headers [k]);
		}

		void LoadEtags ()
		{
			//TODO: Find cross platform way to optionally serialize and store etags.
		}

		void SaveEtags ()
		{
			//TODO: Find cross platform way to optionally deserialize and store etags.
		}

		public Request (string method, string uri, byte[] bytes)
		{
			this.method = method;
			this.uri = new Uri (uri);
			this.bytes = bytes;
		}

		public void AddHeader (string name, string value)
		{
			GetHeaders (name).Add (value);
		}

		public string GetHeader (string name)
		{
			List<string> header = GetHeaders (name);
			if (header.Count == 0)
				return "";
			return header [0];
		}

		public List<string> GetHeaders (string name)
		{
			foreach (string key in headers.Keys) {
				if (string.Compare (name, key, true) == 0)
					return headers [key];
			}
			List<string> newHeader = new List<string> ();
			headers.Add (name, newHeader);
			return newHeader;
		}

		public void SetHeader (string name, string value)
		{
			List<string> header = GetHeaders (name);
			header.Clear ();
			header.Add (value);
		}

		public void PopHeader (string name)
		{
			if (headers.ContainsKey (name)) {
				headers.Remove (name);
			}
		}

		ActiveConnection GetClient (string url, int port, bool useSsl)
		{
			var key = string.Format ("{0}:{1}", url, port);
			ActiveConnection connection = null;
			List<ActiveConnection> pool = null;
			if (!activeConnections.TryGetValue (key, out pool)) {
				pool = activeConnections [key] = new List<ActiveConnection> ();	
			}
			while (pool.Count > 0) {
				connection = pool [0];
				pool.RemoveAt (0);
				if (connection.client.Connected)
					break;
				connection = null;
			}
			
			if (connection == null) {
				connection = new ActiveConnection () { key = key };
				connection.client = new TcpClient ();
				connection.client.Connect (uri.Host, uri.Port);
				if (useSsl) {
					connection.stream = new SslStream (connection.client.GetStream (), false, new RemoteCertificateValidationCallback (ValidateServerCertificate));
					var ssl = connection.stream as SslStream;
					ssl.AuthenticateAsClient (uri.Host);
				} else {
					connection.stream = connection.client.GetStream ();
				}
			} 
			return connection;
		}

		public string CalculateHash (string input)
		{
			var sb = new StringBuilder ();
			using (var md5 = System.Security.Cryptography.MD5.Create ()) {
				var inputBytes = System.Text.Encoding.ASCII.GetBytes (input);
				var hash = md5.ComputeHash (inputBytes);
				for (int i = 0; i < hash.Length; i++) {
					sb.Append (hash [i].ToString ("X2"));
				}
			}
			return sb.ToString ();
		}

		public void Send (System.Action<Response> responseHandler)
		{
			SimpleWWW.Instance.Send (this, responseHandler);
		}

		public void Send (System.Action<Request> requestHandler)
		{
			SimpleWWW.Instance.Send (this, requestHandler);
		}

		public void Send ()
		{
			isDone = false;
			state = RequestState.Waiting;
			
			if (acceptGzip)
				SetHeader ("Accept-Encoding", "gzip");
			ThreadPool.QueueUserWorkItem (new WaitCallback (delegate(object t) {
				try {
					
					var retry = 0;
					while (++retry < maximumRedirects) {
						if (useCache) {
							string etag = "";
							if (etags.TryGetValue (uri.AbsoluteUri, out etag)) {
								SetHeader ("If-None-Match", etag);
							}
						}
						var hostHeader = uri.Host;
						if (uri.Port != 80 && uri.Port != 443)
							hostHeader += ":" + uri.Port.ToString ();
						SetHeader ("Host", hostHeader);
						
						ActiveConnection connection;
						try {
							//pull a connection from the pool (a new one is created if needed)
							connection = GetClient (uri.Host, uri.Port, uri.Scheme.ToLower () == "https");
						} catch (Exception e) {
							exception = e;
							response = null;
							break;
						}
						
						try {
							WriteToStream (connection.stream);
						} catch (IOException e) {
							exception = new IOException ("Server closed the connection:" + e.ToString ());
							response = null;
							break;
						}
						response = new Response (this);
						state = RequestState.Reading;
						try {
							response.ReadFromStream (connection.stream);
						} catch (IOException e) {
							exception = new IOException ("Server closed the connection:" + e.ToString ());
							response = null;
							break;
						}

						switch (response.status) {
						case 101:
							upgradedConnection = connection;
							retry = maximumRedirects;
							break;
						case 304:
							retry = maximumRedirects;
							break;
						case 307:
						case 302:
						case 301:
							uri = new Uri (response.GetHeader ("Location"));
							if (OnRedirect != null) {
								OnRedirect (uri);
								retry = maximumRedirects;
							}
							break;
						default:
							retry = maximumRedirects;
							break;
						}
						//place the connection back into the pool if not upgraded.
						if(upgradedConnection == null) {	
							if (response.GetHeader ("Connection").ToLower () != "keep-alive") {
								connection.stream.Close ();
							}
							activeConnections [connection.key].Add (connection);	
						}
					}
					if (useCache) {
						string etag = response.GetHeader ("etag");
						if (etag.Length > 0) {
							etags [uri.AbsoluteUri] = etag;
							SaveEtags ();
						}
					}
				} catch (Exception e) {
					exception = e;
					response = null;
				}
				state = RequestState.Done;
				isDone = true;
			}));
		}

		public string Text {
			set { bytes = value == null ? null : System.Text.Encoding.UTF8.GetBytes (value); }
		}

		public static bool ValidateServerCertificate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			Debug.LogWarning ("SSL Cert Error:" + sslPolicyErrors.ToString ());
			return true;
		}

		void WriteToStream (Stream outputStream)
		{
			var stream = new BinaryWriter (outputStream);
			bool hasBody = false;
			
			stream.Write (ASCIIEncoding.ASCII.GetBytes (method.ToUpper () + " " + uri.PathAndQuery + " " + protocol));
			stream.Write (EOL);
			if (bytes != null && bytes.Length > 0) {
				SetHeader ("Content-Length", bytes.Length.ToString ());
				// Override any previous value
				hasBody = true;
			} else {
				PopHeader ("Content-Length");
			}
			
			foreach (string name in headers.Keys) {
				foreach (string value in headers[name]) {
					stream.Write (ASCIIEncoding.ASCII.GetBytes (name + ": " + value));
					stream.Write (EOL);
				}
			}
			
			stream.Write (EOL);
			
			if (hasBody)
				stream.Write (bytes);
			
			
		}

	}

	public class ActiveConnection
	{
		public string key = null;
		public TcpClient client = null;
		public Stream stream = null;
	}
}
