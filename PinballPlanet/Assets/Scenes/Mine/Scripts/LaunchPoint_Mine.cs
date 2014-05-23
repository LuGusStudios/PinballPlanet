using UnityEngine;
using System.Collections;

public class LaunchPoint_Mine : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.name != "Ball")
			return;
		
		GameObject.Find("FlowBackPreventer_02").GetComponent<FlowBackPreventer_02>().Reset();
		
	}
}
