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

public class Playtomic_Data : Playtomic_Responder
{
	private static string SECTION;
	private static string VIEWS;
	private static string PLAYS;
	private static string PLAYTIME;
	private static string CUSTOMMETRIC;
	private static string LEVELCOUNTERMETRIC;
	private static string LEVELAVERAGEMETRIC;
	private static string LEVELRANGEDMETRIC;
	
	internal static void Initialise(string apikey)
	{
		SECTION = Playtomic_Encode.MD5("data-" + apikey);
		VIEWS = Playtomic_Encode.MD5("data-views-" + apikey);
		PLAYS = Playtomic_Encode.MD5("data-plays-" + apikey);
		PLAYTIME = Playtomic_Encode.MD5("data-playtime-" + apikey);
		CUSTOMMETRIC = Playtomic_Encode.MD5("data-custommetric-" + apikey);
		LEVELCOUNTERMETRIC = Playtomic_Encode.MD5("data-levelcountermetric-" + apikey);
		LEVELAVERAGEMETRIC = Playtomic_Encode.MD5("data-levelaveragemetric-" + apikey);
		LEVELRANGEDMETRIC = Playtomic_Encode.MD5("data-levelrangedmetric-" + apikey);
	}
	
	/// <summary>
	/// Returns the Views for all time
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator Views() 
	{
		return Views(0, 0, 0);
	}
		
	/// <summary>
	/// Returns views for a specific date
	/// </summary>
	/// <param name="day">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="month">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="year">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator Views(int day, int month, int year)
	{
		return General(VIEWS, "Views", day, month, year);
	}
	
	/// <summary>
	/// Returns the plays for all time
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator Plays()
	{
		return Plays(0, 0, 0);
	}
		
	/// <summary>
	/// Returns plays for a date
	/// </summary>
	/// <param name="day">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="month">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="year">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator Plays(int day, int month, int year)
	{
		return General(PLAYS, "Plays", day, month, year);
	}
	
	/// <summary>
	/// Returns playtime for all time
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator PlayTime()
	{
		return PlayTime(0, 0, 0);
	}
	
	/// <summary>
	/// Returns playtime for a date
	/// </summary>
	/// <param name="day">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="month">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="year">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator PlayTime(int day, int month, int year)
	{
		return General(PLAYTIME, "PlayTime", day, month, year);
	}
	
	private IEnumerator General(string action, string type, int day, int month, int year)
	{
		var postdata = new Dictionary<String, String>();
		postdata.Add("day", day.ToString());
		postdata.Add("month", month.ToString());
		postdata.Add("year", year.ToString());
		
		return GetData(type, action, postdata);
	}
	
	/// <summary>
	/// Returns a custom metric's value for all time
	/// </summary>
	/// <param name="metric">
	/// A <see cref="System.String"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator CustomMetric(string metric)
	{
		return CustomMetric(metric, 0, 0, 0);
	}
		
	/// <summary>
	/// Returns a custom metric's value for a date
	/// </summary>
	/// <param name="metric">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="day">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="month">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <param name="year">
	/// A <see cref="System.Int32"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator CustomMetric(string metric, int day, int month, int year)
	{
		var postdata = new Dictionary<String, String>();
		postdata.Add("day", day.ToString());
		postdata.Add("month", month.ToString());
		postdata.Add("year", year.ToString());
		postdata.Add("metric", metric);
		
		return GetData("CustomMetric", CUSTOMMETRIC, postdata);
	}
	
	/// <summary>
	/// Returns a level counter metric's all time value for a level
	/// </summary>
	/// <param name="metric">
	/// A <see cref="System.String"/>
	/// </param>
	/// <param name="level">
	/// A <see cref="System.String"/>
	/// </param>
	/// <returns>
	/// A <see cref="IEnumerator"/>
	/// </returns>
	public IEnumerator LevelCounter(string metric, string level)
	{
		return LevelCounter(metric, level, 0, 0, 0);
	}
	
	public IEnumerator LevelCounter(string metric, int level)
	{
		return LevelCounter(metric, level.ToString(), 0, 0, 0);
	}
		
	public IEnumerator LevelCounter(string metric, int level, int day, int month, int year)
	{
		return LevelCounter(metric, level.ToString(), day, month, year);
	}
	
	public IEnumerator LevelCounter(string metric, string level, int day, int month, int year)
	{
		return LevelMetric("Counter", LEVELCOUNTERMETRIC, metric, level, day, month, year);
	}
	
	public IEnumerator LevelAverage(string metric, string level)
	{
		return LevelAverage(metric, level, 0, 0, 0);
	}
	
	public IEnumerator LevelAverage(string metric, int level)
	{
		return LevelAverage(metric, level.ToString(), 0, 0, 0);
	}
		
	public IEnumerator LevelAverage(string metric, int level, int day, int month, int year)
	{
		return LevelAverage(metric, level.ToString(), day, month, year);
	}
	
	public IEnumerator LevelAverage(string metric, string level, int day, int month, int year)
	{
		return LevelMetric("Average", LEVELAVERAGEMETRIC, metric, level, day, month, year);
	}
		
	public IEnumerator LevelRanged(string metric, string level)
	{
		return LevelRanged(metric, level, 0, 0, 0);
	}
	
	public IEnumerator LevelRanged(string metric, int level)
	{
		return LevelRanged(metric, level.ToString(), 0, 0, 0);
	}
	
	public IEnumerator LevelRanged(string metric, int level, int day, int month, int year)
	{
		return LevelRanged(metric, level.ToString(), day, month, year);
	}
	
	public IEnumerator LevelRanged(string metric, string level, int day, int month, int year)
	{
		return LevelMetric("Ranged", LEVELRANGEDMETRIC, metric, level, day, month, year);
	}
	
	private IEnumerator LevelMetric(string type, string action, string metric, string level, int day, int month, int year)
	{
		var postdata = new Dictionary<String, String>();
		postdata.Add("day", day.ToString());
		postdata.Add("month", month.ToString());
		postdata.Add("year", year.ToString());
		postdata.Add("metric", metric);
		postdata.Add("level", level);
		
		return GetData(type, action, postdata);
	}
	
	private IEnumerator GetData(string type, string action, Dictionary<String, String> postdata)
	{
		string url;
		WWWForm post;
		
		Playtomic_Request.Prepare(SECTION, action, postdata, out url, out post);
		
		WWW www = new WWW(url, post);
		yield return www;
		
		Debug.Log(www.text);
		
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
		
		SetResponse(response, type);
	}
}