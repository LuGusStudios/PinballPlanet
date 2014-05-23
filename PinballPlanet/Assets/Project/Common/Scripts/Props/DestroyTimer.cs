using UnityEngine;

public class DestroyTimer : MonoBehaviour {
	
	public float TimeToDestroy = 0.0f;

	// Update is called once per frame
	void Update () {
		
		Destroy(gameObject, TimeToDestroy);
		
	}
}
