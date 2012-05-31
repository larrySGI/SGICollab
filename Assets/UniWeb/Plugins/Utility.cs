using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace HTTP
{
	public class URL
	{
		static string safeChars = "-_.~abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		public static string Encode (string value)
		{
			var result = new StringBuilder ();
			foreach (var s in value) {
				if (safeChars.IndexOf (s) != -1) {
					result.Append (s);
				} else {
					result.Append ('%' + String.Format ("{0:X2}", (int)s));
				}
			}
			return result.ToString ();
		}
	}
}


