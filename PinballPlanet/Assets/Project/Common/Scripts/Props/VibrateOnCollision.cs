using UnityEngine;
using System.Collections;

public class VibrateOnCollision : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag != "Ball")
			return;

		Vibrator.Vibrate();
	}
}
