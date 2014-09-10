using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BatCave : MonoBehaviour 
{
	public ThombStone thombStone = null;
	
	public Material openMaterial = null;
	public Material closeMaterial = null;
	
	public List<Transform> bats = new List<Transform>();
	public Transform batPrefab = null;
	public Transform BatTarget = null;
	
	public int spawnCount = 2;
	
	// Use this for initialization
	void Start () 
	{
		if( thombStone == null )
			thombStone = transform.parent.GetComponent<ThombStone>();
		
		if( thombStone == null )
			Debug.LogError("BatCave: ThombStone was null!");
		
		if( batPrefab == null )
			Debug.LogError("BatCave: BatPrefab was null!");
			
		
		thombStone.onOpen += OnThombstoneOpen;
		
		Close();
	}
	
	public void OnThombstoneOpen()
	{
		Open();
	}
	
	public void Open()
	{
		renderer.material = openMaterial;
		
		SpawnBats(spawnCount);
	}
	
	public int destroyedBats = 0;
	
	public void OnBatHit()
	{
		destroyedBats++;
		
		if( destroyedBats == bats.Count )
		{
            //Debug.Log("All bats destroyed: resetting the batcave!");
			
			thombStone.Close();
			Close();
			
			destroyedBats = 0;
			//SpawnBats(spawnCount);
		}
	}
	
	public void SpawnBats( int nr )
	{
		bats = new List<Transform>();
		
		for( int i = 0; i < nr; ++i )
		{
			Transform newBat = (Transform) GameObject.Instantiate( batPrefab );
			newBat.position = transform.position;
			
			Vector3 target = BatTarget.position + new Vector3( Random.Range(-15,15), Random.Range(-25,25), 0);
			
			iTween.MoveTo( newBat.gameObject, target, 2.0f);
			
			newBat.GetComponent<Bat>().onHit += OnBatHit;
			
			bats.Add( newBat );
		}
	}
	
	public void Close()
	{
		renderer.material = closeMaterial;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
