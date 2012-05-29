//  This file is part of the official Playtomic API for Unity games.  
//  Playtomic is a real time analytics platform for casual games 
//  and services that go in casual games.  If you haven't used it 
//  before check it out:
//  http://playtomic.com/
//
//  Created by ben at the above domain on 2/25/11.
//  Copyright 2011 Playtomic LLC. All rights reserved.
//
//  Documentation is available at:
//  http://playtomic.com/api/unity
//
// PLEASE NOTE:
// You may modify this SDK if you wish but be kind to our servers.  Be
// careful about modifying the analytics stuff as it may give you 
// borked reports.
//
// If you make any awesome improvements feel free to let us know!
//
// -------------------------------------------------------------------------
// THIS SOFTWARE IS PROVIDED BY PLAYTOMIC, LLC "AS IS" AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

public class Playtomic_GeoIP : Playtomic_Responder
{
	public Playtomic_GeoIP() { }
	
	private static string SECTION;
	private static string LOOKUP;
	
	internal static void Initialise(string apikey)
	{
		SECTION = Playtomic_Encode.MD5("geoip-" + apikey);
		LOOKUP = Playtomic_Encode.MD5("geoip-lookup-" + apikey);
	}
	
	public IEnumerator Lookup()
	{
		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, LOOKUP, null, out url, out post);
		
		WWW www = new WWW(url, post);
		yield return www;
		
		var response = Playtomic_Request.Process(www);
	
		if (response.Success)
		{
			var data = (Hashtable)response.JSON;

			foreach(string key in data.Keys)
			{
				var name = WWW.UnEscapeURL(key);
				var value = WWW.UnEscapeURL((string)data[key]);
				response.Data.Add(name, value);
			}
		}
		
		SetResponse(response, "Lookup");
	}
}