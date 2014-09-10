using UnityEngine;
using System.Collections;

public class SkullBallCatch : MonoBehaviour
{
    private OpeningSkull _skull;

    void Start()
    {
        _skull = GameObject.Find("Skull_Opening").GetComponent<OpeningSkull>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ball")
            return;

        _skull.CloseSkull();
        _skull.Ball = other.gameObject.GetComponent<Ball>();

        other.transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
        other.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
