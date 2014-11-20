using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepChallengesMenu : IMenuStep
{
    protected Button ChallengesButton = null;
    protected Button SocialButton = null;
    protected Button SettingsButton = null;
    protected Vector3 OriginalPosition = Vector3.zero;
    protected TextMesh StarsText = null;
    protected Transform StarIcon = null;

	protected Button restartButton = null;
	protected Transform restartOverlay = null;

	protected Button removeChallengeYes = null;
	protected Button removeChallengeNo = null;
	protected GameObject removeConfirmation = null;
	protected Challenge challengeToRemove = null;
	private int _removeChallengeLevelLimit = 3;
	private int _removeChallengeStarCost = 3;

    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;
    public GameObject ChallengePrefab;

    public GameObject StarPrefab;

	private float _challengeAnimTime = 0.6f;

	private bool _finishedAnimations = false;
	
	private float _starAnimTime = 1.5f;
	private float _starAnimDelay = 0.2f;
	private float _challengeReorderTime = 0.2f;
	private float _challengeFlyOffTime = 0.7f;
	private float _challengeAppearTime = 0.7f;

    private int _oldStars = 0;

	private string whooshSound = "Whoosh01";

	protected ILugusCoroutineHandle _updateChallengesHandle = null;
	protected ILugusCoroutineHandle _createChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _setChallengeCompletedIconHandle = null;
	protected ILugusCoroutineHandle _giveStarsForCompletedChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _removeCompletedChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _reorderChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _addNewChallengeObjectsHandle = null;

    public List<Pair<GameObject, Challenge>> ChallengeObjects = null;

    public Sprite MissionIconCheck = null;


    public override void SetupLocal()
    {
		restartButton = gameObject.FindComponentInChildren<Button>(true, "Button_Restart");
		restartOverlay = transform.FindChildRecursively("AllChallengesCompleted");
		restartOverlay.gameObject.SetActive(false);

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

        if (ChallengesButton == null)
        {
            ChallengesButton = transform.FindChild("Button_Trophy").GetComponent<Button>();
        }
        if (ChallengesButton == null)
        {
            Debug.Log("StepMainMenu: Missing trophy button.");
        }

        if (StarsText == null)
        {
            StarsText = transform.FindChild("Text_Stars").GetComponent<TextMesh>();

            StarsText.text = PlayerData.use.Stars.ToString();
            PlayerData.use.StarTextMeshes.Add(StarsText);
        }
        if (StarsText == null)
        {
            Debug.Log("StepGameOverMenu: Missing stars mesh!");
        }

        if (ChallengesTopTransform == null)
        {
            ChallengesTopTransform = transform.FindChild("Challenge_Top");
        }
        if (ChallengesTopTransform == null)
        {
            Debug.Log("StepMainMenu: Missing challenge top transform button.");
        }

        if (ChallengesBotTransform == null)
        {
            ChallengesBotTransform = transform.FindChild("Challenge_Bot");
        }
        if (ChallengesBotTransform == null)
        {
            Debug.Log("StepMainMenu: Missing challenge bottom transform button.");
        }

        if (StarIcon == null)
        {
            StarIcon = transform.FindChild("MissionStar");
        }
        if (StarIcon == null)
        {
            Debug.Log("StepMainMenu: Missing challenge Star Icon.");
        }

		removeChallengeYes = gameObject.FindComponentInChildren<Button>(true, "Button_Yes");
		removeChallengeNo = gameObject.FindComponentInChildren<Button>(true, "Button_No");

		removeConfirmation = transform.FindChild("RemoveConfirmation").gameObject;
		removeConfirmation.SetActive(false);

        OriginalPosition = transform.position;

        // Initialize challenge manager by calling it.
        ChallengeManager.use.enabled = true;

        // Add first challenges.
        ChallengeManager.use.FillChallenges();
        ChallengeObjects = new List<Pair<GameObject, Challenge>>();
        for (int i = 0; i < PlayerData.MaxChallenges; i++)
        {
            ChallengeObjects.Add(new Pair<GameObject, Challenge>(null, null));
        }
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

        if (ChallengesButton.pressed || LugusInput.use.KeyDown(KeyCode.Escape))
        {
            if (Application.loadedLevelName == "Pinball_MainMenu")
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
            else
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
        }
        else if (SocialButton.pressed)
        {
            //MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.ProfileMenu, false);
        }
        else if (SettingsButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
        }
		else if (restartButton.pressed)
		{
			Debug.LogError("Restarting Challenges");
			foreach(Challenge challenge in ChallengeManager.use.AllChallenges)
			{
				challenge.Completed = false;
//				if (LugusConfig.use.User.Exists(challenge.ID))
//				{
				LugusConfig.use.User.Remove("Challenge_" + challenge.ID + "_Done");
//				}
			}
			//PlayerData.use.Save();
			LugusConfig.use.SaveProfiles();
			PlayerData.use.Load();
			ChallengeManager.use.reset();
			SceneLoader.use.LoadNewScene(PlayerData.MainLvlName);
		}
		else if (removeChallengeYes.pressed)
		{
			if (challengeToRemove != null)
			{
				ChallengeManager.use.ManuallyMarkChallengeAsComplete(challengeToRemove);
				LugusCoroutines.use.StartRoutine(UpdateAfterManualEdit());
				PlayerData.use.Stars -= _removeChallengeStarCost;
				foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
				{
					if (starText != null)
						starText.text = PlayerData.use.Stars+"";
				}
			}
			removeConfirmation.SetActive(false);

		}
		else if (removeChallengeNo.pressed)
		{
			removeConfirmation.SetActive(false);
		}
		else if (_finishedAnimations) 
		{
			foreach (Pair<GameObject, Challenge> pair in ChallengeObjects)
			{
				if (pair.First.GetComponent<Button>().pressed)
				{
					// Special challenge with no way to complete it
					// Costs no stars to remove
					// Dont show confirmation
					if (pair.Second != null && pair.Second.ID == "ImpossibleChallenge")
					{
						challengeToRemove = pair.Second;
						ChallengeManager.use.ManuallyMarkChallengeAsComplete(challengeToRemove);
						LugusCoroutines.use.StartRoutine(UpdateAfterManualEdit());
					}
					else if (PlayerData.use.GetLevel() < _removeChallengeLevelLimit)
					{
						Popup newPopup = PopupManager.use.CreateBox(LugusResources.use.Localized.GetText("PowerupRemoveLowLevel"));
						newPopup.transform.localPosition = newPopup.transform.localPosition.zAdd(-10f);
						newPopup.blockInput = true;
						newPopup.boxType = Popup.PopupType.Continue;
						newPopup.onContinueButtonClicked += popupContinue;
						newPopup.Show();
					}
					else if (PlayerData.use.Stars < _removeChallengeStarCost)
					{
						Popup newPopup = PopupManager.use.CreateBox(LugusResources.use.Localized.GetText("PowerupRemoveLowLevel"));
						newPopup.transform.localPosition = newPopup.transform.localPosition.zAdd(-10f);
						newPopup.blockInput = true;
						newPopup.boxType = Popup.PopupType.Continue;
						newPopup.onContinueButtonClicked += popupContinue;
						newPopup.Show();
					}
					else {
						challengeToRemove = pair.Second;
						removeConfirmation.SetActive(true);
					}
				}
			}
		}
    }

    public override void Activate(bool animate = true)
    {
        LugusCoroutines.use.StartRoutine(ActivateRoutine());
    }

    protected IEnumerator ActivateRoutine()
    {
        activated = true;
        gameObject.SetActive(true);

		_finishedAnimations = false;

		if (removeConfirmation != null)
			removeConfirmation.SetActive(false);

        // Hide correct menu.
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

        // Only update challenges in main menu.
        if (Application.loadedLevelName == PlayerData.MainLvlName)
        {
            // Update stars.
            _oldStars = PlayerData.use.Stars;
            foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
            {
                if (challenge.Completed)
                {
                    // Add challenge stars.
                    PlayerData.use.Stars += challenge.StarsReward;
                }
            }

            // Save.
            PlayerData.use.Save();

            // Wait frame.
            yield return null;

            // Show challenges.
			Debug.LogError(ChallengeManager.use.AreAllChallengesCompleted());
			Debug.LogError(ChallengeManager.use.AllChallenges.Count);
			Debug.LogError(ChallengeManager.use.CurrentChallenges.Count);

			if (ChallengeManager.use.AreAllChallengesCompleted())            	
				ShowChallengesCompleteOverlay();
			else
				_updateChallengesHandle = LugusCoroutines.use.StartRoutine(UpdateChallenges());
        }
        else
		{
			if (ChallengeManager.use.AreAllChallengesCompleted())
				ShowChallengesCompleteOverlay();
			else				
				_updateChallengesHandle = LugusCoroutines.use.StartRoutine(UpdateChallengesInPause());
		}
    }

    public override void Deactivate(bool animate = true)
    {
		if (!activated)
			return;

        activated = false;
		restartOverlay.gameObject.SetActive(false);
        gameObject.SetActive(false);

        // Stop any update coroutines still going on.
        if (_updateChallengesHandle != null && _updateChallengesHandle.Running)
            _updateChallengesHandle.StopRoutine();

		if (_createChallengeObjectsHandle != null && _createChallengeObjectsHandle.Running)
			_createChallengeObjectsHandle.StopRoutine();

		if (_setChallengeCompletedIconHandle != null && _setChallengeCompletedIconHandle.Running)
			_setChallengeCompletedIconHandle.StopRoutine();

		if (_giveStarsForCompletedChallengeObjectsHandle != null && _giveStarsForCompletedChallengeObjectsHandle.Running)
			_giveStarsForCompletedChallengeObjectsHandle.StopRoutine();

		if (_removeCompletedChallengeObjectsHandle != null && _removeCompletedChallengeObjectsHandle.Running)
			_removeCompletedChallengeObjectsHandle.StopRoutine();

		if (_reorderChallengeObjectsHandle != null && _reorderChallengeObjectsHandle.Running)
			_reorderChallengeObjectsHandle.StopRoutine();

		if (_addNewChallengeObjectsHandle != null && _addNewChallengeObjectsHandle.Running)
			_addNewChallengeObjectsHandle.StopRoutine();

		// Destroy all remaining stars.
		foreach (GameObject star in GameObject.FindGameObjectsWithTag("ChallengeStar"))
		{
			Destroy(star);
		}

		if (Application.loadedLevelName == PlayerData.MainLvlName)
		{
			// Replace completed challenges.
			foreach (Pair<GameObject, Challenge> challengeObj in ChallengeObjects)
			{
				if (challengeObj.Second != null)
				{
					if (challengeObj.Second.Completed)
					{
						ChallengeManager.use.removeChallenge(challengeObj.Second);
						//ChallengeManager.use.ReplaceChallenge(challengeObj.Second);
						challengeObj.Second = null;
						Destroy(challengeObj.First);
					}
				}
			}
		}
		
		ChallengeManager.use.FillChallenges();
    }

	private void ShowChallengesCompleteOverlay()
	{
		restartOverlay.gameObject.SetActive(true);
	}

	// Updates challenges and does challenge animations.
	private IEnumerator UpdateChallenges()
	{

		_finishedAnimations = false;

		// Set text to value before completed challenges stars were added so that animation can be done.
		foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
		{
			if (starText != null)
				starText.text = _oldStars.ToString();
		}

		_createChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(CreateChallengeObjects());
		yield return _createChallengeObjectsHandle.Coroutine;

		yield return new WaitForSeconds(0.5f);

		_setChallengeCompletedIconHandle = LugusCoroutines.use.StartRoutine(SetChallengeCompletedIcon());
		yield return _setChallengeCompletedIconHandle.Coroutine;
		
		_giveStarsForCompletedChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(GiveStarsForCompletedChallengeObjects());
		yield return _giveStarsForCompletedChallengeObjectsHandle.Coroutine;
		
		_removeCompletedChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(RemoveCompletedChallengeObjects());
		yield return _removeCompletedChallengeObjectsHandle.Coroutine;
		
		_reorderChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(ReorderChallengeObjects());
		yield return _reorderChallengeObjectsHandle.Coroutine;
		
		_addNewChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(AddNewChallengeObjects());
		yield return _addNewChallengeObjectsHandle.Coroutine;

		// Add colliders to the key icons
		foreach (Pair<GameObject, Challenge> pair in ChallengeObjects)
		{
			if (pair.First.GetComponent<Button>() == null){
				pair.First.AddComponent<Button>();
				pair.First.GetComponent<BoxCollider>().size = new Vector3(1.3f, 1.75f, 0);
			}
		}

		// Save.
		PlayerData.use.Save();

		_finishedAnimations = true;
	}

	private IEnumerator UpdateAfterManualEdit()
	{
		_finishedAnimations = false;

		_removeCompletedChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(RemoveCompletedChallengeObjects());
		yield return _removeCompletedChallengeObjectsHandle.Coroutine;
		
		_reorderChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(ReorderChallengeObjects());
		yield return _reorderChallengeObjectsHandle.Coroutine;
		
		_addNewChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(AddNewChallengeObjects());
		yield return _addNewChallengeObjectsHandle.Coroutine;

		// Add colliders to the key icons
		foreach (Pair<GameObject, Challenge> pair in ChallengeObjects)
		{
			if (pair.First.GetComponent<Button>() == null){
				pair.First.AddComponent<Button>();
				pair.First.GetComponent<BoxCollider>().size = new Vector3(1.3f, 1.75f, 0);
			}
		}

		_finishedAnimations = true;
	}

	private IEnumerator UpdateChallengesInPause()
	{
		_createChallengeObjectsHandle = LugusCoroutines.use.StartRoutine(CreateChallengeObjects());
		yield return _createChallengeObjectsHandle.Coroutine;
	}
	
//	private IEnumerator AnimateStar(Vector3 startPos, Vector3 targetPos, int newStarCount)
//	{
//		// Create new star
//		GameObject star = Instantiate(StarPrefab) as GameObject;
//		star.transform.position = startPos;
//		
//		Vector3 midPoint = (startPos + targetPos) / 2 + new Vector3 (-0.5f, 1.0f, -5);
//		Vector3[] path = new Vector3[]{startPos, midPoint, targetPos};
//		
//		// Play star animation.
//		star.MoveTo(path).Time(_starAnimTime).EaseType(iTween.EaseType.easeInQuad).Execute();
//		
//		// Wait for animation to end.
//		yield return new WaitForSeconds(_starAnimTime);
//		
//		// Update star text mesh.
//		foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
//		{
//			if (starText != null)
//				starText.text = (newStarCount).ToString();
//		}
//		
//		// Destroy star.
//		Destroy(star);
//	}
	
	// Create challengeObjects and place them in a list based on their order in challengemanager
	private IEnumerator CreateChallengeObjects()
	{
		// Clear existing list
		foreach (Pair<GameObject, Challenge> pair in ChallengeObjects)
		{
			GameObject.Destroy(pair.First);
		};
		
		ChallengeObjects = new List<Pair<GameObject, Challenge>>();
		
		for (int i = 0; i < ChallengeManager.use.CurrentChallenges.Count; i++)
		{
			Challenge challenge = ChallengeManager.use.CurrentChallenges[i];
			Pair<GameObject, Challenge> challengePair = CreateNewChallengePair(challenge);
			ChallengeObjects.Add(challengePair);
			
			
			// TODO Determine behaviour of new flag
			
			// If challenge was marked as not viewed, mark it as viewed now and remove the "new" icon after a pause.
			GameObject newFlag = challengePair.First.transform.FindChild("Challenge/Text_New").gameObject;
			if (challenge.Viewed)
			{
				newFlag.SetActive(false);
			}
			else
			{
				challenge.Viewed = true;
				newFlag.SetActive(true);
				LugusCoroutines.use.StartRoutine( HideGameObject(newFlag, 2.0f) );
			}
		}
		
		yield break;
	}

	private IEnumerator SetChallengeCompletedIcon()
	{
		foreach (Pair<GameObject, Challenge> challenge in ChallengeObjects)
		{
			if (challenge.Second.Completed)
			{
				// Set icon sprite.
				challenge.First.transform.FindChild("Challenge/MissionIcon").GetComponent<SpriteRenderer>().sprite = MissionIconCheck;
				yield return new WaitForSeconds(0.2f);
			}
		}

		yield return new WaitForSeconds(0.5f);
	}
	
	private IEnumerator GiveStarsForCompletedChallengeObjects()
	{
		// Give stars for completed challenges.
		int count = 0;
		int newStars = _oldStars;
		foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
		{
			if (challenge.Completed)
			{
				if (ChallengeObjects[count].First == null)
					Debug.Log("*** Problem with " + challenge.ID + " object, count: " + count + " ***");
				// Set icon sprite.
				ChallengeObjects[count].First.transform.FindChild("Challenge/MissionIcon").GetComponent<SpriteRenderer>().sprite = MissionIconCheck;

				starSoundIndex = starSoundStartIndex;

				// Show stars animation.
				for (int i = 0; i < challenge.StarsReward; i++)
				{
					// create new star at the challenge and move to the starIcon position
					Vector3 startPos = ChallengeObjects[count].First.transform.position.zAdd(-5.0f);
					Vector3 endPos = StarIcon.position;
					LugusCoroutines.use.StartRoutine(AnimateStar(startPos, endPos, ++newStars));
					// Wait for animation to end.
					yield return new WaitForSeconds(_starAnimDelay);
				}
				
			}
			++count;
		}
		yield break;
	}
	
	// Find challengeObjects that are completed and animate them out of the list.
	private IEnumerator RemoveCompletedChallengeObjects()
	{
		// Loop through challengeObjects and remove the ones that are completed
		// Loop Back to front so we can safely remove
		for (int i = ChallengeObjects.Count - 1; i >= 0; --i)
		{
			if (ChallengeObjects[i].Second != null && ChallengeObjects[i].Second.Completed)
			{
				// Make old challenge fly off.
				Vector3 destPos = ChallengeObjects[i].First.transform.position.xAdd(15.0f);
				ChallengeObjects[i].First.MoveTo(destPos).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeInBack).Execute();

				LugusAudio.use.SFX().Play(LugusResources.use.Shared.GetAudio(whooshSound));

				// Wait for animation to end.
				yield return new WaitForSeconds(_challengeFlyOffTime);
				
				// Remove challenge from ChallengeManager
				ChallengeManager.use.removeChallenge(ChallengeObjects[i].Second);
				
				// Destroy game object.
				Destroy(ChallengeObjects[i].First);
				ChallengeObjects[i].First = null;
				ChallengeObjects[i].Second = null;
				ChallengeObjects.Remove(ChallengeObjects[i]);
			}
		}
		yield break;
	}
	
	// When challenges are completed, empty spaces can appear in the list. 
	// Slide challenges up to their matching index in the challengemanager to fill these holes.
	private IEnumerator ReorderChallengeObjects()
	{
		for (int i = 0; i < ChallengeManager.use.CurrentChallenges.Count; i++)
		{
			GameObject challengeGameObject = ChallengeObjects[i].First;		
			Vector3 destPos = GetChallengePosition(i);
			challengeGameObject.MoveTo(destPos).Time(_challengeAnimTime).Execute();
			yield return new WaitForSeconds(_challengeReorderTime);
		}
		yield break;
	}
	
	// Append new challenge objects at the end of the list, based on the challenges in ChallengeManager.
	// Set their icon to "new"
	private IEnumerator AddNewChallengeObjects()
	{
		// Add new challenges to the challengemanager to replace completed challenges.
		ChallengeManager.use.FillChallenges();
		
		int newChallengeCount = ChallengeManager.use.CurrentChallenges.Count;
		int challengesToAdd = newChallengeCount - ChallengeObjects.Count;
		
		if (challengesToAdd > 0)
		{
			for (int i = newChallengeCount - challengesToAdd; i < newChallengeCount; i++)
			{
				Challenge challenge = ChallengeManager.use.CurrentChallenges[i];
				Pair<GameObject, Challenge> challengePair = CreateNewChallengePair(challenge);
				ChallengeObjects.Add(challengePair);
				
				GameObject challengeGameObject = challengePair.First;
				
				Vector3 oldScale = challengeGameObject.transform.localScale;
				challengeGameObject.transform.localScale = Vector3.zero;
				challengeGameObject.ScaleTo(oldScale).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeOutBack).Execute();
				
				yield return new WaitForSeconds(_challengeAppearTime);
			}
		}
		yield break;
	}
	
	private Pair<GameObject, Challenge> CreateNewChallengePair(Challenge challenge)
	{
		
		// Set pos in window.
		Vector3 pos = GetChallengePosition(challenge);
		
		// Instantiate challenge game object and set parent
		GameObject challengeGameObj = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;
		challengeGameObj.transform.parent = gameObject.transform;
		challengeGameObj.transform.localScale = Vector3.one;
		
		// Update description.
		challengeGameObj.transform.FindChild("Challenge/Text_Challenge").GetComponent<TextMesh>().text = challenge.Description;
		
		// Add new GameObject-challenge pair to challengeObjects
		Pair<GameObject, Challenge> newChallengePair = new Pair<GameObject, Challenge>(challengeGameObj, challenge);			
		return newChallengePair;
	}

	private int starSoundStartIndex = 1;
	private int starSoundEndIndex = 5;
	private int starSoundIndex = 1;
	private string starSound = "ChallengeStar0";

	private IEnumerator AnimateStar(Vector3 startPos, Vector3 targetPos, int newStarCount)
	{
		// Create new star
		GameObject star = Instantiate(StarPrefab) as GameObject;
		star.transform.position = startPos;

		Vector3 midPoint = (startPos + targetPos) / 2 + new Vector3 (-0.5f, 1.0f, -5);
		Vector3[] path = new Vector3[]{startPos, midPoint, targetPos};

		// Play star animation.
		star.MoveTo(path).Time(_starAnimTime).EaseType(iTween.EaseType.easeInQuad).Execute();

		// Wait for animation to end.
		yield return new WaitForSeconds(_starAnimTime);

		if (starSoundIndex > starSoundEndIndex)
			starSoundIndex = starSoundEndIndex;

		LugusAudio.use.SFX().Play(LugusResources.use.Shared.GetAudio(starSound + starSoundIndex));

		starSoundIndex++;

		// Update star text mesh.
		foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
		{
			if (starText != null)
				starText.text = (newStarCount).ToString();
		}
		
		// Destroy star.
		Destroy(star);
	}

    // Updates the UI representation of the challenges.
    private IEnumerator UpdateChallengeObjects()
    {
        // Show challenges.
        int count = 0;
        foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
        {
            if (ChallengeObjects[count].Second == null)
            {
                // Set pos in window.
                Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position) / (PlayerData.MaxChallenges - 1) * count;

                // Instantiate challenge game object.
                GameObject challengeGameObj = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;

                // Set parent.
                challengeGameObj.transform.parent = gameObject.transform;

                // Update description.
                challengeGameObj.transform.FindChild("Challenge/Text_Challenge").GetComponent<TextMesh>().text = challenge.Description;

                // Store.
                ChallengeObjects[count] = new Pair<GameObject, Challenge>(challengeGameObj, challenge);
            }

            // Hide new text.
            if (ChallengeObjects[count].Second.Viewed)
            {
                ChallengeObjects[count].First.transform.FindChild("Challenge/Text_New").gameObject.SetActive(false);
            }
            else
            {
                // Hide untill animation.
                ChallengeObjects[count].First.SetActive(false);

                // Hide new after a while.  
                ChallengeObjects[count].First.transform.FindChild("Challenge/Text_New").gameObject.SetActive(true);
                LugusCoroutines.use.StartRoutine(HideGameObject(ChallengeObjects[count].First.transform.FindChild("Challenge/Text_New").gameObject, 2.0f));
            }

            ++count;
        }

        // Animate unviewed challenges.
        foreach (Pair<GameObject, Challenge> challengeObj in ChallengeObjects)
        {
            if (challengeObj.Second != null)
            {
                if (!challengeObj.Second.Viewed)
                {
                    // Unhide for animation.
                    challengeObj.First.SetActive(true);

                    // Set to viewed.
                    challengeObj.Second.Viewed = true;

                    // Scale of challenge object before animation;
                    Vector3 oldScale = challengeObj.First.transform.localScale;

                    // Make new challenge pop in.
                    Vector3 destPos = challengeObj.First.transform.position;
                    challengeObj.First.transform.localScale = Vector3.zero;
                    challengeObj.First.ScaleTo(oldScale).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeOutBack).Execute();

                    // Wait for animation to end.
                    Debug.Log("Playing challenge animation.");
                    yield return new WaitForSeconds(_challengeAnimTime);
                }
            }
            else
            {
                // Remove challenge objects there is no challenge.
                Destroy(challengeObj.First);
            }
        }
    }

	protected Vector3 GetChallengePosition(int index)
	{
		Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position) / (PlayerData.MaxChallenges - 1) * index;
		return pos;
	}
	
	protected Vector3 GetChallengePosition(Challenge challenge)
	{
		int index = ChallengeManager.use.CurrentChallenges.IndexOf(challenge);
		return GetChallengePosition(index);
	}

    // Hides a game object with a delay.
    private IEnumerator HideGameObject(GameObject target, float delay)
    {
        // Wait for delay.
        yield return new WaitForSeconds(delay);

        if (target != null)
            target.SetActive(false);
    }

	private void popupContinue(Popup sender)
	{
		// Unsubscribe from popup event and hide it.
		sender.transform.localPosition = sender.transform.localPosition.zAdd(10f);
		sender.onContinueButtonClicked -= popupContinue;
		sender.Hide();
	}
}