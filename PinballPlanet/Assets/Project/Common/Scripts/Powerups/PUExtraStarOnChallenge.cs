using UnityEngine;
using System.Collections;

public class PUExtraStarOnChallenge : Powerup {

	public PUExtraStarOnChallenge (int id) : base(id)
	{
		iconName = "Icon_Holygrail01";
		name = "Holy Grail";
		description = "Get one bonus star per completed challenge";
		unlockLevel = 7;
	}

	public override void Activate ()
	{
		base.Activate ();
		PlayerData.use.bonusStarsOnChallengeComplete = 1;
		//Debug.LogWarning("PUExtraStarOnChallenge: Not Yet Implemented");

	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
		PlayerData.use.bonusStarsOnChallengeComplete = 0;
		//Debug.LogWarning("PUExtraStarOnChallenge: Not Yet Implemented");
	}
}
