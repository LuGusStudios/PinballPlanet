using UnityEngine;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour
{
    // Singleton Instance.
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

            // Load data if not yet done.
            if (_instance.LevelsHighscores == null)
            {
                _instance.LevelsHighscores = new Dictionary<string, List<int>>();
                _instance.LevelsHighscores.Add(HalloweenLvlName, new List<int>());
                _instance.LevelsHighscores.Add(PirateLvlName, new List<int>());
                _instance.LevelsHighscores.Add(MineLvlName, new List<int>());

                _instance.Load();
            }

            return _instance;
        }
    }

    // List of high scores for each level.
    public Dictionary<string, List<int>> LevelsHighscores;
    public const int MaxHighScores = 5;

    // Level strings.
    public const string MainLvlName = "Pinball_MainMenu";
    public const string HalloweenLvlName = "Pinball_Halloween";
    public const string PirateLvlName = "Pinball_Ship";
    public const string MineLvlName = "Pinball_Mine";

    // Total stars earned.
    private int _stars = 0;
    public int Stars
    {
        get { return _stars; }
        set
        {
            _stars = value;

            if (ChallengesMenuStars != null)
            {
                Debug.Log("Updating star count text.");
                ChallengesMenuStars.text = value.ToString();
            }
            else
                Debug.Log("ChallengesMenuStars text mesh not set.");
        }
    }
    
    public TextMesh ChallengesMenuStars;
    public int ScorePerStar = 5000;

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

        // Save stars.
        LugusConfig.use.User.SetInt("Stars", Stars, true);

        // Save to files.
        Debug.Log("Saving High Scores");
        LugusConfig.use.SaveProfiles();
    }

    // Load Data.
    public void Load()
    {
        Debug.Log("Loading High Scores");

        //// Initialize lists.
        //if (LevelsHighscores == null)
        //{
        //}

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

        // Load stars.
        Debug.Log("Loading Stars");
        Stars = LugusConfig.use.User.GetInt("Stars", 0);
    }

    // Adds a highscore if high enough.
    public void AddHighScore(string lvlName, int score)
    {
        // Check if highscores exist.
        if (LevelsHighscores == null)
        {
            Debug.LogError("LevelsHighscores dictionary not initialized.");
            return;
        }

        // Check if level name is valid.
        if (!LevelsHighscores.ContainsKey(lvlName))
        {
            Debug.LogError("High score list of level " + lvlName + " not found!");
            return;
        }

        // Add high score if list isn't full yet.
        if (LevelsHighscores[lvlName].Count < MaxHighScores)
        {
            LevelsHighscores[lvlName].Add(score);
            Debug.Log("Adding high score to level: " + lvlName);
        }
        // Add high score if a lower score is present.
        else
        {
            int lastIndex = LevelsHighscores[lvlName].Count - 1;
            if (LevelsHighscores[lvlName][lastIndex] < score)
            {
                LevelsHighscores[lvlName][lastIndex] = score;
                Debug.Log("Adding high score to level: " + lvlName);
            }
        }

        // Sort list.
        Sort(lvlName);

        Save();
    }

    // Returns the highest highscore from a given level.
    public int GetHighestScore(string lvlName)
    {
        // Check if highscores exist.
        if (LevelsHighscores == null)
        {
            Debug.LogError("LevelsHighscores dictionary not initialized.");
            return 0;
        }

        // Check if level name is valid.
        if (!LevelsHighscores.ContainsKey(lvlName))
        {
            Debug.LogError("High score list of level " + lvlName + " not found!");
            return 0;
        }

        // Check if there is a high score.
        if (LevelsHighscores[lvlName].Count > 0)
            return LevelsHighscores[lvlName][0];
        else
            return 0;
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
