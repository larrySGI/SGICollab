/*
 * 
 * Use this emitter to load heatmap data from Playtomic (Free on the Asset Store)
 * into the editor and view it while testing your game in run-time. 
 * If you have a very large data set this could cause lag, 
 * so be sure to turn it off when not in use.
 * 
*/



using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HeatmapEmitter : MonoBehaviour {
	
	// TXT format CSV file.  This has higher prescedence than URL
	public TextAsset textSet;				
	
	//URL without "HTTP://" to a CSV file.
	public string URL;
	
	//This is used if you have multiple heatmaps on the server and
	//can access them by name attached to the end of a URL.
	public string heatmapName;
	
	//Use a higher number if your datapoints are further apart.
	public float particleSize = 5;
	
	//Set the non-tracked value of the map's position.
	public float zPositionOfHeatMap = 0;	
	
	//Which value your CSV uses between data points.
	public char splitValue = ',';
	
	//Which particle emitter will you use to create the particles.
	//Defaults to the included emitter.
	public GameObject emitterPrefab;
	
	//Turn the heatmap on and off.  Large datasets can cause strain 
	//in run time and should only be used when analyzing data.
	public bool off = false;
	
	//First tracked position data.  Start with Zero
	public int xColumn = 0;
	//Second tracked position data.
	public int yColumn = 1;
	//Alpha Value
	public int aColumn = 2;
	//Red Value
	public int rColumn = 3;
	//Blue Value
	public int bColumn = 4;
	//Green Value
	public int gColumn = 5;
	
	
	private float x = 0;
	private float y = 0;
	private float r = 0;
	private float g = 0;
	private float b = 0;
	private float a = 0;
	private Transform emitterTransform;
	private int count = 0;
	private string dataSet;
	private bool created = false;
	private GameObject emitterObject;
	private ParticleEmitter emitter;
	private Vector3 emitterPosition;
	private bool starting = true;
	
	
	IEnumerator Start () {
			
	//Destroy any emitters still around from previous starts.
	var heatMapEmitters = GameObject.FindGameObjectsWithTag ("HeatMapEmitter");
	foreach (var heatMapEmitter in heatMapEmitters)
	{
		DestroyImmediate(heatMapEmitter);	
	}	
	
	//Initialize the text data into a string from a text file.
	if(textSet!=null)
	{
		dataSet = textSet.text;
		starting = true;
	}
	//Initialize the text data into a string from a text URL.
	else if(URL !=null)
	{
		if(heatmapName != null)
			{
				URL = URL + heatmapName;
			}
		WWW www = new WWW(URL);
		starting = true;
		yield return www;
		dataSet = www.text;
		starting = false;
	}
	else
		Debug.Log("No Data Set");

	
	if(!off)
		emitMap();
	
	}		
	
	void Update () {
		if(off)
		{
			
			var heatMapEmitters = GameObject.FindGameObjectsWithTag ("HeatMapEmitter");
			foreach (var heatMapEmitter in heatMapEmitters)
			{
				if(heatMapEmitter != null)
				DestroyImmediate(heatMapEmitter);	
			}	
			created = false;
			count = 0;
			
		}
		else if(!created && !starting)
			emitMap();
	}
	
	public void emitMap()
	{
		emitterObject = Instantiate(emitterPrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
		emitterPosition = gameObject.transform.position;
		emitter = emitterObject.particleEmitter;
		
		created = true;
		var lines = dataSet.Split('\n');	
		foreach(var line in lines)
		{
			if(string.IsNullOrEmpty(line)) continue;
			count ++;
			var data = line.Split(splitValue);
		
			if(data[xColumn] != null)
				float.TryParse(data[xColumn],out x);
			if(data[yColumn] != null)
				float.TryParse(data[yColumn],out y);
			if(data[aColumn] != null)
				float.TryParse(data[aColumn], out a);
			if(data[rColumn] != null)
				float.TryParse(data[rColumn],out r);
			if(data[bColumn] != null)
				float.TryParse(data[bColumn],out b);
			if(data[gColumn] != null)
				float.TryParse(data[gColumn],out g);
			
			if(r>1)
			r /= 255;
			
			if(g>1)
			g /= 255;
			
			if(b>1)
			b /= 255;
			
			if(a>1)
			a /= 255;

			if(count % 13000 == 0)
			{
				emitterObject = Instantiate(emitterPrefab, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				emitter = emitterObject.particleEmitter;
				emitter.Emit(new Vector3(x,y,zPositionOfHeatMap), emitterPosition, particleSize, 1000000000, new Vector4(r,g,b,a));
			}
			else		
				emitter.Emit(new Vector3(x,y,zPositionOfHeatMap), emitterPosition, particleSize, 1000000000, new Vector4(r,g,b,a));
		}
	}
}
