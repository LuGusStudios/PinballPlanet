using UnityEngine;
using System.Collections;

public class PUMultiplierBall : Powerup {

	public PUMultiplierBall (int id) : base(id)
	{
		iconName = "Icon_Multiplier01";
		name = "Multiplier Ball";
		description = "Build up extra score over time";
	}

	public override void Activate ()
	{
		base.Activate ();
		ScoreManager.use.SetScoreMultiplierSettings(0.5f, 0.01f, 0.0f, 1.5f);
		resetOnNewBall = true;
	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
	}
}
