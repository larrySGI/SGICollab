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

public class Playtomic_LogRequest
{
	private static int Failed = 0;
	private static List<Playtomic_LogRequest> Pool = new List<Playtomic_LogRequest>();
	
	private string Data = "";
	public bool Ready = false;

	public static Playtomic_LogRequest Create()
	{
		Playtomic_LogRequest request = null;
		if (Pool.Count > 0)
		{
			request = Pool[0];
			Pool.RemoveAt(0);
		}
		else
		{
			request = new Playtomic_LogRequest();
		}
		
		request.Data = "";
		request.Ready = false;
		return request;
	}
	
	
	public void MassQueue(List<string> data)
	{
		if(Failed > 3)
			return;
		
		for(int i=data.Count-1; i>-1; i--)
		{
			Data += (Data == "" ? "" : "~") + data[i];
			data.RemoveAt(i);

			if(Data.Length > 300)
			{
				Playtomic_LogRequest request = Create();
				request.MassQueue(data);
				
				Ready = true;
				Send();
				return;
			}
		}
		
		Playtomic.Log.Request = this;
	}		

	public void Queue(string data)
	{
		//Debug.Log("Adding event " + data);
		if(Failed > 3)
			return;
		
		if(Data.Length > 0)
			Data += "~";
		
		Data += data;

		if(Data.Length > 300 || data.StartsWith("v/") || data.StartsWith("t/"))
		{
			//Debug.Log("Ready");
			Ready = true;
		}
	}

	public void Send()
	{
		//Debug.Log("Sending (logrequest)");
		Playtomic.API.StartCoroutine(Playtomic_Request.SendStatistics(Data));
		Pool.Add(this);
	}
}