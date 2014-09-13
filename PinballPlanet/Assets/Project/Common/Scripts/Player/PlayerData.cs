using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    // List of high scores for each level.
    public Dictionary<string, List<int>> LevelsHighscores;
    public const int MaxHighScores = 5;

    // Level strings.
    public const string MainLvlName = "Pinball_MainMenu";
    public const string HalloweenLvlName = "Pinball_Halloween";
    public const string PirateLvlName = "Pinball_Ship";
    public const string MineLvlName = "Pinball_Mine";

    private static PlayerData _instance = null;
    public static PlayerData use
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(PlayerData)) as PlayerData;

                if (_instance == null)
                {
                    _instance = new GameObject("PlayerData").AddComponent<PlayerData>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
            }

            return _instance;
        }
    }

    // Initialization.
    void Start()
    {
    }

    // Sort highscores.
    public void Sort(string lvlName)
    {
        LevelsHighscores[lvlName].Sort();
        LevelsHighscores[lvlName].Reverse();
    }

    // Save data.
    public void Save()
    {
        // Initialize lists.
        if (LevelsHighscores == null)
        {
            Debug.LogError("LevelsHighscores dictionary not initialized.");
            return;
        }

        // Save scores.
        foreach (KeyValuePair<string, List<int>> lvlHighScores in LevelsHighscores)
        {
            int i = 0;
            foreach (int score in lvlHighScores.Value)
            {
                Debug.Log("Saving High Score: " + score);

                LugusConfig.use.User.SetInt("Highscore_" + lvlHighScores.Key + "_" + i, score, true);
                ++i;
            }
        }

        // Save to files.
        Debug.Log("Saving High Scores");
        LugusConfig.use.SaveProfiles();
    }

    // Load Data.
    public void Load()
    {
        Debug.Log("Loading High Scores");

        // Initialize lists.
        if (LevelsHighscores == null)
        {
            LevelsHighscores = new Dictionary<string, List<int>>();
            LevelsHighscores.Add(HalloweenLvlName, new List<int>());
            LevelsHighscores.Add(PirateLvlName, new List<int>());
            LevelsHighscores.Add(MineLvlName, new List<int>());
        }

        // Load scores.
        foreach (KeyValuePair<string, List<int>> lvlHighScores in LevelsHighscores)
        {
            for (int i = 0; i < MaxHighScores; i++)
            {
                string key = "Highscore_" + lvlHighScores.Key + "_" + i;
                int score = LugusConfig.use.User.GetInt(key, 0);

                if (score > 0)
                {
                    lvlHighScores.Value.Add(score);
                    Debug.Log("Loaded High Score: " + key + ", " + score);
                }
            }
            Sort(lvlHighScores.Key);
        }
    }

    // Adds a highscore if high enough.
    public void AddHighScore(string lvlName, int score)
    {
        // Initialize lists.
        if (LevelsHighscores == null)
        {
            Debug.LogError("LevelsHighscores dictionary not initialized.");
            return;
        }

        if (!LevelsHighscores.ContainsKey(lvlName))
        {
            Debug.LogError("High score list of level " + lvlName + " not found!");
            return;
        }

        if (LevelsHighscores[lvlName].Count < MaxHighScores)
        {
            LevelsHighscores[lvlName].Add(score);
            Debug.Log("Adding high score to level: " + lvlName);
        }
        else
        {
            int lastIndex = LevelsHighscores[lvlName].Count - 1;
            if (LevelsHighscores[lvlName][lastIndex] < score)
            {
                LevelsHighscores[lvlName][lastIndex] = score;
                Debug.Log("Adding high score to level: " + lvlName);
            }
        }

        Sort(lvlName);

        // Save all data.
        Save();
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
