using UnityEngine;

public class Ball : MonoBehaviour
{
    // Pause
    public bool Paused = false;
    private Vector3 _pausedVelocity;

    // True if ball is touching the launcher.
    public bool TouchingLauncher = false;

    // Ball Fading.
    public float FireFadeMinSpeed = 300.0f;
    public float FireFadeMaxSpeed = 500.0f;

    // Height under which ball is destroyed.
    public float KillHeight = 0;

    // Particle trail that follows ball if it's over a certain speed.
    public GameObject TrailParticle;

	private float minSqrDistForIdle = 1f;
	private int maxIdleFrames = 200;
	private int idleFramesCount = 0;
	private Vector3 prevPos = Vector3.zero;
	private Vector3 avgPos = Vector3.zero;
	private Vector3 prevAvgPos = Vector3.zero;
	private Vector3 dist = Vector3.zero;

	private float flipperPositionY = 0;

    void Start()
    {
        TrailParticle = transform.GetChild(0).gameObject;
		Bounds flipperBounds = GameObject.Find("Flippers_New/LeftFlipperBuffer").collider.bounds;
		flipperPositionY = flipperBounds.center.y + flipperBounds.extents.y;
		//Debug.LogError("FlipperPosY = " + flipperPositionY);
    }

    void Update()
    {
        if (TrailParticle == null)
            return;

        // alpha = (value - min) / (max - min)
        float lerpValue = (rigidbody.velocity.magnitude - FireFadeMinSpeed) / (FireFadeMaxSpeed - FireFadeMinSpeed);
        float alpha = Mathf.Lerp(0, 1, lerpValue);

        // Only show particles when above a certain speed.
        TrailParticle.GetComponent<ParticleSystem>().startColor = new Color(1, 1, 1, alpha);

		bool ballIsTooLowToReset = transform.position.y < flipperPositionY;
		//Debug.LogError("BallTooLow = " + ballIsTooLowToReset);

		// Test if ball is stuck (idle)
		if (!TouchingLauncher && !ballIsTooLowToReset)
		{
			avgPos = (transform.position + prevPos)*0.5f;
			dist = prevAvgPos - avgPos;

			prevAvgPos = avgPos;
			prevPos = transform.position;

			if (dist.sqrMagnitude < minSqrDistForIdle)
			{	
				idleFramesCount++;
				if (idleFramesCount > maxIdleFrames)
				{
					transform.position = GameObject.Find("UnstuckPos").transform.position.z(transform.position.z);
					FixedIdleBall();
				} 
			}
			else 
			{
				idleFramesCount = 0;
			}
		}
    }

	public void FixedIdleBall()
	{
		idleFramesCount = 0;
	}

    public void OnPause()
    {
        //Debug.LogError("BALL PAUSE");
        Paused = true;
        _pausedVelocity = rigidbody.velocity;
        rigidbody.isKinematic = true;
    }

    public void OnUnpause()
    {
        //Debug.LogError("BALL UNPAUSE");
        Paused = false;
        rigidbody.isKinematic = false;
        rigidbody.velocity = _pausedVelocity;
        rigidbody.WakeUp();
    }

    // Destroy the ball if it gets too far down below the flippers (meaning it's obviously lost)
    void LateUpdate()
    {
        if (transform.position.y < KillHeight)
        {
            GameObject p = GameObject.Find("Player");
            Player player = p.GetComponent<Player>();
            player.DestroyBall(this);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Launcher")
        {
            TouchingLauncher = true;
            GameObject.Find("GameMenu").GetComponent<StepGameMenu>().ShowLaunchHelp(true);  
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Launcher")
            TouchingLauncher = false;
    }
}
