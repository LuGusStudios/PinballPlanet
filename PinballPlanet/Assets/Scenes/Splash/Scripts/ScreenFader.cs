using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenFader : LugusSingletonRuntime<ScreenFader>
{
	protected SpriteRenderer fadeRenderer = null;
	protected GameObject cameraFade = null;
	protected GUITexture fadeGUITexture = null;
	protected ILugusCoroutineHandle fadeRoutine = null;

	public void SetupLocal()
	{
	}

	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();
		if (fadeRenderer == null)
		{
	
			GameObject fadeImageObject = new GameObject("Fader");
			fadeRenderer = fadeImageObject.AddComponent<SpriteRenderer>();
			fadeRenderer.sprite = LugusResources.use.Shared.GetSprite("ScreenFade01");
			fadeImageObject.transform.localScale = Vector3.one*100;
			fadeImageObject.transform.parent = LugusCamera.ui.transform;
			fadeImageObject.transform.localPosition = Vector3.zero.z(1);
			fadeImageObject.layer = LayerMask.NameToLayer("GUI");
		}
	}

	public void SetupGlobal()
	{

	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start () 
	{
		SetupGlobal();
	}

	public void FadeOut(float time)
	{
		Debug.Log("ScreenFader: Fading out.");

		fadeRenderer.color = fadeRenderer.color.a(0.0f);

		if (fadeRoutine != null && fadeRoutine.Running)
		{
			fadeRoutine.StopRoutine();
		}

		fadeRoutine = LugusCoroutines.use.StartRoutine(FadeRoutine(1.0f, time));
	}

	public void FadeIn(float time)
	{
		Debug.Log("ScreenFader: Fading in.");

		fadeRenderer.color = fadeRenderer.color.a(1.0f);

		if (fadeRoutine != null && fadeRoutine.Running)
		{
			fadeRoutine.StopRoutine();
		}

		fadeRoutine = LugusCoroutines.use.StartRoutine(FadeRoutine(0.0f, time));
	}

	protected IEnumerator FadeRoutine(float targetAlpha, float duration)
	{
		fadeRenderer.enabled = true;
		
		if (duration <= 0)
		{
			fadeRenderer.color = fadeRenderer.color.a(targetAlpha);
			yield break;
		}
		
		float startAlpha = fadeRenderer.color.a;
		float timerStart = Time.realtimeSinceStartup;
		
		while ((Time.realtimeSinceStartup - timerStart) <= duration)
		{
			fadeRenderer.color = fadeRenderer.color.a( Mathf.Lerp(startAlpha, targetAlpha, (Time.realtimeSinceStartup - timerStart) / duration ));
			yield return null;
		}
		
		fadeRenderer.color = fadeRenderer.color.a(targetAlpha);	// this will ensure the fade always reaches perfect completion

		if (fadeRenderer.color.a <= 0)
		{
			fadeRenderer.enabled = false;
		}

		yield break;
	}
}
