using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepLevelSelectMenu : IMenuStep
{
    protected Button HelpButton = null;
    protected Button BackButton = null;
    protected Button PlayButton = null;
    protected Button LockButton = null;
    protected TextMesh LevelName = null;
    protected SpriteRenderer Thumbnail = null;
    protected List<Button> LevelSelectButtons;
    protected List<GameObject> Highscores = new List<GameObject>();
    protected TextMesh StarsText = null;
    protected GameObject StarCost = null;
    protected TextMesh StarCostText = null;

	protected Transform noLevelSelected = null;

    protected Vector3 OriginalPosition = Vector3.zero;

    private Button _selectedLevelButton = null;
    private GameObject _planet = null;

    public float RotationSpeed = 0.15f;
    public float DragSpeed = 1.0f;

    public GameObject HighScorePrefab;

    public string _lvlName;

    private string _messageLvlUnlockKey = "Message_LvlUnlock_Seen";
    public Sprite StarIcon = null;

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

        if (LockButton == null)
        {
            LockButton = transform.FindChild("LockButton").GetComponent<Button>();
        }
        if (LockButton == null)
        {
            Debug.Log("StepMainMenu: Missing lock button.");
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

        if (StarsText == null)
        {
            StarsText = transform.FindChild("Text_Stars").GetComponent<TextMesh>();

            StarsText.text = PlayerData.use.Stars.ToString();
            PlayerData.use.StarTextMeshes.Add(StarsText);
        }
        if (StarsText == null)
        {
            Debug.Log("StepGameOverMenu: Missing stars text mesh!");
        }

        if (StarCost == null)
        {
            StarCost = transform.FindChild("StarCost").gameObject;
        }
        if (StarCost == null)
        {
            Debug.Log("StepGameOverMenu: Missing star cost game object!");
        }

        if (StarCostText == null)
        {
            StarCostText = transform.FindChild("StarCost/Text_Cost").GetComponent<TextMesh>();
        }
        if (StarCostText == null)
        {
            Debug.Log("StepGameOverMenu: Missing star cost text mesh!");
        }

		if (noLevelSelected == null)
		{
			noLevelSelected = transform.FindChild("NoLevelSelected");
		}
		if (noLevelSelected == null)
		{
			Debug.Log("StepGameOverMenu: Missing noLevelSelected!");
		}

		noLevelSelected.gameObject.SetActive(false);

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

        // Store original position.
        OriginalPosition = transform.position;

        // Turn of unneeded objects.
        PlayButton.gameObject.SetActive(false);
        LockButton.gameObject.SetActive(false);
        StarCost.SetActive(false);
        Thumbnail.gameObject.SetActive(false);

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
			if (_selectedLevelButton == null)
			{
				noLevelSelected.gameObject.SetActive(true);

				LevelName.text = "Level";
				LevelName.gameObject.SetActive(true);

				for (int i = 0; i < 3; i++)
				{
					GameObject highscore = Instantiate(HighScorePrefab) as GameObject;
					highscore.transform.parent = gameObject.transform;
					highscore.transform.position = transform.FindChild("HighScore").position + Vector3.zero.yAdd(-0.6f * i);
					highscore.transform.FindChild("Text_Score").GetComponent<TextMesh>().text = ""+(12000 - i * 1000);
					Highscores.Add(highscore);
				}
			}
        }
		else if (BackButton.pressed || LugusInput.use.KeyDown(KeyCode.Escape))
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu);
            HideLevel();
        }
        else if (PlayButton.pressed)
        {
            Invoke("LoadLevel", PlayButton.clickAnimationTime);
        }
        else if (LockButton.pressed)
        {
            // Unlock level if player has enough stars.
            if (PlayerData.use.Stars >= PlayerData.use.UnlockCost)
            {
                Debug.Log("Unlocked " + _lvlName);

                // Remove stars.
                PlayerData.use.Stars -= PlayerData.use.UnlockCost;

                // Set level to unlocked.
                PlayerData.use.LevelsUnlocked["Pinball_" + _lvlName] = true;

				// If there are empty challenges, fill them with this level's unique challenges.

                // Show play button.
                PlayButton.gameObject.SetActive(true);

                // Hide lock.
                LockButton.gameObject.SetActive(false);
                StarCost.SetActive(false);
                _selectedLevelButton.transform.FindChild("LevelSelectLock").gameObject.SetActive(false);

                // Build thumbnail name.
                string thumbnailPath = "Shared/UI/Level" + _lvlName;
                Sprite thumbnail = Resources.Load<Sprite>(thumbnailPath);
                Thumbnail.sprite = thumbnail;

                // Save data.
                PlayerData.use.Save();

				ChallengeManager.use.FillChallenges();
            }
        }
        else
        {
            // Check if level is selected.
            foreach (Button levelButton in LevelSelectButtons)
            {
                if (levelButton.pressed)
                {
                    // Store selected.
                    _selectedLevelButton = levelButton;
                    _lvlName = new List<string>(_selectedLevelButton.name.Split('_'))[1];

                    // Show level info.
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

                    break;
                }
            }
        }

        // Allow rotating world when dragging except when mouse is dragging over a button.
        if (LugusInput.use.RayCastFromMouse() == null)
        {
            if (LugusInput.use.dragging)
            {
                // Hide level info.
                if (_selectedLevelButton != null)
                {
                    _selectedLevelButton = null;
                    HideLevel();
                }

                // Stop any iTweens animations on planet.
                iTween.Stop(_planet);

                // Calculate rotation amount.
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
		noLevelSelected.gameObject.SetActive(false);

		if (_selectedLevelButton == null)
		{
			// Destroy all highscores.
			foreach (GameObject highscore in Highscores)
			{
				Destroy(highscore);
			}
			Highscores.Clear();

			LevelName.gameObject.SetActive(false);
		}

        // Activate level select buttons.
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.gameObject.SetActive(true);

            // Show lock if level is not yet unlocked.
            //Debug.Log("Showing lock of level " + levelButton.name.Split('_')[1] + ": " + PlayerData.use.LevelsUnlocked["Pinball_" + levelButton.name.Split('_')[1]]);
            levelButton.transform.FindChild("LevelSelectLock").gameObject.SetActive(!PlayerData.use.LevelsUnlocked["Pinball_" + levelButton.name.Split('_')[1]]);
        }

        // Move camera to level select position.
        Vector3 target = GameObject.Find("Camera_LevelSelect").transform.position;
        Camera.main.gameObject.MoveTo(target).Time(0.5f).EaseType(iTween.EaseType.easeInOutQuad).Execute();

        // Show welcome message.
        if (!LugusConfig.use.User.GetBool(_messageLvlUnlockKey, false))
        {
            Popup newPopup = PopupManager.use.CreateBox("Spend stars to unlock a level of your choice. When you earn more stars, you can unlock another one!", StarIcon);
            newPopup.blockInput = true;
            newPopup.boxType = Popup.PopupType.Continue;
            newPopup.onContinueButtonClicked += LvlUnlockContinue;
            newPopup.Show();
        }
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);

        if (Application.loadedLevelName != "Pinball_MainMenu")
            return;

        // Deactivate level select buttons.
        foreach (Button levelButton in LevelSelectButtons)
        {
            levelButton.gameObject.SetActive(false);
            levelButton.transform.FindChild("LevelSelectLock").gameObject.SetActive(true);
        }

        //iTween.Stop(gameObject);
        //gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
    }

    public void ShowLevel()
    {
        // Get level name.
        string levelName = new List<string>(_selectedLevelButton.name.Split('_'))[1];
     
        // Show high scores.
        int i = 0;
        //Debug.Log("Showing " + PlayerData.use.LevelsHighscores["Pinball_" + levelName].Count + " high scores.");
        foreach (int score in PlayerData.use.LevelsHighscores["Pinball_" + levelName])
        {
            //Debug.Log("Showing score: " + score + " of level Pinball_" + levelName);

            GameObject highscore = Instantiate(HighScorePrefab) as GameObject;
            highscore.transform.parent = gameObject.transform;
            highscore.transform.position = transform.FindChild("HighScore").position + Vector3.zero.yAdd(-0.6f * i);
            highscore.transform.FindChild("Text_Score").GetComponent<TextMesh>().text = score.ToString();
            Highscores.Add(highscore);

            ++i;
        }
        
        // Build thumbnail name.
        string thumbnailPath = "Shared/UI/Level" + levelName;

        // Show either play button or unlock button.
        if (PlayerData.use.LevelsUnlocked["Pinball_" + levelName])
        {
            PlayButton.gameObject.SetActive(true);

        }
        else
        {
            LockButton.gameObject.SetActive(true);
            StarCost.SetActive(true);
            StarCostText.text = "-" + PlayerData.use.UnlockCost;
            thumbnailPath += "_Locked";
        }

        // Show thumbnail picture.
        Sprite thumbnail = Resources.Load<Sprite>(thumbnailPath);
        if (thumbnail != null)
            Thumbnail.sprite = thumbnail;
        else
            Debug.LogError("--- Sprite with name: Shared/UI/Level" + levelName + " not found in resources. ---");

        Thumbnail.gameObject.SetActive(true);

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
        LockButton.gameObject.SetActive(false);
        StarCost.SetActive(false);
    }

    void LoadLevel()
    {
		SceneLoader.use.LoadNewScene("Pinball_" + _lvlName);
    }

    private void LvlUnlockContinue(Popup sender)
    {
        // Unsubscribe from popup event and hide it.
        LugusConfig.use.User.SetBool(_messageLvlUnlockKey, true, true);
        sender.onContinueButtonClicked -= LvlUnlockContinue;
        sender.Hide();
    }
}