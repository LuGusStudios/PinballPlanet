using UnityEngine;
using System.Collections;

public class HardenAfterImpact : MonoBehaviour 
{
	void OnTriggerEnter(Collider collider)
	{
		GameObject ball = collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		StartCoroutine( EnableColliderRoutine() );
    }
	
	public IEnumerator EnableColliderRoutine()
	{
		yield return new WaitForSeconds(2.0f);
		
		collider.isTrigger = false;
	}
}
