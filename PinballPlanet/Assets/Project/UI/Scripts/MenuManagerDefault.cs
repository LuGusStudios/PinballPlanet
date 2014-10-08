using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : LugusSingletonExisting<MenuManagerDefault>
{
}

public class MenuManagerDefault : MonoBehaviour
{
    public Dictionary<MenuTypes, IMenuStep> Menus = new Dictionary<MenuTypes, IMenuStep>();

    protected bool firstFrame = true;

    // Design width of the GUI
    private const float _designWidth = 10.7f;
    public static float DesignWidth
    {
        get { return _designWidth; }
    }

    // Design height of the GUI.
    private const float _designHeight = 16.2f;
    public static float DesignHeight
    {
        get { return _designHeight; }
    }

    public enum MenuTypes
    {
        NONE = -1,
        GameMenu = 1,
        GameOverMenu = 2,
        PauseMenu = 3,
        HelpGameMenu = 4,
        MainMenu = 5,
        LevelSelectMenu = 6,
        SocialMenu = 7,
        OptionsMenu = 8,
        ChallengesMenu = 9,
        MainHelpMenu = 10,
        LevelSelectHelpMenu = 11,
    }

    public MenuTypes StartMenu = MenuTypes.MainMenu;
    public MenuTypes ActiveMenu = MenuTypes.NONE;

    public void SetupLocal()
    {
        StepMainMenu mainMenu = transform.FindChild("MainMenu").GetComponent<StepMainMenu>();
        if (mainMenu != null)
            Menus.Add(MenuTypes.MainMenu, mainMenu);
        else
            Debug.LogError("MenuManager: Missing main menu!");

        StepMainHelpMenu mainHelpMenu = transform.FindChild("MainHelpMenu").GetComponent<StepMainHelpMenu>();
        if (mainHelpMenu != null)
            Menus.Add(MenuTypes.MainHelpMenu, mainHelpMenu);
        else
            Debug.LogError("MenuManager: Missing main help menu!");

        StepLevelSelectMenu levelSelectMenu = transform.FindChild("LevelSelectMenu").GetComponent<StepLevelSelectMenu>();
        if (levelSelectMenu != null)
            Menus.Add(MenuTypes.LevelSelectMenu, levelSelectMenu);
        else
            Debug.LogError("MenuManager: Missing level select menu!");

        IMenuStep levelSelectHelpMenu = transform.FindChild("LevelSelectHelpMenu").GetComponent<StepLevelSelectHelpMenu>();
        if (levelSelectHelpMenu != null)
            Menus.Add(MenuTypes.LevelSelectHelpMenu, levelSelectHelpMenu);
        else
            Debug.LogError("MenuManager: Missing level select help menu!");

        StepSocialMenu socialMenu = transform.FindChild("SocialMenu").GetComponent<StepSocialMenu>();
        if (socialMenu != null)
            Menus.Add(MenuTypes.SocialMenu, socialMenu);
        else
            Debug.LogError("MenuManager: Missing social menu!");

        StepOptionsMenu optionsMenu = transform.FindChild("OptionsMenu").GetComponent<StepOptionsMenu>();
        if (optionsMenu != null)
            Menus.Add(MenuTypes.OptionsMenu, optionsMenu);
        else
            Debug.LogError("MenuManager: Missing options menu!");

        StepChallengesMenu challengesMenu = transform.FindChild("ChallengesMenu").GetComponent<StepChallengesMenu>();
        if (optionsMenu != null)
            Menus.Add(MenuTypes.ChallengesMenu, challengesMenu);
        else
            Debug.LogError("MenuManager: Missing challenges menu!");

        StepGameMenu gameMenu = transform.FindChild("GameMenu").GetComponent<StepGameMenu>();
        if (gameMenu != null)
            Menus.Add(MenuTypes.GameMenu, gameMenu);
        else
            Debug.LogError("MenuManager: Missing game menu!");

        StepGameOverMenu gameOverMenu = transform.FindChild("GameOverMenu").GetComponent<StepGameOverMenu>();
        if (gameOverMenu != null)
            Menus.Add(MenuTypes.GameOverMenu, gameOverMenu);
        else
            Debug.LogError("MenuManager: Missing game over menu!");

        StepPauseMenu pauseMenu = transform.FindChild("PauseMenu").GetComponent<StepPauseMenu>();
        if (pauseMenu != null)
            Menus.Add(MenuTypes.PauseMenu, pauseMenu);
        else
            Debug.LogError("MenuManager: Missing pause menu!");

        StepGameHelpMenu gameHelpMenu = transform.FindChild("GameHelpMenu").GetComponent<StepGameHelpMenu>();
        if (gameHelpMenu != null)
            Menus.Add(MenuTypes.HelpGameMenu, gameHelpMenu);
        else
            Debug.LogError("MenuManager: Missing game help menu!");

        foreach (MenuTypes key in Enum.GetValues(typeof(MenuTypes)))
        {
            if (key != MenuTypes.NONE)
            {
                Menus[key].SetupLocal();
                Menus[key].ScaleElements();
            }
        }
    }

    public void SetupGlobal()
    {
        ActivateMenu(StartMenu, true);
		SetSavedSettings();

        //Debug.Log("Saving");
        //LugusConfig.use.User.SetBool(Application.loadedLevelName, true, true);
        //LugusConfig.use.SaveProfiles();

        // Make sure to disable game menu in main menu.
        //if(Application.loadedLevelName == "Pinball_MainMenu")
        //    Menus[MenuTypes.GameMenu].gameObject.SetActive(true);
    }

	public void SetSavedSettings() 
	{
		if (LugusConfig.use.System.GetBool("musicMuted", false))
		{
			LugusAudio.use.Music().VolumePercentage = 0.0f;
		} 
		else 
		{
			LugusAudio.use.Music().VolumePercentage = 1.0f;
		}

		if (LugusConfig.use.System.GetBool("SFXMuted", false))
		{
			LugusAudio.use.SFX().VolumePercentage = 0.0f;
		}
		else 
		{
			LugusAudio.use.SFX().VolumePercentage = 1.0f;
		}
	}

    protected void Awake()
    {
        SetupLocal();
    }

    protected void Start()
    {
        SetupGlobal();
    }

    protected void Update()
    {
        if (firstFrame)
            firstFrame = false;
    }

    protected void DeactivateAllMenus(bool animate = true)
    {
        foreach (IMenuStep step in Menus.Values)
        {
            if (firstFrame)
            {
                step.Deactivate(false);
            }
            else
            {
                if (step.IsActive())
                {
                    step.Deactivate(animate);
                }
                else
                {
                    step.Deactivate(false);
                }
            }
        }
    }

    public void ActivateMenu(MenuTypes type, bool animate = true)
    {
        IMenuStep nextStep = null;

        if (type == MenuTypes.NONE)
        {
            DeactivateAllMenus(animate);
            return;
        }

        if (Menus.ContainsKey(type))
        {
            nextStep = Menus[type];
        }

        if (nextStep != null)
        {
            // if there is only one level, we want to bypass the level selection screen and go directly to the level
            bool proceed = true;

            if (proceed)
            {
                DeactivateAllMenus(animate);
                nextStep.Activate(animate);
                ActiveMenu = type;
            }
        }
        else
        {
            Debug.LogError("MenuManagerDefault: Unknown menu:" + type + "!");
        }
    }

    public Transform GetChildMenu(string menuName)
    {
        if (string.IsNullOrEmpty(menuName))
        {
            Debug.LogError("MenuManagerDefault: String is empty!");
            return null;
        }

        foreach (Transform t in transform)
        {
            if (menuName == t.name)
            {
                return t;
            }
        }

        Debug.LogError("MenuManagerDefault: Could not find child menu: " + menuName);
        return null;
    }

    // Relative position in GUI according to design height.
    public Vector3 CalculateRelativeUIPos(Vector3 pos)
    {
        return new Vector3(pos.x / (DesignWidth / 2.0f), pos.y / (DesignHeight / 2.0f));
    }

    // New position in GUI depending on screen ratio.
    public Vector3 CalculateUIPos(Vector3 pos)
    {
        float newXRatio = (Screen.width / (float)Screen.height);
        float oldXRatio = (DesignWidth / DesignHeight);

        pos.x *= newXRatio / oldXRatio;

        return pos;
    }

    void OnLevelWasLoaded(int level) 
    {
        Debug.Log("Resetting time scale.");
        Time.timeScale = 1.0f;
    }
}
