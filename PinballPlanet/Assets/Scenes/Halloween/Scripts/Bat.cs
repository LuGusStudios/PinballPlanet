using UnityEngine;
using System.Collections;

public class Bat : MonoBehaviour 
{
	
	public Material state1 = null;
	public Material state2 = null;
	
	public float timeBetweenStates = 0.2f;
	
	public bool animating = false;
	public bool randomMovement = false;
	
	protected Vector3 targetPosition;
	protected Vector3 originalPosition;
	
	public delegate void OnHit();
	public OnHit onHit;
	
	// Use this for initialization
	void Start () 
	{
		collider.enabled = false;
		
		
		StartCoroutine( AnimationRoutine() );
		StartCoroutine( RandomMovementRoutine() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( randomMovement )
			transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 2);
	}
	
	public IEnumerator RandomMovementRoutine()
	{
		yield return new WaitForSeconds(2.0f);
	
		originalPosition = transform.position;
		
		collider.enabled = true;
		randomMovement = true;
		
		while( animating )
		{
			float range = 10.0f;
			
			float x = transform.position.x + Random.Range(-range, range);
			float y = transform.position.y + Random.Range(-range, range);
			float z = transform.position.z;// + Random.Range(-range, range);
			
			targetPosition = new Vector3(x, y, z);
			
			if( Vector3.Distance(originalPosition, targetPosition) > 20 )
				targetPosition = originalPosition;
			
			yield return new WaitForSeconds(3.0f);
		}
	}
	
	protected IEnumerator AnimationRoutine()
	{
		animating = true;
		
		yield return new WaitForSeconds( Random.Range(0.0f, 0.3f) );
		
		
		while( animating )
		{
			//Debug.Log("ANIMATING THE BAT");
			
			yield return new WaitForSeconds(timeBetweenStates);
			
			renderer.material = state1; 
			
			yield return new WaitForSeconds(timeBetweenStates);
			
			renderer.material = state2; 
		}
	}
	
	/*
	void OnCollisionEnter(Collision collision) 
	{
		GameObject ball = collision.collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		GameObject.Destroy(this.gameObject);
    }
    */
	
	void OnTriggerEnter(Collider collider)
	{
		GameObject ball = collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		//ScoreHit s = GetComponent<ScoreHit>();
		//if( s != null )
		//	s.DoScore();
		
		this.collider.enabled = false;
		StartCoroutine( DestroyRoutine() );
		
		if( onHit != null )
			onHit();
		
	}
	
	protected IEnumerator DestroyRoutine()
	{
		
		Vector3 newPos = transform.position + new Vector3(0, 0, 120);
		iTween.MoveTo( this.gameObject, newPos, 5.0f);
		
		yield return new WaitForSeconds(5f);
		
		GameObject.Destroy(this.gameObject);
	}
}
