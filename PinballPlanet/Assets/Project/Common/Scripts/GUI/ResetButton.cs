using UnityEngine;

public class ResetButton : MonoBehaviour 
{
	protected Camera uiCamera = null;

	// Use this for initialization
	void Start () 
	{
		uiCamera = GameObject.Find("UICamera").camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Transform t = LugusInput.use.RayCastFromMouseDown(uiCamera);
		//if( t != null )
		//	Debug.Log("HIT : " + t.name);
		
		if( t == this.transform  || Input.GetKeyDown(KeyCode.R) )
		{
			Debug.LogError("RESETTING!");
			Application.LoadLevel("Simulation");
		}
	}
}
