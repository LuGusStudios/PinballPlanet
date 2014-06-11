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
	
	public void SetupLocal()
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

		OriginalPosition = transform.position;
	}
	
	public void SetupGlobal()
	{
	}
	
	protected void Awake()
	{
		SetupLocal();
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
            Application.LoadLevel("MainMenu");
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
        HighScore.SetText(ScoreManager.use.TotalScore.ToString());
	}

	public override void Deactivate(bool animate = true)
	{
		activated = false;
		//gameObject.SetActive(false);

		iTween.Stop(gameObject);
		gameObject.MoveTo(OriginalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}
}
