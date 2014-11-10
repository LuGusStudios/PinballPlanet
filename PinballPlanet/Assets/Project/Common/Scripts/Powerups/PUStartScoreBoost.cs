using UnityEngine;
using System.Collections;

public class PUStartScoreBoost : Powerup {

	public PUStartScoreBoost(int id) : base(id)
	{
		iconName = "Icon_Inthebank01";
		name = "In The Bank";
		description = "Start the game with 10000 points";
		starCost = 1;
	}

	public override void Activate ()
	{
		base.Activate ();
		ScoreManager.use.ShowScore(10000, Vector3.zero, 1.0f, null, Color.white, null);
	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
	}
}
