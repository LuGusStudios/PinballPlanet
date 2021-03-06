using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StepMainHelpMenu : IMenuStep 
{
    protected Button helpButton = null;
    protected Vector3 originalPosition = Vector3.zero;
	protected TextMesh textLite = null;

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

		textLite = gameObject.FindComponentInChildren<TextMesh>(true, "Text_Share");

		if (Version.isLite)
		{
			textLite.text = "Buy";
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

		if (helpButton.pressed || LugusInput.use.KeyDown(KeyCode.Escape))
	    {
	        MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu); 
	    }
        else if (LugusInput.use.up)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu); 
        }
	}


    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu].Activate(false);
    }


	public override void Deactivate(bool animate = true)
	{
		activated = false;
		gameObject.SetActive(false);
	}
} 
