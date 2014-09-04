using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{
    public Material GhostBallMaterial;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ball")
            return;

        // Give Ghostball.
        foreach (GameObject ball in Player.use.BallsInPlay)
        {
            ball.renderer.material = GhostBallMaterial;
        }

        // Enable Multiball.
        GameObject.Find("MultiBall").GetComponent<MultiBall>().ActivateMultiBall();

        // Allow ball through wall.
        GameObject.Find("Ghost_Wall").collider.enabled = false;

        // Go to Wall.
        GetComponent<Follower>().ObjectToFollow.GetComponent<GhostPathFollower>().WallMove();
    }
}
