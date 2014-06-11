using UnityEngine;

public class StepPauseMenu : IMenuStep 
{
    protected Button ResumeButton = null;
    protected Button MainMenuButton = null;
    protected Button NoButton = null;
    protected Button YesButton = null;
    protected Transform ExitConfirmation = null;
    protected Vector3 originalPosition = Vector3.zero;
	
	public void SetupLocal()
	{
        if (ResumeButton == null)
        {
            ResumeButton = transform.FindChild("Button_Resume").GetComponent<Button>();
        }
        if (ResumeButton == null)
        {
            Debug.Log("StepPauseMenu: Missing resume button.");
        }

        if (MainMenuButton == null)
        {
            MainMenuButton = transform.FindChild("Button_MainMenu").GetComponent<Button>();
        }
        if (MainMenuButton == null)
        {
            Debug.Log("StepPauseMenu: Missing main menu button.");
        }

        if (NoButton == null)
        {
            NoButton = transform.FindChild("ExitConfirmation/Button_No").GetComponent<Button>();
        }
        if (NoButton == null)
        {
            Debug.Log("StepPauseMenu: Missing no button.");
        }

        if (YesButton == null)
        {
            YesButton = transform.FindChild("ExitConfirmation/Button_Yes").GetComponent<Button>();
        }
        if (YesButton == null)
        {
            Debug.Log("StepPauseMenu: Missing yes button.");
        }

        if (ExitConfirmation == null)
        {
            ExitConfirmation = transform.FindChild("ExitConfirmation");
        }
        if (ExitConfirmation == null)
        {
            Debug.Log("StepPauseMenu: Missing exit confirmation.");
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

	protected void Start () 
	{
		SetupGlobal();
	}
	
	protected void Update () 
	{
		if (!activated)
			return;

        if (ResumeButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu);
        }
        else if (MainMenuButton.pressed)
        {
            ExitConfirmation.gameObject.SetActive(true);
            ResumeButton.gameObject.SetActive(false);
            MainMenuButton.gameObject.SetActive(false);
        }
        else if (NoButton.pressed)
        {
            ExitConfirmation.gameObject.SetActive(false);
            ResumeButton.gameObject.SetActive(true);
            MainMenuButton.gameObject.SetActive(true);
        }
        else if (YesButton.pressed)
        {
            Application.LoadLevel("MainMenu");
        }
	}


	public override void Activate(bool animate = true)
	{
		activated = true;

		gameObject.SetActive(true);

		iTween.Stop(gameObject);

		transform.position = originalPosition + new Vector3(30, 0, 0);

		gameObject.MoveTo(originalPosition).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}

	public override void Deactivate(bool animate = true)
	{
		activated = false;

		iTween.Stop(gameObject);
		gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}
}
