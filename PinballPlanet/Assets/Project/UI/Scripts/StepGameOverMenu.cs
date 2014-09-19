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
    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;

    public GameObject CompletedChallengePrefab;

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

		OriginalPosition = transform.position;
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
		activated = true;

		gameObject.SetActive(true);

		iTween.Stop(gameObject);

		transform.position = OriginalPosition + new Vector3(30, 0, 0);

		gameObject.MoveTo(OriginalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();

        // Fill in score text.
        Score.SetText(ScoreManager.use.TotalScore.ToString());
        HighScore.SetText(PlayerData.use.GetHighestScore(Application.loadedLevelName).ToString());

        // Show completed challenges.
        int i = 0;
        foreach (Challenge challenge in ChallengeManager.use.CompletedLvlChallenges)
        {
            Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position) / (StepChallengesMenu.MaxChallenges - 1) * i;
            GameObject challengeGameObj = Instantiate(CompletedChallengePrefab, pos, Quaternion.identity) as GameObject;
            challengeGameObj.transform.parent = gameObject.transform;
            challengeGameObj.transform.FindChild("Text_Challenge").GetComponent<TextMeshWrapper>().SetText(challenge.Description);

            ++i;
        }

	}

	public override void Deactivate(bool animate = true)
	{
		activated = false;
		//gameObject.SetActive(false);

		iTween.Stop(gameObject);
		gameObject.MoveTo(OriginalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}
}
