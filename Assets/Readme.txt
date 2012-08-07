* COMPATIBILITY

	Unity 3.4.x and later. Previous versions have not been tested.
  
  

* BASIC USAGE

To start using the TTS library first you need to instance the wrapper class.

	Javascript => var myTTS : TTS = new TTS();
	
	C# => TTS myTTS = new TTS();

This initializes the TTS plugin and will have it ready to use. Then to make it talk, just use the following function:

	myTTS.Say("Hello World", 1.0);  The second parameter is the volume, being 1 the maximum.
	
	
	
* ADVANCED USAGE

The following is optional to customize the voice of the library.

This library has different voices to work with, Kal is the default one.

	Kal, Kal16, Rms, Awb and Stl
	
To change the voice just use:

	myTTS.SetVoice("Stl");
	
Appart from the volume, you can set up the pitch, speed and variance of the voice with the following command:

	myTTS.SetVoiceParameters(pitch, speed, variance)
	
NOTES: 
	Pitch has a value between 0.0 and 200, being 200 the higest pitch. The Default value is 100.
	Speed works in reverse, meaning, the lower the value, the faster it is, the default value is 1.
	Variance works from 0 to 100.
	   

	   
* BUILDING APP WITH XCODE
	
	Unity 3.5 	= Works without any additions
	Unity 3.4.x = You need to add a Link to the AVFoundation.framework manually.