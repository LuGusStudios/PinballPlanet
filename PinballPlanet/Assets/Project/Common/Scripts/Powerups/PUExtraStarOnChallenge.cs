using UnityEngine;
using System.Collections;

public class PUExtraStarOnChallenge : Powerup {

	public PUExtraStarOnChallenge (int id) : base(id)
	{
		iconName = "Icon_Holygrail01";
		name = "Holy Grail";
		description = "Get one bonus star per completed challenge";
	}

	public override void Activate ()
	{
		base.Activate ();
		Debug.LogWarning("PUExtraStarOnChallenge: Not Yet Implemented");

	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
		Debug.LogWarning("PUExtraStarOnChallenge: Not Yet Implemented");
	}
}
