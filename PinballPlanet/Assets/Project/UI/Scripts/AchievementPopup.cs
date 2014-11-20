using UnityEngine;
using System.Collections;

public class AchievementPopup : LugusSingletonExisting<AchievementPopup> {

	private bool _isShowing = false;
	protected Transform icon = null;
	private Transform startTransform = null;
	private Transform showTransform = null;
	private Transform endTransform = null;
	private ParticleSystem particles = null;

	public override void InitializeSingleton ()
	{
		base.InitializeSingleton ();
	}

	void SetupLocal()
	{
		_isShowing = false;

		icon = transform.FindChild("PopupIcon");
		if (icon == null) Debug.LogWarning("AchievementPopup: icon missig.");
		
		startTransform = transform.FindChild("StartTransform");
		if (startTransform == null) Debug.LogWarning("AchievementPopup: startTransform missig.");
		
		showTransform = transform.FindChild("ShowTransform");
		if (showTransform == null) Debug.LogWarning("AchievementPopup: showTransform missig.");
		
		endTransform = transform.FindChild("EndTransform");
		if (endTransform == null) Debug.LogWarning("AchievementPopup: endTransform missig.");

		particles = gameObject.FindComponentInChildren<ParticleSystem>(true, "Particles");

		icon.transform.localPosition = startTransform.localPosition;
		icon.transform.localEulerAngles = startTransform.localEulerAngles;
	}

	void Awake()
	{
		SetupLocal();
	}
	
	// Update is called once per frame
	void Update () {
		//Show();
	}

	public void Show()
	{
		if (_isShowing)
			return;

		if (MenuManager.use.ActiveMenu == MenuManagerDefault.MenuTypes.GameOverMenu)
			return;

		LugusCoroutines.use.StartRoutine(ShowRoutine());
	}

	protected IEnumerator ShowRoutine()
	{
		_isShowing = true;
		yield return LugusCoroutines.use.StartRoutine(PopupRoutine(0.2f, 2.0f, 0.5f)).Coroutine;
		_isShowing = false;
	}

	protected IEnumerator PopupRoutine(float revealDuration, float showDuration, float hideDuration)
	{
		icon.transform.localPosition = startTransform.localPosition;
		icon.transform.localEulerAngles = startTransform.localEulerAngles;

		float timerStart = Time.realtimeSinceStartup;

		LugusAudio.use.SFX().Play(LugusResources.use.Shared.GetAudio("Whoosh01"));

		// Reveal the icon
		while ((Time.realtimeSinceStartup - timerStart) <= revealDuration)
		{
			float lerpVal = (Time.realtimeSinceStartup - timerStart) / revealDuration;

			icon.transform.localPosition = Vector3.Lerp(startTransform.localPosition, showTransform.localPosition, lerpVal);			
			icon.transform.localEulerAngles = Vector3.Lerp(startTransform.localEulerAngles, showTransform.localEulerAngles, lerpVal);

			yield return null;
		}

		icon.transform.localPosition = showTransform.localPosition;
		icon.transform.localEulerAngles = showTransform.localEulerAngles;

		// reset timer
		timerStart = Time.realtimeSinceStartup;

		particles.Play();

		// show the icon
		while ((Time.realtimeSinceStartup - timerStart) <= showDuration)
		{
			yield return null;
		}

		// reset timer
		timerStart = Time.realtimeSinceStartup;

		// hide the icon
		while ((Time.realtimeSinceStartup - timerStart) <= hideDuration)
		{
			float lerpVal = (Time.realtimeSinceStartup - timerStart) / hideDuration;
			
			icon.transform.localPosition = Vector3.Lerp(showTransform.localPosition, endTransform.localPosition, lerpVal);			
			icon.transform.localEulerAngles = Vector3.Lerp(showTransform.localEulerAngles, endTransform.localEulerAngles, lerpVal);
			
			yield return null;
		}
		
		icon.transform.localPosition = endTransform.localPosition;
		icon.transform.localEulerAngles = endTransform.localEulerAngles;
		
		yield break;
	}
}
