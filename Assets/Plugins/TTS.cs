/*==============================================================================
Copyright (c) 2012 baKno Games.
All Rights Reserved.
==============================================================================*/

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;

public enum TTSVoices
{
    Kal = 0,
    Kal16,
    Rms,
    Awb,
    Stl
}

public class TTS
{
    private struct ThreadData
    {
        public string message;
        public float volume;
    }
    
    private Thread mThread;
    
    [DllImport("__Internal")]
    private static extern void InitializeTTS();
    
    [DllImport("__Internal")]
    private static extern void Speak(string message, float volume);
    
    [DllImport("__Internal")]
    private static extern void SetParameters(float pitch, float speed, float variance);
    
    [DllImport("__Internal")]
    private static extern void SetVoice(int voiceIndex);
    
    [DllImport("__Internal")]
    private static extern void Terminate();
       
    public TTS()
    {
        InitializeTTS();
    }
    
    public void SetVoice(TTSVoices voice)
    {
        SetVoice((int) voice);
    }
    
    public void SetVoice(string voice)
    {
        SetVoice((TTSVoices) System.Enum.Parse(typeof(TTSVoices), voice));
    }

    
    public void SetVoiceParameters(float pitch, float speed, float variance)
    {
        SetParameters(pitch, speed, variance);
    }
    
    public void Say(string message, float volume)
    {
        ThreadData mData = new ThreadData();
        mData.message = message;
        mData.volume = volume;
        if(mThread != null)
            mThread.Abort();
        mThread = new Thread(new ParameterizedThreadStart(DoSpeak));
        mThread.Start(mData);
    }
    
    private void DoSpeak(object ob)
    {
        ThreadData tData = (ThreadData)ob;
        Debug.Log(tData.message);
        Speak(tData.message, tData.volume);
    }
    
    public void Close()
    {
        Terminate();
    }
}
