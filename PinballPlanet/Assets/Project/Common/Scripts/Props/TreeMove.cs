using UnityEngine;
using System.Collections;

public class TreeMove : MonoBehaviour {

	private Vector3 originalRot;
	private Vector3 direction;
	private float amplitude = 5f;
	private float frequency = 2f;
	private float phase = 0;
	
	// Use this for initialization
	void Start () {
		originalRot = transform.localEulerAngles;

		direction = new Vector3(0, 1, 0);
		
		phase = Random.Range(0f, 2*Mathf.PI);
		frequency += Random.Range(-0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {		
		float waving = Mathf.Sin (Time.realtimeSinceStartup*frequency);
		transform.localEulerAngles = originalRot + direction * waving * amplitude;
	}
}
