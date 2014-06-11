using UnityEngine;
using System.Collections;

public class StepMainMenu : IMenuStep 
{
    protected Button helpButton = null;
    protected Button playButton = null;
    protected GameObject titleLogo = null;
    protected Vector3 originalPosition = Vector3.zero;

    private float _fadeTime = 0.65f;

    public void SetupLocal()
    {
        if (helpButton == null)
        {
            helpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (helpButton == null)
        {
            Debug.Log("StepMainMenu: Missing help button.");
        }

        if (playButton == null)
        {
            playButton = transform.FindChild("PlayButton").GetComponent<Button>();
        }
        if (playButton == null)
        {
            Debug.Log("StepMainMenu: Missing play button.");
        }

        if (titleLogo == null)
        {
            titleLogo = transform.FindChild("PlanetPinballLogo").gameObject;
        }
        if (titleLogo == null)
        {
            Debug.Log("StepMainMenu: Missing title logo.");
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

    protected void Start()
    {
        SetupGlobal();
    }

    protected void Update()
    {
        if (!activated)
            return;

        if (helpButton.pressed)
        {
            //MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu);
        }
        else if (playButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.LevelSelectMenu);
        }
    }


    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        helpButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);

        // Move camera to main menu position.
        Vector3 target = GameObject.Find("Camera_MainMenu").transform.position;
        Camera.main.gameObject.MoveTo(target).Time(_fadeTime).EaseType(iTween.EaseType.easeInOutQuad).Execute();

        // Pop in title.
        titleLogo.ScaleTo(Vector3.one).Time(_fadeTime).EaseType(iTween.EaseType.easeOutElastic).Execute();
    }


    public override void Deactivate(bool animate = true)
    {
        activated = false;

        helpButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);

        // Pop out title.
        titleLogo.ScaleTo(Vector3.zero).Time(_fadeTime).EaseType(iTween.EaseType.easeInCubic).Execute();

        //iTween.Stop(gameObject);
        //gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
    }
}