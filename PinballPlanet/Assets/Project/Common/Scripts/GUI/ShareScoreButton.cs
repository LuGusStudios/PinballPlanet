using UnityEngine;

public class ShareScoreButton : MonoBehaviour 
{
	public Material SuccesMaterial = null;

	protected Camera UICamera = null;

	// Use this for initialization
	void Start () 
	{
		UICamera = GameObject.Find("UICamera").camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Transform t = LugusInput.use.RayCastFromMouseDown(UICamera);
		
		if( t == this.transform || Input.GetKeyDown(KeyCode.P) )
		{
			ShareScore();
		}
	}
	
	public void ShareScore()
	{
		int score = GameObject.Find("JESUS").GetComponent<ScoreManager>().TotalScore;
		KetnetController kc = GameObject.Find("GOD").GetComponent<KetnetController>();
		
		kc.onStatusAdded += GoToSuccess;
		kc.AddStatus(score);
	}
	
	public void GoToSuccess()
	{
		renderer.material = SuccesMaterial;
	}
}
