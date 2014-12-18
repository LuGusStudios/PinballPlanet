using UnityEngine;
using System.Collections;
//#if !UNITY_WP8
//using UnityEngine.Advertisements;
//#endif

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
//#if !UNITY_WP8
//		if (!Advertisement.isInitialized)
//		{
//			if (Advertisement.isSupported) 
//			{
//				Advertisement.allowPrecache = true;
//				Advertisement.Initialize("20569");
//			}
//			else 
//			{
//				Debug.LogWarning("Advertisements: Platform not supported");
//			}
//		}
//#endif

		LugusCoroutines.use.StartRoutine(SplashRoutine());
	}

	
	// Update is called once per frame
	void Update () {
	
	}

	protected IEnumerator SplashRoutine()
	{
		// Set Black and short wait
		//ScreenFader.use.FadeOut(0.0f);
		ScreenFader.use.SetAlpha(1.0f);
		yield return null;
		yield return new WaitForSeconds(3.2f); // 3 seconds until unity logo vanishes

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
