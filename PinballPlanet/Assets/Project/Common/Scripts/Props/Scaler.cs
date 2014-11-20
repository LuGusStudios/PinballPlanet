using UnityEngine;
using System.Collections;

public class Scaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.one * Mathf.Sin(Time.realtimeSinceStartup*3.5f)*0.1f + Vector3.one * 1f;
	}
}
