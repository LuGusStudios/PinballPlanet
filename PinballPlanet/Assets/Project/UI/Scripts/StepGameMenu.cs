using UnityEngine;

public class StepGameMenu : IMenuStep
{
    protected Button pauseButton = null;
    protected Button helpButton = null;
    protected TextMeshWrapper totalScore = null;
    protected TextMeshWrapper ballsLeft = null;
    protected Vector3 originalPosition = Vector3.zero;
    protected Transform launchHelp = null;
    protected Button StarButton = null;

    public GameObject StarPrefab = null;
    public int StarsEarned = 0;

    public override void SetupLocal()
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
            totalScore = transform.FindChild("ScorePlane/Text_TotalScore").GetComponent<TextMeshWrapper>();
        }
        if (totalScore == null)
        {
            Debug.Log("StepGameMenu: Missing total score text mesh!");
        }

        if (ballsLeft == null)
        {
            ballsLeft = transform.FindChild("BallsLeftPlane/Text_BallCount").GetComponent<TextMeshWrapper>();
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
            helpButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
        }
        else if (helpButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.HelpGameMenu);
            helpButton.gameObject.SetActive(false);
        }

        // Disable launch help.
        if (LugusInput.use.up)
        {
            launchHelp.gameObject.SetActive(false);
        }

        // Give star when enough points are earned.
        if (ScoreManager.use.TotalScore / PlayerData.use.ScorePerStar > StarsEarned)
        {
            ++StarsEarned;
            SpawnStar();
        }
    }

    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        helpButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);

        pauseButton.scaleDownFactor = 0.9f;
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        if (pauseButton != null) 
            pauseButton.scaleDownFactor = 1;
    }

    public void ShowLaunchHelp(bool show)
    {
        launchHelp.gameObject.SetActive(show);
    }

    public void SpawnStar()
    {
        Debug.Log("Spawning Star");

        GameObject star = Instantiate(StarPrefab) as GameObject;
        StarButton = star.transform.GetChild(0).GetComponent<Button>();
        star.transform.parent = transform;
        star.transform.localPosition = Vector3.zero.zAdd(-2.0f);
    }
}
