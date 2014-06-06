using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepGameHelpMenu : IMenuStep 
{
    protected Button helpButton = null;
    protected Button pauseButton = null;
    protected Vector3 originalPosition = Vector3.zero;
	
	public void SetupLocal()
	{
        if (helpButton == null)
        {
            helpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (helpButton == null)
        {
            Debug.Log("StepGameMenu: Missing help button.");
        }

        if (pauseButton == null)
        {
            pauseButton = transform.FindChild("PauseButton").GetComponent<Button>();
        }
        if (pauseButton == null)
        {
            Debug.Log("StepGameMenu: Missing pause button.");
        }

		originalPosition = transform.position;


		//musicTrackSettings = new LugusAudioTrackSettings().Loop(true);
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

	    if (helpButton.pressed)
	    {
	        MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu); 
	    }
	    else if (pauseButton.pressed)
	    {
	        MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu); 
	    }
        else if (LugusInput.use.up)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu); 
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

        //iTween.Stop(gameObject);

        //transform.position = originalPosition + new Vector3(30, 0, 0);

        //gameObject.MoveTo(originalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();

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
		gameObject.SetActive(false);

        //iTween.Stop(gameObject);
        //gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}
} 
