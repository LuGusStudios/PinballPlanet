using UnityEngine;
using System.Collections;

public class StepSocialMenu : IMenuStep
{
    protected Button socialButton = null;
    protected Vector3 originalPosition = Vector3.zero;

    public override void SetupLocal()
    {
        if (socialButton == null)
        {
            socialButton = transform.FindChild("Button_Social").GetComponent<Button>();
        }
        if (socialButton == null)
        {
            Debug.Log("StepMainMenu: Missing social button.");
        }

        originalPosition = transform.position;
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

        if (socialButton.pressed)
        {
            if (Application.loadedLevelName == "Pinball_MainMenu")
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.MainMenu, false);
            else
                MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.PauseMenu, false);
        }
    }

    public override void Activate(bool animate = true)
    {
        activated = true;
        gameObject.SetActive(true);

        if (Application.loadedLevelName == "Pinball_MainMenu")
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu].Activate(false);
        else
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu].Activate(false);
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);
    }
}