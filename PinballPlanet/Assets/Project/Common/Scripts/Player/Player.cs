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

    // The force when launching the ball.
    public float LaunchMaxForce;
    public float LaunchAnimationTime;

    // How hard the ball launches.
    private float _ballLaunchForce;
    public float BallLaunchForce
    {
        get { return _ballLaunchForce; }
        private set { _ballLaunchForce = value; }
    }

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
        if (ScoreManager.use.BallCount <= 0)
        {
            //GameObject.Find("GOD").GetComponent<KetnetController>().AddScore(totalScore);
            GameObject.Find("JESUS").GetComponent<UIGameController>().ShowGameoverGUI();

            return null;
        }

        // Now create a new ball
        GameObject newball;
        newball = (GameObject)Instantiate(BallPrefab, BallLaunch.transform.position + new Vector3(0, 25, 0), Quaternion.identity);
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

        // Update our ball array so other objects can quickly do a seek on all balls
        UpdateBallArray();
        // Only release a ball if there's no ball in play
        if (BallsInPlay.Count == 0)
        {
            // Play release sound.
            if (audio != null)
            {
                audio.loop = false;
                audio.Stop();
                audio.PlayOneShot(BallLostSound);
            }

            // Release ball after short while.
            ReleaseBallDelayed();
        }
        //else
        //{
        //    Debug.Log("--- Balls remaining: " + BallsInPlay.Count + " ---");
        //}

        //Debug.Log("Ball Destroyed! nr balls remaining : " + ballsInPlay.Length );
        ScoreManager.use.BallDestroyed();

        return true;
    }

    void Start()
    {
        ReleaseBall();
    }

    protected virtual void Update()
    {
        if (Paused)
            return;

        //GameObject ball;

        //Debug.Log("--- Ball Ready for launch: " + IsSingleBallReadyForLaunch() + " ---");

        // Control the launch spring
        if (IsSingleBallReadyForLaunch())
        {
            ControlBallLaunchByKeyboard();
        }
        else
        {
            // Spring resets back to normal position
            ResetSpring();
        }
    }

    private bool IsSingleBallReadyForLaunch()
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

                //ballLaunch.localScale.z = Mathf.Lerp(SpringMaxScale, SpringMinScale, BallLaunchForce / LaunchMaxForce);
            }

            if (audio != null)
            {
                audio.clip = LaunchSound;
                audio.loop = false;
                audio.Play();
            }
        }
        else
        {
            ResetSpring();
        }

        // Release the ball
        if (Input.GetAxis("Vertical") >= 0)
        {
            //var flowBackPreventer = GameObject.Find("PreventFlowbackCollider");
            //flowBackPreventer.collider.isTrigger = true;

            var ball = GameObject.Find("Ball");
            if (ball != null)
            {
                Ball ballScript = ball.GetComponent<Ball>();
                if (ballScript.TouchingLauncher && _ballLaunchForce > 0)
                {
                    //Debug.Log("Launch force: " + BallLaunchForce);
                    ball.rigidbody.AddForceAtPosition(new Vector3(0, _ballLaunchForce, 0), ball.transform.position);
                    // Reset launch force.
                    _ballLaunchForce = 0;

                    // Spawn fire particles.
                    //Instantiate(FireParticlesPrefab, transform.position, Quaternion.identity);
                    Instantiate(LaunchParticlesPrefab);

                    // Play release sound.
                    if (audio != null)
                    {
                        audio.loop = false;
                        audio.Stop();
                        audio.PlayOneShot(ReleaseSound);
                    }
                }
            }
        }
    }

    private void ResetSpring()
    {
        //// Reset the launch trigger.
        //var launchTrigger = GameObject.Find("Blunderbuss_Trigger01");
        //launchTrigger.transform.rotation = TriggerStartTransform.rotation;

        //    if (ballLaunch.localScale.z < SpringMaxScale) 
        //	{
        //		ballLaunch.localScale.z = SpringMaxScale;
        //	}		
    }

    // Play light on sound.
    public void PlayLightOnSound()
    {
        if (audio != null)
        {
            audio.loop = false;
            //audio.Stop();
            audio.PlayOneShot(LightOnSound);
        }
    }

    // Play light off sound.
    public void PlayLightOffSound()
    {
        if (audio != null)
        {
            audio.loop = false;
            //audio.Stop();
            audio.PlayOneShot(LightOffSound);
        }
    }
}
