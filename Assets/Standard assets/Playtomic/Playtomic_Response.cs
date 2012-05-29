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

using System;
using System.Collections;
using System.Collections.Generic;

public class Playtomic_Response
{
	public bool Success;
	public int ErrorCode;
	public string ErrorMessage;
	public Hashtable JSON;
	public ArrayList ARRAY;
	public Dictionary<String, String> Data = new Dictionary<String, String>();
	public List<Playtomic_PlayerLevel> Levels;
	public List<Playtomic_PlayerScore> Scores;
	public PFObject PObject;
	public List<PFObject> PObjects;
	public long NumItems;
	
	public static Playtomic_Response GeneralError(string message)
	{
		return new Playtomic_Response { 
					Success = false,
					ErrorCode = 1,
					ErrorMessage = message
				};
	}
	
	public static Playtomic_Response GeneralError(int nodataerror)
	{
		return new Playtomic_Response { 
					Success = false,
					ErrorCode = -1
				};
	}
	
	public static Playtomic_Response Error(int errorcode)
	{
		return new Playtomic_Response { 
					Success = false,
					ErrorCode = errorcode
				};
	}
	
	public string GetValue(string s)
	{
		return Data[s];
	}
	
	public string ErrorDescription
	{
		get
		{
			if(!string.IsNullOrEmpty(ErrorMessage))
				return ErrorMessage;
			
			if(Success || ErrorCode == 0)
				return "Nothing went wrong!";
			
			switch(ErrorCode)
			{
				case -1:
					return "No data was returned from the server.  We might be under heavy load or have broken stuff";
					
				case 1:
					return "General error, usually connectivity or a glitch on the servers";
				
				case 2:
					return "Invalid game credentials - make sure you get your details on the API settings page in the dashboard";
					
				case 3:
					return "Request timed out.";
					
				case 4:
					return "Invalid request.";

				case 100:
					return "GeoIP API has been disabled for your game.  This may happen in the first couple minutes till the tracker servers get updated that your game exists, or in very bad cases if your game is placing undue stress on our servers";
					
				case 200:
					return "Leaderboard API has been disabled for your game.  This may happen in the first couple minutes till the tracker servers get updated that your game exists, or in very bad cases if your game is placing undue stress on our servers";
					
				case 201:
					return "The source url or name weren't provided when saving the score.  Make sure the game is initialized, and the Playtomic_PlayerScore has a Name";
					
				case 202:
					return "Invalid auth key.  This usually will only happen if someone messes with the URL the scores save to";
					
				case 203:
					return "No Facebook user id, on a Facebook score submission";
				
				case 204:
					return "Table name wasn't specified for creating a private leaderboard.";
	
				case 205:
					return "Permalink structure wasn't specified: http://website.com/game/whatever?leaderboard=";
					
				case 206:
					return "Leaderboard id wasn't provided loading a private leaderboard.";
					
				case 207:
					return "Invalid leaderboard id was provided for a private leaderboard.";
				
				case 208:
					return "Player is banned from submitting scores in your game.";
				
				case 209:
					return "Score was not the player's best score.  You can notify the player, or circumvent this by pecifying 'allowduplicates' to be true in your save options.";

				case 300:
					return "GameVars API has been disabled for your game.  This may happen in the first couple minutes till the tracker servers get updated that your game exists, or in very bad cases if your game is placing undue stress on our servers";
					
				case 400:
					return "Level sharing API has been disabled for your game.    This may happen in the first couple minutes till the tracker servers get updated that your game exists, or in very bad cases if your game is placing undue stress on our servers";
					
				case 401:
					return "Invalid rating.  Ratings must be integers between 1 and 10";
					
				case 402:
					return "Player already rated that level";
					
				case 403:
					return "The level name was not provided when saving a level";
					
				case 404:
					return "Invalid thumbnail authentication.  You should not see this normally";
					
				case 405:
					return "Invalid thumbnail authentication (Again).  You should not see this normally";
					
				case 406:
					return "That level already exists.  We check for duplicates based on name, player name and source website.  If your players are not entering names use random numbers";
					
				case 500:
					return "Data API is disabled.  You may have to enable it in your game settings in the dashboard because it can reveal your game's data.  This may also happen in the first couple minutes till the tracker servers get updated that your game exists, or in very bad cases if your game is placing undue stress on our servers";
					
				case 600:
					return "You have not configured your Parse.com database.  Sign up at Parse and then enter your API credentials in your Playtomic dashboard.";
					
				case 601:
					return "No response was returned from Parse.  If you experience this a lot let us know exactly what you're doing so we can sort out a fix for it.";
					
				case 6021:
					return "Parse's servers had an error.";
					
				case 602101:
					return "Object not found.  Make sure you include the classname and objectid and that they are correct.";
					
				case 602102:
					return "Invalid query.  If you think you're doing it right let us know what you're doing and we'll look into it.";
					
				case 602103:
					return "Invalid classname.";
					
				case 602104:
					return "Missing objectid.";
					
				case 602105:
					return "Invalid key name.";
					
				case 602106:
					return "Invalid pointer.";
					
				case 602107:
					return "Invalid JSON.";
					
				case 602108:
					return "Command unavailable.";
				
				default:
					return "Unknown error!";
			}
		}
	}
}