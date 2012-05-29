using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Playtomic_Request
{
	public static Dictionary<String, Playtomic_Response> Requests = new Dictionary<String, Playtomic_Response>();
	private static String URLStub;
	private static String URLTail;
	private static String URL;
	
	public static void Initialise()
	{
		URLStub = (Playtomic.UseSSL ? "https://g" : "http://g") + Playtomic.GameGuid + ".api.playtomic.com/";
		URLTail = "swfid=" + Playtomic.GameId + "&js=y";
		URL = URLStub + "v3/api.aspx?" + URLTail;
	}
	
	public static IEnumerator SendStatistics(string data)
	{
		//Debug.Log("Request created");
		WWWForm post = new WWWForm();
		post.AddField("x", "x");

		var r = new System.Random();
		var turl = URLStub + "tracker/q.aspx?q=" + data + "&swfid=" + Playtomic.GameId + "&url=" + Escape(Playtomic.SourceUrl) + "&" + r.Next(1000000) + "Z";
		
		//turl = "http://g7bf0867af8a64758.api.playtomic.com/tracker/q.aspx?q=p/1~pls/Tutorial+1+targets~lc/Starts/Tutorial+1+targets~plw/Tutorial+1+targets~lc/Completed/Tutorial+1+targets~la/Elapsed+Time/Tutorial+1+targets/16.9799880981445&swfid=4526&url=http%3A%2F%2Flocalhost%2F&508820Z";
		//Debug.Log("Sending data to " + turl);
		
		var www = new WWW(turl, post);
		yield return www;
		
		Debug.Log(www.text);
	}
	
	public static void Prepare(string section, string action, Dictionary<String, String> postdata, out string url, out WWWForm post)
	{
		var r = new System.Random();
		url = URL + "&r=" + r.Next(10000000) + "Z";
		var timestamp = (DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0))).TotalSeconds.ToString();
		var nonce = Playtomic_Encode.MD5(timestamp + Playtomic.SourceUrl + Playtomic.GameGuid);
	
		var pd = new ArrayList();
		pd.Add("nonce=" + nonce);
		pd.Add("timestamp=" + timestamp);
		
		if(postdata != null)
			foreach(string key in postdata.Keys)
				pd.Add(key + "=" + Escape(postdata[key]));

		GenerateKey("section", section, ref pd);
		GenerateKey("action", action, ref pd);
		GenerateKey("signature", nonce + timestamp + section + action + url + Playtomic.GameGuid, ref pd);
		
		var joined = "";
		
		foreach(var item in pd)
			joined += (joined == "" ? "" : "&") + item;
		
		post = new WWWForm();
		post.AddField("data", Escape(Playtomic_Encode.Base64(joined)));
	}
	
	public static Playtomic_Response Process(WWW www)
	{
		if(www == null)
			return Playtomic_Response.GeneralError(1);
		
		if (www.error != null)
			return Playtomic_Response.GeneralError(www.error);

		if (string.IsNullOrEmpty(www.text))
			return Playtomic_Response.Error(1);
		
		var results = (Hashtable)Playtomic_JSON.JsonDecode(www.text);
		
		if(!results.ContainsKey("Status") || !results.ContainsKey("ErrorCode"))
			return Playtomic_Response.GeneralError(1);
		
		var response = new Playtomic_Response();
		response.Success = ((int)(double)results["Status"] == 1);
		response.ErrorCode = (int)(double)results["ErrorCode"];
		
		if(response.Success && results.ContainsKey("Data"))
		{
			if(results["Data"] is Hashtable)
				response.JSON = (Hashtable)results["Data"];
			
			if(results["Data"] is ArrayList)
				response.ARRAY = (ArrayList)results["Data"];
		}
		
		return response;
	}
	
	private static void GenerateKey(string name, string key, ref ArrayList arr)
	{
		var strarray = (string[]) arr.ToArray(typeof(string));
		Array.Sort(strarray);
		
		var joined = "";
		
		foreach(var item in strarray)
			joined += (joined == "" ? "" : "&") + item;
		
		arr.Add(name + "=" + Playtomic_Encode.MD5(joined + key));
	}
	
	private static string Escape(string str)
	{
		str = String.Join("%25", (string[])str.Split('%'));
		str = String.Join("%3B", (string[])str.Split(';'));
		str = String.Join("%3F", (string[])str.Split('?'));
		str = String.Join("%2F", (string[])str.Split('/'));
		str = String.Join("%3A", (string[])str.Split(':'));
		str = String.Join("%23", (string[])str.Split('#'));
		str = String.Join("%26", (string[])str.Split('&'));
		str = String.Join("%3D", (string[])str.Split('='));
		str = String.Join("%2B", (string[])str.Split('+'));
		str = String.Join("%24", (string[])str.Split('$'));
		str = String.Join("%2C", (string[])str.Split(','));
		str = String.Join("%20", (string[])str.Split(' '));
		str = String.Join("%3C", (string[])str.Split('<'));
		str = String.Join("%3E", (string[])str.Split('>'));
		str = String.Join("%7E", (string[])str.Split('~'));
		return str;
	}
}