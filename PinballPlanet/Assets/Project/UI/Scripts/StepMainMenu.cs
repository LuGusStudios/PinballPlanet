using UnityEngine;
using System.Collections;

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
                LugusConfig.use.User.ClearAllData();
                LugusConfig.use.SaveProfiles();
                Application.LoadLevel(Application.loadedLevel);
            }
        }
    }

    public void DisableButtons()
    {
        HelpButton.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(false);
        SocialButton.gameObject.SetActive(false);
        SettingsButton.gameObject.SetActive(false);
        TrophyButton.gameObject.SetActive(false);
    }
}