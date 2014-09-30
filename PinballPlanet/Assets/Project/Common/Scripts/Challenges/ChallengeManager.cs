using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    private List<Challenge> _uncompletedChallenges = new List<Challenge>();

    // All possible challenges.
    public List<Challenge> AllChallenges = new List<Challenge>();

    // Completed challenges in a level.
    public List<Challenge> CompletedLvlChallenges = new List<Challenge>();

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

        // Parse challenges file.
        ParseChallenges(ref challengesText);
    }

    // Loads in all challenges.
    private void ParseChallenges(ref TextAsset challengesText)
    {
        TinyXmlReader reader = new TinyXmlReader(challengesText.text);

        // Start read.
        while (reader.Read("Challenges"))
        {
            // Read challenge.
            if (reader.tagName == "Challenge" && reader.tagType == TinyXmlReader.TagType.OPENING)
            {
                Challenge newChallenge = new Challenge();
                Debug.Log("--------Reading challenge--------");

                while (reader.Read("Challenge"))
                {
                    if (reader.tagType == TinyXmlReader.TagType.OPENING)
                    {
                        // Read unique ID
                        if (reader.tagName == "ID")
                        {
                            if (!HasID(reader.content))
                            {
                                newChallenge.ID = reader.content;
                                Debug.Log("Challenge ID: " + newChallenge.ID);
                            }
                            else
                                Debug.LogError("Challenge ID: " + reader.content + " is not unique!.");
                        }

                        // Read stars.
                        else if (reader.tagName == "Stars")
                        {
                            int result;
                            if (int.TryParse(reader.content, out result))
                            {
                                newChallenge.StarsReward = result;
                                Debug.Log("Stars: " + result);
                            }
                            else
                                Debug.LogError("Stars parameter value invalid.");
                        }

                        // Read priority.
                        else if (reader.tagName == "Priority")
                        {
                            int result;
                            if (int.TryParse(reader.content, out result))
                            {
                                newChallenge.Priority = result;
                                Debug.Log("Priority: " + result);
                            }
                            else
                                Debug.LogError("Priority parameter value invalid.");
                        }

                        // Read required level.
                        else if (reader.tagName == "Level")
                        {
                            LevelKey key = (LevelKey)Enum.Parse(typeof(LevelKey), reader.content);
                            newChallenge.LevelKey = key;
                            Debug.Log("Level: " + key);
                        }

                        // Read description.
                        else if (reader.tagName == "TextKey")
                        {
                            newChallenge.Description = reader.content;
                            Debug.Log("Description: " + newChallenge.Description);
                        }

                        // Read conditions.
                        else if (reader.tagName == "Condition")
                        {
                            newChallenge.Conditions.Add(ParseCondition(reader));
                        }
                    }
                }

                // Make sure required keys are found before adding challenge.
                if (newChallenge.ID == "")
                    Debug.Log("Challenge ID not found! Challenge not added.");
                else if (newChallenge.Description == "")
                    Debug.Log("Challenge TextKey not found! Challenge not added.");
                else
                    AllChallenges.Add(newChallenge);
            }
        }

        // Sort challenges.
        AllChallenges.Sort(delegate(Challenge first, Challenge second) { return first.Priority.CompareTo(second.Priority); });

        Debug.Log("--- Sorted challenges. ---");
        foreach (Challenge chall in AllChallenges)
        {
            Debug.Log("Challenge " + chall.ID + " Priority: " + chall.Priority);
        }
        Debug.Log("-------------------------");
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
                        case "BallCount":
                            Debug.Log("New 'BallCount' condition created.");
                            newCondition = new BallCountCondition();
                            break;
                        case "ObjectHit":
                            Debug.Log("New 'ObjectHit' condition created.");
                            newCondition = new ObjectHitCondition();
                            break;
                        case "ButtonPressed":
                            Debug.Log("New 'ButtonPressed' condition created.");
                            newCondition = new ButtonPressedCondition();
                            break;
                        case "InputKey":
                            Debug.Log("New 'InputKey' condition created.");
                            newCondition = new InputKeyCondition();
                            break;
                        case "Flipper":
                            Debug.Log("New 'Flipper' condition created.");
                            newCondition = new FlipperCondition();
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
        //  Don't track challenges in game over menu.
        if (MenuManager.use.ActiveMenu != MenuManagerDefault.MenuTypes.GameOverMenu)
            CheckChallenges();
    }

    // Checks if any challenges are completed.
    public void CheckChallenges()
    {
        // Looping backwards over uncompleted challenges to safely remove completed ones.
        for (int i = _uncompletedChallenges.Count - 1; i >= 0; i--)
        {
            // Check if challenge was already completed.
            if (!CompletedLvlChallenges.Contains(_uncompletedChallenges[i]))
            {
                if (_uncompletedChallenges[i].IsCompleted())
                {
                    // Store completed challenge.
                    CompletedLvlChallenges.Add(_uncompletedChallenges[i]);
                    //ReplaceChallenge(_uncompletedChallenges[i]);
                    //_uncompletedChallenges.Remove(_uncompletedChallenges[i]);

                    // Save.
                    //PlayerData.use.Save();

                    //TODO: Show particle effect/message.
                }
            }
        }
    }

    // Called when a new level was loaded.
    void OnLevelWasLoaded(int level)
    {
        // Clear completed challenges in level.
        CompletedLvlChallenges.Clear();

        // Call 'OnLevelWasLoaded' function on all active conditions.
        foreach (Challenge challenge in CurrentChallenges)
        {
            foreach (Condition condition in challenge.Conditions)
            {
                condition.OnLevelWasLoaded();
            }
        }
    }

    // Removes a current challenge and adds the next one, returns whether a new challenge was found.
    public bool ReplaceChallenge(Challenge challengeToRemove)
    {
        //Debug.Log("* Start current challenges count: " + CurrentChallenges.Count);

        // Remove challenge.
        int index = CurrentChallenges.FindIndex(delegate(Challenge chall) { return chall == challengeToRemove; });

        // List with possible challenges to be added.
        List<Challenge> possibleChallenges = new List<Challenge>();

        // Loops until a challenge was added or no more challenges are available to add.
        for (int i = 0; i < AllChallenges.Count; i = i)
        {
            int currentCount = i;

            // Add from any valid challenge with the same priority.
            for (int j = 0; currentCount + j < AllChallenges.Count; ++j)
            {
                if (AllChallenges[currentCount + j].Priority != AllChallenges[currentCount].Priority)
                    break;

                Challenge challenge = AllChallenges[currentCount + j];

                // Look for uncompleted challenge.
                if (!challenge.Completed)
                {
                    // Check if challenge is already added.
                    if (!CurrentChallenges.Contains(challenge))
                    {
                        // Check if required level is unlocked.
                        if (challenge.LevelKey == LevelKey.None || PlayerData.use.LevelsUnlocked["Pinball_" + challenge.LevelKey.ToString()])
                        {
                            possibleChallenges.Add(challenge);
                            //Debug.Log("*** Found possible challenge: " + challenge.ID + ", " + challenge.Completed);
                        }
                    }
                }

                ++i;
            }

            // Replace with random possible challenge challenge at index.
            if (possibleChallenges.Count > 0)
            {
                // Generate random index.
                int randIndex = UnityEngine.Random.Range(0, possibleChallenges.Count);

                CurrentChallenges[index] = (possibleChallenges[randIndex]);
                _uncompletedChallenges[index] = (possibleChallenges[randIndex]);

                Debug.Log("Adding challenge: " + possibleChallenges[randIndex].ID + " Rand index: " + randIndex + "/" + (possibleChallenges.Count - 1));
                //Debug.Log("* End current challenges count: " + CurrentChallenges.Count);

                return true;
            }
        }

        // Remove challenge (if it isn't overwritten).
        CurrentChallenges.Remove(challengeToRemove);
        return false;
    }

    // Adds an uncompleted challenge to the current challenges list.
    public void FillChallenges()
    {
        // List with possible challenges to be added.
        List<Challenge> possibleChallenges = new List<Challenge>();

        // Loop until enough possible challenges or no more challenges are available.
        for (int i = 0; possibleChallenges.Count < PlayerData.MaxChallenges - CurrentChallenges.Count && i < AllChallenges.Count; i = i)
        {
            int currentCount = i;

            // Add from any valid challenge with the same priority.
            for (int j = 0; currentCount + j < AllChallenges.Count; ++j)
            {
                if (AllChallenges[currentCount + j].Priority != AllChallenges[currentCount].Priority)
                    break;

                //Debug.Log("Loop i: " + currentCount + ", j: " + j + ", Nr needed: " + (PlayerData.MaxChallenges - CurrentChallenges.Count) + ", Nr found: " + possibleChallenges.Count + ", Total count: " + AllChallenges.Count);
                //Debug.Log("Checking challenge: " + AllChallenges[currentCount + j].ID);
                //Debug.Log("Checking to select possible challenge: " + AllChallenges[currentCount + j].ID + " with Priority: " + AllChallenges[currentCount + j].Priority);

                Challenge challenge = AllChallenges[currentCount + j];

                // Look for uncompleted challenge.
                if (!challenge.Completed)
                {
                    // Check if challenge is already added.
                    if (!CurrentChallenges.Contains(challenge))
                    {
                        // Check if required level is unlocked.
                        if (challenge.LevelKey == LevelKey.None || PlayerData.use.LevelsUnlocked["Pinball_" + challenge.LevelKey.ToString()])
                        {
                            possibleChallenges.Add(challenge);
                            //Debug.Log("Selecting possible challenge: " + challenge.ID);
                        }
                    }
                }

                ++i;
            }

            // Keep adding challenges untill filled.
            while (possibleChallenges.Count > 0 && CurrentChallenges.Count < PlayerData.MaxChallenges)
            {
                int randIndex = UnityEngine.Random.Range(0, possibleChallenges.Count);

                Debug.Log("Adding challenge: " + possibleChallenges[randIndex].ID + " Rand index: " + randIndex + "/" + (possibleChallenges.Count - 1));

                CurrentChallenges.Add(possibleChallenges[randIndex]);
                _uncompletedChallenges.Add(possibleChallenges[randIndex]);

                possibleChallenges.Remove(possibleChallenges[randIndex]);
            }
        }
    }

    // Adds an uncompleted challenge to the current challenges list.
    public void AddChallenge()
    {
        // List with possible challenges to be added.
        List<Challenge> possibleChallenges = new List<Challenge>();

        // Loops until a challenge was added or no more challenges are available to add.
        for (int i = 0; i < AllChallenges.Count; i = i)
        {
            int currentCount = i;

            // Add from any valid challenge with the same priority.
            for (int j = 0; currentCount + j < AllChallenges.Count; ++j)
            {
                if (AllChallenges[currentCount + j].Priority != AllChallenges[currentCount].Priority)
                {
                    //Debug.Log("Next challenge: " + AllChallenges[currentCount + j].ID + " priority: " + AllChallenges[currentCount + j].Priority + " != " + AllChallenges[currentCount].Priority);
                    break;
                }

                //Debug.Log("Checking to select possible challenge: " + AllChallenges[currentCount + j].ID + " with Priority: " + AllChallenges[currentCount + j].Priority);

                Challenge challenge = AllChallenges[currentCount + j];

                // Check if challenge is already added.
                if (!CurrentChallenges.Contains(challenge))
                {
                    // Look for uncompleted challenge.
                    if (!challenge.Completed)
                    {
                        // Check if required level is unlocked.
                        if (challenge.LevelKey == LevelKey.None || PlayerData.use.LevelsUnlocked["Pinball_" + challenge.LevelKey.ToString()])
                        {
                            possibleChallenges.Add(challenge);
                            //Debug.Log("Selecting possible challenge: " + challenge.ID);
                        }
                    }
                }

                ++i;
            }

            // Add random possible challenge.
            if (possibleChallenges.Count > 0)
            {
                // Generate random index.
                int randIndex = UnityEngine.Random.Range(0, possibleChallenges.Count);

                CurrentChallenges.Add(possibleChallenges[randIndex]);
                _uncompletedChallenges.Add(possibleChallenges[randIndex]);

                Debug.Log("Adding challenge: " + possibleChallenges[randIndex].ID + " Rand index: " + randIndex + "/" + (possibleChallenges.Count - 1));

                return;
            }
        }
    }

    // Check if a challenge exists with a given ID.
    public bool HasID(string id)
    {
        foreach (Challenge challenge in AllChallenges)
        {
            if (challenge.ID == id)
                return true;
        }

        return false;
    }

    // Gives all conditions of a certain type.
    public List<Condition> GetConditionsOfType<T>()
        where T : Condition
    {
        List<Condition> typeConditions = new List<Condition>();

        // Loop over all conditions.
        foreach (Challenge chal in AllChallenges)
        {
            foreach (Condition cond in chal.Conditions)
            {
                // Check if correct type.
                if (cond.GetType() == typeof(T))
                {
                    typeConditions.Add(cond);
                }
            }
        }

        return typeConditions;
    }

    void OnDestroy()
    {
        _instance = null;
    }
    void OnDisable()
    {
        _instance = null;
    }

    void LogChallenges()
    {
        Debug.Log("--- Current challenges. ---");
        foreach (Challenge chall in CurrentChallenges)
        {
            Debug.Log("ID: " + chall.ID);
        }
        Debug.Log("--------------------------");
    }
}
