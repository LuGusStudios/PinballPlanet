using UnityEngine;
using System.Collections;

public class StepLiteBuyMenu : IMenuStep {

	protected Button buttonLater = null;
	protected Button buttonBuy = null;

	public override void SetupLocal()
	{
		buttonLater = gameObject.FindComponentInChildren<Button>(true, "Button_Later");
		buttonBuy = gameObject.FindComponentInChildren<Button>(true, "Button_Buy");
	}

	public override void Activate(bool animate = true){
		gameObject.SetActive(true);
		activated = true;
	}

	public override void Deactivate(bool animate = true){
		gameObject.SetActive(false);
		activated = false;
	}

	void Update ()
	{
		if (buttonLater.pressed)
		{
			MenuManager.use.DeactivateOverlayMenu(this, false);
		}
		if (buttonBuy.pressed)
		{
			#if UNITY_ANDROID
			Application.OpenURL("market://details?id=com.Lugus.PinballPlanet_TestBuild");
			//#elif UNITY_IPHONE
			//Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
			#endif
		}
	}
}