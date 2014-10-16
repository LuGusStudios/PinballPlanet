using UnityEngine;
using System.Collections;

public class StepOptionsMenu : IMenuStep
{
    protected Button optionsButton = null;
    protected Button TrophyButton = null;
    protected Button SocialButton = null;
	protected Button resetButton = null;
	protected Transform resetConfirmation = null;
	protected Button resetYes = null; 
	protected Button resetNo = null;
    protected Button musicCheckBox = null;
    private bool _musicChecked = true;
    protected Button effectsCheckBox = null;
    private bool _effectsChecked = true;
    protected Vector3 originalPosition = Vector3.zero;

    public Sprite CheckBoxChecked = null;
    public Sprite CheckBoxUnChecked = null;

    public override void SetupLocal()
    {
        if (SocialButton == null)
        {
            SocialButton = transform.FindChild("Button_Social").GetComponent<Button>();
        }
        if (SocialButton == null)
        {
            Debug.Log("StepMainMenu: Missing social button.");
        }

        if (optionsButton == null)
        {
            optionsButton = transform.FindChild("Button_Settings").GetComponent<Button>();
        }
        if (optionsButton == null)
        {
            Debug.Log("StepMainMenu: Missing social button.");
        }

        if (TrophyButton == null)
        {
            TrophyButton = transform.FindChild("Button_Trophy").GetComponent<Button>();
        }
        if (TrophyButton == null)
        {
            Debug.Log("StepMainMenu: Missing trophy button.");
        }

		if (resetButton == null)
		{
			resetButton = transform.FindChild("Button_Reset").GetComponent<Button>();
		}

        if (musicCheckBox == null)
        {
            musicCheckBox = transform.FindChild("CheckBox_Music").GetComponent<Button>();
        }
        if (musicCheckBox == null)
        {
            Debug.Log("StepMainMenu: Missing music checkbox.");
        }

        if (effectsCheckBox == null)
        {
            effectsCheckBox = transform.FindChild("CheckBox_Effects").GetComponent<Button>();
        }
        if (effectsCheckBox == null)
        {
            Debug.Log("StepMainMenu: Missing sound effects checkbox.");
        }

		if (resetConfirmation == null)
		{
			resetConfirmation = transform.FindChild("ResetConfirmation");
		}
		if (resetConfirmation == null)
		{
			Debug.Log("StepMainMenu: Missing resetConfirmation.");
		}
		
		if (resetYes == null)
		{
			resetYes = transform.FindChild("ResetConfirmation/Button_Yes").GetComponent<Button>();
		}
		if (resetYes == null)
		{
			Debug.Log("StepMainMenu: Missing Button Yes");
		}
		
		if (resetNo == null)
		{
			resetNo = transform.FindChild("ResetConfirmation/Button_No").GetComponent<Button>();
		}
		if (resetNo == null)
		{
			Debug.Log("StepMainMenu: Missing Button No");
		}

        originalPosition = transform.position;
    }

    public void SetupGlobal()
    {
		GetSavedSettings();
    }

	public void GetSavedSettings() 
	{
		_musicChecked = !(LugusConfig.use.System.GetBool("musicMuted", false));
		_effectsChecked = !(LugusConfig.use.System.GetBool("SFXMuted", false));

		if (_musicChecked) 
		{
			musicCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxChecked;
		}
		else 
		{
			musicCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxUnChecked;
		}

		if (_effectsChecked) 
		{
			effectsCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxChecked;
		}
		else 
		{
			effectsCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxUnChecked;
		}

	}

    protected void Start()
    {
        SetupGlobal();
    }

    protected void Update()
    {
        if (!activated)
            return;

        if (optionsButton.pressed)
        {
            //Debug.Log("--- Loaded Level: " + Application.loadedLevelName + " ---");
            if (Application.loadedLevelName == "Pinball_MainMenu")
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
            else
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
        }
        else if (SocialButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
        }
        else if (TrophyButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ChallengesMenu, false);
        }
		else if (resetButton.pressed) 
		{
			Debug.Log("Pressed reset button");
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);

			foreach (Transform child in transform) {
				child.gameObject.SetActive(false);
			}
			resetConfirmation.gameObject.SetActive(true);
		}
		else if (resetYes.pressed)
		{
			Debug.Log("Resetting");
			ResetGame();
		}
		else if (resetNo.pressed)
		{
			foreach (Transform child in transform) {
				child.gameObject.SetActive(true);
			}
			resetConfirmation.gameObject.SetActive(false);
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
		}
        else if (musicCheckBox.pressed)
        {
            if (_musicChecked)
            {
                musicCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxUnChecked;
                LugusAudio.use.Music().VolumePercentage = 0;
				LugusConfig.use.System.SetBool("musicMuted", true, true);
            }
            else
            {
                musicCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxChecked;
                LugusAudio.use.Music().VolumePercentage = 1.0f;
				LugusConfig.use.System.SetBool("musicMuted", false, true);
            }

			LugusConfig.use.System.Store();

            _musicChecked = !_musicChecked;
        }
        else if (effectsCheckBox.pressed)
        {
            if (_effectsChecked)
            {
                effectsCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxUnChecked;
                LugusAudio.use.SFX().VolumePercentage = 0;
				LugusConfig.use.System.SetBool("SFXMuted", true, true);
            }
            else
            {
                effectsCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxChecked;
                LugusAudio.use.SFX().VolumePercentage = 1.0f;
				LugusConfig.use.System.SetBool("SFXMuted", false, true);
            }

			LugusConfig.use.System.Store();

            _effectsChecked = !_effectsChecked;
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

    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        if (Application.loadedLevelName == PlayerData.MainLvlName)
        {
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu].Activate(false);
            (MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu] as StepMainMenu).DisableButtons();
        }
        else
        {
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu].Activate(false);
            (MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu] as StepPauseMenu).DisableButtons();
        }
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);
    }
}