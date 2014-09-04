using UnityEngine;
using System.Collections;

public class HalloweenLauncher : MonoBehaviour
{
    // Animation start/end transform.
    private Transform _animStartTransform;
    private Transform _animEndTransform;

    // Animation Speed.
    public float AnimPos = 0;
    public float AnimSpeed;
    public float AnimMinSpeed = 0;
    public float AnimMaxSpeed = 0.2f;

    // Ball.
    private Ball _ball;

    // Use this for initialization
    void Start()
    {
        _animStartTransform = transform.parent.FindChild("BallLaunch_Left");
        _animEndTransform = transform.parent.FindChild("BallLaunch_Right");
    }

    // Notifies that the ball just touched launcher.
    public void BallTouched(Ball ball)
    {
        _ball = ball;

        // Make ball invisible and show launcher.
        _ball.renderer.enabled = false;
        _ball.TrailParticle.GetComponent<ParticleSystem>().active = false;

        gameObject.renderer.enabled = true;

        // Disable Multiball.
        GameObject.Find("MultiBall").GetComponent<MultiBall>().DeactivateMultiBall();

        // Disallow ball through wall.
        GameObject.Find("Ghost_Wall").collider.enabled = true;

        Player.use.BallLaunchForce = 0;
    }

    // Notifies that the ball just exited launcher.
    public void BallExit()
    {
        // Make ball visible and hide launcher.
        _ball.renderer.enabled = true;
        _ball.TrailParticle.GetComponent<ParticleSystem>().active = true;

        gameObject.renderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(_animStartTransform.position, _animEndTransform.position, Mathf.PingPong(AnimPos, 1));

        AnimSpeed = Mathf.Lerp(AnimMinSpeed, AnimMaxSpeed, Player.use.BallLaunchForce / Player.use.LaunchMaxForce);
        AnimPos += AnimSpeed;

        if (AnimSpeed > AnimMaxSpeed)
            AnimSpeed = AnimMaxSpeed;
    }
}
