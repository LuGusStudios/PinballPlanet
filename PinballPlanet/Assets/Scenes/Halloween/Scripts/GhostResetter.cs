using UnityEngine;
using System.Collections;

public class GhostResetter : MonoBehaviour
{
    private OpeningSkull _skull;
    private GameObject _wall;
    public Material OriginalBallMaterial;

    void Start()
    {
        _skull = GameObject.Find("Skull_Opening").GetComponent<OpeningSkull>();
        _wall = GameObject.Find("Ghost_Wall");
    }

    void OnCollisionEnter(Collision collision)
    {
        Reset();
    }

    void OnTriggerEnter(Collider other)
    {
        Reset();
    }

    void Reset()
    {
        // Reset skull.
        _skull.Unbreak();

        // Reset graves.
        foreach (var objective in _skull.Objectives)
        {
            objective.Unbreak();
        }

        // Reset ghost.
        Destroy(GameObject.Find("Ghost_Parent(Clone)"));
        Destroy(GameObject.Find("Ghost_PathFollower(Clone)"));

        // Reset wall.
        Invoke("EnableWall", 1.0f);

        // Reset ball.
        Player.use.BallsInPlay[0].renderer.material = OriginalBallMaterial;
    }

    void EnableWall()
    {
        _wall.collider.enabled = true;
    }
}
