using UnityEngine;

public class HoverButton : MonoBehaviour 
{
	protected Camera uiCamera = null;
	
	protected Vector3 normalScale;
	protected Vector3 enlargedScale;

	// Use this for initialization
	void Start () 
	{
		uiCamera = GameObject.Find("UICamera").camera;
		
		normalScale = transform.localScale;
		enlargedScale = normalScale * 1.2f;
	}
	
	protected bool enlarged = false;
	
	// Update is called once per frame
	void Update () 
	{
		//return;
		
		Transform hover = LugusInput.use.RayCastFromMouse(uiCamera);
		//if( hover == null )
		//	return;
		
		// bigger
		if( hover == this.transform && !enlarged )
		{
			enlarged = true;
			
			iTween.Stop(this.gameObject);
			Debug.Log(Time.frameCount + " HOVERING OVER " + this.name);
			
			this.transform.localScale = enlargedScale;
			
			//iTween.ScaleTo( this.gameObject, enlargedScale, 1.0f );
		}
		
		// smaller
		if( hover != this.transform && enlarged )
		{
			enlarged = false;
			
			iTween.Stop(this.gameObject);
			Debug.Log("ONMOUSEOUT " + this.name);
			
			this.transform.localScale = normalScale;
			
			//iTween.ScaleTo( this.gameObject, normalScale, 1.0f );
		}
	}
	
	
}
