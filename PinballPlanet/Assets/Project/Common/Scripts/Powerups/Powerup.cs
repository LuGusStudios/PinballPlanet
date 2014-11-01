using UnityEngine;
using System.Collections;

public class Powerup {

//	protected float scoreMultiplier = 1.0f;
//	protected float scoreMultiplierIncrement = 0.0f;
//	protected float scoreMultiplierMin = 0.0f;
//	protected float scoreMultiplierMax = 2.0f;

	public string name = "";
	public string iconName = "";
	public string description = "";
	public bool resetOnNewBall = false;
	public int id = 0;

	public Powerup(int id)
	{
		this.id = id;
	}

	public virtual void Activate(){
		Debug.Log ("Activating Powerup: " + this.GetType().ToString());
	}

	public virtual void Deactivate()
	{
		Debug.Log ("Deactivating Powerup: " + this.GetType().ToString());
	}

}
