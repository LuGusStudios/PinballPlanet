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

    protected TextMesh StarsText = null;
    protected Transform StarIcon = null;

    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;

    public GameObject StarPrefab;
    private float _starAnimTime = 0.4f;
    private float _starAnimDelay = 0.15f;
    private float _challengeAnimTime = 0.6f;

    private int _oldStars = 0;

    public GameObject ChallengePrefab;
    public List<Pair<GameObject, Challenge>> ChallengeObjects = null;

    protected ILugusCoroutineHandle _updateChallengesHandle = null;
    private bool _firstChallengeObjectsUpdate = true;

    public Sprite MissionIconCheck = null;


	public override void SetupLocal()
	{
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

        if (RestartButton.pressed)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else if (MainMenuButton.pressed)
        {
            Application.LoadLevel("Pinball_MainMenu");
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

        // Play menu animation.
        transform.position = OriginalPosition + new Vector3(30, 0, 0);
        gameObject.MoveTo(OriginalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();

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
    }

	public override void Deactivate(bool animate = true)
	{
		activated = false;
		//gameObject.SetActive(false);

        // Play move away animation.
        iTween.Stop(gameObject);
        gameObject.MoveTo(OriginalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
        
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
        int nrChallenges = ChallengeObjects.Count;
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
                if (ChallengeObjects[count].First == null)
                    Debug.Log("*** Problem with " + challenge.ID + " object, count: " + count + " ***");
                // Set icon sprite.
                ChallengeObjects[count].First.transform.FindChild("Challenge/MissionIcon").GetComponent<SpriteRenderer>().sprite = MissionIconCheck;

                // Show stars animation.
                for (int i = 0; i < challenge.StarsReward; i++)
                {
                    // Spawn star.
                    GameObject star = Instantiate(StarPrefab) as GameObject;
                    star.transform.position = ChallengeObjects[count].First.transform.position.zAdd(-5.0f);

                    // Play star animation.
                    star.MoveTo(StarIcon).Time(_starAnimTime).EaseType(iTween.EaseType.easeOutQuad).Execute();

                    // Wait for animation to end.
                    yield return new WaitForSeconds(_starAnimTime);

                    // Update star text mesh.
                    ++newStars;
                    foreach (TextMesh starText in PlayerData.use.StarTextMeshes)
                    {
                        if (starText != null)
                            starText.text = (newStars).ToString();
                    }

                    // Destroy star.
                    Destroy(star);
                }
            }
            ++count;
        }

        // Update completed challenges (looping backwards so we can safely remove challenge objects from list).
        for (int i = ChallengeObjects.Count - 1; i>= 0; --i)
        {
            if (ChallengeObjects[i].Second != null)
            {
                if (ChallengeObjects[i].Second.Completed)
                {
                    // Replace old with new challenge.
                    bool toRemove = ChallengeManager.use.ReplaceChallenge(ChallengeObjects[i].Second);

                    // Make old challenge fly off.
                    Vector3 destPos = ChallengeObjects[i].First.transform.position.xAdd(15.0f);
                    ChallengeObjects[i].First.MoveTo(destPos).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeInBack).Execute();

                    // Set challenge to null so that it will be updated to new one next time.
                    ChallengeObjects[i].Second = null;

                    // Wait for animation to end.
                    yield return new WaitForSeconds(_challengeAnimTime);

                    // Destroy game object.
                    Destroy(ChallengeObjects[i].First);
                    ChallengeObjects[i].First = null;

                    // Remove if challenge was not replaced with a new one.
                    if(toRemove)
                        ChallengeObjects.Remove(ChallengeObjects[i]);
                }
            }
        }

        // Fill empty challenge spots.
        ChallengeManager.use.FillChallenges();

        // Update challenge objects.
        LugusCoroutines.use.StartRoutine(UpdateChallengeObjects());
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
                challengeGameObj.transform.localScale = Vector3.one;

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
                // Hide new after a while.  
                ChallengeObjects[count].First.transform.FindChild("Challenge/Text_New").gameObject.SetActive(true);
                LugusCoroutines.use.StartRoutine(HideGameObject(ChallengeObjects[count].First.transform.FindChild("Challenge/Text_New").gameObject, 2.0f));
            }

            // First update all challenges have to animate, otherwise only new ones.
            if (_firstChallengeObjectsUpdate || (!_firstChallengeObjectsUpdate && ChallengeObjects[count].Second.Viewed == false))
            {
                // Scale of challenge object before animation;
                Vector3 oldScale = Vector3.one;

                // Make new challenge pop in.
                Vector3 destPos = ChallengeObjects[count].First.transform.position;
                ChallengeObjects[count].First.transform.localScale = Vector3.zero;
                ChallengeObjects[count].First.ScaleTo(oldScale).Time(_challengeAnimTime).EaseType(iTween.EaseType.easeOutBack).Execute();

                // Set to viewed.
                ChallengeObjects[count].Second.Viewed = true;

                // Wait for animation to end.
                yield return new WaitForSeconds(_challengeAnimTime);
            }

            ++count;
        }

        // Alternate between second and first time this function is called.
        _firstChallengeObjectsUpdate = !_firstChallengeObjectsUpdate;
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
