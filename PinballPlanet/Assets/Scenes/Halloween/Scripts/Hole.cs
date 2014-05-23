using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour 
{
	public BallShooter[] exits;
	
	public delegate void PassedThrough();
	public PassedThrough passedThrough;
	
	public bool multiBall = false;
	public GameObject ballPrefab = null;
	
	public bool activated = false;
	protected bool busy = false;

	// Use this for initialization
	void Start () 
	{
		if( multiBall && ballPrefab == null )
			Debug.LogError("Hole: ballprefab was null for multiball!");
		
		if( multiBall )
			activated = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//foreach( BallShooter exit in exits )
		//	Debug.DrawRay( exit.transform.position, exit.transform.forward * 20 );
		
		/*
		if( Input.GetKeyDown(KeyCode.H) && multiBall )
		{
			GameObject ball = GameObject.FindGameObjectWithTag("Ball");
			
			ball.transform.position = this.transform.position;
		}
		*/
		
	}
	
	public void Activate()
	{
		activated = true;
		collider.enabled = true;
	}
	
	public void Deactivate()
	{
		activated = false;
		collider.enabled = false;
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		if( !activated )
			return;
		
		GameObject ball = collision.collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		if( exits.Length == 0 )
		{
			Debug.LogError("Hole: no exits known!");
			return;
		}
		
		if( busy )
			return;
		
		busy = true;
		
		StartCoroutine( ExitBall(ball.transform) );
    }
	
	protected IEnumerator ExitBall(Transform ball)
	{
		
		
		ball.position = new Vector3(this.transform.position.x, this.transform.position.y, ball.position.z);
		collider.enabled = false;
		ball.rigidbody.isKinematic = true;
		
		yield return new WaitForSeconds(0.70f);
		
		ball.rigidbody.isKinematic = false;
		
		if( !multiBall )
		{
			BallShooter exit = exits[ Random.Range(0, exits.Length - 1) ];
			
			ball.transform.position = exit.transform.position;
			
			ball.rigidbody.velocity = Vector3.zero;
			ball.rigidbody.isKinematic = true;
			
			
			//StartCoroutine( ExitRoutine(ball, exit.gameObject) );
			exit.GetComponent<BallShooter>().ShootBall( ball.gameObject );
		}
		else
		{
			// spawn x more balls, 1 for each exit
			// keep the existing ball for exit nr. 0
			GameObject[] balls = new GameObject[ exits.Length ];
			balls[0] = ball.gameObject;
			for( int i = 1; i < exits.Length; ++i )
			{
				balls[i] = (GameObject) GameObject.Instantiate( ballPrefab );
			}
			
			for( int i = 0; i < balls.Length; ++i )
			{
				balls[i].transform.position = exits[i].transform.position;
				balls[i].rigidbody.velocity = Vector3.zero;
				balls[i].rigidbody.isKinematic = true;
				
				
				exits[i].GetComponent<BallShooter>().ShootBall( balls[i] );
				
				//StartCoroutine( ExitRoutine(balls[i], exits[i].gameObject) );
			}
			
			GameObject.Find("JESUS").GetComponent<ScoreManager>().AddBalls(4);
		}
		
				//ball.rigidbody.AddForceAtPosition(Vector3(0,ballLaunchForce,0), ball.transform.position);			
		
		//exit.
		
		
		collider.enabled = true;
		
		
		GameObject.Find("FireLogo").GetComponent<FireLogo>().Light();
		
		yield return new WaitForSeconds(2.0f);
		
		busy = false;
		
		if( passedThrough != null )
			passedThrough();
	}
	/*
	protected IEnumerator PassedThroughDelay()
	{
		yield return new WaitForSeconds(2.0f);
		
		if( passedThrough != null )
			passedThrough();
	}
	*/
}
