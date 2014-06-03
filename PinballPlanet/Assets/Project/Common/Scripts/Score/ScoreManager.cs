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

    private float _timeSinceScore = 0;
    private float _scoreComboTime = 0.8f;
    private float _scoreComboHeight = 7;
    private int _scoreCombo = 0;

	// Use this for initialization
	void Start () 
	{
		ShowTotalScore();
		ShowBallCount();     
	}
	
	// Player.js uses SendMessage to call this
	public void BallDestroyed()
	{
		BallCount--;
		ShowBallCount();
	}
	
	public void AddBalls(int amount)
	{
		BallCount += amount;
		ShowBallCount();

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
	
	protected void ShowBallCount()
	{
        if (BallCountText != null)
            BallCountText.text = "" + BallCount;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _timeSinceScore += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(Application.loadedLevel);
    }
	
	// Instantiates a score popup and awards the player points.
	public GameObject ShowScore( int score, Vector3 position, float time, AudioClip sound, Color color )
	{
		if( score == 0 )
		{
			if( sound != null )
				audio.PlayOneShot( sound );
			
			return null;
		}
		
        // Randomize position.
        position.xAdd(Random.Range(-5, 5));
        position.yAdd(Random.Range(-5, 5));
        //position.zAdd(Random.Range(-5, 5));

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

        Transform scoreText = (Transform)Instantiate(ScoreTextPrefab, position, ScoreTextPrefab.transform.rotation);
		scoreText.GetComponent<TextMesh>().text = "" + score;
		scoreText.GetComponent<TextMesh>().renderer.material.color = color;

	    Vector3 posAdd = new Vector3(0, 0, 25);
	    if (!MoveScoreUp)
	        posAdd *= -1;

        iTween.MoveTo(scoreText.gameObject, position + posAdd, time);
		Destroy(scoreText.gameObject, time);
		
		if( sound != null )
			 audio.PlayOneShot( sound );
		
		int newScore = TotalScore + score;
		if( newScore >= 0 )
			TotalScore += score;
		else
			TotalScore = 0;
		
		ShowTotalScore();

	    return scoreText.gameObject;
	}
	
}
