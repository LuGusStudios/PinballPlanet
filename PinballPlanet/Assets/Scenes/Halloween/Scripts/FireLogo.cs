using UnityEngine;
using System.Collections;

public class FireLogo : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Light()
	{
		StartCoroutine( LightRoutine() );
	}
	
	protected IEnumerator LightRoutine()
	{
		renderer.enabled = true;
		
		yield return new WaitForSeconds(3.0f);
		
		renderer.enabled = false;
	}
}
