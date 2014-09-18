using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepChallengesMenu : IMenuStep
{
    protected Button ChallengesButton = null;
    protected Vector3 OriginalPosition = Vector3.zero;
    protected TextMesh StarsText = null;

    public static int MaxChallenges = 4;
    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;
    public GameObject ChallengePrefab;

    public override void SetupLocal()
    {
        if (ChallengesButton == null)
        {
            ChallengesButton = transform.FindChild("Button_Trophy").GetComponent<Button>();
        }
        if (ChallengesButton == null)
        {
            Debug.Log("StepMainMenu: Missing trophy button.");
        }

        if (StarsText == null)
        {
            StarsText = transform.FindChild("Text_Stars").GetComponent<TextMesh>();

            StarsText.text = PlayerData.use.Stars.ToString();
            PlayerData.use.StarTextMeshes.Add(StarsText);
        }
        if (StarsText == null)
        {
            Debug.Log("StepGameOverMenu: Missing stars mesh!");
        }

        if (ChallengesTopTransform == null)
        {
            ChallengesTopTransform = transform.FindChild("Challenge_Top");
        }
        if (ChallengesTopTransform == null)
        {
            Debug.Log("StepMainMenu: Missing challenge top transform button.");
        }

        if (ChallengesBotTransform == null)
        {
            ChallengesBotTransform = transform.FindChild("Challenge_Bot");
        }
        if (ChallengesBotTransform == null)
        {
            Debug.Log("StepMainMenu: Missing challenge bottom transform button.");
        }

        OriginalPosition = transform.position;

        // Initialize challenge manager by calling it.
        ChallengeManager.use.enabled = true;
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

        if (ChallengesButton.pressed)
        {
            if(Application.loadedLevelName == "Pinball_MainMenu")
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

        // Show challenges.
        int i = 0;
        foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
        {
            Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position) / (MaxChallenges - 1) * i;
            GameObject challengeGameObj = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;
            challengeGameObj.transform.parent = gameObject.transform;
            challengeGameObj.transform.FindChild("Text_Challenge").GetComponent<TextMesh>().text = challenge.Description;

            ++i;
        }
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);
    }
}