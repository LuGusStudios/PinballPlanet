using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepGameHelpMenu : IMenuStep 
{
    protected Button helpButton = null;
    protected Button pauseButton = null;
    protected Vector3 originalPosition = Vector3.zero;
	
	public void SetupLocal()
	{
        if (helpButton == null)
        {
            helpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (helpButton == null)
        {
            Debug.Log("StepGameHelpMenu: Missing help button.");
        }

        if (pauseButton == null)
        {
            pauseButton = transform.FindChild("PauseButton").GetComponent<Button>();
        }
        if (pauseButton == null)
        {
            Debug.Log("StepGameHelpMenu: Missing pause button.");
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

	    if (helpButton.pressed)
	    {
	        MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu); 
	    }
	    else if (pauseButton.pressed)
	    {
	        MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu); 
	    }
        else if (LugusInput.use.up)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu); 
        }
	}


    public override void Activate(bool animate = true)
    {
        activated = true;

        gameObject.SetActive(true);
    }


	public override void Deactivate(bool animate = true)
	{
		activated = false;
		gameObject.SetActive(false);

        //iTween.Stop(gameObject);
        //gameObject.MoveTo(originalPosition + new Vector3(-30, 0, 0)).Time(0.5f).EaseType(iTween.EaseType.easeOutBack).Execute();
	}
} 
