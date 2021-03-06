using UnityEngine;

public class ScoreManager : LugusSingletonExisting<ScoreManager>
{
	public Transform ScoreTextPrefab = null;
	//public AudioClip ScoreSound = null;
	
	public TextMesh TotalScoreText = null;
	public TextMesh BallCountText = null;

    public bool MoveScoreUp = true;

	public int TotalScore = 0;
	public int ScoreLength = 8;
	
	public int BallCount = 5;

	private int _displayBallCount = 0;
	private int _multiballsInPlay = 0;

    private float _timeSinceScore = 0;
    private float _scoreComboTime = 0.65f;
    private float _scoreDelay = 0.25f;
    private float _scoreComboHeight = 7;
    private int _scoreCombo = 0;

    private GameObject _lastObject = null;

	private float scoreMultiplier = 1.0f;
	private float scoreMultiplierIncrement = 0.0f;
	private float scoreMultiplierMin = 0.0f;
	private float scoreMultiplierMax = 2.0f;
    
	// Use this for initialization
	void Start () 
	{
		_displayBallCount = BallCount;
		ShowTotalScore();
		ShowBallCount();     
	}
	
	// Player uses SendMessage to call this
	public void BallDestroyed()
	{
		BallCount--;

		if (_multiballsInPlay > 0)
			_multiballsInPlay--;
		else 
			_displayBallCount--;

		//ShowBallCount();
	}
	
	public void AddBalls(int amount)
	{
		BallCount += amount;
		//ShowBallCount();

        Player.use.UpdateBallArray();
    }

	public void AddMultiBalls(int amount)
	{
		BallCount += amount;
		_multiballsInPlay += amount;
		//ShowBallCount();
		
		Player.use.UpdateBallArray();
	}

	public void SetBallCount(int amount)
	{
		_displayBallCount = amount;
		BallCount = amount;
		//ShowBallCount();
		
		Player.use.UpdateBallArray();
	}
	
	public void Reset()
	{
		TotalScore = 0;
	}
	
	protected void ShowTotalScore()
	{
		if(TotalScoreText != null)
		    TotalScoreText.text = GetTotalScore();
	}
	
	public string GetTotalScore()
	{
		string text = "" + TotalScore;
		for( int i = text.Length; i < ScoreLength; ++i )
			text = "0" + text;
		
		return text;
	}

	private int _OldDisplayBallCount = 0;

	protected void ShowBallCount()
	{
        if (BallCountText != null)
		{
			int displayVal = _displayBallCount - 1;

			if (Player.use.IsSingleBallReadyForLaunch())
				displayVal += 1;

			if (_OldDisplayBallCount != displayVal)
			{
				_OldDisplayBallCount = displayVal;

				if (displayVal >= 0)
					BallCountText.text = "" + displayVal;
				else 
					BallCountText.text = "0";
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timeSinceScore += Time.deltaTime;
		ShowBallCount();
	}

	public void SetScoreMultiplierSettings(float multiplier, float multiplierIncrement, float multiplierMin, float multiplierMax)
	{
		scoreMultiplier = multiplier;
		scoreMultiplierIncrement = multiplierIncrement;
		scoreMultiplierMin = multiplierMin;
		scoreMultiplierMax = multiplierMax;
	}

    // Instantiates a score popup and awards the player points.
    public GameObject ShowScore(int score, Vector3 position, float time, AudioClip sound, Color color, GameObject sender)
    {
		// Apply score multiplier
		score = (int)(score * scoreMultiplier);
		scoreMultiplier += scoreMultiplierIncrement;
		scoreMultiplier = Mathf.Clamp(scoreMultiplier, scoreMultiplierMin, scoreMultiplierMax);

        // If no score this can also be used to just play a sound.
        if (score == 0)
        {
            if (sound != null)
                LugusAudio.use.SFX().Play(sound).Loop = false;

            return null;
        }

        // Don't give score when same object is hit very quickly.
        if (_lastObject == sender)
        {
            if (_timeSinceScore < _scoreDelay)
            {
                _lastObject = sender;
                return null;
            }
        }

        // Randomize position.
        position.xAdd(Random.Range(-5, 5));
        position.yAdd(Random.Range(-5, 5));

        // Put score slightly higher if scoring in quick sucession.
        if (_timeSinceScore <= _scoreComboTime)
        {
            position.z += _scoreComboHeight * _scoreCombo;
            ++_scoreCombo;
        }
        else
        {
            _scoreCombo = 0;
        }
        // Reset time since score.
        _timeSinceScore = 0;

        // Overwrite last object.
        _lastObject = sender;

        // Spawn score object.
        Transform scoreText = (Transform)Instantiate(ScoreTextPrefab, position, ScoreTextPrefab.transform.rotation);
        scoreText.GetComponent<TextMesh>().text = "" + score;
        scoreText.GetComponent<TextMesh>().renderer.material.color = color;

        // Add height.
        Vector3 posAdd = new Vector3(0, 0, 25);
        if (!MoveScoreUp)
            posAdd *= -1;

        // Play move animation.
        iTween.MoveTo(scoreText.gameObject, position + posAdd, time);
        Destroy(scoreText.gameObject, time);

        // Play hit sound.
        if (sound != null)
        {
            LugusAudio.use.SFX().Play(sound);
        }

        // Add score.
        int newScore = TotalScore + score;
        if (newScore >= 0)
            TotalScore += score;
        else
            TotalScore = 0;

        // Update text mesh.
        ShowTotalScore();

        return scoreText.gameObject;
    }
	
}
