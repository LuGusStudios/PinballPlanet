using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChallengeManager : MonoBehaviour
{
    // Singleton
    private static ChallengeManager _instance = null;
    public static ChallengeManager use
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ChallengeManager)) as ChallengeManager;

                if (_instance == null)
                {
                    _instance = new GameObject("ChallengeManager").AddComponent<ChallengeManager>();
                    DontDestroyOnLoad(_instance.gameObject);
                }

                // Load data if not yet done.
                _instance.SetupLocal();
            }

            return _instance;
        }
    }

    // List of current challenges.
    public List<Challenge> CurrentChallenges = new List<Challenge>();

    // Dictionary of all possible challenges, with a bool indicating completion.
    public List<Challenge> AllChallenges = new List<Challenge>();

    // Challenges menu.
    private StepChallengesMenu _challengesMenu;

    // Initialization.
    public void SetupLocal()
    {
        Debug.Log("Initializing challenge manager.");

        // Load challenges file into reader.
        TextAsset challengesText = LugusResources.use.Shared.GetTextAsset("Challenges");

        // Check if challenges file is found.
        if (challengesText == LugusResources.use.errorTextAsset)
        {
            Debug.LogError("Challenges file not Found!");
            return;
        }

        // Read challenges file.
        TinyXmlReader reader = new TinyXmlReader(challengesText.text);
        while (reader.Read("Challenges"))
        {
            // Read challenge.
            if (reader.tagName == "Challenge" && reader.tagType == TinyXmlReader.TagType.OPENING)
            {
                Challenge newChallenge = new Challenge();

                while (reader.Read("Challenge"))
                {
                    if (reader.tagType == TinyXmlReader.TagType.OPENING)
                    {
                        // Read description.
                        if (reader.tagName == "TextKey")
                        {
                            newChallenge.Description = reader.content;
                            Debug.Log("New challenge read: " + newChallenge.Description);
                        }

                        // Read conditions.
                        else if (reader.tagName == "Condition")
                        {
                            newChallenge.Conditions.Add(ParseCondition(reader));
                        }
                    }
                }

                // Add challenges.
                AllChallenges.Add(newChallenge);

                if (CurrentChallenges.Count < 4)
                    CurrentChallenges.Add(newChallenge);
            }
        }

        // Fill in challenges.
        //Condition scoreCondition = new ScoreCondition(Functor.Greater<int>(), 5000);

        //Condition levelCondition = new LevelCondition(Functor.Equal<string>(), PlayerData.MineLvlName);

        //Condition ballsLostCondition = new BallsInPlayCondition(Functor.Equal<int>(), 0);
        //ballsLostCondition.CountChangedOnly = true;
        //ballsLostCondition.CountToMeet = 2;
        //ballsLostCondition.LevelLoadReset = true;

        //Challenge scoreChallenge = new Challenge("Score more than 5000 points in the mine level.", scoreCondition, levelCondition);
        //Challenge ballsLostChallenge = new Challenge("Lose 3 balls.", ballsLostCondition);

        //AllChallenges.Add(scoreChallenge, false);
        //AllChallenges.Add(ballsLostChallenge, false);

        //CurrentChallenges.Add(scoreChallenge);
        //CurrentChallenges.Add(ballsLostChallenge);
    }

    // Fill in condition with TinyXmlReader.
    private Condition ParseCondition(TinyXmlReader reader)
    {
        Condition newCondition = null;

        while (reader.Read("Condition"))
        {
            if (reader.tagType == TinyXmlReader.TagType.OPENING)
            {
                // Read Type.
                if (reader.tagName == "Type")
                {
                    switch (reader.content)
                    {
                        case "Score":
                            Debug.Log("New 'Score' condition created.");
                            newCondition = new ScoreCondition();
                            break;
                        case "Level":
                            Debug.Log("New 'Level' condition created.");
                            newCondition = new LevelCondition();
                            break;
                        case "BallsInPlay":
                            Debug.Log("New 'BallsInPlay' condition created.");
                            newCondition = new BallsInPlayCondition();
                            break;
                        default:
                            Debug.LogError("Condition type not found! Make sure it's a valid type.");
                            return null;
                    }
                }

                // Read Parameters.
                if (reader.tagName == "Parameters")
                {
                    Dictionary<string, string> parameters = new Dictionary<string, string>();

                    while (reader.Read("Parameters"))
                    {
                        if (reader.tagType == TinyXmlReader.TagType.OPENING)
                        {
                            // Store parameter in dictionary.
                            parameters.Add(reader.tagName, reader.content);
                            //Debug.Log("Added parameter: " + reader.tagName + ", " + reader.content);
                        }
                    }

                    // Initialize condition.
                    newCondition.InitializeFromParameters(parameters);
                }
            }
        }

        return newCondition;
    }

    // Called every frame.
    void Update()
    {
        // Loop backwards over challenges so we can safely remove them.
        for (int i = CurrentChallenges.Count - 1; i > 0; i--)
        {
            if (CurrentChallenges[i].IsCompleted())
            {
                Debug.Log("--- Challenge completed: " + CurrentChallenges[i].Description + " ---");
                CurrentChallenges.Remove(CurrentChallenges[i]);
            }
        }
    }

    // Called when a new level was loaded.
    void OnLevelWasLoaded(int level)
    {
        // Call 'OnLevelWasLoaded' function on all active conditions.
        foreach (Challenge challenge in CurrentChallenges)
        {
            foreach (Condition condition in challenge.Conditions)
            {
                condition.OnLevelWasLoaded();
            }
        }
    }

    void OnDestroy()
    {
        _instance = null;
    }
    void OnDisable()
    {
        _instance = null;
    }
}
