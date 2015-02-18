using UnityEngine;
using System.Collections.Generic;
using System;

public enum LevelKey
{
    None = 0, MainMenu = 1, Halloween = 2, Pirate = 3, Mine = 4
}

public enum CameraMode
{ 
    NONE = 0, Instant = 1, Smooth = 2, Fixed = 3
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
                _instance._unlockCosts.Add(15);
                _instance._unlockCosts.Add(30);
                _instance._unlockCosts.Add(100);

                _instance.Load();
            }
            return _instance;
        }
    }

    public CameraMode camMode = CameraMode.Smooth;

	public Powerup permanentPowerup = null;
	public Powerup temporaryPowerup = null;

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
        get {return _stars; }
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

	private int _exp = 0;
	private int _oldExp = 0;
	private int _expToNext = 10000;

	private int _level = 0;
	private int _oldLevel = 0;


	public float expMultiplier = 1.0f;

	public void AddExp(int value)
	{
		_oldExp = _exp;
		_oldLevel = _level;

		value = Mathf.FloorToInt( (float) value * expMultiplier );
		_exp += value;

		Debug.Log("Adding exp " + value);

		// Allow Multiple level-ups
		while (_exp >= _expToNext)
		{
			Debug.Log("Exp " + _exp + " To next " + _expToNext);
			_exp -= _expToNext;
			_level += 1;
			_expToNext = CalcExpToNext(_level);
		}
	}

	public int GetExp()
	{
		return _exp;
	}

	public int GetOldExp()
	{
		return _oldExp;
	}

	public float GetExpPercentage()
	{
		return (float)_exp/(float)_expToNext;
	}

	public int GetLevel()
	{
		return _level;
	}

	public int getOldLevel()
	{
		return _oldLevel;
	}

	public int CalcExpToNext(int level)
	{
		// Calculation is zero based
		// Subtract one from current level
		level --;
		if (level < 0) 
			level = 0;

		int nextExpValue = 10000;
		int increment = 10000;
		bool useHalfOfThisValue = false;

		for (int i = 1; i <= level; i++)
		{
			if (useHalfOfThisValue)
			{
				increment = nextExpValue/2;
			}

			nextExpValue += increment;

			useHalfOfThisValue = !useHalfOfThisValue;
		}

		return nextExpValue;
	}
		
	public void CheatLevel(int level)
	{
		_level = level;
		_oldLevel = level;
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

	public int bonusStarsOnChallengeComplete = 0;

    // Sort highscores.
    public void Sort(string lvlName)
    {
        LevelsHighscores[lvlName].Sort();
        LevelsHighscores[lvlName].Reverse();
    }

	private float _prevPlayTime = 0;
	private float _realTimeAtLastSave = 0;

	public string voidGetPlaytimeString(bool includeSeconds = false)
	{
		float time = 0;
		if (_prevPlayTime == 0)
		{
			time = LugusConfig.use.User.GetFloat("TotalPlayTime", 0.0f);
			_prevPlayTime = time;
		}
		else 
		{
			time = _prevPlayTime;
		}

		time += Time.realtimeSinceStartup;



//		float secondsPerHour = 3600;
//		float secondsPerMinute = 60;
//
//		int hours = Mathf.FloorToInt(time/secondsPerHour);
//		time -= (float)(hours*secondsPerHour);
//		int minutes = Mathf.FloorToInt(time/secondsPerMinute);
//		time -= (float)(minutes*secondsPerMinute);

		TimeSpan ts = TimeSpan.FromSeconds(time);
		int hours = (int)Math.Floor(ts.TotalHours);
		int minutes = ts.Minutes;

		if (includeSeconds)
			return hours + "h " + minutes + "min " + ts.Seconds + "s";
		else
			return hours + "h " + minutes + "min";
	}
		
	public void SavePlaytime()
	{
		float val = LugusConfig.use.User.GetFloat("TotalPlayTime", 0.0f);
		val += Time.realtimeSinceStartup - _realTimeAtLastSave;
		LugusConfig.use.User.SetFloat("TotalPlayTime", val, true);
		_realTimeAtLastSave = Time.realtimeSinceStartup;
	}

	public int numberOfGamesPlayed = 0;

	public void SaveNumberOfGamesPlayed()
	{
		LugusConfig.use.User.SetInt("NumberOfGamesPlayed", numberOfGamesPlayed, true);
	}

	public void loadNumberOfGamesPlayed()
	{
		numberOfGamesPlayed = LugusConfig.use.User.GetInt("NumberOfGamesPlayed", 0);
	}
	
    // Save data.
    public void Save()
    {
        //Debug.LogError("------ Saving player data. ------");

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

        LugusConfig.use.System.SetInt("CamMode", (int)camMode, true);

		SavePlaytime();
		SaveNumberOfGamesPlayed();

        // Save stars.
        LugusConfig.use.User.SetInt("Stars", Stars, true);

		// save level and exp
		LugusConfig.use.User.SetInt ("Level", _level, true);
		LugusConfig.use.User.SetInt ("Exp", _exp, true);

		// Save Powerups
		if (permanentPowerup != null)
		{
			LugusConfig.use.User.SetInt ("PermanentPowerup", permanentPowerup.id, true);
		}
		else 
		{
			LugusConfig.use.User.SetInt ("PermanentPowerup", 0, true);
		}

//		if (temporaryPowerup != null)
//		{
//			LugusConfig.use.User.SetInt ("TemporaryPowerup", temporaryPowerup.id, true);
//		}
//		else
//		{
//			LugusConfig.use.User.SetInt ("TemporaryPowerup", 0, true);
//		}

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
        Debug.LogWarning("Saving High Scores");
        LugusConfig.use.SaveProfiles();
    }

    // Load Data.
    public void Load()
    {
        //Debug.LogError("------ Loading player data. ------");

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

		loadNumberOfGamesPlayed();
        camMode = (CameraMode) LugusConfig.use.System.GetInt("CamMode", 1);

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

		// Load Level and exp 
		_level = LugusConfig.use.User.GetInt ("Level", 1);
		_exp = LugusConfig.use.User.GetInt ("Exp", 0);
		_expToNext = CalcExpToNext(_level);

		// Load powerups
		int permPUKey = LugusConfig.use.User.GetInt ("PermanentPowerup", 0);
		int tempPUKey = LugusConfig.use.User.GetInt ("TemporaryPowerup", 0);

		PowerupManager.use.SetPermanentPowerup((PowerupKey) permPUKey);
//		PowerupManager.use.SetTemporaryPowerup((PowerupKey) tempPUKey);
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

		if (LevelsHighscores[lvlName].Contains(score))
		{
			Debug.Log("High score list of level " + lvlName + " already contains score " + score);
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
