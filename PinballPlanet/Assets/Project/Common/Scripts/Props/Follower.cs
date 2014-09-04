using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    public Transform ObjectToFollow;

    // Update is called once per frame
    void Update()
    {
        transform.position = ObjectToFollow.position;
    }
}
