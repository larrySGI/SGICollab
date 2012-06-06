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
using System.Globalization;

public class Playtomic_PlayerLevel
{
	public string LevelId;
	public string PlayerSource = "";
	public string PlayerId = "";
	public string PlayerName = "";
	public string Name;
	public string Data;
	public int Votes;
	public int Plays;
	public double Rating;
	public int Score;
	public DateTime SDate;
	public string RDate;
	public Dictionary<string, string> CustomData = new Dictionary<string, string>();
	
	public Playtomic_PlayerLevel() 
	{ 
		SDate = new DateTime();
		RDate = "Just now";
	}
	
	public string Thumbnail
	{
		get { return "http:/api.playtomic.com/playerlevels/thumb.aspx?swfid=" + Playtomic.GameId + "&guid=" + Playtomic.GameGuid + "&levelid=" + LevelId; }
	}
	
	// for JS
	public void AddCustomData(String field, String data) 
	{
    	CustomData.Add(field, data);   
	}
	
	public Hashtable GetCustomDataAsHashtable() 
	{

    	var result = new Hashtable();

    	foreach (string key in CustomData.Keys) 
    	{
    	    result.Add(key, CustomData[key]);
   		}

    	return result;
	}
	
	public void SetCustomData(Hashtable customDataAsHashtable) 
	{	
	    foreach (var key in customDataAsHashtable.Keys)
    	{
        	CustomData.Add(key.ToString(), customDataAsHashtable[key].ToString());
	    }
	}
}
