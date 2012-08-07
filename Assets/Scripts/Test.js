//public variables
var ttsWaitTime : float;

//private variables
private var myTTS : TTS;
private var canSpeak : boolean;
private var volume : float;
private var pitch: float;
private var speed : float;
private var variance : float;
private var voices : String[] = ["Kal", "Kal16", "Rms", "Awb", "Stl"];
private var textToRead : String;
private var selectedVoice : int;

function Start () {
    myTTS = new TTS();
    canSpeak = true;
    textToRead = "This is a Test";
    volume = 1.0;
    pitch = 100.0;
    speed = 1.0;
    variance = 50.0;
}

function OnGUI () {
    GUI.enabled = canSpeak;
    GUI.Label(Rect(20.0, (Screen.height * 0.5) - 200.0, 130.0, 50), "Voice");
    selectedVoice = GUI.SelectionGrid(Rect(20.0, (Screen.height * 0.5) - 150.0, 130.0, 300.0), selectedVoice, voices, 1);
    
    GUI.Label(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) - 200.0, 300.0, 50.0), "Text to Read");
    textToRead = GUI.TextField(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) - 150.0, 300.0, 60.0), textToRead);
    
    if(GUI.Button(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) - 70.0, 140.0, 60.0), "Read")){
       Speak();
    }
    
    GUI.Label(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 10.0, 200.0, 30.0), "Volume: " + volume.ToString());
    volume = GUI.HorizontalSlider(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 40.0, 200.0, 50.0), volume, 0.1, 1.0);
    
    GUI.Label(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 90.0, 200.0, 30.0), "Pitch: " + pitch.ToString());
    pitch = GUI.HorizontalSlider(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 120.0, 200.0, 50.0), pitch, 0.1, 200.0);
    
    GUI.Label(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 170.0, 200.0, 30.0), "Speed: " + speed.ToString());
    speed = GUI.HorizontalSlider(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 200.0, 200.0, 50.0), speed, 0.1, 3.0);
    
    GUI.Label(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 250.0, 200.0, 30.0), "Variance: " + variance.ToString());
    variance = GUI.HorizontalSlider(Rect((Screen.width * 0.5) - 100.0, (Screen.height * 0.5) + 280.0, 200.0, 50.0), variance, 0.1, 100.0);
}

function Speak(){
    canSpeak = false;
    myTTS.SetVoice(voices[selectedVoice]);
    myTTS.SetVoiceParameters(pitch, speed, variance);
    myTTS.Say(textToRead, volume);
    yield WaitForSeconds(ttsWaitTime);
    canSpeak = true;
}