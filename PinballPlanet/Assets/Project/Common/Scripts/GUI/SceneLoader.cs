using UnityEngine;
using System.Collections;

public class SceneLoader : LugusSingletonExisting<SceneLoader> {

	// Use this for initialization
	void Start () {
		ScreenFader.use.FadeIn(1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void OnLevelWasLoaded(int level) 
	{

	}

	public void LoadNewScene(string sceneName, float fadeTime = 0.3f)
	{
		if (fadeTime < 0)
		{
			Application.LoadLevel(sceneName);
		}
		else
		{
			LugusCoroutines.use.StartRoutine(LoadNewSceneRoutine(sceneName, fadeTime));
		}
	}

	public void LoadNewScene(int sceneId, float fadeTime = 0.3f)
	{
		if (fadeTime < 0)
		{
			Application.LoadLevel(sceneId);
		}
		else
		{
			LugusCoroutines.use.StartRoutine(LoadNewSceneRoutine(sceneId, fadeTime));
		}
	}

	protected IEnumerator LoadNewSceneRoutine(string sceneName, float fadeTime)
	{
		ScreenFader.use.FadeOut(fadeTime);
		// This allows us to wait while the game is pauzed, unlike WaitForSeconds
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
		// This allows us to wait while the game is pauzed, unlike WaitForSeconds
		float timerStart = Time.realtimeSinceStartup;		
		while ((Time.realtimeSinceStartup - timerStart) <= fadeTime + 0.1f)
		{
			yield return null;
		}

		Application.LoadLevel(sceneId);
		yield break;
	}
}
