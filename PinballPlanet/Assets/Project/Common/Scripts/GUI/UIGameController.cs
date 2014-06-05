using UnityEngine;

public class UIGameController : MonoBehaviour 
{
	public GameObject pauzeMenu = null;
	public MainMenu mainMenu = null;
	
	public GameObject ingameGUI = null;
	
	public GameObject gameOverGUI = null;
	public AudioClip gameOverSound = null;
	
	public void PauseGameFunctionality()
	{
		PauseGameFunctionality(true);
	}
	
	public void UnpauseGameFunctionality()
	{
		UnpauseGameFunctionality(true);
	}
	
	public void PauseGameFunctionality(bool setTimescale)
	{
		if( setTimescale )
		{
			Time.timeScale = 0.0f;
			return;
		}
		
		//Debug.LogError("PAUZING THE GAME");
		
		GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
		foreach( GameObject ball in balls )
			ball.GetComponent<Ball>().OnPause();
		
        Player.use.OnPause();
	}
	
	public void UnpauseGameFunctionality(bool setTimescale)
	{
		if( setTimescale )
		{
			Time.timeScale = 1.0f;
			return;
		}
		
        //Debug.LogError("UNPAUZING THE GAME");
		
		GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
		foreach( GameObject ball in balls )
		{
            Debug.Log("Ball name: " + ball.name);
            ball.GetComponent<Ball>().OnUnpause();
		}
        Player.use.OnUnpause();
    }
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetKeyDown(KeyCode.Escape) )
		{
			//GameObject pauzeMenu = GameObject.Find("MenuPauze");
			//if( ! )
			
			ShowPauzeMenu();
		}

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
            Time.timeScale = 1.0f;
        }

		//if( Input.GetKeyDown(KeyCode.G) )
		//	ShowGameoverGUI();
	}
	
	public void ShowPauzeMenu()
	{
        if (!mainMenu.gameObject.activeInHierarchy && !gameOverGUI.gameObject.activeInHierarchy)
		{
            if (pauzeMenu.activeInHierarchy)
				UnpauseGameFunctionality();//Time.timeScale = 1.0f;
			else
				PauseGameFunctionality(); //Time.timeScale = 0.0f;

            pauzeMenu.SetActive(!pauzeMenu.activeInHierarchy);
		}
	}
	
	/*
	public void ShowMainMenu()
	{
		pauzeMenu.SetActiveRecursively(false);
		ingameGUI.SetActiveRecursively(false);
		gameOverGUI.SetActiveRecursively(false);
		
		mainMenu.gameObject.SetActiveRecursively(true);
		mainMenu.Activate();
	}
	*/
	
	public void ShowIngameGUI()
	{
		ingameGUI.SetActive(true);
	}

    public void ShowHelpGUI()
    {
        ingameGUI.SetActive(true);
    }

	public void ShowGameoverGUI()
	{
		if( gameOverSound != null )
			audio.PlayOneShot( gameOverSound );
		
		PauseGameFunctionality();//Time.timeScale = 0.0f;
		
		ingameGUI.SetActive(false);

		pauzeMenu.SetActive(false);
		
		//GameObject.Find("TotalScore").GetComponent<TextMesh>().text = "" + ScoreManager.use.GetTotalScore();
		
		gameOverGUI.SetActive(true);
		
        //KetnetController kc = GameObject.Find("GOD").GetComponent<KetnetController>();
        //kc.GetLeaderboards();
        //kc.GetFriendLeaderboards();
		
        //kc.ShowLeaderBoards();
        //kc.ShowFriendLeaderBoards();
		
	}
	
	/*
	public void ResetGame()
	{
		GameObject.Find("Player").SendMessage("Reset");
		GameObject.Find("JESUS").GetComponent<ScoreManager>().Reset();
	}
	*/
}
