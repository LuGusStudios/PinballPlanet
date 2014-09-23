using UnityEngine;
using System.Collections;

public class Pentagram : BreakableMultiObjective
{
    // Ghost to spawn.
    public GameObject Ghost;
    public GameObject PathFollower;

    // Use this for initialization
    protected override void Start()
    {
        // Hide Pentagram.
        renderer.enabled = false;
        transform.FindChild("Light").GetComponent<Light>().enabled = false;

        base.Start();
    }

    public override void Activate()
    {
        // Show Pentagram.
        renderer.enabled = true;
        transform.FindChild("Light").GetComponent<Light>().enabled = true;

        // Give bonus score for completing all objectives.
        Vector3 scorePos = transform.position + new Vector3(-20,0);
        GameObject scorePopup = ScoreManager.use.ShowScore(BonusPoints, scorePos.zAdd(20), 1.5f, null, Color.white, gameObject);
        scorePopup.transform.localScale = new Vector3(1.5f,1.5f,1.5f);

        // Spawn ghost.
        GameObject ghost = Instantiate(Ghost) as GameObject;
        GameObject follower = Instantiate(PathFollower) as GameObject;

        // Set ghost follow.
        ghost.GetComponent<Follower>().ObjectToFollow = follower.transform;
    }

    public override void Unbreak()
    {
        // Hide Pentagram.
        renderer.enabled = false;
        transform.FindChild("Light").GetComponent<Light>().enabled = false;

        base.Unbreak();
    }
}
