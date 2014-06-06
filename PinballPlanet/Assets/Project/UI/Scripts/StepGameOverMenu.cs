using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepGameOverMenu : IMenuStep 
{
    protected Button RestartButton = null;
    protected Button MainMenuButton = null;
    protected TextMeshWrapper Score = null;
    protected TextMeshWrapper HighScore = null;
    protected Vector3 OriginalPosition = Vector3.zero;
	
	public void SetupLocal()
	{
        if (RestartButton == null)
        {
            RestartButton = transform.FindChild("Button_Restart").GetComponent<Button>();
        }
        if (RestartButton == null)
        {
            Debug.Log("StepGameMenu: Missing restart button.");
        }

        if (MainMenuButton == null)
        {
            MainMenuButton = transform.FindChild("Button_MainMenu").GetComponent<Button>();
        }
        if (MainMenuButton == null)
        {
            Debug.Log("StepGameMenu: Missing main menu button.");
        }

        if (Score == null)
        {
            Score = transform.FindChild("Text_ScoreNr").GetComponent<TextMeshWrapper>();
        }
        if (Score == null)
        {
            Debug.Log("StepGameMenu: Missing score text mesh!");
        }

        if (HighScore == null)
        {
            HighScore = transform.FindChild("Text_HighScoreNr").GetComponent<TextMeshWrapper>();
        }
        if (HighScore == null)
        {
            Debug.Log("StepGameMenu: Missing high score text mesh!");
        }

		OriginalPosition = transform.position;
	}
	
	public void SetupGlobal()
	{
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start () 
	{
		SetupGlobal();
	}
	
	protected void Update () 
	{
		if (!activated)
			return;

        if (RestartButton.pressed)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else if (MainMenuButton.pressed)
        {
            Application.LoadLevel("MainMenu");
        }
	}

	protected void LoadLevelData()
	{
		//LugusResources.use.ChangeLanguage("nl");

		// TO DO: Set data about levels here (name, description, etc.)
		string key = Application.loadedLevelName + ".main.";
	
        //title.SetText(LugusResources.use.Levels.GetText(key + "title"));
        //description.SetText(LugusResources.use.Levels.GetText(key + "description"));

        //Sprite imageSprite = null;

        //if (LugusResources.use.Levels.HasText(key + "image"))
        //{
        //    imageSprite = LugusResources.use.Shared.GetSprite(LugusResources.use.Levels.GetText(key + "image"));
        //}
        //else
        //{
        //    imageSprite = LugusResources.use.Shared.GetSprite( Application.loadedLevelName + "_Main_Image");
        //}

        //if (imageSprite == null || imageSprite == LugusResources.use.errorSprite)
        //{
        //    image.gameObject.SetActive(false);
        //}
        //else
        //{
        //    image.sprite = imageSprite;
        //    image.gameObject.SetActive(true);
        //}
	
	}

	public override void Activate(bool animate = true)
	{
		activated = true;

		gameObject.SetActive(true);
		LoadLevelData();

		iTween.Stop(gameObject);

		transform.position = OriginalPosition + new Vector3(30, 0, 0);

		gameObject.MoveTo(OriginalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();

        // Fill in score text.
        Score.SetText(ScoreManager.use.TotalScore.ToString());
        HighScore.SetText(ScoreManager.use.TotalScore.ToString());

		//LugusCoroutines.use.StartRoutine(MusicLoop());
	}

    //protected IEnumerator MusicLoop()
    //{
    //    LugusAudio.use.Music().StopAll();
    //    LugusAudio.use.Music().Play(LugusResources.use.Shared.GetAudio("MenuIntro01"));

    //    while ( LugusAudio.use.Music().IsPlaying )
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
	
    //    LugusAudio.use.Music().Play(LugusResources.use.Shared.GetAudio("MenuLoop01"), true, musicTrackSettings);
    //}

	public override void Deactivate(bool animate = true)
	{
		activated = false;
		//gameObject.SetActive(false);

		iTween.Stop(gameObject);
		gameObject.MoveTo(OriginalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}
}
