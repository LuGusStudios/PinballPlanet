using UnityEngine;

public class HideCurrentMenuButton : MonoBehaviour 
{
	protected Camera uiCamera = null;
	public Transform menuToHide = null;

	// Use this for initialization
	void Start () 
	{
		uiCamera = GameObject.Find("UICamera").camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Transform t = LugusInput.use.RayCastFromMouseDown(uiCamera);
		
		if( t == this.transform )
		{
			//Time.timeScale = 1.0f;
			GameObject.Find("JESUS").GetComponent<UIGameController>().UnpauseGameFunctionality();
			
			menuToHide.gameObject.SetActive(false);
		}
	}
}
