using UnityEngine;

public class MainMenuButton : MonoBehaviour 
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
		
		if( t == this.transform || Input.GetKeyDown(KeyCode.M) )
		{
			Application.LoadLevel("Simulation");
			//GameObject.Find("JESUS").GetComponent<UIGameController>().ShowMainMenu();
		}
	}
}