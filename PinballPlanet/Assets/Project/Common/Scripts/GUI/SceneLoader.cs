using UnityEngine;
using System.Collections;
#if !UNITY_WP8
using UnityEngine.Advertisements;
#endif

public class SceneLoader : LugusSingletonExisting<SceneLoader> {

	public bool enableInitialFade = true;

	// Use this for initialization
	void Start () {

//		if (Advertisement.isReady())
//		{
//			Advertisement.Show(null, new ShowOptions {
//				pause = true,
//				resultCallback = result => {
//					Debug.Log(">>> " + result.ToString());
//				}
//			});
//		}

		if (enableInitialFade)
			ScreenFader.use.FadeIn(1.0f);
	}

	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();

		#if !UNITY_WP8
		if (!Advertisement.isInitialized)
		{
			if (Advertisement.isSupported) 
			{
				Advertisement.allowPrecache = true;
				Advertisement.Initialize("20569");
			}
			else 
			{
				Debug.LogWarning("Advertisements: Platform not supported");
			}
		}
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void OnLevelWasLoaded(int level) 
	{

	}

	public void LoadNewScene(string sceneName, float fadeTime = 0.3f, bool canShowAd = false)
	{
#if !UNITY_WP8
		if (Version.isLite && canShowAd && Advertisement.isSupported)
		{
			LugusCoroutines.use.StartRoutine(LoadNewSceneRoutineWithAd(sceneName, fadeTime));
		}
		else 
		{
#endif
			if (fadeTime < 0)
			{
				Application.LoadLevel(sceneName);
			}
			else
			{
				LugusCoroutines.use.StartRoutine(LoadNewSceneRoutine(sceneName, fadeTime));
			}
#if !UNITY_WP8
		}
#endif
	}

	public void LoadNewScene(int sceneId, float fadeTime = 0.3f, bool canShowAd = false)
	{
#if !UNITY_WP8
		if (Version.isLite && canShowAd && Advertisement.isSupported) 
		{
			LugusCoroutines.use.StartRoutine(LoadNewSceneRoutineWithAd(sceneId, fadeTime));
		}
		else 
		{
#endif
			if (fadeTime < 0)
			{
				Application.LoadLevel(sceneId);
			}
			else
			{
				LugusCoroutines.use.StartRoutine(LoadNewSceneRoutine(sceneId, fadeTime));
			}
#if !UNITY_WP8
		}
#endif
	}


	protected IEnumerator LoadNewSceneRoutineWithAd(string sceneName, float fadeTime)
	{
		int count = 0;

		ScreenFader.use.FadeOut(fadeTime);
		// This allows us to wait while the game is paused, unlike WaitForSeconds
		float timerStart = Time.realtimeSinceStartup;		
		while ((Time.realtimeSinceStartup - timerStart) <= fadeTime + 0.1f)
		{
			yield return null;
		}

#if !UNITY_WP8
		while(!Advertisement.isReady())
		{
			count++;
			if (count > 1000)
			{
				Debug.LogError("More than 1000 frames");
				yield break;
			}
			yield return null;
		}

		Advertisement.Show(null, new ShowOptions {
			pause = true,
			resultCallback = result => {
				Debug.Log(">>> " + result.ToString() + " Frames: " + count);
				Application.LoadLevel(sceneName);
			}
		});
#else
		Application.LoadLevel(sceneName);
#endif
	}

	protected IEnumerator LoadNewSceneRoutineWithAd(int sceneId, float fadeTime)
	{
		int count = 0;

		ScreenFader.use.FadeOut(fadeTime);
		// This allows us to wait while the game is paused, unlike WaitForSeconds
		float timerStart = Time.realtimeSinceStartup;		
		while ((Time.realtimeSinceStartup - timerStart) <= fadeTime + 0.1f)
		{
			yield return null;
		}
		
#if !UNITY_WP8
		while(!Advertisement.isReady())
		{
			count++;
			if (count > 1000)
			{
				Debug.LogError("More than 1000 frames");
				yield break;
			}
			yield return null;
		}
		
		Advertisement.Show(null, new ShowOptions {
			pause = true,
			resultCallback = result => {
				Debug.Log(">>> " + result.ToString() + " Frames: " + count);
				Application.LoadLevel(sceneId);
			}
		});
#else
		Application.LoadLevel(sceneId);
#endif
	}



	protected IEnumerator LoadNewSceneRoutine(string sceneName, float fadeTime)
	{
		ScreenFader.use.FadeOut(fadeTime);
		// This allows us to wait while the game is paused, unlike WaitForSeconds
		float timerStart = Time.realtimeSinceStartup;		
		while ((Time.realtimeSinceStartup - timerStart) <= fadeTime + 0.1f)
		{
			yield return null;
		}

		Application.LoadLevel(sceneName);
		yield break;
	}

	protected IEnumerator LoadNewSceneRoutine(int sceneId, float fadeTime)
	{
		ScreenFader.use.FadeOut(fadeTime);
		// This allows us to wait while the game is paused, unlike WaitForSeconds
		float timerStart = Time.realtimeSinceStartup;		
		while ((Time.realtimeSinceStartup - timerStart) <= fadeTime + 0.1f)
		{
			yield return null;
		}

		Application.LoadLevel(sceneId);
		yield break;
	}
}
