﻿using UnityEngine;
using System.Collections;

public class StepOptionsMenu : IMenuStep
{
    protected Button optionsButton = null;
    protected Button musicCheckBox = null;
    private bool _musicChecked = true;
    protected Button effectsCheckBox = null;
    private bool _effectsChecked = true;
    protected Vector3 originalPosition = Vector3.zero;

    public Sprite CheckBoxChecked = null;
    public Sprite CheckBoxUnChecked = null;

    public override void SetupLocal()
    {
        if (optionsButton == null)
        {
            optionsButton = transform.FindChild("Button_Settings").GetComponent<Button>();
        }
        if (optionsButton == null)
        {
            Debug.Log("StepMainMenu: Missing social button.");
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

        if (optionsButton.pressed)
        {
            //Debug.Log("--- Loaded Level: " + Application.loadedLevelName + " ---");
            if (Application.loadedLevelName == "Pinball_MainMenu")
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
            else
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
        }
        else if (musicCheckBox.pressed)
        {
            if (_musicChecked)
            {
                musicCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxUnChecked;
                LugusAudio.use.Music().VolumePercentage = 0;
            }
            else
            {
                musicCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxChecked;
                LugusAudio.use.Music().VolumePercentage = 100.0f;
            }

            _musicChecked = !_musicChecked;
        }
        else if (effectsCheckBox.pressed)
        {
            if (_effectsChecked)
            {
                effectsCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxUnChecked;
                LugusAudio.use.SFX().VolumePercentage = 0;
            }
            else
            {
                effectsCheckBox.GetComponent<SpriteRenderer>().sprite = CheckBoxChecked;
                LugusAudio.use.SFX().VolumePercentage = 100.0f;
            }

            _effectsChecked = !_effectsChecked;
        }
    }

    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        if (Application.loadedLevelName == "Pinball_MainMenu")
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu].Activate(false);
        else
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu].Activate(false);
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);
    }
}