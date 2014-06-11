using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : LugusSingletonExisting<MenuManagerDefault>
{
}

public class MenuManagerDefault: MonoBehaviour
{
	public Dictionary<MenuTypes, IMenuStep> menus = new Dictionary<MenuTypes, IMenuStep>();
	public Sprite backgroundSprite = null;

	protected bool firstFrame = true;

	public enum MenuTypes
	{
		NONE = -1,
		GameMenu = 1,
        GameOverMenu = 2,
        PauseMenu = 3,
        HelpGameMenu = 4,
        MainMenu = 5,
        LevelSelectMenu = 6
	}

    public MenuTypes startMenu = MenuTypes.MainMenu;

	public void SetupLocal()
	{
        StepMainMenu mainMenu = transform.FindChild("MainMenu").GetComponent<StepMainMenu>();
        if (mainMenu != null)
            menus.Add(MenuTypes.MainMenu, mainMenu);
        else
            Debug.LogError("MenuManager: Missing main menu!");

        StepLevelSelectMenu levelSelectMenu = transform.FindChild("LevelSelectMenu").GetComponent<StepLevelSelectMenu>();
        if (levelSelectMenu != null)
            menus.Add(MenuTypes.LevelSelectMenu, levelSelectMenu);
        else
            Debug.LogError("MenuManager: Missing level select menu!");
        
        StepGameMenu gameMenu = transform.FindChild("GameMenu").GetComponent<StepGameMenu>();
		if (gameMenu != null)
			menus.Add(MenuTypes.GameMenu, gameMenu);
		else
			Debug.LogError("MenuManager: Missing game menu!");

        StepGameOverMenu gameOverMenu = transform.FindChild("GameOverMenu").GetComponent<StepGameOverMenu>();
        if (gameOverMenu != null)
            menus.Add(MenuTypes.GameOverMenu, gameOverMenu);
        else
            Debug.LogError("MenuManager: Missing game over menu!");

        StepPauseMenu pauseMenu = transform.FindChild("PauseMenu").GetComponent<StepPauseMenu>();
        if (pauseMenu != null)
            menus.Add(MenuTypes.PauseMenu, pauseMenu);
        else
            Debug.LogError("MenuManager: Missing pause menu!");

        StepGameHelpMenu gameHelpMenu = transform.FindChild("HelpMenu").GetComponent<StepGameHelpMenu>();
        if (gameHelpMenu != null)
            menus.Add(MenuTypes.HelpGameMenu, gameHelpMenu);
        else
            Debug.LogError("MenuManager: Missing game help menu!");
	}
	
	public void SetupGlobal()
	{
        ActivateMenu(startMenu);
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
		if (firstFrame)
			firstFrame = false;
	}

	protected void DeactivateAllMenus()
	{
		foreach(IMenuStep step in menus.Values)
		{
			if (firstFrame)
			{
				step.Deactivate(false);
			}
			else
			{
				if (step.IsActive())
				{
					step.Deactivate(true);
				}
				else
				{
					step.Deactivate(false);
				}
			}
		}
	}

	public void ActivateMenu(MenuTypes type)
	{
		IMenuStep nextStep = null;

		if (type == MenuTypes.NONE)
		{
			//background.gameObject.SetActive(false);
			DeactivateAllMenus();
			return;
		}

		if (menus.ContainsKey(type))
		{
			nextStep = menus[type];
		}

		if (nextStep != null)
		{
			// if there is only one level, we want to bypass the level selection screen and go directly to the level
			bool proceed = true;
            //if( nextStep.GetComponent<StepLevelMenu>()!= null )
            //{
            //    proceed = !nextStep.GetComponent<StepLevelMenu>().LoadSingleLevel();
            //}

            //Debug.LogError("PROCEED " + proceed);

			if( proceed )
			{
                //if (!background.gameObject.activeSelf)
                //    background.gameObject.SetActive(true);

				DeactivateAllMenus();
				nextStep.Activate();
			}
		}
		else
		{
			Debug.LogError("MenuManagerDefault: Unknown menu!");
		}
	}

	public Transform GetChildMenu(string menuName)
	{
		if (string.IsNullOrEmpty(menuName))
		{
			Debug.LogError("MenuManagerDefault: String is empty!");
			return null;
		}

		foreach(Transform t in transform)
		{
			if (menuName == t.name)
			{
				return t;
			}
		}
		 
		Debug.LogError("MenuManagerDefault: Could not find child menu: " + menuName);
		return null;
	}
}
