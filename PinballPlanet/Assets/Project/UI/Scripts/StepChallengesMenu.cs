using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepChallengesMenu : IMenuStep
{
    protected Button ChallengesButton = null;
    protected Vector3 OriginalPosition = Vector3.zero;
    public TextMeshWrapper Stars = null;

    private const int _maxChallenges = 4;
    protected List<GameObject> Challenges;
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

        if (Stars == null)
        {
            Stars = transform.FindChild("Text_Stars").GetComponent<TextMeshWrapper>();

            Stars.textMesh.text = PlayerData.use.Stars.ToString();
            PlayerData.use.ChallengesMenuStars = Stars.textMesh;
        }
        if (Stars == null)
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

        // Spawn 4 challenges.
        Challenges = new List<GameObject>();
        for (int i = 0; i < _maxChallenges; i++)
        {
            Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position)/(_maxChallenges - 1)*i;
            GameObject challenge = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;
            challenge.transform.parent = gameObject.transform;
        }

        OriginalPosition = transform.position;
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
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);
    }
}