using UnityEngine;
using System.Collections;

public class OpeningSkull : BreakableMultiObjective
{
    private bool _opened = false;
    public Ball Ball;

    private Vector3 _closedEulAngles;
    private Vector3 _openEulAngles;

    private AudioClip _spitSound;

    protected override void Start()
    {
        // Store animation rotations.
        _closedEulAngles = gameObject.transform.eulerAngles;
        _openEulAngles = GameObject.Find("Skull_Open").transform.eulerAngles;

        // Load sounds.
        _spitSound = LugusResources.use.Shared.GetAudio("SkullBallSpit01");

        base.Start();
    }

    public override void Activate()
    {
        OpenSkull();

        collider.enabled = false;
        GameObject.Find("Skull_BallCatch").collider.enabled = true;
    }

    public override void Unbreak()
    {
        collider.enabled = true;
        GameObject.Find("Skull_BallCatch").collider.enabled = false;

        base.Unbreak();
    }

    public void OpenSkull()
    {
        if (!_opened)
        {
            iTween.Stop(this.gameObject);
            iTween.RotateTo(this.gameObject,
                iTween.Hash("rotation", _openEulAngles,
                            "time", 2.0f));

            _opened = true;
        }
    }

    public void CloseSkull()
    {
        // Close skull.
        if (_opened)
        {
            iTween.Stop(this.gameObject);
            iTween.RotateTo(this.gameObject,
                    iTween.Hash("rotation", _closedEulAngles,
                                "time", 1.0f,
                                "oncomplete", "OnSkullClosed"));

            _opened = false;
        }
    }

    void OnSkullClosed()
    {
        // Rotate to target.
        iTween.RotateTo(gameObject,
            iTween.Hash("rotation", GameObject.Find("Skull_End").transform.eulerAngles,
                        "time", 2.0f,
                        "oncomplete", "OnSkullAimed"));
    }

    void OnSkullAimed()
    {
        // Unfreeze ball.
        Ball.rigidbody.velocity = Vector3.zero;
        Ball.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        // Disable collider.
        GameObject.Find("CollisionBox_Skull").collider.enabled = false;

        // Set ball to new position.
        Transform shooter = GameObject.Find("Skull_BallShoot").transform;
        Ball.transform.position = new Vector3(shooter.position.x, shooter.position.y, Ball.transform.position.z);

        // Shoot ball with smal random deviation.
        Vector3 randVec = Vector3.zero.zAdd(Random.Range(-10, 10));
        Ball.rigidbody.AddForce((shooter.up.normalized + randVec) * 3000);

        // Play shoot sound.
        LugusAudio.use.SFX().Play(_spitSound);

        // Rotate back to original position.
        iTween.RotateTo(gameObject,
            iTween.Hash("rotation", GameObject.Find("Skull_Start").transform.eulerAngles,
                    "time", 2.0f,
                    "oncomplete", "OnSkullReset"));
    }

    void OnSkullReset()
    {
        GameObject.Find("CollisionBox_Skull").collider.enabled = true;

        Ball = null;

        Invoke("Unbreak", ResetDelay);
    }
}
