using UnityEngine;
using System.Collections.Generic;

public class PlayerData : LugusSingletonExisting<PlayerData>
{
    // List of high scores for each level.
    public Dictionary<string, List<int>> LevelsHighscores;
    public const int MaxHighScores = 5;

    // Level strings.
    public const string MainLvlName = "Pinball_MainMenu";
    public const string HalloweenLvlName = "Pinball_Halloween";
    public const string PirateLvlName = "Pinball_Ship";
    public const string MineLvlName = "Pinball_Mine";

    // Initialization.
    void Start()
    {
        // Initialize lists.
        LevelsHighscores = new Dictionary<string, List<int>>();

        LevelsHighscores.Add(HalloweenLvlName, new List<int>());
        LevelsHighscores.Add(PirateLvlName, new List<int>());
        LevelsHighscores.Add(MineLvlName, new List<int>());

        // Load Data.
        Load();
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
        LugusConfig.use.SaveProfiles();
    }

    // Load Data.
    public void Load()
    {
        // Load scores.
        foreach (KeyValuePair<string, List<int>> lvlHighScores in LevelsHighscores)
        {
            for (int i = 0; i < MaxHighScores; i++)
            {
                int score = LugusConfig.use.User.GetInt("Highscore_" + lvlHighScores.Key + "_" + i, 0);
                if (score > 0)
                {
                    lvlHighScores.Value.Add(score);
                    Debug.Log("Loaded High Score: " + score);
                }
            }
            Sort(lvlHighScores.Key);
        }
    }

    // Adds a highscore if high enough.
    public void AddHighScore(string lvlName, int score)
    {
        if (LevelsHighscores[lvlName].Count < 5)
        {
            LevelsHighscores[lvlName].Add(score);
        }
        else
        {
            int lastIndex = LevelsHighscores[lvlName].Count - 1;
            if (LevelsHighscores[lvlName][lastIndex] < score)
                LevelsHighscores[lvlName][lastIndex] = score;      
        }

        Sort(lvlName);
        
        // Save all data.
        Save();
    }
}
