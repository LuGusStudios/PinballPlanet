using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MultiBall : MonoBehaviour
{
    // List of exits a new ball can come from.
    public List<Transform> Exits; 

    // Ball prefab.
    public GameObject BallPrefab;

    // Can only start multiball once activated.
    public bool Activated = false;

    // How hard the new balls launch.
    public float LaunchForce = 500;

    // Use this for initialization
    void Start()
    {
        if (BallPrefab == null)
            Debug.LogError("Ballprefab is null for multiball!");
    }

    // Activate the multiball.
    public void ActivateMultiBall()
    {
        //Debug.Log("--- Multiball active. ---");
        
        Activated = true;
        //collider.enabled = true;
    }

    // Deactivate the multiball.
    public void DeactivateMultiBall()
    {
        //Debug.Log("--- Multiball inactive. ---");
        
        Activated = false;
        //collider.enabled = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Return if not activated.
        if (!Activated)
            return;

        // Return if not colliding with ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

        SpawnBalls();
    }

    void OnTriggerEnter(Collider other)
    {
        // Return if not activated.
        if (!Activated)
            return;

        // Return if not colliding with ball.
        if (other.gameObject.tag != "Ball")
            return;

        SpawnBalls();
    }

    void SpawnBalls()
    {
        // Check if there are exits.
        if (Exits.Count == 0)
            return;

        // Spawn a new ball at each exit.
        foreach (Transform exit in Exits)
        {
            GameObject newBall = Instantiate(BallPrefab, exit.position, exit.rotation) as GameObject;
            //Debug.Log("New ball at: " + newBall.transform.position + " from " + exit.position);
            //newBall.transform.position = newBall.transform.position.z(5);
            newBall.rigidbody.velocity = exit.up.normalized * LaunchForce;
            ScoreManager.use.AddBalls(1);
        }

        // Deactivate multiball untill it's reacivated.
        DeactivateMultiBall();
    }
}
