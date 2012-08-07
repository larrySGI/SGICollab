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
using System.Text;

public class Playtomic_Leaderboards : Playtomic_Responder
{
	private static string SECTION;
	private static string CREATEPRIVATELEADERBOARD;
	private static string LOADPRIVATELEADERBOARD;
	private static string SAVEANDLIST;
	private static string SAVE;
	private static string LIST;
	
	internal static void Initialise(string apikey)
	{
		SECTION = Playtomic_Encode.MD5("leaderboards-" + apikey);
		CREATEPRIVATELEADERBOARD = Playtomic_Encode.MD5("leaderboards-createprivateleaderboard-" + apikey);
		LOADPRIVATELEADERBOARD = Playtomic_Encode.MD5("leaderboards-loadprivateleaderboard-" + apikey);
		SAVEANDLIST = Playtomic_Encode.MD5("leaderboards-saveandlist-" + apikey);
		SAVE = Playtomic_Encode.MD5("leaderboards-save-" + apikey);
		LIST = Playtomic_Encode.MD5("leaderboards-list-" + apikey);
	}
	
	public IEnumerator CreatePrivateLeaderboard(string table, bool highest, string permalink)
	{
		var postdata = new Dictionary<String, String>();
		postdata.Add("table", table);
		postdata.Add("highest", highest ? "y" : "n");
		postdata.Add("permalink", permalink);
		
		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, CREATEPRIVATELEADERBOARD, postdata, out url, out post);
		
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
		
		SetResponse(response, "CreatePrivateLeaderboard");
	}
	
	public IEnumerator LoadPrivateLeaderboard(string tableid)
	{
		var postdata = new Dictionary<String, String>();
		postdata.Add("tableid", tableid);
		
		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, LOADPRIVATELEADERBOARD, postdata, out url, out post);
		
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
		
		SetResponse(response, "LoadPrivateLeaderboard");
	}
	
	public string GetLeaderboardFromUrl()
	{
		var url = Playtomic.SourceUrl;
		
		if(!url.Contains("?"))
			return null;
		
		if(!url.Contains("leaderboard="))
			return null;
		
		var afterq = url.Substring(url.IndexOf("leaderboard=") + 12);
			
		if(afterq.IndexOf("&") > -1)
			afterq = afterq.Substring(0, afterq.IndexOf("&"));
			   
		if(afterq.IndexOf("#") > -1)
		   afterq = afterq.Substring(0, afterq.IndexOf("#"));
		
		if(afterq.Length == 24)
			return afterq;
		
		return null;
	}
	
	public IEnumerator Save(string table, Playtomic_PlayerScore score, bool highest)
	{
		return Save(table, score, highest, false, false);
	}
	
	public IEnumerator Save(string table, Playtomic_PlayerScore score, bool highest, bool allowduplicates)
	{
		return Save(table, score, highest, allowduplicates, false);
	}
	
	public IEnumerator Save(string table, Playtomic_PlayerScore score, bool highest, bool allowduplicates, bool facebook)
	{
		var postdata = new Dictionary<String, String>();
		postdata.Add("table", table);
		postdata.Add("highest", highest ? "y" : "n");
		postdata.Add("name", score.Name);
		postdata.Add("points", score.Points.ToString());
		postdata.Add("allowduplicates", allowduplicates ? "y" : "n");
		postdata.Add("auth", Playtomic_Encode.MD5(Playtomic.SourceUrl + score.Points));
		postdata.Add("customfields", score.CustomData.Count.ToString());
		postdata.Add("fbuserid", string.IsNullOrEmpty(score.FBUserId) ? "" : score.FBUserId);
		postdata.Add("fb", facebook ? "y" : "n");
		postdata.Add("url", Playtomic.SourceUrl);
		
		var n = 0;
		
		foreach(var key in score.CustomData.Keys)
		{
			postdata.Add("ckey" + n, key);
			postdata.Add("cdata" + n, score.CustomData[key]);
			n++;
		}
		
		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, SAVE, postdata, out url, out post);
		
		WWW www = new WWW(url, post);
		yield return www;
		
		var response = Playtomic_Request.Process(www);
		SetResponse(response, "Save");
	}
	
	public IEnumerator SaveAndList(string table, Playtomic_PlayerScore score, bool highest, string mode, int perpage, bool isglobal)
	{
		return SaveAndList(table, score, highest, mode, perpage, isglobal, false, false, new Dictionary<string, string>());
	}
	
	public IEnumerator SaveAndList(string table, Playtomic_PlayerScore score, bool highest, string mode, int perpage, bool isglobal, bool allowduplicates)
	{
		return SaveAndList(table, score, highest, mode, perpage, isglobal, allowduplicates, false, new Dictionary<string, string>());
	}
	
	public IEnumerator SaveAndList(string table, Playtomic_PlayerScore score, bool highest, string mode, int perpage, bool isglobal, bool allowduplicates, bool facebook, Hashtable customdatahashtable)
	{
		var dict = new Dictionary<String, String>();
		
		foreach(var key in customdatahashtable.Keys)
		{
			dict.Add(key.ToString(), customdatahashtable[key].ToString());
		}
		
		return SaveAndList(table, score, highest, mode, perpage, isglobal, allowduplicates, facebook, dict);
	}
	
	public IEnumerator SaveAndList(string table, Playtomic_PlayerScore score, bool highest, string mode, int perpage, bool isglobal, bool allowduplicates, bool facebook, Dictionary<String, String> customfilters)
	{
		var postdata = new Dictionary<String, String>();
		
		// common data
		postdata.Add("table", table);
		postdata.Add("highest", highest ? "y" : "n");
		postdata.Add("fb", facebook ? "y" : "n");
		
		// save data
		postdata.Add("name", score.Name);
		postdata.Add("points", score.Points.ToString());
		postdata.Add("allowduplicates", allowduplicates ? "y" : "n");
		postdata.Add("auth", Playtomic_Encode.MD5(Playtomic.SourceUrl + score.Points));
		postdata.Add("numfields", score.CustomData.Count.ToString());
		postdata.Add("fbuserid", string.IsNullOrEmpty(score.FBUserId) ? "" : score.FBUserId);
		postdata.Add("url", Playtomic.SourceUrl);
		
		var n = 0;
		
		foreach(var key in score.CustomData.Keys)
		{
			postdata.Add("ckey" + n, key);
			postdata.Add("cdata" + n, score.CustomData[key]);
			n++;
		}
		
		// list data
		var numfilters = customfilters == null ? 0 : customfilters.Count;
		
		postdata.Add("global", isglobal ? "y" : "n");
		postdata.Add("perpage", perpage.ToString());
		postdata.Add("mode", mode);
		postdata.Add("numfilters", numfilters.ToString());
		
		var fieldnumber = 0;
		
		if(numfilters > 0)
		{
		    foreach(var key in customfilters.Keys)
		    {
				postdata.Add("lkey" + fieldnumber, key);
				postdata.Add("ldata" + fieldnumber, customfilters[key]);
				fieldnumber++;
		    }
		}

		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, SAVEANDLIST, postdata, out url, out post);
		
		WWW www = new WWW(url, post);
		yield return www;
		
		var response = Playtomic_Request.Process(www);
	
		if (response.Success)
		{
			var data = (Hashtable)response.JSON;
			var scores = (ArrayList)data["Scores"];
			var len = scores.Count;
			
			response.NumItems = (int)(double)data["NumScores"];
			response.Scores = new List<Playtomic_PlayerScore>();
			
			for(var i=0; i<len; i++)
			{
				Hashtable item = (Hashtable)scores[i];	
				
				var sscore = new Playtomic_PlayerScore();
				sscore.Name = WWW.UnEscapeURL((string)item["Name"]);
				sscore.Points = (int)(double)item["Points"];
				sscore.SDate = DateTime.Parse((string)item["SDate"]);
				sscore.RDate = WWW.UnEscapeURL((string)item["RDate"]);
				sscore.Rank = (long)(double)item["Rank"];
				
				if(item.ContainsKey("SubmittedOrBest"))
					sscore.SubmittedOrBest = item["SubmittedOrBest"].ToString() == "true";

				if(item.ContainsKey("CustomData"))
				{
					Hashtable customdata = (Hashtable)item["CustomData"];
	
					foreach(var key in customdata.Keys)
						sscore.CustomData.Add((string)key, WWW.UnEscapeURL((string)customdata[key]));
				}
				
				response.Scores.Add(sscore);
			}
		}
		
		SetResponse(response, "SaveAndList");
	}
	
	public IEnumerator List(string table, bool highest, string mode, int page, int perpage)
	{
		return List(table, highest, mode, page, perpage, false, new Dictionary<String, String>(), null);
	}
	
	public IEnumerator List(string table, bool highest, string mode, int page, int perpage, bool facebook)
	{
		return List(table, highest, mode, page, perpage, facebook, new Dictionary<String, String>(), null);
	}
		
	public IEnumerator List(string table, bool highest, string mode, int page, int perpage, bool facebook, Hashtable customdatahashtable)
	{	
		var dict = new Dictionary<String, String>();
		
		foreach(var key in customdatahashtable.Keys)
		{
			dict.Add(key.ToString(), customdatahashtable[key].ToString());
		}
				
		return List(table, highest, mode, page, perpage, facebook, dict, null);
	}
	
	public IEnumerator List(string table, bool highest, string mode, int page, int perpage, bool facebook, Dictionary<String, String> customfilters)
	{	
		return List(table, highest, mode, page, perpage, facebook, customfilters, null);
	}
	
	public IEnumerator List(string table, bool highest, string mode, int page, int perpage, bool facebook, Hashtable customdatahashtable, string[] friendslist)
	{
		var dict = new Dictionary<String, String>();
		
		foreach(var key in customdatahashtable.Keys)
		{
			dict.Add(key.ToString(), customdatahashtable[key].ToString());
		}
		
		return List(table, highest, mode, page, perpage, facebook, dict, friendslist);
	}
	
	
	public IEnumerator List(string table, bool highest, string mode, int page, int perpage, bool facebook, Dictionary<String, String> customfilters, string[] friendslist)
	{
		var numfilters = customfilters == null ? 0 : customfilters.Count;
		var postdata = new Dictionary<String, String>();
		postdata.Add("table", table);
		postdata.Add("highest", highest ? "y" : "n");
		postdata.Add("mode", mode);
		postdata.Add("page", page.ToString());
		postdata.Add("perpage", perpage.ToString());
		postdata.Add("filters", numfilters.ToString());
		postdata.Add("url", "global");
		
		if(numfilters > 0)
		{
			var fieldnumber = 0;
		    
		    foreach(var key in customfilters.Keys)
		    {
				postdata.Add("ckey" + fieldnumber, key);
				postdata.Add("cdata" + fieldnumber, customfilters[key]);
				fieldnumber++;
		    }
		}

		if(facebook)
		{
			postdata.Add("fb", "y");
			
			if(friendslist != null && friendslist.Length > 0)
			{
				postdata.Add("friendslist", string.Join(",", friendslist));
			}
		}
		else
		{
			postdata.Add("fb", "n");
		}
			
		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, LIST, postdata, out url, out post);
		
		WWW www = new WWW(url, post);
		yield return www;
		
		var response = Playtomic_Request.Process(www);
	
		if (response.Success)
		{
			var data = (Hashtable)response.JSON;
			var scores = (ArrayList)data["Scores"];
			var len = scores.Count;
			
			response.NumItems = (int)(double)data["NumScores"];
			response.Scores = new List<Playtomic_PlayerScore>();
			
			for(var i=0; i<len; i++)
			{
				Hashtable item = (Hashtable)scores[i];	
				
				var sscore = new Playtomic_PlayerScore();
				sscore.Name = WWW.UnEscapeURL((string)item["Name"]);
				sscore.Points = (int)(double)item["Points"];
				sscore.SDate = DateTime.Parse((string)item["SDate"]);
				sscore.RDate = WWW.UnEscapeURL((string)item["RDate"]);
				sscore.Rank = (long)(double)item["Rank"];
				
				if(item.ContainsKey("CustomData"))
				{
					Hashtable customdata = (Hashtable)item["CustomData"];
	
					foreach(var key in customdata.Keys)
						sscore.CustomData.Add((string)key, WWW.UnEscapeURL((string)customdata[key]));
				}
				
				response.Scores.Add(sscore);
			}
		}
		
		SetResponse(response, "List");
	}	
}