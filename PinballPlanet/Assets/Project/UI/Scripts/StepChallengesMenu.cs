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

    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;
    public GameObject ChallengePrefab;

    public GameObject StarPrefab;
    private float _starAnimTime = 1.5f;
    private float _starAnimDelay = 0.2f;
    private float _challengeAnimTime = 0.6f;

    private int _oldStars = 0;

    protected ILugusCoroutineHandle _updateChallengesHandle = null;

    public List<Pair<GameObject, Challenge>> ChallengeObjects = null;

    public Sprite MissionIconCheck = null;


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

        if (ChallengesButton.pressed)
        {
            if (Application.loadedLevelName == "Pinball_MainMenu")
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
            else
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
        }
        else if (SocialButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
        }
        else if (SettingsButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
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
            _updateChallengesHandle = LugusCoroutines.use.StartRoutine(UpdateChallenges());
        }
        else
            LugusCoroutines.use.StartRoutine(UpdateChallengeObjects());
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;
        gameObject.SetActive(false);

        // Replace completed challenges.
        foreach (Pair<GameObject, Challenge> challengeObj in ChallengeObjects)
        {
            if (challengeObj.Second != null)
            {
                if (challengeObj.Second.Completed)
                {
                    ChallengeManager.use.ReplaceChallenge(challengeObj.Second);
                    challengeObj.Second = null;
                    Destroy(challengeObj.First);
                }
            }
        }

        // Destroy all remaining stars.
        foreach (GameObject star in GameObject.FindGameObjectsWithTag("ChallengeStar"))
        {
            Destroy(star);
        }

        // Stop any update coroutines still going on.
        if (_updateChallengesHandle != null && _updateChallengesHandle.Running)
            _updateChallengesHandle.StopRoutine();
    }

    // Updates challenges and does challenge animations.
    private IEnumerator UpdateChallenges()
    {
        // Fill challenges when some are empty.
        ChallengeManager.use.FillChallenges();
        int nrChallenges = ChallengeManager.use.CurrentChallenges.Count;
        if (nrChallenges < PlayerData.MaxChallenges)
        {
            for (int i = 0; i < PlayerData.MaxChallenges - nrChallenges; i++)
            {
                ChallengeObjects.Add(new Pair<GameObject, Challenge>(null, null));
            }
        }

        // Set text to value before completed challenges stars were added so that animation can be done.
        foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
        {
            if (starText != null)
                starText.text = _oldStars.ToString();
        }

        // Update challenge objects.
        yield return LugusCoroutines.use.StartRoutine(UpdateChallengeObjects()).Coroutine;

        // Give stars for completed challenges.
        int count = 0;
        int newStars = _oldStars;
        foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
        {
            if (challenge.Completed)
            {
                // Set icon sprite.
                ChallengeObjects[count].First.transform.FindChild("Challenge/MissionIcon").GetComponent<SpriteRenderer>().sprite = MissionIconCheck;

                // Show stars animation.
                for (int i = 0; i < challenge.StarsReward; i++)
                {
					// create new star at the challenge and move to the starIcon position
					Vector3 startPos = ChallengeObjects[count].First.transform.position.zAdd(-5.0f);
					Vector3 endPos = StarIcon.position;
					LugusCoroutines.use.StartRoutine(AnimateStar(startPos, endPos, ++newStars));
					// Wait for animation to end.
					yield return new WaitForSeconds(_starAnimDelay);

//                    // Spawn star.
//                    GameObject star = Instantiate(StarPrefab) as GameObject;
//                    star.transform.position = ChallengeObjects[count].First.transform.position.zAdd(-5.0f);
//
//                    // Play star animation.
//                    star.MoveTo(StarIcon).Time(_starAnimTime).EaseType(iTween.EaseType.easeOutQuad).Execute();
//
//                    // Wait for animation to end.
//                    yield return new WaitForSeconds(_starAnimDelay);
//
//                    // Update star text mesh.
//                    ++newStars;
//                    foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
//                    {
//                        if (starText != null)
//                            starText.text = (newStars).ToString();
//                    }
//
//                    // Destroy star.
//                    Destroy(star);
                }
            }

            ++count;
        }

        // Update completed challenges.
        count = 0;
        foreach (Pair<GameObject, Challenge> challengeObj in ChallengeObjects)
        {
            if (challengeObj.Second != null)
            {
                if (challengeObj.Second.Completed)
                {
                    // Replace old with new challenge.
                    ChallengeManager.use.ReplaceChallenge(challengeObj.Second);

                    // Make old challenge fly off.
                    Vector3 destPos = challengeObj.First.transform.position.xAdd(15.0f);
                    challengeObj.First.MoveTo(destPos).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeInBack).Execute();

                    // Set challenge to null so that it will be updated to new one next time.
                    challengeObj.Second = null;

                    // Wait for animation to end.
                    yield return new WaitForSeconds(_challengeAnimTime);

                    // Destroy game object.
                    Destroy(challengeObj.First);
                    challengeObj.First = null;
                }
            }
            ++count;
        }

        // Update challenge objects.
        LugusCoroutines.use.StartRoutine(UpdateChallengeObjects());
    }

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

    // Hides a game object with a delay.
    private IEnumerator HideGameObject(GameObject target, float delay)
    {
        // Wait for delay.
        yield return new WaitForSeconds(delay);

        if (target != null)
            target.SetActive(false);
    }
}