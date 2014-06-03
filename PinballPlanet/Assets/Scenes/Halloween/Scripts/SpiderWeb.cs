using UnityEngine;
using System.Collections;

public class SpiderWeb : MonoBehaviour 
{
	public Transform spider = null;
	public bool activated = true;
	
	public bool autoRespawn = true;
	public float respawnInterval = 30.0f;
	
	// Use this for initialization
	void Start () 
	{
		if( spider == null )
			Debug.LogError("SpiderWeb : Spider was null!");
		
		Activate();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if( Input.GetKeyDown(KeyCode.S) )
		//	OnHitBy(null);
	}
	
	public void Activate()
	{
		activated = true;
		
		transform.FindChild("spiderwebweb").renderer.enabled = true;
		collider.enabled = true;
	}
	
	public void Deactivate()
	{
		activated = false;
		
		transform.FindChild("spiderwebweb").renderer.enabled = false;
		collider.enabled = false;
		
		if( autoRespawn )
			StartCoroutine( AutoRespawnRoutine() );
	}
	
	protected IEnumerator AutoRespawnRoutine()
	{
		yield return new WaitForSeconds(respawnInterval);
		
		Activate();
	}
	
	public void OnHitBy(Transform hitter)
	{	
		if( hitter == null ) // just to make debugging easier
		{
			hitter = GameObject.FindGameObjectWithTag("Ball").transform;
			hitter.position = transform.position;
		}
		
		StartCoroutine( HitRoutine(hitter) );
	}
	
	protected IEnumerator HitRoutine(Transform hitter)
	{
		hitter.rigidbody.isKinematic = true;
		
		
		Vector3 spiderStartPos = new Vector3(-58.87466f, 11.84898f, -162.2651f);//spider.parent.transform.position;
		
		//Debug.Log(Time.frameCount + "Spider parent " + spider.parent.gameObject + " -> " + spiderStartPos);
		
		/*iTween.MoveTo( spider.parent.gameObject, 
				iTween.Hash("position",new Vector3(-58.87466f, 11.84898f, -37.04593f),
							"time", 2.6f));
		*/
		
		iTween.MoveTo( spider.parent.gameObject, new Vector3(-58.87466f, 11.84898f, -37.04593f), 2.0f );
		
		spider.animation.Stop();
		spider.animation.Play("C4D Animation Take");
		
		
		yield return new WaitForSeconds(2.6f);
		
		Debug.Log(Time.frameCount + " Spider parent2 " + spider.parent.gameObject + " -> " + spiderStartPos);
		
		iTween.Stop(spider.parent.gameObject);
		
		iTween.MoveTo( spider.parent.gameObject, 
			iTween.Hash("position",spiderStartPos,
						"time", 4.0f));
		
		hitter.rigidbody.isKinematic = false;
		
		// the spiderweb is on the LEFT side of the board
		// we want it to shoot the ball out to the right in about 90 angles surrounding the 0 angle (so 45degrees in 1st and 4th quadrant)
	
		// Vector3.right is 1,0,0
		// so if we add a random y component between -1 and 1, we should get the expected behaviour...
		Vector3 direction = Vector3.right + new Vector3(0, Random.Range(-1.0f, 1.0f), 0);
		
		hitter.rigidbody.AddForce( direction * 1000 );
			
		
		//spider.animation["bounce"].speed = 0;
		
		activated = false; // make sure we don't get back into the spiderweb right away...
		
		yield return new WaitForSeconds(0.5f);
		Deactivate();
		
		//spider.animation["bounce"].speed = 1.0f;
		
		//Debug.Log("BOUNCE LENGHT : " + spider.animation["bounce"].length );
		
		yield return null;
	}
	
	void OnTriggerEnter(Collider collider)
	{
		if( !activated )
			return;
		
		
		GameObject ball = collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		OnHitBy( ball.transform );
    }
}
