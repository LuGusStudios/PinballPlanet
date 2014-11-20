using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepProfileMenu : IMenuStep
{
	protected Vector3 originalPosition = Vector3.zero;
	protected Button socialButton = null;
	protected Button SettingsButton = null;
	protected Button TrophyButton = null;

	protected Button PowerupButton = null;

	private string levelPrefix = "Level ";
	

	protected SpriteRenderer powerupIcon = null;

	protected Transform levelBar = null;
	protected TextMesh txt_PlayerLevel = null;

	protected TextMesh txt_TopScore = null;
	protected TextMesh txt_TimeInGame = null;
	protected TextMesh txt_GamesPlayed = null;
	protected TextMesh txt_ChallengesCompleted = null;

	private Powerup _selectedPowerup = null;
	
	public override void SetupLocal()
	{
		// Menu buttons
		socialButton = gameObject.FindComponentInChildren<Button>(true, "Button_Social");
		SettingsButton = gameObject.FindComponentInChildren<Button>(true, "Button_Settings");
		TrophyButton = gameObject.FindComponentInChildren<Button>(true, "Button_Trophy");

		PowerupButton = gameObject.FindComponentInChildren<Button>(true, "Powerup");

		// Powerup 
		powerupIcon = gameObject.FindComponentInChildren<SpriteRenderer>(true, "PowerupIcon");

		// Level
		txt_PlayerLevel = gameObject.FindComponentInChildren<TextMesh>(true, "Text_Level");
		levelBar = gameObject.FindComponentInChildren<Transform>(true, "LevelProgressBar");

		// Stats
		txt_TopScore = gameObject.FindComponentInChildren<TextMesh>(true, "Text_Highscore");
		txt_TimeInGame = gameObject.FindComponentInChildren<TextMesh>(true, "Text_TimeInGame");
		txt_GamesPlayed = gameObject.FindComponentInChildren<TextMesh>(true, "Text_GamesPlayed");
		txt_ChallengesCompleted = gameObject.FindComponentInChildren<TextMesh>(true, "Text_ChallengesCompleted");

		txt_PlayerLevel.text = levelPrefix + PlayerData.use.GetLevel(); 
		levelBar.localScale = new Vector3(PlayerData.use.GetExpPercentage(), 1, 1);

		originalPosition = transform.position;
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

		txt_TimeInGame.text = PlayerData.use.voidGetPlaytimeString(true);

		// Keep track of powerup and update if it's changed
		if (_selectedPowerup != PowerupManager.use.GetPermanentPowerup())
		{
			UpdatePowerup();
			_selectedPowerup = PowerupManager.use.GetPermanentPowerup();
		}
		
		if (socialButton.pressed || LugusInput.use.KeyDown(KeyCode.Escape))
		{
			if (Application.loadedLevelName == "Pinball_MainMenu")
				MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
			else
				MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
		}
		else if (SettingsButton.pressed)
		{
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
		}
		else if (TrophyButton.pressed)
		{
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ChallengesMenu, false);
		} else if (PowerupButton.pressed)
		{
			transform.parent.gameObject.FindComponentInChildren<StepPowerupSelector>(true, "PowerupSelector").updatingPermanentPowerups = true;
			MenuManager.use.ActivateOverlayMenu(MenuManagerDefault.MenuTypes.PowerupSelector, false);
		}

	}

	protected void UpdatePowerup()
	{
		string iconName = "Icon_NoPower01";
		Powerup pu = PowerupManager.use.GetPermanentPowerup();

		if (pu != null)
		{
			iconName = pu.iconName;
			Debug.Log (iconName);
		}

		Debug.Log (iconName);

		powerupIcon.sprite = LugusResources.use.Shared.GetSprite("Powerups/"+iconName);
	}

	protected void UpdatePlayerStats()
	{
		txt_TopScore.text = "" + GetTopScore(); 
		txt_TimeInGame.text = PlayerData.use.voidGetPlaytimeString(true);
		txt_GamesPlayed.text = "" + PlayerData.use.numberOfGamesPlayed;
		txt_ChallengesCompleted.text = "" + GetNumberOfCompletedChallenges();
	}

	protected int GetTopScore()
	{
		int maxScore = 0;
		foreach(KeyValuePair<string, List<int>> entry in PlayerData.use.LevelsHighscores)
		{
			List<int> scores = entry.Value;
			if (scores.Count > 0) {
				int score = scores[0];
				maxScore = Mathf.Max(maxScore, score);
			}
		}
		return maxScore;
	}

	protected int GetNumberOfCompletedChallenges()
	{
		int count = 0;
		foreach(Challenge challenge in ChallengeManager.use.AllChallenges)
		{
			if (challenge.Completed)
				count++;
		}
		return count;
	}
	
	public override void Activate(bool animate = true)
	{
		activated = true;
		gameObject.SetActive(true);

		if (Application.loadedLevelName == PlayerData.MainLvlName)
		{
			MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu].Activate(false);
			(MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu] as StepMainMenu).DisableButtons();
			PowerupButton.enabled = true;
		}
		else
		{
			MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu].Activate(false);
			(MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu] as StepPauseMenu).DisableButtons();
			PowerupButton.enabled = false;
		}

		UpdatePlayerStats();
		UpdatePowerup();
	}
	
	public override void Deactivate(bool animate = true)
	{
		activated = false;
		
		gameObject.SetActive(false);
	}
}
