using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		LugusCoroutines.use.StartRoutine(SplashRoutine());
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	protected IEnumerator SplashRoutine()
	{
		// Set Black and short wait
		ScreenFader.use.FadeOut(0.0f);
		yield return new WaitForSeconds(1.0f);

		// start music just before revealing logo
		LugusAudio.use.Music().Play(LugusResources.use.Shared.GetAudio("LugusSplashShort01"));
		yield return new WaitForSeconds(0.2f);

		// Reveal Logo
		ScreenFader.use.FadeIn(1.0f);
		yield return new WaitForSeconds(1.8f);

		// Fade to black and let audio finish
		ScreenFader.use.FadeOut(0.4f);
		yield return new WaitForSeconds(1.1f); 

		// change scene immediately
		SceneLoader.use.LoadNewScene("Pinball_MainMenu", -1);
		//SceneLoader.use.LoadNewScene("Pinball_SplashScreen", -1);
	}
}
