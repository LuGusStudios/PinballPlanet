using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepLevelSelectHelpMenu : IMenuStep 
{
    protected Button helpButton = null;
    protected Vector3 originalPosition = Vector3.zero;
	
	public override void SetupLocal()
	{
        if (helpButton == null)
        {
            helpButton = transform.FindChild("HelpButton").GetComponent<Button>();
        }
        if (helpButton == null)
        {
            Debug.Log("StepGameHelpMenu: Missing help button.");
        }

		originalPosition = transform.position;
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

	    if (helpButton.pressed || LugusInput.use.up)
	    {
	        MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.LevelSelectMenu); 
	    }
	}


    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        MenuManager.use.Menus[MenuManagerDefault.MenuTypes.LevelSelectMenu].Activate(false);
    }


	public override void Deactivate(bool animate = true)
	{
		activated = false;
		gameObject.SetActive(false);
	}
} 
