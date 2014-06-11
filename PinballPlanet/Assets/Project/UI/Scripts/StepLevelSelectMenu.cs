using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepLevelSelectMenu : IMenuStep
{
    protected Button helpButton = null;
    protected Button backButton = null;
    protected List<Button> LevelSelectButtons;
    protected Vector3 originalPosition = Vector3.zero;
    

    private Button _selectedLevelButton = null;
    private GameObject _planet = null;

    public void SetupLocal()
    {
        if (helpButton == null)
        {
            helpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (helpButton == null)
        {
            Debug.Log("StepLevelSelectMenu: Missing help button.");
        }

        if (backButton == null)
        {
            backButton = transform.FindChild("BackButton").GetComponent<Button>();
        }
        if (backButton == null)
        {
            Debug.Log("StepLevelSelectMenu: Missing back button.");
        }

        LevelSelectButtons = new List<Button>();
        foreach (GameObject levelButton in GameObject.FindGameObjectsWithTag("LevelSelectButton"))
        {
            LevelSelectButtons.Add(levelButton.GetComponent<Button>());
        }

        if (LevelSelectButtons.Count != Application.levelCount - 1)
        {
            Debug.LogWarning("The number of level select buttons does not correspond to levels in build.");
        }

        if (_planet == null)
        {
            _planet = GameObject.Find("Planet");
        }
        if (_planet == null)
        {
            Debug.Log("StepLevelSelectMenu: Can't find planet.");
        }

        originalPosition = transform.position;
    }

    public void SetupGlobal()
    {
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
        if (!activated)
            return;

        if (helpButton.pressed)
        {
            //MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu);
        }
        else if (backButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu);
        }
        else
        {
            foreach (Button levelButton in LevelSelectButtons)
            {
                if (levelButton.pressed)
                {
                    // Store selected.
                    _selectedLevelButton = levelButton;
               
                    // Rotate to level.
                    string levelName = new List<string>(_selectedLevelButton.name.Split('_'))[1];
                    Debug.Log("--- Selected " + levelName + " level ---");

                    string targetName = "Planet_Rotation_" + levelName;
                    Transform target = GameObject.Find(targetName).transform;
                    if (target != null)
                        iTween.RotateTo(_planet, iTween.Hash("rotation", target.eulerAngles, "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
                    else
                        Debug.LogError("--- Make sure the buttons follow the correct naming convention: Button_LevelName ---");

                    // Put flag in right place.
                    HideLevelButtonFlags();
                    levelButton.transform.FindChild("LevelSelectFlag").gameObject.SetActive(true);

                    // Exit loop.
                    break;
                }
            }
        }
    }

    private void HideLevelButtonFlags()
    {
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.transform.FindChild("LevelSelectFlag").gameObject.SetActive(false);
        }
    }

    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        // Activate level select buttons.
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.gameObject.SetActive(true);
        }

        // Move camera to level select position.
        Vector3 target = GameObject.Find("Camera_LevelSelect").transform.position;
        Camera.main.gameObject.MoveTo(target).Time(0.5f).EaseType(iTween.EaseType.easeInOutQuad).Execute();
    }


    public override void Deactivate(bool animate = true)
    {
        activated = false;
        gameObject.SetActive(false);

        // Deactivate level select buttons.
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.gameObject.SetActive(false);
        }


        //iTween.Stop(gameObject);
        //gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
    }
}