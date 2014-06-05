using UnityEngine;
using System.Collections;

public class PathItem : MonoBehaviour 
{
	
	public delegate void OnLit();
	public OnLit onLit;
	
	public bool lit = false;
	
	public Material litMaterial = null;
	public Material unlitMaterial = null;
	
	// Use this for initialization
	void Start () 
	{
		Unlight();
	}
	
	public void Light()
	{
		lit = true;
		renderer.material = litMaterial;
		
		
		ScoreHit s = GetComponent<ScoreHit>();
		if( s != null )
			s.DoScore();
		
		if( onLit != null )
			onLit();
	}
	
	public void Unlight()
	{
		lit = false;
		renderer.material = unlitMaterial;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if( Input.GetKeyDown(KeyCode.L) )
		//	Light();
	}
	
	void OnTriggerEnter(Collider collider)
	{
		GameObject ball = collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		
		Light();
    }
}
