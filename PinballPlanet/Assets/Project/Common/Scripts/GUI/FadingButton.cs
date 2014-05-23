using UnityEngine;
using System.Collections;

public class FadingButton : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine( FadingRoutine() );
	}
	
	protected IEnumerator FadingRoutine()
	{
		yield return new WaitForSeconds(5.0f);
		
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
