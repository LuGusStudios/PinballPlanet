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

    void Start()
    {
        TrailParticle = transform.GetChild(0).gameObject;
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
