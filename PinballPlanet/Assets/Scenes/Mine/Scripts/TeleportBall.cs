using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TeleportBall : MonoBehaviour
{
    // List of exits a new ball can come from.
    public List<Transform> Exits; 

    // Ball prefab.
    public GameObject BallPrefab;

    // How hard the new balls launch.
    public float LaunchForce = 500;

    // Use this for initialization
    void Start()
    {
        if (BallPrefab == null)
            Debug.LogError("Ballprefab is null for multiball!");
    }

    void OnCollisionEnter(Collision collision)
    {
        // Return if not colliding with ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

        // Check if there are exits.
        if (Exits.Count == 0)
            return;
		
		Destroy(collision.collider.gameObject);
		//Player.use.DestroyBall(collision.collider.gameObject);

        // Spawn a new ball at each exit.
        foreach (Transform exit in Exits)
        {
            //Debug.Log("--- New ball! ---");
            
            GameObject newBall = Instantiate(BallPrefab, exit.position, exit.rotation) as GameObject;
            newBall.transform.position = newBall.transform.position.z(5);
            newBall.rigidbody.velocity = exit.up.normalized*LaunchForce;
            
            ScoreManager.use.AddBalls(1);
			
        }
    }
}
