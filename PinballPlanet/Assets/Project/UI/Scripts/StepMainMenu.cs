using UnityEngine;
using System.Collections;
using System;

public class StepMainMenu : IMenuStep
{
    protected Button HelpButton = null;
    protected Button PlayButton = null;
    protected Button SocialButton = null;
    protected Button SettingsButton = null;
    protected Button TrophyButton = null;
    protected GameObject TitleLogo = null;
    protected Vector3 OriginalPosition = Vector3.zero;

    private float _fadeTime = 0.65f;
    private float _rotationSpeed = 0.15f;
    private GameObject _planet = null;

    public AudioClip ThemeMusic;

    private string _messageSeenKey = "Message_Welcome_Seen";
    private string _playLockedKey = "PlayButton_Locked";

    public Sprite ChallengeIcon = null;
    public Sprite HelpIcon = null;
	public Sprite starIcon = null;


    public override void SetupLocal()
    {
        if (HelpButton == null)
        {
            HelpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (HelpButton == null)
        {
            Debug.Log("StepMainMenu: Missing help button.");
        }

        if (PlayButton == null)
        {
            PlayButton = transform.FindChild("PlayButton").GetComponent<Button>();
        }
        if (PlayButton == null)
        {
            Debug.Log("StepMainMenu: Missing play button.");
        }

        if (SocialButton == null)
        {
            SocialButton = transform.FindChild("Button_Social").GetComponent<Button>();
        }
        if (SocialButton == null)
        {
            Debug.Log("StepMainMenu: Missing social button.");
        }

        if (SettingsButton == null)
        {
            SettingsButton = transform.FindChild("Button_Settings").GetComponent<Button>();
        }
        if (SettingsButton == null)
        {
            Debug.Log("StepMainMenu: Missing settings button.");
        }

        if (TrophyButton == null)
        {
            TrophyButton = transform.FindChild("Button_Trophy").GetComponent<Button>();
        }
        if (TrophyButton == null)
        {
            Debug.Log("StepMainMenu: Missing trophy button.");
        }

        if (TitleLogo == null)
        {
            TitleLogo = transform.FindChild("PlanetPinballLogo").gameObject;
        }
        if (TitleLogo == null)
        {
            Debug.Log("StepMainMenu: Missing title logo.");
        }

        // Only search these items when in main menu.
        if (Application.loadedLevelName == "Pinball_MainMenu")
        {
            if (_planet == null)
            {
                _planet = GameObject.Find("Planet");
            }
            if (_planet == null)
            {
                Debug.Log("StepLevelSelectMenu: Can't find planet.");
            }
        }

        OriginalPosition = transform.position;
    }

    public void SetupGlobal()
    {
        // Unlock play button.
        transform.FindChild("LevelSelectLock").gameObject.SetActive(LugusConfig.use.User.GetBool(_playLockedKey, true));
    }

    protected void Start()
    {
        SetupGlobal();
        LugusAudio.use.Music().Play(ThemeMusic).Loop = true;
    }

    protected void Update()
    {
        if (!activated)
            return;

        if (HelpButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainHelpMenu, false);
            HelpButton.gameObject.SetActive(false);
        }
        else if (PlayButton.pressed)
        {
            // Show locked message.
            if (LugusConfig.use.User.GetBool(_playLockedKey, true))
            {
                Popup newPopup = PopupManager.use.CreateBox("You'll need some stars before you can play, click the challenges button.", ChallengeIcon);
                newPopup.blockInput = true;
                newPopup.boxType = Popup.PopupType.Continue;
                newPopup.onContinueButtonClicked += PlayLockedContinue;
                newPopup.Show();
            }
            else
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.LevelSelectMenu);
        }
        else if (SocialButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
        }
        else if (SettingsButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
        }
        else if (TrophyButton.pressed)
        {
            // Unlock play button.
            if (LugusConfig.use.User.GetBool(_playLockedKey, true))
            {
                LugusConfig.use.User.SetBool(_playLockedKey, false, true);
                transform.FindChild("LevelSelectLock").gameObject.SetActive(false);
            }

            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ChallengesMenu, false);
        }

        _planet.transform.Rotate(Vector3.up, _rotationSpeed, Space.World);
    }

    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        // Enable buttons.
        HelpButton.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(true);
        SocialButton.gameObject.SetActive(true);
        SettingsButton.gameObject.SetActive(true);
        TrophyButton.gameObject.SetActive(true);

        // Move camera to main menu position.
        Vector3 target = GameObject.Find("Camera_MainMenu").transform.position;
        Camera.main.gameObject.MoveTo(target).Time(_fadeTime).EaseType(iTween.EaseType.easeInOutQuad).Execute();

        TitleLogo.SetActive(true);

        // Pop in title.
        if (animate)
        {
            TitleLogo.transform.localScale = Vector3.zero;
            TitleLogo.ScaleTo(Vector3.one).Time(_fadeTime).EaseType(iTween.EaseType.easeOutElastic).Execute();
        }

        // Animate trophy button if any new or completed challenges.
        foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
        {
            if (challenge.Completed || !challenge.Viewed)
            {
                TrophyButton.gameObject.animation.Play("ButtonActive");
                break;
            }
        }

        // Show welcome message.
        if (!LugusConfig.use.User.GetBool(_messageSeenKey, false))
        {

			Popup newPopup = PopupManager.use.CreateBox(LugusResources.use.Localized.GetText("MainMenuPopupHelp"), HelpIcon);
            newPopup.blockInput = true;
            newPopup.boxType = Popup.PopupType.Continue;
            newPopup.onContinueButtonClicked += WelcomeContinue;
            newPopup.Show();
        } 

		checkDailyStars();
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;
        gameObject.SetActive(false);

        // Pop out title.
        if (animate)
            TitleLogo.ScaleTo(Vector3.zero).Time(_fadeTime).EaseType(iTween.EaseType.easeInCubic).Execute();
        else
            TitleLogo.SetActive(false);
    }

    void OnGUI()
    {
        if (LugusDebug.debug)
        {
            if (GUI.Button(new Rect(10, 50, 250, 200), "Add 10 Stars"))
                PlayerData.use.Stars += 10;

            if (GUI.Button(new Rect(10, 250, 250, 200), "Clear save data."))
            {
				ResetGame();
            }
        }
    }

	void ResetGame()
	{
		LugusConfig.use.User.ClearAllData();
		LugusConfig.use.SaveProfiles();
		PlayerData.use.Load();
		ChallengeManager.use.reset();
		SceneLoader.use.LoadNewScene(Application.loadedLevel);
	}

	protected void checkDailyStars()
	{
		DateTime today = DateTime.Now.Date;
		
		if (LugusConfig.use.User.Exists("LastLoginDate"))
		{
			DateTime lastLoginDate = DateTime.Parse( LugusConfig.use.User.GetString("LastLoginDate", today.ToString()) );
			
			int dateComparison = today.CompareTo(lastLoginDate);
			if (dateComparison > 0) // comparison is 1 if today is later
			{
				// Give daily star
				PlayerData.use.Stars = PlayerData.use.Stars + 1;
				Popup newPopup = PopupManager.use.CreateBox("Congratulations, you have earned a daily star!", starIcon);
				newPopup.blockInput = true;
				newPopup.boxType = Popup.PopupType.Continue;
				newPopup.onContinueButtonClicked += dailyStarContinue;
				newPopup.Show();
			}
		}
		else 
		{
			Debug.Log("No saved last login date yet");
		}
		
		LugusConfig.use.User.SetString("LastLoginDate", today.ToString(), true);
		LugusConfig.use.User.Store();
	}

    public void DisableButtons()
    {
        HelpButton.gameObject.SetActive(false);	
        PlayButton.gameObject.SetActive(false);
        SocialButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        TrophyButton.gameObject.SetActive(false);
    }

    private void WelcomeContinue(Popup sender)
    {
        // Unsubscribe from popup event and hide it.
        LugusConfig.use.User.SetBool(_messageSeenKey, true, true);
        sender.onContinueButtonClicked -= WelcomeContinue;
        sender.Hide();
    }

    private void PlayLockedContinue(Popup sender)
    {
        // Unsubscribe from popup event and hide it.
        sender.onContinueButtonClicked -= PlayLockedContinue;
        sender.Hide();
    }

	private void dailyStarContinue(Popup sender)
	{
		sender.onContinueButtonClicked -= dailyStarContinue;
		sender.Hide();
	}
}