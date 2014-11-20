using UnityEngine;
using System.Collections;

public class Floater : MonoBehaviour {

	private Vector3 originalPos;
	private Vector3 direction;
	public float amplitude = 2f;
	public float frequency = 2f;
	private float phase = 0;

	// Use this for initialization
	void Start () {
		originalPos = transform.localPosition;

		Vector3 origin = GameObject.Find("Planet/Meshes").transform.localPosition;
		direction = transform.localPosition - origin;
		direction.Normalize();

		phase = Random.Range(0f, 2*Mathf.PI);
		frequency += Random.Range(-0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

		float floating = Mathf.Sin (Time.realtimeSinceStartup*frequency);
		transform.localPosition = originalPos + direction * floating * amplitude;
	}
}
