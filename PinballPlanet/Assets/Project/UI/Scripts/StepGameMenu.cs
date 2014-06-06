using UnityEngine;

public class StepGameMenu : IMenuStep
{
    protected Button pauseButton = null;
    protected Button helpButton = null;
    protected TextMeshWrapper totalScore = null;
    protected TextMeshWrapper ballsLeft = null;
    protected Vector3 originalPosition = Vector3.zero;
    protected Transform launchHelp = null;

    public void SetupLocal()
    {
        if (pauseButton == null)
        {
            pauseButton = transform.FindChild("PauseButton").GetComponent<Button>();
        }
        if (pauseButton == null)
        {
            Debug.Log("StepGameMenu: Missing pause button.");
        }

        if (helpButton == null)
        {
            helpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (helpButton == null)
        {
            Debug.Log("StepGameMenu: Missing help button.");
        }

        if (totalScore == null)
        {
            totalScore = transform.FindChild("Text_TotalScore").GetComponent<TextMeshWrapper>();
        }
        if (totalScore == null)
        {
            Debug.Log("StepGameMenu: Missing total score text mesh!");
        }

        if (ballsLeft == null)
        {
            ballsLeft = transform.FindChild("Text_BallCount").GetComponent<TextMeshWrapper>();
        }
        if (ballsLeft == null)
        {
            Debug.Log("StepGameMenu: Missing balls count text mesh!");
        }

        if (launchHelp == null)
        {
            launchHelp = transform.FindChild("LaunchHelp");
        }
        if (launchHelp == null)
        {
            Debug.Log("StepGameMenu: Missing launch help!");
        }

        originalPosition = transform.position;

        // Activate launch help
        launchHelp.gameObject.SetActive(true);

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

        if (pauseButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu);
        }
        else if (helpButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.HelpGameMenu);
        }
        // Disable launch help.
        if (LugusInput.use.up)
        {
            launchHelp.gameObject.SetActive(false);
        }
    }

    public override void Activate(bool animate = true)
    {
        activated = true;

        gameObject.SetActive(true);

        pauseButton.scaleDownFactor = 0.9f;
        helpButton.scaleDownFactor = 0.9f;
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        pauseButton.scaleDownFactor = 1;
        helpButton.scaleDownFactor = 1;
    }
}
