using UnityEngine;
using System.Collections;

public class Vibrator : MonoBehaviour {

	public static bool canVibrate = false;

	public static void Vibrate()
	{
		if (canVibrate)
			Handheld.Vibrate();
	}
}
