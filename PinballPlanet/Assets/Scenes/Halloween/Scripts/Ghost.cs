using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    private float _timeAlive = 0;

    private float _flickerStart = 24.0f;
    private float _flickerDelay = 0.5f;
    private float _flickerDelayModifier = -0.015f;
    private float _flickerLength = 0.1f;
    private float _flickerMinEnd = 0.2f;

    private bool _shouldRender = false;

    // Called every frame.
    void Update()
    {
        bool oldRender = _shouldRender;

        _timeAlive += Time.deltaTime;

        if (_timeAlive > _flickerStart)
        {
            float totalFlicker = _flickerDelay + _flickerLength;
            float flickerValue = totalFlicker - (_timeAlive % totalFlicker);

            if (flickerValue < _flickerLength)
            {
                _shouldRender = false;
            }
            else
            {
                _shouldRender = true;
            }
        }

        // Only set when changed.
        if (_shouldRender != oldRender)
        {
            transform.GetChild(0).GetChild(0).renderer.enabled = _shouldRender;

            if (_shouldRender)
            {
                _flickerDelay += _flickerDelayModifier;
            }
        }

        // If flicker too fast, destroy ghost.
        if (_flickerDelay <= _flickerMinEnd)
        {
            Destroy(GetComponent<Follower>().ObjectToFollow.gameObject);
            Destroy(gameObject);
        }
    }

    // Old Ghost functionality.
    //-------------------------
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag != "Ball")
    //        return;

    //    // Give Ghostball.
    //    foreach (GameObject ball in Player.use.BallsInPlay)
    //    {
    //        ball.renderer.material = GhostBallMaterial;
    //    }

    //    // Enable Multiball.
    //    GameObject.Find("MultiBall").GetComponent<MultiBall>().ActivateMultiBall();

    //    // Allow ball through wall.
    //    GameObject.Find("Ghost_Wall").collider.enabled = false;

    //    // Go to Wall.
    //    GetComponent<Follower>().ObjectToFollow.GetComponent<GhostPathFollower>().WallMove();
    //}
}
