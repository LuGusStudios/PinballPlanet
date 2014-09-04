using UnityEngine;
using System.Collections;

public class ThombStone : MonoBehaviour 
{
	public delegate void OnOpen();
	public OnOpen onOpen;
	
	
	public delegate void OnClose();
	public OnClose onClose;
	
	// Use this for initialization
	void Start () 
	{
		originalRotation = transform.eulerAngles;
	}
	
	void OnCollisionEnter(Collision collision) 
	{
		GameObject ball = collision.collider.gameObject;
		if( ball.tag != "Ball" )
			return;
		
		
		Open();
    }
	
	protected Vector3 originalRotation;
	
	public void Open()
	{
		transform.eulerAngles = originalRotation;
		
		iTween.Stop(this.gameObject);
		this.collider.enabled = false;
		
		//Debug.Log("ThombStone open : " + transform.localEulerAngles + " -> " + transform.eulerAngles + " + " + transform.rotation);
		
		iTween.RotateAdd( this.gameObject, 
				iTween.Hash("amount",new Vector3(-50, 0, 0),
							"time", 2.0f/*,
							"isLocal",true*/));
		
		if( onOpen != null )
			onOpen();
	}
	
	public void Close()
	{
		//Debug.Log("ThombStone close : " + transform.localEulerAngles + " -> " + transform.eulerAngles + " + " + transform.rotation);
		
		iTween.Stop(this.gameObject);
		iTween.RotateAdd( this.gameObject, 
				iTween.Hash("amount",new Vector3(50, 0, 0),
							"time", 2.0f/*,
							"isLocal",true*/));
		
		this.collider.enabled = true;
		
		if( onClose != null )
			onClose();
	}
}
