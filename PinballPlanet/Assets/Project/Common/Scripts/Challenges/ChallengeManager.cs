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

    // Dictionary of all possible challenges, with a bool indicating completion.
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

                // Add challenges.
                AllChallenges.Add(newChallenge);
            }
        }
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
        // Looping backwards over uncompleted challenges to safely remove completed ones.
        for (int i = _uncompletedChallenges.Count - 1; i > 0; i--)
        {
            if (_uncompletedChallenges[i].IsCompleted())
            {
                // Store completed challenge.
                CompletedLvlChallenges.Add(_uncompletedChallenges[i]);
                _uncompletedChallenges.Remove(_uncompletedChallenges[i]);

                //TODO: Show particle effect/message.
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

    // Removes a current challenge and adds the next one.
    public void ReplaceChallenge(Challenge challengeToRemove)
    {
        CurrentChallenges.Remove(challengeToRemove);

        AddNewChallenge();
    }

    // Adds an uncompleted challenge to the current challenges list.
    public void AddNewChallenge()
    {
        // Find next uncompleted challenge.
        foreach (Challenge challenge in AllChallenges)
        {
            if (!CurrentChallenges.Contains(challenge))
            {
                // Look for uncompleted challenge.
                if (!challenge.Done)
                {
                    // No required level, just add challenge.
                    if (challenge.LevelKey == LevelKey.None)
                    {
                        Debug.Log("--- New challenge added. ---");
                        CurrentChallenges.Add(challenge);
                        _uncompletedChallenges.Add(challenge);
                        return;
                    }

                    // Check if required level is unlocked.
                    //Debug.Log("--- Checking if challenge required level " + challenge.LevelKey.ToString() + " is unlocked. ---");
                    if (PlayerData.use.LevelsUnlocked["Pinball_" + challenge.LevelKey.ToString()])
                    {
                        Debug.Log("--- New challenge added. ---");
                        CurrentChallenges.Add(challenge);
                        _uncompletedChallenges.Add(challenge);
                        return;
                    }
                }
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

    void OnDestroy()
    {
        _instance = null;
    }
    void OnDisable()
    {
        _instance = null;
    }
}
