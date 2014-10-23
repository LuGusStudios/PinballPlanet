using UnityEngine;
using System.Collections.Generic;

public enum LevelKey
{
    None = 0, MainMenu = 1, Halloween = 2, Pirate = 3, Mine = 4
}

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

                _instance.LevelsUnlocked.Add(HalloweenLvlName, false);
                _instance.LevelsUnlocked.Add(PirateLvlName, false);
                _instance.LevelsUnlocked.Add(MineLvlName, false);

                _instance._unlockCosts.Add(10);
                _instance._unlockCosts.Add(20);
                _instance._unlockCosts.Add(50);
                _instance._unlockCosts.Add(100);

                _instance.Load();
            }

            return _instance;
        }
    }

    // List of high scores for each level.
    public Dictionary<string, List<int>> LevelsHighscores;
    public static int MaxHighScores = 5;

    // Max challenges at a time.
    public static int MaxChallenges = 3;

    // Level strings.
    public const string MainLvlName = "Pinball_MainMenu";
    public const string HalloweenLvlName = "Pinball_Halloween";
    public const string PirateLvlName = "Pinball_Pirate";
    public const string MineLvlName = "Pinball_Mine";

    // Level keys.
    public static string MainLvlKey
    {
        get
        {
            return LevelKey.MainMenu.ToString();
        }
    }
    public static string HalloweenLvlKey
    {
        get
        {
            return LevelKey.Halloween.ToString();
        }
    }
    public static string PirateLvlKey
    {
        get
        {
            return LevelKey.Pirate.ToString();
        }
    }
    public static string MineLvlKey
    {
        get
        {
            return LevelKey.Mine.ToString();
        }
    }

    // Total stars earned.
    private int _stars = 0;
    public int Stars
    {
        get { return _stars; }
        set
        {
            _stars = value;

            foreach (TextMesh starText in StarTextMeshes)
            {
                if (starText != null)
                    starText.text = value.ToString();
            }
        }
    }

    // Text meshes linked to star count.
    public List<TextMesh> StarTextMeshes = new List<TextMesh>();

    // Score needed to get a star.
    public int ScorePerStar = 20000;

    // Unlock costs.
    public int UnlockCost
    {
        get
        {
            int nrLvlsUnlocked = 0;
            foreach (var lvlUnlocked in LevelsUnlocked)
            {
                if (lvlUnlocked.Value)
                    ++nrLvlsUnlocked;
            }

            return _unlockCosts[nrLvlsUnlocked];
        }
    }
    private List<int> _unlockCosts = new List<int>();

    // Levels that have been unlocked.
    public Dictionary<string, bool> LevelsUnlocked = new Dictionary<string, bool>();

    // Sort highscores.
    public void Sort(string lvlName)
    {
        LevelsHighscores[lvlName].Sort();
        LevelsHighscores[lvlName].Reverse();
    }

    // Save data.
    public void Save()
    {
        Debug.Log("------ Saving player data. ------");

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

        // Save levels unlocked.
        foreach (var lvlUnlocked in LevelsUnlocked)
        {
            Debug.Log("Saving level " + lvlUnlocked.Key + " unlocked = " + lvlUnlocked.Value);

            LugusConfig.use.User.SetBool(lvlUnlocked.Key + "_Unlocked", lvlUnlocked.Value, true);
        }

        // Save completed challenges.
        foreach (Challenge challenge in ChallengeManager.use.AllChallenges)
        {
            if (challenge.Completed)
                LugusConfig.use.User.SetBool("Challenge_" + challenge.ID + "_Done", challenge.Completed, true);
        }

        // Save to files.
        Debug.Log("Saving High Scores");
        LugusConfig.use.SaveProfiles();
    }

    // Load Data.
    public void Load()
    {
        Debug.Log("------ Loading player data. ------");

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
                }
            }
            Sort(lvlHighScores.Key);
        }

        // Load levels unlocked.
        string[] lvlNames = new string[LevelsUnlocked.Count];
        LevelsUnlocked.Keys.CopyTo(lvlNames, 0);
        foreach (string lvlName in lvlNames)
        {
            LevelsUnlocked[lvlName] = LugusConfig.use.User.GetBool(lvlName + "_Unlocked", false);
        }

        // Load completed challenges.
        foreach (Challenge challenge in ChallengeManager.use.AllChallenges)
        {
            if (LugusConfig.use.User.Exists("Challenge_" + challenge.ID + "_Done"))
            {
                challenge.Completed = true;
                if (LugusConfig.use.User.GetBool("Challenge_" + challenge.ID + "_Done", false))
                {
                    challenge.Completed = true;
                }
            }
        }

        // Load stars.
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
