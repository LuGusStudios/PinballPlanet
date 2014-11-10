using UnityEngine;
using System.Collections;

public class PUStarCheaper : Powerup {

	private int oldValue = 0;

	public PUStarCheaper (int id) : base(id)
	{
		iconName = "Icon_Stardom01";
		name = "Stardom";
		description = "Catch a star for every 15000 points";
		unlockLevel = 10;
	}

	public override void Activate ()
	{
		base.Activate ();
		oldValue = PlayerData.use.ScorePerStar;
		PlayerData.use.ScorePerStar = 15000;
	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
		if (oldValue != 0) // prevent Deactivate from accidentally being called early and fucking up the value
			PlayerData.use.ScorePerStar = oldValue;
	}
}
