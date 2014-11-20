using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepGameOverMenu : IMenuStep 
{
    protected Button RestartButton = null;
    protected Button MainMenuButton = null;
    protected TextMeshWrapper Score = null;
    protected TextMeshWrapper HighScore = null;
    protected Vector3 OriginalPosition = Vector3.zero;

	protected Button shareButton = null;
	protected Button facebookButton = null;
	protected Button twitterButton = null;
	protected Button shareBackground = null;

	protected Transform shareOverlay = null;

    protected TextMesh StarsText = null;
    protected Transform StarIcon = null;

    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;

    public GameObject StarPrefab;

	private float _challengeAnimTime = 0.6f;

	private float _starAnimTime = 1.5f;
	private float _starAnimDelay = 0.2f;
	private float _challengeReorderTime = 0.2f;
	private float _challengeFlyOffTime = 0.7f;
	private float _challengeAppearTime = 0.7f;

	private string whooshSound = "Whoosh01";

    private int _oldStars = 0;

	private Transform expBar = null;
	private TextMesh expText = null;
	private ParticleSystem expFullParticles = null;

    public GameObject ChallengePrefab;
    public List<Pair<GameObject, Challenge>> ChallengeObjects = null;

	protected ILugusCoroutineHandle _updateChallengesHandle = null;
	protected ILugusCoroutineHandle _createChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _setChallengeCompletedIconHandle = null;
	protected ILugusCoroutineHandle _giveStarsForCompletedChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _removeCompletedChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _reorderChallengeObjectsHandle = null;
	protected ILugusCoroutineHandle _addNewChallengeObjectsHandle = null;

    private bool _firstChallengeObjectsUpdate = true;

    public Sprite MissionIconCheck = null;


	public override void SetupLocal()
	{
		expBar = gameObject.FindComponentInChildren<Transform>(true, "LevelProgressBar");
		expText = gameObject.FindComponentInChildren<TextMesh>(true, "Text_Level");
		expFullParticles = gameObject.FindComponentInChildren<ParticleSystem>(true, "LevelParticle");

		shareButton = gameObject.FindComponentInChildren<Button>(true, "Button_Share");
		facebookButton = gameObject.FindComponentInChildren<Button>(true, "Button_Facebook");
		twitterButton = gameObject.FindComponentInChildren<Button>(true, "Button_Twitter");
		shareBackground = gameObject.FindComponentInChildren<Button>(true, "Share_Background");

		shareOverlay = transform.FindChild("Share");
		shareOverlay.gameObject.SetActive(false);

        if (RestartButton == null)
        {
            RestartButton = transform.FindChild("Button_Restart").GetComponent<Button>();
        }
        if (RestartButton == null)
        {
            Debug.Log("StepGameOverMenu: Missing restart button.");
        }

        if (MainMenuButton == null)
        {
            MainMenuButton = transform.FindChild("Button_MainMenu").GetComponent<Button>();
        }
        if (MainMenuButton == null)
        {
            Debug.Log("StepGameOverMenu: Missing main menu button.");
        }

        if (Score == null)
        {
            Score = transform.FindChild("Text_ScoreNr").GetComponent<TextMeshWrapper>();
        }
        if (Score == null)
        {
            Debug.Log("StepGameOverMenu: Missing score text mesh!");
        }

        if (HighScore == null)
        {
            HighScore = transform.FindChild("Text_HighScoreNr").GetComponent<TextMeshWrapper>();
        }
        if (HighScore == null)
        {
            Debug.Log("StepGameOverMenu: Missing high score text mesh!");
        }

        if (ChallengesTopTransform == null)
        {
            ChallengesTopTransform = transform.FindChild("Challenge_Top");
        }
        if (ChallengesTopTransform == null)
        {
            Debug.Log("StepGameOverMenu: Missing challenge top transform button.");
        }

        if (ChallengesBotTransform == null)
        {
            ChallengesBotTransform = transform.FindChild("Challenge_Bot");
        }
        if (ChallengesBotTransform == null)
        {
            Debug.Log("StepGameOverMenu: Missing challenge bottom transform button.");
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

        if (StarIcon == null)
        {
            StarIcon = transform.FindChild("MissionStar");
        }
        if (StarIcon == null)
        {
            Debug.Log("StepMainMenu: Missing challenge Star Icon.");
        }

		OriginalPosition = transform.position;

        ChallengeObjects = new List<Pair<GameObject, Challenge>>();
    }
	
	public void SetupGlobal()
	{
	}
	
	protected void Start () 
	{
		SetupGlobal();
	}
	
	protected void Update () 
	{
		if (!activated)
			return;
		if (LugusInput.use.KeyDown(KeyCode.Escape))
		{
			if (shareOverlay.gameObject.activeSelf)
			{
				shareOverlay.gameObject.SetActive(false);
			}
			else
			{
				SceneLoader.use.LoadNewScene("Pinball_MainMenu");
			}
		}
        else if (RestartButton.pressed)
        {
			SceneLoader.use.LoadNewScene(Application.loadedLevel);
        }
		else if (MainMenuButton.pressed)
        {
			SceneLoader.use.LoadNewScene("Pinball_MainMenu");
        } 
		else if (shareButton.pressed)
		{
			shareOverlay.gameObject.SetActive(!shareOverlay.gameObject.activeSelf);
		} 
		else if (shareBackground.pressed)
		{
			shareOverlay.gameObject.SetActive(false);
		}
		else if (facebookButton.pressed)
		{
			string levelName = Application.loadedLevelName.Replace("Pinball_", "");
			string score = ScoreManager.use.TotalScore.ToString();

			SocialShare.use.facebook.Share("I got a score of " + score + " in the " + levelName + " level. Can you do better?");
		}
		else if (twitterButton.pressed)
		{
			string levelName = Application.loadedLevelName.Replace("Pinball_", "");
			string score = ScoreManager.use.TotalScore.ToString();

			SocialShare.use.twitter.Share("I got a score of " + score + " in the " + levelName + " level. Can you do better?");
		}
	}

	public override void Activate(bool animate = true)
	{
        //activated = true;

        //gameObject.SetActive(true);

        //iTween.Stop(gameObject);

        //transform.position = OriginalPosition + new Vector3(30, 0, 0);

        //gameObject.MoveTo(OriginalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();

        //// Fill in score text.
        //Score.SetText(ScoreManager.use.TotalScore.ToString());
        //HighScore.SetText(PlayerData.use.GetHighestScore(Application.loadedLevelName).ToString());

        //// Show completed challenges.
        //int i = 0;
        //foreach (Challenge challenge in ChallengeManager.use.CompletedLvlChallenges)
        //{
        //    // Instantiate challenge object.
        //    Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position) / (PlayerData.MaxChallenges - 1) * i;
        //    GameObject challengeGameObj = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;
        //    challengeGameObj.transform.parent = gameObject.transform;

        //    // Set description.
        //    challengeGameObj.transform.FindChild("Text_Challenge").GetComponent<TextMeshWrapper>().SetText(challenge.Description);

        //    // Store challenge objects so they can be animated.
        //    ChallengeObjects.Add(new Pair<GameObject,Challenge>(challengeGameObj, challenge));

        //    ++i;
        //}

        //// Save.
        //PlayerData.use.Save();


        LugusCoroutines.use.StartRoutine(ActivateRoutine());
    }

    protected IEnumerator ActivateRoutine()
    {
        // Show menu.
        activated = true;
        gameObject.SetActive(true);

        // Stop any animations on menu.
        iTween.Stop(gameObject);

		// Note: this caused issues with the placement of individual challenges
		// Disabled since animation was barely visible.
        // Play menu animation.
        //transform.position = OriginalPosition + new Vector3(30, 0, 0);
        //gameObject.MoveTo(OriginalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();

        // Fill in score text.
        Score.SetText(ScoreManager.use.TotalScore.ToString());
        HighScore.SetText(PlayerData.use.GetHighestScore(Application.loadedLevelName).ToString());
        
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
		LugusCoroutines.use.StartRoutine(UpdateExperienceBar());
    }

	public override void Deactivate(bool animate = true)
	{
		activated = false;
		//gameObject.SetActive(false);

        // Play move away animation.
        iTween.Stop(gameObject);
        gameObject.MoveTo(OriginalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
        
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
		
		ChallengeManager.use.FillChallenges();

		PlayerData.use.Save();
    }

	private IEnumerator UpdateExperienceBar()
	{
		int oldExp = PlayerData.use.GetOldExp();
		int oldLevel = PlayerData.use.getOldLevel();
		int newExp = PlayerData.use.GetExp();
		int newLevel = PlayerData.use.GetLevel();

		float startExp = (float)oldExp/(float)PlayerData.use.CalcExpToNext(oldLevel);
		float endExp = PlayerData.use.GetExpPercentage();
		int levelUps = newLevel - oldLevel;

		expBar.localScale = new Vector3(startExp, 1, 1);
		expText.text = "Level " + oldLevel;

		float animateSpeed = 1.0f;

		float scale = expBar.localScale.x;

		int levelSound = 5;

		// Animate the bar to full for each level up.
		for (int l = 0; l < levelUps; l++)
		{
			while (scale < 1.0f)
			{
				scale += animateSpeed * Time.deltaTime;
				expBar.localScale = new Vector3(scale, 1, 1);
				yield return null;
			}
			expText.text = "Level " + (oldLevel + l + 1);
			expFullParticles.Play();

			LugusAudio.use.SFX().Play(LugusResources.use.Shared.GetAudio("OrchestraPlop0" + levelSound));

			levelSound --;
			if (levelSound < 3)
				levelSound = 3;

			scale = 0.0f;
		}

		// animate the bar to the correct percentage 
		while (scale < endExp)
		{
			scale += animateSpeed * Time.deltaTime;
			if (scale > endExp)
				scale = endExp;
			expBar.localScale = new Vector3(scale, 1, 1);
			yield return null;
		}

		yield break;
	}

	// Updates challenges and does challenge animations.
	private IEnumerator UpdateChallenges()
	{
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
				Vector3 pos = GetChallengePosition(count);
				
				// Instantiate challenge game object.
				GameObject challengeGameObj = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;
				
				// Set parent.
				challengeGameObj.transform.parent = gameObject.transform;
				challengeGameObj.transform.localScale = Vector3.one;
				
				// Update description.
				challengeGameObj.transform.FindChild("Challenge/Text_Challenge").GetComponent<TextMesh>().text = challenge.Description;
				
				// Store.
				ChallengeObjects[count] = new Pair<GameObject, Challenge>(challengeGameObj, challenge);
			}
			
			// Hide "new challenge" text.
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

		count = 0; 
		// Animate viewed challenges. + set viewed to correct position
		foreach (Pair<GameObject, Challenge> challengeObj in ChallengeObjects)
		{
			if (challengeObj.Second != null)
			{
				Debug.Log(count);
				if (challengeObj.Second.Viewed)
				{
					Debug.Log("Moving challenge " + challengeObj.Second.ID + " to index " + count);
					Vector3 destPos = GetChallengePosition(count);
					Debug.Log(destPos);
					challengeObj.First.MoveTo(destPos).Time(_challengeAnimTime).Execute();
					yield return new WaitForSeconds(_challengeAnimTime);
				} 
				else 
				{
					count --;
				}
            }
            else
            {
                // Remove challenge objects there is no challenge.
                Destroy(challengeObj.First);
				count --;
            }
			count++;
        }

		// Animate unviewed challenges.
		foreach (Pair<GameObject, Challenge> challengeObj in ChallengeObjects)
		{
			if (challengeObj.Second != null)
			{
				Debug.Log(count);
				if (!challengeObj.Second.Viewed)
				{
					Debug.Log("Adding challenge " + challengeObj.Second.ID + " to index " + count);
					// Unhide for animation.
					challengeObj.First.SetActive(true);
					
					// Set to viewed.
					challengeObj.Second.Viewed = true;
					
					// Scale of challenge object before animation;
					Vector3 oldScale = challengeObj.First.transform.localScale;
					
					// Make new challenge pop in.
					Vector3 destPos = GetChallengePosition(count);
					challengeObj.First.transform.position = destPos;
					challengeObj.First.transform.localScale = Vector3.zero;
					challengeObj.First.ScaleTo(oldScale).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeOutBack).Execute();
					
					// Wait for animation to end.
					Debug.Log("Playing challenge animation.");
					yield return new WaitForSeconds(_challengeAnimTime);
				} 
				else
				{
					count --;
				}
			}
			else
			{
				// Remove challenge objects there is no challenge.
				Destroy(challengeObj.First);
				count --;
			}
			count++;
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
}
