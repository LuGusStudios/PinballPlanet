using UnityEngine;
using System.Collections;

public class PUStarCatcher : Powerup {

	public PUStarCatcher(int id) : base(id)
	{
		iconName = "Icon_Starcatcher01";
		name = "Star Catcher";
		description = "Catch stars automatically";
		unlockLevel = 5;
	}
	
	public override void Activate ()
	{
		base.Activate ();
		StepGameMenu gm = MenuManager.use.GetChildMenu("GameMenu").gameObject.GetComponent<StepGameMenu>();
		gm.AutoCaptureStars = true;
	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
		StepGameMenu gm = MenuManager.use.GetChildMenu("GameMenu").gameObject.GetComponent<StepGameMenu>();
		gm.AutoCaptureStars = false;
	}
}
