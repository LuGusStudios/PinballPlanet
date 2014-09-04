using UnityEngine;
using System.Collections;

public class OpeningSkull : BreakableMultiObjective
{
    public float OpenDelay = 0.5f;

    // Ghost to spawn.
    public GameObject Ghost;
    public GameObject PathFollower;

    private bool _opened = false;

    // Called when all subobjectives break.
    public override void Activate()
    {
        // Add delay to activation.
        Invoke("OpenSkull", OpenDelay);
    }

    public override void Unbreak()
    {
        // Close Animation.
        CloseSkull();

        // Enable collision.
        transform.GetChild(0).collider.enabled = true;

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

        // Disable collision.
        transform.GetChild(0).collider.enabled = false;

        // Spawn ghost.
        GameObject ghost = Instantiate(Ghost) as GameObject;
        GameObject follower = Instantiate(PathFollower) as GameObject;

        // Set ghost follow.
        ghost.GetComponent<Follower>().ObjectToFollow = follower.transform;

        // Reset after a while.
        //Invoke("Unbreak", ResetDelay);
    }

    public void CloseSkull()
    {
        if (_opened)
        {
            iTween.Stop(this.gameObject);
            iTween.RotateAdd(this.gameObject,
                    iTween.Hash("amount", new Vector3(50, 0, 0),
                                "time", 2.0f,
                                "isLocal", true));

            _opened = false;
        }
    }
}
