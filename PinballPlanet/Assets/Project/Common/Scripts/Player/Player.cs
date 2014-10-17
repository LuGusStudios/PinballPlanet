using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
///  This script manages controls and ball releases
/// </summary>
public class Player : LugusSingletonExisting<Player>
{
    // Balls
    public GameObject BallPrefab;
    public Transform BallLaunch;
    public List<GameObject> BallsInPlay;

    // Audio
    public AudioClip LaunchSound;
    public AudioClip ReleaseSound;
    public AudioClip BallLostSound;
    public AudioClip LightOnSound;
    public AudioClip LightOffSound;
    public AudioClip LeftFlipperSound;
    public AudioClip RightFlipperSound;
    public AudioClip Music;

    // The force when launching the ball.
    public float LaunchMaxForce;
    public float LaunchAnimationTime;

    // How much force the ball launches with.
    private float _ballLaunchForce;
    public float BallLaunchForce
    {
        get { return _ballLaunchForce; }
        set { _ballLaunchForce = value; }
    }

    private bool _launchSoundPlaying = false;

    // True when game is paused.
    public bool Paused = false;

    // Particles to spawn on launch.
    public GameObject LaunchParticlesPrefab;

    public void OnPause()
    {
        Paused = true;
    }

    public void OnUnpause()
    {
        Paused = false;
    }

    public void UpdateBallArray()
    {
        BallsInPlay.Clear();
        BallsInPlay = GameObject.FindGameObjectsWithTag("Ball").ToList();
    }

    // Releases a ball into the playfield after a bit.
    private void ReleaseBallDelayed()
    {
        Invoke("ReleaseBall", 0.5f);
    }

    // Releases a ball into the playfield
    public Ball ReleaseBall()
    {
        // Game Over.
        if (ScoreManager.use.BallCount <= 0)
        {
            MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameOverMenu);

            PlayerData.use.AddHighScore(Application.loadedLevelName, ScoreManager.use.TotalScore);

            return null;
        }

        // Now create a new ball
        GameObject newball;
        newball = (GameObject)Instantiate(BallPrefab, BallLaunch.transform.position + new Vector3(0, 25, 0), BallPrefab.transform.rotation);
        newball.name = "Ball";

        // Update our ball array so other objects can quickly do a seek on all balls
        UpdateBallArray();

        // Reset flow back preventer.
        GameObject.FindGameObjectWithTag("FlowBackPreventer").SendMessage("Reset");

        return newball.GetComponent<Ball>();
    }

    void Reset()
    {
        foreach (GameObject ball in BallsInPlay)
        {
            DestroyBall(ball.GetComponent<Ball>());
        }
    }

    public bool DestroyBall(Ball ball)
    {
        // Untag ball so that he can't be found again by his tag.
        ball.tag = "Untagged";
        Destroy(ball.gameObject);
        ball = null;

        // Update our ball array so other objects can quickly do a seek on all balls
        UpdateBallArray();
        // Only release a ball if there's no ball in play
        if (BallsInPlay.Count == 0)
        {
            // Play release sound.
            LugusAudio.use.SFX().Play(BallLostSound, true).Loop = false;

            // Release ball after short while.
            ReleaseBallDelayed();
        }

        //Debug.Log("Ball Destroyed! nr balls remaining : " + ballsInPlay.Length );
        ScoreManager.use.BallDestroyed();

        return true;
    }

    void Start()
    {
        ReleaseBall();

        LugusAudio.use.Music().Play(Music, true, new LugusAudioTrackSettings().Loop(true));
    }

    protected virtual void Update()
    {
        if (Paused)
            return;

        // Control the launch.
        if (IsSingleBallReadyForLaunch())
        {
            // Do not use keyboard launch controls when mouse/touch is being used.
            if (!LugusInput.use.down && !LugusInput.use.dragging)
            {
                ControlBallLaunchByKeyboard();
            }

            // Play launch sound if launched.
            if (BallLaunchForce > 0)
            {
                if (!_launchSoundPlaying)
                {
                    //Debug.Log("Playing LaunchSound");
                    LugusAudio.use.SFX().Play(LaunchSound, true).Loop = true;
                    _launchSoundPlaying = true;
                }
            }

            // Stop sound when max reached.
            if (BallLaunchForce >= LaunchMaxForce)
            {
                LugusAudio.use.SFX().StopAll();
            }
        }

        if (LugusDebug.debug)
        {
            // Unstuck ball.
            if (Input.GetKeyDown(KeyCode.U))
                BallsInPlay[0].transform.position = GameObject.Find("UnstuckPos").transform.position.z(BallsInPlay[0].transform.position.z);
        }
    }

    public bool IsSingleBallReadyForLaunch()
    {
        if (BallsInPlay.Count != 1)
        {
            return false;
        }
        else
        {
            GameObject ball = BallsInPlay[0];

            if (ball != null)
            {
                Ball ballScript = ball.GetComponent<Ball>();
                return ballScript.TouchingLauncher;
            }
            else
                return false;
        }
    }

    private void ControlBallLaunchByKeyboard()
    {
        if (Input.GetAxis("Vertical") < 0)
        {
            // When the player holds down the Down Arrow button, pull down the ball launch
            var forceMultiplier = LaunchMaxForce / LaunchAnimationTime;
            if (_ballLaunchForce < LaunchMaxForce)
            {
                float dt = Time.deltaTime;
                _ballLaunchForce += dt * forceMultiplier;
            }
        }

        // Release the ball if down button is released.
        if (Input.GetAxis("Vertical") >= 0)
        {
            var ball = GameObject.Find("Ball");
            if (ball != null)
            {
                Ball ballScript = ball.GetComponent<Ball>();
                if (ballScript.TouchingLauncher && _ballLaunchForce > 0)
                {
                    LaunchBall();
                }
            }
        }
    }

    public void LaunchBall()
    {
        var ball = GameObject.Find("Ball");
        ball.rigidbody.velocity = Vector3.zero;

        //Debug.Log("Launch force: " + BallLaunchForce);
        ball.rigidbody.AddForceAtPosition(new Vector3(0, _ballLaunchForce, 0), ball.transform.position);

        // Reset launch force.
        BallLaunchForce = 0;

        // Spawn fire particles.
        if (LaunchParticlesPrefab != null)
            Instantiate(LaunchParticlesPrefab);

        // Play release sound.
        Debug.Log("Playing ReleaseSound");
        LugusAudio.use.SFX().Play(ReleaseSound, true).Loop = false;
        _launchSoundPlaying = false;

        GameObject.Find("GameMenu").GetComponent<StepGameMenu>().ShowLaunchHelp(false);
    }

    // Play light on sound.
    public void PlayLightOnSound()
    {
        //LugusAudio.use.SFX().Play(LightOnSound).Loop = false;
    }

    // Play light off sound.
    public void PlayLightOffSound()
    {
        //LugusAudio.use.SFX().Play(LightOffSound).Loop = false;
    }

    public void PlayLeftFlipperSound()
    {
        LugusAudio.use.SFX().Play(LeftFlipperSound).Loop = false;
    }

    public void PlayRightFlipperSound()
    {
        LugusAudio.use.SFX().Play(RightFlipperSound).Loop = false;
    }

    void OnGUI()
    {
        if (LugusDebug.debug)
        {
            if (GUI.Button(new Rect(0, 75, 75, 25), "Reset Ball"))
            {
                BallsInPlay[0].transform.position = GameObject.Find("UnstuckPos").transform.position.z(BallsInPlay[0].transform.position.z);
            }
			if (GUI.Button(new Rect(0, 200, 75, 25), "add 1000"))
			{
				ScoreManager.use.ShowScore(1000, Vector3.zero, 100, null, Color.white, gameObject);
			}
        }
    }

    public void PauseGame()
    {
        Time.timeScale = float.Epsilon;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0f;
    }
}
