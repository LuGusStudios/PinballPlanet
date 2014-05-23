using UnityEngine;

public class FPSCounter : MonoBehaviour 
{
//
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
//
// It is also fairly accurate at very low FPS counts (<10).
// We do this not by simply counting frames per interval, but
// by accumulating FPS for each frame. This way we end up with
// correct overall FPS even if the interval renders something like
// 5.5 frames.
 
public  float updateInterval = 0.5F;
 
private float accum   = 0; // FPS accumulated over the interval
private int   frames  = 0; // Frames drawn over the interval
private float timeleft; // Left time for current interval
 
	public TextMesh textMesh = null;
	
void Start()
{
		/*
    if( !guiText )
    {
        Debug.Log("UtilityFramesPerSecond needs a GUIText component!");
        enabled = false;
        return;
    }
    */
		
	textMesh = GetComponent<TextMesh>();
		
    timeleft = updateInterval;  
}
 
void Update()
{
    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
 
    // Interval ended - update GUI text and start new interval
    if( timeleft <= 0.0 )
    {
        // display two fractional digits (f2 format)
	float fps = accum/frames;
	string format = System.String.Format("{0:F2} FPS",fps);
	textMesh.text = format;
 
	if(fps < 30)
		textMesh.renderer.material.color = Color.yellow;
	else 
		if(fps < 10)
			textMesh.renderer.material.color = Color.red;
		else
			textMesh.renderer.material.color = Color.green;
			
	//	DebugConsole.Log(format,level);
        timeleft = updateInterval;
        accum = 0.0F;
        frames = 0;
    }
}
}