using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour 
{
	public List<PathItem> pathItems = new List<PathItem>();

	// Use this for initialization
	void Start () 
	{
		foreach( Transform child in this.transform )
		{
			PathItem pathItem = child.GetComponent<PathItem>();
			if( pathItem == null )
				pathItem = child.gameObject.AddComponent<PathItem>();
			
			pathItem.onLit += OnPathItemLit;
			pathItems.Add( pathItem );
		}
	}
	
	
	private bool _allLit = false;
	
	public void OnPathItemLit()
	{
		// could be we already did the score for this!
		// _allLit is set to false when the path has effectively gone out for a while
		if( _allLit )
			return;
		
		
		_allLit = true;
		foreach( PathItem item in pathItems )
		{
			if( !item.lit )
			{
				_allLit = false;
				break;
			}
		}
		
		if( !_allLit )
			return;
		
		//Debug.LogError("ALL ITEMS LIT!!!");
		
		ScoreHit s = GetComponent<ScoreHit>();
		if( s != null )
			s.DoScore();
		
		StartCoroutine( PathUnlightRoutine() );
	}
	
	protected IEnumerator PathUnlightRoutine()
	{
		yield return new WaitForSeconds(3.0f);
		
		//foreach( PathItem item in pathItems )
		for( int i = 0; i < pathItems.Count; ++i )
		{
			PathItem item = pathItems[i];
			item.Unlight();
			
			//yield return new WaitForSeconds(1.0f);
		}
		
		_allLit = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
