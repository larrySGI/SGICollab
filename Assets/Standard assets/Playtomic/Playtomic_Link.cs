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

public class Playtomic_Link
{
	private List<string> Clicks = new List<string>();

	public bool Open(string url, string name, string group)
	{
		int unique = 0;
		int bunique = 0;
		int total = 0;
		int btotal = 0;
		int fail = 0;
		int bfail = 0;
		string key = url + "." + name;
		bool result;

		string baseurl = url;
		baseurl = baseurl.Replace("http://", "");
		
		if(baseurl.IndexOf("/") > -1)
			baseurl = baseurl.Substring(0, baseurl.IndexOf("/"));
			
		if(baseurl.IndexOf("?") > -1)
			baseurl = baseurl.Substring(0, baseurl.IndexOf("?"));				
			
		baseurl = "http://" + baseurl + "/";

		string baseurlname = baseurl;
		
		if(baseurlname.IndexOf("//") > -1)
			baseurlname = baseurlname.Substring(baseurlname.IndexOf("//") + 2);
		
		baseurlname = baseurlname.Replace("www.", "");

		if(baseurlname.IndexOf("/") > -1)
		{
			baseurlname = baseurlname.Substring(0, baseurlname.IndexOf("/"));
		}

		Application.OpenURL(url);

		if(Clicks.IndexOf(key) > -1)
		{
			total = 1;
		}
		else
		{
			total = 1;
			unique = 1;
			Clicks.Add(key);
		}

		if(Clicks.IndexOf(baseurlname) > -1)
		{
			btotal = 1;
		}
		else
		{
			btotal = 1;
			bunique = 1;
			Clicks.Add(baseurlname);
		}

		result = true;
		
		// if it failed, you would:
		// {
		//	fail = 1;
		//	bfail = 1;
		//	result = false;
		// }
		// but there's no way to detect failure in opening the URL right now (and failure may not be possible, there's no setting akin to Flash's popup-blocking setting)
					
		Playtomic.Log.Link(baseurl, baseurlname.ToLower(), "DomainTotals", bunique, btotal, bfail);
		Playtomic.Log.Link(url, name, group, unique, total, fail);
		Playtomic.Log.ForceSend();

		return result;
	}
}