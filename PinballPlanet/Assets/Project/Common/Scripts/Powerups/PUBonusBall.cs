using UnityEngine;
using System.Collections;

public class PUBonusBall : Powerup {

	public PUBonusBall(int id) : base(id)
	{
		iconName = "Icon_Bonusball01";
		name = "Bonus Ball";
		description = "Get one extra ball";
		starCost = 1;
	}

	public override void Activate ()
	{
		base.Activate ();
		ScoreManager.use.SetBallCount(ScoreManager.use.BallCount + 1);
	}

	public override void Deactivate ()
	{
		base.Deactivate ();
	}
}
