using UnityEngine;

public class StepPauseMenu : IMenuStep 
{
    protected Button ResumeButton = null;
    protected Button MainMenuButton = null;
    protected Button NoButton = null;
    protected Button YesButton = null;
    protected Button SocialButton = null;
    protected Button SettingsButton = null;
    protected Button TrophyButton = null;
    protected Transform ExitConfirmation = null;
    protected Vector3 originalPosition = Vector3.zero;
	
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
	}
	
	public void SetupGlobal()
	{
	}
	
	protected void Start () 
	{
		SetupGlobal();
	}
	
	protected void Update () 
	{
		if (!activated)
			return;

        if (ResumeButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu);
        }
        else if (MainMenuButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
            ExitConfirmation.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);
        }
        else if (NoButton.pressed)
        {
            ExitConfirmation.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);
        }
        else if (YesButton.pressed)
        {
            Application.LoadLevel("Pinball_MainMenu");
        }
        else if (SocialButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
            SocialButton.gameObject.SetActive(false);
        }
        else if (SettingsButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
            SettingsButton.gameObject.SetActive(false);
        }
        else if (TrophyButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ChallengesMenu, false);
            TrophyButton.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);
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
	}

	public override void Deactivate(bool animate = true)
	{
		activated = false;

        gameObject.SetActive(false);
    }
}
