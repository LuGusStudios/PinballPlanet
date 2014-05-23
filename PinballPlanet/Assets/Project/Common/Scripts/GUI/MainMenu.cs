using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	protected bool activated = false;
	protected Camera uiCamera = null;
	
	protected GameObject playButton = null;
	protected GameObject instructionsButton = null;
	protected GameObject scoreButton = null;
	protected GameObject loginButton = null;
	
	public AudioClip startAudio = null;

	// Use this for initialization
	void Start () 
	{
		uiCamera = GameObject.Find("UICamera").camera;
		
		playButton = GameObject.Find("PlayButton");
		instructionsButton = GameObject.Find("InstructionsButton");
		scoreButton = GameObject.Find("ScoreButton");
		loginButton = GameObject.Find("LoginButton");
		
		
		activated = true;
		Activate();
	}
	
	protected void OnLoggedIn()
	{
		Debug.Log("OnLoggedIn " + activated);
		
		if( !activated )
			return;
		
		Debug.Log("OnLoggedIn2");
		
		ShowLoginPopup(false);
		
		loginButton.SetActive(false);
		scoreButton.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Transform t = LugusInput.use.RayCastFromMouseDown(uiCamera);
		if( t == null )
			return;
		
		if( t.gameObject == instructionsButton )
		{
			Debug.LogError("INSTRUCTIONSBUTTON!");
			ShowInstructionsPopup(true);
		}
		else if( t.gameObject == loginButton )
		{
			string p = BrowserCommunicator.GetURL();
			
			GameObject.Find("GOD").GetComponent<KetnetController>().debugTxt = p;
			Debug.LogError(" CSHARPPARMS : " + p);
			
			string[] flashvars = p.Split('$');
			
			for( int i = 0; i < flashvars.Length; ++i )
			{
				string[] param = flashvars[i].Split('@');
				
				if( param.Length == 2 && param[0] == "url" )
				{
					Debug.LogError("URL IS " + param[1]);	
					
					BrowserCommunicator.showLoginform(param[1]);
					break;
				}
			}
			
			Debug.LogError("LOGINBUTTON! after params op te halen");
			
			//ShowLoginPopup(true);
		}
		else if( t.gameObject == scoreButton )
		{
			Debug.LogError("SCOREBUTTON!");
			ShowScorePopup(true);
		}
		else if( t.gameObject == playButton )
		{
			if( startAudio != null )
			{
				GameObject.Find("JESUS").audio.PlayOneShot( startAudio );
				Debug.Log("PLAYED STARTAUDIO");
			}
			
			//Debug.LogError("PLAYBUTTON!");
			Deactivate();
			GameObject.Find("JESUS").GetComponent<UIGameController>().ShowIngameGUI();
		}
	}
	
	public void Activate()
	{
		Time.timeScale = 1.0f;
		
		GameObject.Find("JESUS").GetComponent<UIGameController>().PauseGameFunctionality(false);
		
		activated = true;
		
		
		ShowLoginPopup(false);
		ShowInstructionsPopup(false);
		ShowScorePopup(false);
		
		transform.FindChild("ScoreButton").gameObject.SetActive(false);
		
		//Debug.LogError("MainMenu: Activate!");
		StartCoroutine( CheckForLoginLater() );
	}
	
	protected IEnumerator CheckForLoginLater()
	{
		//Debug.LogError("MainMenu: CheckForLoginLater!");
		yield return new WaitForEndOfFrame();
		//Debug.LogError("MainMenu: CheckForLoginLater!2");
		
		
		KetnetController kc = GameObject.Find("GOD").GetComponent<KetnetController>();
		kc.Reset();
		
		kc.onLoggedIn += OnLoggedIn;
		
		
		if( GameObject.Find("GOD").GetComponent<KetnetController>().logged_in )
		{
			Debug.Log("MainMenu: we're already logged in : activate scores");
			OnLoggedIn();
		}
		else
		{
			Debug.Log("MainMenu: Not logged in yet..." + GameObject.Find("GOD").GetComponent<KetnetController>().logged_in);
			
		}
		//Debug.LogError("MainMenu: CheckForLoginLater!3");
		
		yield return null;
	}
	
	protected void Deactivate()
	{
        //Debug.LogError("MainMenu: Deactivate!");
		activated = false;
		
		GameObject.Find("JESUS").GetComponent<UIGameController>().UnpauseGameFunctionality(false);
		//Time.timeScale = 1.0f;
		//Time.timeScale = 0.0f;

        gameObject.SetActive(false);
	}
	
	public void ShowInstructionsPopup(bool show)
	{
		if( show )
		{
			ShowScorePopup(false);
			ShowLoginPopup(false);
		}
		transform.FindChild("InstructionsPopup").gameObject.SetActive(show);
	}
	
	public void ShowScorePopup(bool show)
	{
		if( show )
		{
			ShowInstructionsPopup(false);
			ShowLoginPopup(false);
			
		}
        transform.FindChild("ScorePopup").gameObject.SetActive(show);
        transform.FindChild("ScoresGlobal").gameObject.SetActive(show);
        transform.FindChild("ScoresME").gameObject.SetActive(show);
		
		if( show )
		{
			KetnetController kc = GameObject.Find("GOD").GetComponent<KetnetController>();
			kc.GetLeaderboards();
			kc.GetFriendLeaderboards();
			
			kc.ShowLeaderBoards();
			kc.ShowFriendLeaderBoards();
		}
	}
	
	public void ShowLoginPopup(bool show)
	{
		if( show )
		{
			ShowScorePopup(false);
			ShowInstructionsPopup(false);
		}

	    transform.FindChild("LoginCancel").gameObject.SetActive(show);
        transform.FindChild("LoginOK").gameObject.SetActive(show);
        transform.FindChild("LoginPopup").gameObject.SetActive(show);
	}
}
