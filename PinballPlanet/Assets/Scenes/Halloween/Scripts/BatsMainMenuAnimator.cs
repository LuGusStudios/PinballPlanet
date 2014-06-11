using UnityEngine;
using System.Collections;

public class BatsMainMenuAnimator : MonoBehaviour 
{
	
	public Vector3 originalPosition;
	public Vector3 targetPosition = new Vector3(103.184f, 525.9459f, -70.03711f);
	
	// x between 103 and 436
	
	public AudioClip batSound = null;
	
	public MainMenu mainMenu = null;

	// Use this for initialization
	void Start () 
	{
		originalPosition = transform.position;
		
		mainMenu = GameObject.Find("MenuMain").GetComponent<MainMenu>();
		
		StartCoroutine( AnimationRoutine() );
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	protected IEnumerator AnimationRoutine()
	{
		while( mainMenu.gameObject.activeInHierarchy )
		{
			yield return new WaitForSeconds(2.0f);

            if (!mainMenu.gameObject.activeInHierarchy)
				yield return null;
			
			targetPosition = new Vector3( Random.Range(103, 436) , targetPosition.y, targetPosition.z);
			
			iTween.Stop(this.gameObject);
			iTween.MoveTo( this.gameObject, targetPosition, 2.0f );
			
			GameObject.Find("JESUS").audio.PlayOneShot(batSound);
			GameObject.Find("JESUS").audio.PlayOneShot(batSound);
			GameObject.Find("JESUS").audio.PlayOneShot(batSound);
			
			yield return new WaitForSeconds(5.0f);
			
			originalPosition = new Vector3( Random.Range(103, 436) , originalPosition.y, originalPosition.z);
			transform.position = originalPosition;
		}
		
		yield return null;
	}
}
