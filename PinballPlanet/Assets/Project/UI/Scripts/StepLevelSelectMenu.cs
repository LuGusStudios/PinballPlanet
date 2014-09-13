using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepLevelSelectMenu : IMenuStep
{
    protected Button HelpButton = null;
    protected Button BackButton = null;
    protected Button PlayButton = null;
    protected TextMesh LevelName = null;
    protected SpriteRenderer Thumbnail = null;
    protected List<Button> LevelSelectButtons;
    protected List<GameObject> Highscores;

    protected Vector3 OriginalPosition = Vector3.zero;

    private Button _selectedLevelButton = null;
    private GameObject _planet = null;

    public float RotationSpeed = 0.15f;
    public float DragSpeed = 1.0f;
    private Vector3 _prevDragPoint;

    public GameObject HighScorePrefab;

    public string _lvlName;

    public override void SetupLocal()
    {
        if (HelpButton == null)
        {
            HelpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (HelpButton == null)
        {
            Debug.Log("StepLevelSelectMenu: Missing help button.");
        }

        if (BackButton == null)
        {
            BackButton = transform.FindChild("BackButton").GetComponent<Button>();
        }
        if (BackButton == null)
        {
            Debug.Log("StepLevelSelectMenu: Missing back button.");
        }

        if (PlayButton == null)
        {
            PlayButton = transform.FindChild("PlayButton").GetComponent<Button>();
        }
        if (PlayButton == null)
        {
            Debug.Log("StepMainMenu: Missing play button.");
        }

        if (LevelName == null)
        {
            LevelName = transform.FindChild("Text_LevelName").GetComponent<TextMesh>();
        }
        if (LevelName == null)
        {
            Debug.Log("StepGameMenu: Missing level name text mesh!");
        }

        if (Thumbnail == null)
        {
            Thumbnail = transform.FindChild("Thumbnail").GetComponent<SpriteRenderer>();
        }
        if (Thumbnail == null)
        {
            Debug.Log("StepGameMenu: Missing thumbnail sprite!");
        }

        LevelSelectButtons = new List<Button>();
        // Only search these items when in main menu.
        if (Application.loadedLevelName == "Pinball_MainMenu")
        {
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
        }

        Highscores = new List<GameObject>();

        OriginalPosition = transform.position;

        // Hide.
        HideLevel();
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

        if (HelpButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.LevelSelectHelpMenu);
            HelpButton.gameObject.SetActive(false);
        }
        else if (BackButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu);
            HideLevel();
        }
        else if (PlayButton.pressed)
        {
            _lvlName = new List<string>(_selectedLevelButton.name.Split('_'))[1];
            Invoke("LoadLevel", PlayButton.clickAnimationTime);
        }
        else
        {
            foreach (Button levelButton in LevelSelectButtons)
            {
                if (levelButton.pressed)
                {
                    // Store selected.
                    _selectedLevelButton = levelButton;

                    ShowLevel();

                    // Rotate to level.
                    string levelName = new List<string>(_selectedLevelButton.name.Split('_'))[1];
                    string targetName = "Planet_Rotation_" + levelName;
                    Transform target = GameObject.Find(targetName).transform;
                    if (target != null)
                        iTween.RotateTo(_planet, iTween.Hash("rotation", target.eulerAngles, "time", 1.0f, "easetype", iTween.EaseType.easeInOutCubic));
                    else
                        Debug.LogError("--- Make sure the buttons follow the correct naming convention: Button_LevelName ---");

                    // Set level name.
                    LevelName.gameObject.SetActive(true);
                    LevelName.GetComponent<TextMesh>().text = levelName;

                    // Exit loop.
                    break;
                }
            }
        }

        // Allow dragging when no button was pressed.
        if (LugusInput.use.RayCastFromMouse() == null)
        {
            if (LugusInput.use.dragging)
            {
                if (_selectedLevelButton != null)
                {
                    _selectedLevelButton = null;
                    HideLevel();
                }
                Vector3 dragVec = LugusInput.use.lastPoint - LugusInput.use.inputPoints[LugusInput.use.inputPoints.Count - 2];
                float dragAmountX = dragVec.x / Screen.width;
                float dragAmountY = dragVec.y / Screen.height;
                _planet.transform.Rotate(Vector3.down, dragAmountX * DragSpeed, Space.World);
                _planet.transform.Rotate(Vector3.right, dragAmountY * DragSpeed, Space.World);
            }
        }

        //// Slowly rotate when no level selected and not dragging.
        //if (_selectedLevelButton == null && !LugusInput.use.dragging)
        //{
        //    _planet.transform.Rotate(Vector3.up, RotationSpeed, Space.World);
        //}
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

        HelpButton.gameObject.SetActive(true);

        // Activate level select buttons.
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.gameObject.SetActive(true);
        }

        // Move camera to level select position.
        Vector3 target = GameObject.Find("Camera_LevelSelect").transform.position;
        Camera.main.gameObject.MoveTo(target).Time(0.5f).EaseType(iTween.EaseType.easeInOutQuad).Execute();

        // Load high scores.
        if(PlayerData.use.LevelsHighscores == null)
            PlayerData.use.Load();
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);

        if (Application.loadedLevelName != "MainMenu")
            return;

        // Deactivate level select buttons.
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.gameObject.SetActive(false);
        }

        //iTween.Stop(gameObject);
        //gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
    }

    public void ShowLevel()
    {
        // Show level thumbnail.
        string levelName = new List<string>(_selectedLevelButton.name.Split('_'))[1];
        Sprite thumbnail = Resources.Load<Sprite>("Shared/UI/Level" + levelName);
        if (thumbnail != null)
            Thumbnail.sprite = thumbnail;
        else
            Debug.LogError("--- Sprite with name: Shared/UI/Level" + levelName + " not found in resources. ---");

        // Show high scores.
        int i = 0;
        Debug.Log("Showing " + PlayerData.use.LevelsHighscores["Pinball_" + levelName].Count + " high scores.");
        foreach (int score in PlayerData.use.LevelsHighscores["Pinball_" + levelName])
        {
            Debug.Log("Showing score: " + score + " of level Pinball_" + levelName);

            GameObject highscore = Instantiate(HighScorePrefab) as GameObject;
            highscore.transform.parent = gameObject.transform;
            highscore.transform.position = transform.FindChild("HighScore").position + Vector3.zero.yAdd(-0.6f * i);
            highscore.transform.FindChild("Text_Score").GetComponent<TextMesh>().text = score.ToString();
            Highscores.Add(highscore);

            ++i;
        }

        //// Show high scores
        //foreach (GameObject highscore in Highscores)
        //{
        //    highscore.SetActive(true);
        //}

        Thumbnail.gameObject.SetActive(true);
        PlayButton.gameObject.SetActive(true);

        // Put flag in right place.
        HideLevelButtonFlags();
        _selectedLevelButton.transform.FindChild("LevelSelectFlag").gameObject.SetActive(true);
    }

    public void HideLevel()
    {
        if (LevelSelectButtons.Count > 0)
            HideLevelButtonFlags();
        //_selectedLevelButton = null;

        // Destroy all highscores.
        foreach (GameObject highscore in Highscores)
        {
            Destroy(highscore);
        }
        Highscores.Clear();

        LevelName.gameObject.SetActive(false);
        Thumbnail.gameObject.SetActive(false);
        PlayButton.gameObject.SetActive(false);
    }

    void LoadLevel()
    {
        Application.LoadLevel("Pinball_" + _lvlName);
    }
}