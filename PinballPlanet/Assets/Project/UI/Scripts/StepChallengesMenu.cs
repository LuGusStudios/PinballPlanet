using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class StepChallengesMenu : IMenuStep
{
    protected Button ChallengesButton = null;
    protected Button SocialButton = null;
    protected Button SettingsButton = null;
    protected Vector3 OriginalPosition = Vector3.zero;
    protected TextMesh StarsText = null;

    public static int MaxChallenges = 4;
    protected Transform ChallengesTopTransform = null;
    protected Transform ChallengesBotTransform = null;
    public GameObject ChallengePrefab;

    public List<Pair<Button, Challenge>> CompletedChallengeButtons = null;

    public Sprite MissionIconCheck = null;

    public override void SetupLocal()
    {
        if (SocialButton == null)
        {
            SocialButton = transform.FindChild("Button_Social").GetComponent<Button>();
        }
        if (SocialButton == null)
        {
            Debug.Log("StepMainMenu: Missing social button.");
        }

        if (SettingsButton == null)
        {
            SettingsButton = transform.FindChild("Button_Settings").GetComponent<Button>();
        }
        if (SettingsButton == null)
        {
            Debug.Log("StepMainMenu: Missing settings button.");
        }

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

        // Add first challenges.
        int nrChallenges = ChallengeManager.use.CurrentChallenges.Count;
        if (nrChallenges < MaxChallenges)
        {
            for (int i = 0; i < MaxChallenges - nrChallenges; i++)
            {
                ChallengeManager.use.AddNewChallenge();
            }
        }
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
        else if (SocialButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.SocialMenu, false);
        }
        else if (SettingsButton.pressed)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.OptionsMenu, false);
        }

        if (CompletedChallengeButtons != null)
        {
            foreach (var buttonChallenge in CompletedChallengeButtons)
            {
                // Check if challenge button is pressed.
                if (buttonChallenge.First.pressed)
                {
                    Debug.Log("Replacing completed challenge.");

                    // Remove challenge and add new one.
                    buttonChallenge.Second.Done = true;
                    ChallengeManager.use.ReplaceChallenge(buttonChallenge.Second);

                    // Add stars.
                    PlayerData.use.Stars += buttonChallenge.Second.StarsReward;

                    // Save data.
                    PlayerData.use.Save();

                    UpdateChallenges();
                }
            }
        }
    }

    public override void Activate(bool animate = true)
    {
        LugusCoroutines.use.StartRoutine(ActivateRoutine());
    }

    private void UpdateChallenges()
    {
        // Destroy all challenge objects.
        foreach (Transform child in transform)
        {
            if (child.name == "Challenge(Clone)")
                Destroy(child.gameObject);
        }

        // Add challenges when some are empty.
        int nrChallenges = ChallengeManager.use.CurrentChallenges.Count;
        if (nrChallenges < MaxChallenges)
        {
            for (int i = 0; i < MaxChallenges - nrChallenges; i++)
            {
                ChallengeManager.use.AddNewChallenge();
            }
        }

        // Add new challenge object.
        int count = 0;
        CompletedChallengeButtons = new List<Pair<Button, Challenge>>();
        foreach (Challenge challenge in ChallengeManager.use.CurrentChallenges)
        {
            Vector3 pos = ChallengesTopTransform.position + (ChallengesBotTransform.position - ChallengesTopTransform.position) / (MaxChallenges - 1) * count;
            GameObject challengeGameObj = Instantiate(ChallengePrefab, pos, Quaternion.identity) as GameObject;
            challengeGameObj.transform.parent = gameObject.transform;
            challengeGameObj.transform.FindChild("Challenge/Text_Challenge").GetComponent<TextMesh>().text = challenge.Description;

            // Show new text.
            if (challenge.Viewed)
                challengeGameObj.transform.FindChild("Challenge/Text_New").gameObject.SetActive(false);
            else
                challenge.Viewed = true;

            if (challenge.Completed)
            {
                // Set icon sprite.
                challengeGameObj.transform.FindChild("Challenge/MissionIcon").GetComponent<SpriteRenderer>().sprite = MissionIconCheck;

                // Allow to remove completed challenges in main menu.
                if (Application.loadedLevelName == PlayerData.MainLvlName)
                {
                    // Enable button.
                    Button challengeButton = challengeGameObj.transform.FindChild("Challenge").GetComponent<Button>();
                    challengeButton.enabled = true;
                    CompletedChallengeButtons.Add(new Pair<Button, Challenge>(challengeButton, challenge));

                    // Enable animation.
                    challengeGameObj.transform.FindChild("Challenge").GetComponent<Animator>().enabled = true;
                }
            }

            ++count;
        }
    }

    public override void Deactivate(bool animate = true)
    {
        activated = false;

        gameObject.SetActive(false);
    }

    protected IEnumerator ActivateRoutine()
    {
        activated = true;
        gameObject.SetActive(true);

        // Hide correct menu.
        if (Application.loadedLevelName == PlayerData.MainLvlName)
        {
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu].Activate(false);
            (MenuManager.use.Menus[MenuManagerDefault.MenuTypes.MainMenu] as StepMainMenu).DisableButtons();
        }
        else
        {
            MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu].Activate(false);
            (MenuManager.use.Menus[MenuManagerDefault.MenuTypes.PauseMenu] as StepPauseMenu).DisableButtons();
        }

        yield return null;

        // Show challenges.
        UpdateChallenges();
    }
}