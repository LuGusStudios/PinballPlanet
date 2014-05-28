using UnityEngine;
using System.Collections;

public class Log : MonoBehaviour
{
    // Called when another object collides.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Ball")
            return;

        transform.parent.GetComponent<Logs>().Break();
    }
}
