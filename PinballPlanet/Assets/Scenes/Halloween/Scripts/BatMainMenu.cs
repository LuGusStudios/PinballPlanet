using UnityEngine;
using System.Collections;

public class BatMainMenu : MonoBehaviour 
{
	
	public Material state1 = null;
	public Material state2 = null;
	
	public float timeBetweenStates = 0.2f;
	
	public bool animating = false;
	
	protected Vector3 targetPosition;
	protected Vector3 originalPosition;
	
	
	// Use this for initialization
	void Start () 
	{	
		//Debug.LogError("batMainMenu Start");
		StartCoroutine( AnimationRoutine() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	protected IEnumerator AnimationRoutine()
	{
		//Debug.LogError("batMainMenu animationroutine started");
		
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
}
