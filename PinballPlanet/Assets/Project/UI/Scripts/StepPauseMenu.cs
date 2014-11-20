using UnityEngine;
using System.Collections.Generic;

public class StepPauseMenu : IMenuStep
{
    protected Button ResumeButton = null;
    protected Button MainMenuButton = null;
	protected Button RetryButton = null;
    protected Button NoButton = null;
    protected Button YesButton = null;
    protected Button SocialButton = null;
    protected Button SettingsButton = null;
    protected Button TrophyButton = null;
    protected Transform ExitConfirmation = null;
    protected Vector3 originalPosition = Vector3.zero;

    // Pause variables.
    private List<Vector3> _ballVelocities = new List<Vector3>();
    private List<FlipperNew> _flippers = new List<FlipperNew>();

    public override void SetupLocal()
    {
        if (ResumeButton == null)
        {
            ResumeButton = transform.FindChild("Button_Resume").GetComponent<Button>();
        }
        if (ResumeButton == null)
        {
            Debug.Log("StepPauseMenu: Missing resume button.");
        }

        if (MainMenuButton == null)
        {
            MainMenuButton = transform.FindChild("Button_MainMenu").GetComponent<Button>();
        }
        if (MainMenuButton == null)
        {
            Debug.Log("StepPauseMenu: Missing main menu button.");
        }

		if (RetryButton == null)
		{
			RetryButton = transform.FindChild("Button_Retry").GetComponent<Button>();
		}
		if (RetryButton == null)
		{
			Debug.Log("StepPauseMenu: Missing retry button.");
		}

        if (NoButton == null)
        {
            NoButton = transform.FindChild("ExitConfirmation/Button_No").GetComponent<Button>();
        }
        if (NoButton == null)
        {
            Debug.Log("StepPauseMenu: Missing no button.");
        }

        if (YesButton == null)
        {
            YesButton = transform.FindChild("ExitConfirmation/Button_Yes").GetComponent<Button>();
        }
        if (YesButton == null)
        {
            Debug.Log("StepPauseMenu: Missing yes button.");
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

        if (ExitConfirmation == null)
        {
            ExitConfirmation = transform.FindChild("ExitConfirmation");
        }
        if (ExitConfirmation == null)
        {
            Debug.Log("StepPauseMenu: Missing exit confirmation.");
        }

        originalPosition = transform.position;

        foreach (object flipper in GameObject.FindObjectsOfType<FlipperNew>())
        {
            _flippers.Add(flipper as FlipperNew);
        }
    }

    public void SetupGlobal()
    {
    }

    protected void Start()
    {
        SetupGlobal();
    }

    protected void Update()
    {
        if (!activated)
            return;

		if (ResumeButton.pressed || LugusInput.use.KeyDown(KeyCode.Escape))
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu);
        }
		else if (RetryButton.pressed)
		{
			SceneLoader.use.LoadNewScene(Application.loadedLevel);
		}
        else if (MainMenuButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
            ExitConfirmation.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);
			RetryButton.gameObject.SetActive(false);
        }
        else if (NoButton.pressed)
        {
            ExitConfirmation.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);
			RetryButton.gameObject.SetActive(true);
        }
        else if (YesButton.pressed)
        {
			PlayerData.use.temporaryPowerup = null;
			SceneLoader.use.LoadNewScene("Pinball_MainMenu");
        }
        else if (SocialButton.pressed)
        {
            //MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ProfileMenu, false);
            SocialButton.gameObject.SetActive(false);
			ExitConfirmation.gameObject.SetActive(false);
        }
        else if (SettingsButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
            SettingsButton.gameObject.SetActive(false);
			ExitConfirmation.gameObject.SetActive(false);
        }
        else if (TrophyButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ChallengesMenu, false);
            TrophyButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);
			ExitConfirmation.gameObject.SetActive(false);
        }
    }


    public override void Activate(bool animate = true)
    {
        activated = true;

        gameObject.SetActive(true);

        SocialButton.gameObject.SetActive(true);
        SettingsButton.gameObject.SetActive(true);
        TrophyButton.gameObject.SetActive(true);
        ResumeButton.gameObject.SetActive(true);
        MainMenuButton.gameObject.SetActive(true);
		RetryButton.gameObject.SetActive(true);

        // Pause game.
        if (Player.Exists())
        {
            Player.use.PauseGame();
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
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);

        // Unpause game.
        if (Player.Exists())
        {
            Player.use.UnpauseGame();
        }
    }

    public void DisableButtons()
    {
        SocialButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        TrophyButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        MainMenuButton.gameObject.SetActive(false);
		RetryButton.gameObject.SetActive(false);
    }
}
