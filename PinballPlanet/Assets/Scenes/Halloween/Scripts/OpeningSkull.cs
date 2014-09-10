using UnityEngine;
using System.Collections;

public class OpeningSkull : BreakableMultiObjective
{
    private bool _opened = false;
    public Ball Ball;

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
            iTween.RotateAdd(this.gameObject,
                iTween.Hash("amount", new Vector3(-50, 0, 0),
                            "time", 2.0f,
                            "isLocal", true));

            _opened = true;
        }

    }

    public void CloseSkull()
    {
        if (_opened)
        {
            iTween.Stop(this.gameObject);
            iTween.RotateAdd(this.gameObject,
                    iTween.Hash("amount", new Vector3(50, 0, 0),
                                "time", 1.0f,
                                "isLocal", true,
                                "oncomplete", "OnSkullClosed"));

            _opened = false;
        }
    }

    void OnSkullClosed()
    {
        iTween.RotateTo(gameObject,
            iTween.Hash("rotation", GameObject.Find("Skull_End").transform.eulerAngles,
                        "time", 2.0f,
                        "oncomplete", "OnSkullAimed"));
    }

    void OnSkullAimed()
    {
        Ball.rigidbody.velocity = Vector3.zero;
        Ball.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

        GameObject.Find("CollisionBox_Skull").collider.enabled = false;

        Transform shooter = GameObject.Find("Skull_BallShoot").transform;
        Ball.transform.position = new Vector3(shooter.position.x, shooter.position.y, Ball.transform.position.z);

        Vector3 randVec = Vector3.zero.zAdd(Random.RandomRange(-10, 10));
        Ball.rigidbody.AddForce((shooter.up.normalized + randVec) * 3000);

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
