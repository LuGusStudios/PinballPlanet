using UnityEngine;
using System.Collections;

public class OpeningSkull : BreakableMultiObjective 
{
	public Hole hole = null;
	
	public int holePassedThroughCount = 0;
	public int holePassedCountBeforeReset = 3;
	
	// Use this for initialization
    protected override void Start () 
	{			
		if( hole == null )
			hole = transform.FindChild("Hole").GetComponent<Hole>();
		
		if( hole == null )
			Debug.LogError("OpeningSkull : hole was undefined!");
		
		//hole.gameObject.SetActiveRecursively(false);
		
		hole.Deactivate();
		
		hole.passedThrough += OnHolePassedThrough;
		
		originalRotation = transform.eulerAngles;

        base.Start();
	}
	
	public void OnHolePassedThrough()
	{
		holePassedThroughCount++;
		
		if( holePassedThroughCount == holePassedCountBeforeReset )
		{
			holePassedThroughCount = 0;
			ResetBreakables();
		}
	}
	
	public void ResetBreakables()
	{	
		Debug.LogError("Resetting breakables!");
		
		CloseSkull();
		
		foreach( Breakable breakable in Objectives )
		{
			breakable.Unbreak();
		}
	}
	
	public void OnBreakableBroken(GameObject sender)
	{
		bool allbroken = true;
		
		Debug.LogError("OnBreakableBroken!");

        foreach (Breakable breakable in Objectives)
		{
			if( !breakable.IsBroken )	
			{
				allbroken = false;
				break;
			}
		}
		
		if( allbroken )
			OpenSkull();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if( Input.GetKeyDown(KeyCode.C) )
		//	ResetBreakables();
	}
	
	protected Vector3 originalRotation;
	
	public void OpenSkull()
	{
		transform.eulerAngles = originalRotation;
		
		//iTween.RotateTo( this.gameObject, new Vector3(18.81357f, 189.9967f, 28.66025f), 2.0f );
		
		iTween.Stop(this.gameObject);
			iTween.RotateAdd( this.gameObject, 
				iTween.Hash("amount",new Vector3(-50, 0, 0),
							"time", 2.0f,
							"isLocal",true));
		
		collider.enabled = false;
		
		hole.Activate();
		//hole.gameObject.SetActiveRecursively(true);
	}
	
	public void CloseSkull()
	{
		
		iTween.Stop(this.gameObject);
		iTween.RotateAdd( this.gameObject, 
				iTween.Hash("amount",new Vector3(50, 0, 0),
							"time", 2.0f,
							"isLocal",true));
		
		collider.enabled = true;
		
		hole.Deactivate();
		//hole.gameObject.SetActiveRecursively(false);
	}
}
