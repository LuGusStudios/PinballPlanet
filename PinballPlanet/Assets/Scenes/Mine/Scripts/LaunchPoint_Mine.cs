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
		
        // Reset flowback preventer.
		GameObject.Find("FlowBackPreventer_02").GetComponent<FlowBackPreventer_02>().Reset();
	
        // Reset logs.
        GameObject.Find("Logs").GetComponent<Logs>().Unbreak();

        // Reset launch lights.
        GameObject.Find("FloorLight_Launch_Link1").GetComponent<FloorLight_Link>().Unbreak();
    }
}
