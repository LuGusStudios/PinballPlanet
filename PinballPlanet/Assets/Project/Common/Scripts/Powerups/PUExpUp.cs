using UnityEngine;
using System.Collections;

public class PUExpUp : Powerup {

	public PUExpUp(int id) : base(id)
	{
		iconName = "Icon_Mrfancy01";
		name = "Mr. Fancy";
		description = "Get 20% more experience points";
	}

	public override void Activate ()
	{
		base.Activate ();
		PlayerData.use.expMultiplier = 1.2f;
	}
	
	public override void Deactivate ()
	{
		base.Deactivate ();
		PlayerData.use.expMultiplier = 1.0f;
	}
}
