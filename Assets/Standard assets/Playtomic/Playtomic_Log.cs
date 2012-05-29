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
using System.Collections;
using System.Collections.Generic;

public class Playtomic_Log
{
	// API settings
	public bool Enabled = false;
	public bool Queue = true;

	// play timer, goal tracking etc
	public Playtomic_LogRequest Request;
	private int Pings = 0;
	private int Plays = 0;
	//private int HighestGoal = 0;		
	
	private bool Frozen = false;
	private List<string> FrozenQueue = new List<string>();

	// unique, logged metrics
	private List<string> Customs = new List<string>();
	private List<string> LevelCounters = new List<string>();
	private List<string> LevelAverages = new List<string>();
	private List<string> LevelRangeds = new List<string>();
	
	// ------------------------------------------------------------------------------
	// SetReferrer
	// Logs the site that has refered to us
	// ------------------------------------------------------------------------------
	public void SetReferrer(string referrer)
	{
		Playtomic.API.StartCoroutine(Playtomic_RRequest.SendReferrer(referrer));
	}
	
	// ------------------------------------------------------------------------------
	// View
	// Logs a view and initialises the SWFStats API
	// ------------------------------------------------------------------------------
	public void View()
	{
		if(Playtomic.GameId == 0 || string.IsNullOrEmpty(Playtomic.GameGuid))
			return;

		Enabled = true;
		Request = Playtomic_LogRequest.Create();
		
		int views = GetCookie("views");
		views++;
		SaveCookie("views", views);

		Send("v/" + views, true);
		
		//Debug.Log("Sent a view");

		// Start the play timer
		Playtomic.API.StartCoroutine(TimerLoop());
	}

	// ------------------------------------------------------------------------------
	// Play
	// Logs a play.
	// ------------------------------------------------------------------------------
	public void Play()
	{						
		if(!Enabled)
			return;

		LevelCounters = new List<string>();
		LevelAverages = new List<string>();
		LevelRangeds = new List<string>();
			
		Plays++;
		Send("p/" + Plays);
	}
	
	
	// ------------------------------------------------------------------------------
	// Player levels
	// Logs metrics for player levels
	// ------------------------------------------------------------------------------
	internal void PlayerLevelStart(string levelid)
	{
		if(!Enabled)
			return;
	
		Send("pls/" + levelid);
	}
	
	internal void PlayerLevelWin(string levelid)
	{
		if(!Enabled)
			return;
	
		Send("plw/" + levelid);
	}
	
	internal void PlayerLevelQuit(string levelid)
	{
		if(!Enabled)
			return;
	
		Send("plq/" + levelid);
	}
	
	internal void PlayerLevelRetry(string levelid)
	{
		if(!Enabled)
			return;
	
		Send("plr/" + levelid);
	}
	
	internal void PlayerLevelFlag(string levelid)
	{
		if(!Enabled)
			return;
	
		Send("plf/" + levelid);
	}
	
	// ------------------------------------------------------------------------------
	// Heatmaps
	// ------------------------------------------------------------------------------
	public void Heatmap(string name, string group, long x, long y)
	{
		if(!Enabled)
			return;
	
		Send("h/" + Clean(name) + "/" + Clean(group) + "/" + x + "/" + y);
	}
	
	// ------------------------------------------------------------------------------
	// CustomMetric
	// Logs a custom metric event.
	// ------------------------------------------------------------------------------
	public void CustomMetric(string name) 					{ CustomMetric(name, null, false); }
	public void CustomMetric(string name, string group)		{ CustomMetric(name, group, false); }
	public void CustomMetric(string name, string group, bool unique)
	{		
		if(!Enabled)
			return;

		if(group == null)
			group = "";

		if(unique)
		{
			if(Customs.IndexOf(name) > -1)
				return;

			Customs.Add(name);
		}
		
		Send("c/" + Clean(name) + "/" + Clean(group));
	}

	// ------------------------------------------------------------------------------
	// LevelCounterMetric, LevelRangedMetric, LevelAverageMetric
	// Logs an event for each level metric type.
	// ------------------------------------------------------------------------------
	public void LevelCounterMetric(string name, int level)							{ LevelCounterMetric(name, level.ToString(), false); }
	public void LevelCounterMetric(string name, int level, bool unique)				{ LevelCounterMetric(name, level.ToString(), unique); }
	public void LevelCounterMetric(string name, string level)						{ LevelCounterMetric(name, level, false); }
	public void LevelCounterMetric(string name, string level, bool unique)
	{		
		if(!Enabled)
			return;

		if(unique)
		{
			var key = name + "." + level;
			
			if(LevelCounters.IndexOf(key) > -1)
				return;

			LevelCounters.Add(key);
		}
		
		Send("lc/" + Clean(name) + "/" + Clean(level));
	}
	
	public void LevelRangedMetric(string name, int level, int value)					{ LevelRangedMetric(name, level.ToString(), value, false); }
	public void LevelRangedMetric(string name, int level, int value, bool unique)	{ LevelRangedMetric(name, level.ToString(), value, unique); }
	public void LevelRangedMetric(string name, string level, int value) 				{ LevelRangedMetric(name, level, value, false); }
	public void LevelRangedMetric(string name, string level, int value, bool unique)
	{			
		if(!Enabled)
			return;

		if(unique)
		{
			var key = name + "." + level;
			
			if(LevelRangeds.IndexOf(key) > -1)
				return;

			LevelRangeds.Add(key);
		}
		
		Send("lr/" + Clean(name) + "/" + Clean(level) + "/" + value);
	}

	public void LevelAverageMetric(string name, int level, double value)					{ LevelAverageMetric(name, level.ToString(), value, false); }
	public void LevelAverageMetric(string name, int level, double value, bool unique)	{ LevelAverageMetric(name, level.ToString(), value, unique); }
	public void LevelAverageMetric(string name, string level, double value)				{ LevelAverageMetric(name, level, value, false); }
	public void LevelAverageMetric(string name, string level, double value, bool unique)
	{
		if(!Enabled)
			return;

		if(unique)
		{
			var key = name + "." + level;
			
			if(LevelAverages.IndexOf(key) > -1)
				return;

			LevelAverages.Add(key);
		}
		
		Send("la/" + Clean(name) + "/" + Clean(level) + "/" + value);
	}

	// ------------------------------------------------------------------------------
	// Links
	// tracks the uniques/totals/fails for links
	// ------------------------------------------------------------------------------
	public void Link(string url, string name, string group, int unique, int total, int fail)
	{
		if(!Enabled)
			return;
		
		Send("l/" + Clean(name) + "/" + Clean(group) + "/" + Clean(url) + "/" + unique + "/" + total + "/" + fail);
	}
	
	// ------------------------------------------------------------------------------
	// Freezing
	// Pauses / unpauses the API
	// ------------------------------------------------------------------------------
	public void Freeze()
	{
		Frozen = true;
	}

	public void UnFreeze()
	{
		Frozen = false;
		Request.MassQueue(FrozenQueue);
	}

	public void ForceSend()
	{
		if(!Enabled)
			return;
			
		Request.Send();
		Request = Playtomic_LogRequest.Create();
		
		if(FrozenQueue.Count > 0)
			Request.MassQueue(FrozenQueue);
	}
	
	// ------------------------------------------------------------------------------
	// Send
	// Creates and sends the url requests to the tracking service.
	// ------------------------------------------------------------------------------
	private void Send(string s)						{ Send(s, false); }
	private void Send(string s, bool view)
	{
		if(Frozen)
		{
			FrozenQueue.Add(s);
			return;
		}
		
		Request.Queue(s);

		if(Request.Ready || view || !Queue)
		{
			//Debug.Log("Sending");
			Request.Send();
			Request = Playtomic_LogRequest.Create();
		}
	}
	
	private static string Clean(string s)
	{
		while(s.IndexOf("/") > -1)
			s = s.Replace("/", "\\");
			
		while(s.IndexOf("~") > -1)
			s = s.Replace("~", "-");				
			
		return WWW.EscapeURL(s);
	}

	// ------------------------------------------------------------------------------
	// GetCookie and SaveCookie
	// Records or retrieves data like how many times the person has played your
	// game.
	// ------------------------------------------------------------------------------
	private static int GetCookie(string n)
	{
		if (PlayerPrefs.HasKey(n))
		{
			return PlayerPrefs.GetInt("playtomic-" + n);
		}
		else
		{
			return 0;
		}
	}
	
	private static void SaveCookie(string n, int v)
	{
		PlayerPrefs.SetInt("playtomic-" + n, v);
	}	

	// ------------------------------------------------------------------------------
	// GetUrl
	// Tries to identify the actual page url, and if it's unable to it reverts to 
	// the default url you passed the View method.  If you're testing the game it
	// should revert to http://local-testing/.
	// ------------------------------------------------------------------------------
	private static string GetUrl(string defaulturl)
	{
		string url;
		
		if (!Application.isWebPlayer || Application.isEditor)
		{
			url = "http://local-testing/";
		}
		else
		{
			url = defaulturl;
		}

		return WWW.EscapeURL(url);
	}
	
	
	private IEnumerator TimerLoop()
	{ 
		yield return new WaitForSeconds(60);
		
		Pings++;
		Send("t/y/" + Pings, true);
		
		do
		{
			yield return new WaitForSeconds(30);
			
			Pings++;
			Send("t/n/" + Pings, true);
		} while (true);
	}
	
}