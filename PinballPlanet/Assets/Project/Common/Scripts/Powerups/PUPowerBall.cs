using UnityEngine;
using System.Collections;

public class PUPowerBall : Powerup {

	public PUPowerBall(int id) : base(id)
	{
		iconName = "Icon_Powerball01";
		name = "Power Ball";
		description = "Only one ball, but 3 times the score";
	}

	public override void Activate ()
	{
		base.Activate ();
		ScoreManager.use.SetScoreMultiplierSettings(3.0f, 0.0f, 0.0f, 3.0f);
		ScoreManager.use.SetBallCount(1);
	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
	}
}
