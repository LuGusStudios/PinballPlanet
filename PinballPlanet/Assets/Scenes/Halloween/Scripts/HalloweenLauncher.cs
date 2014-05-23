using UnityEngine;
using System.Collections;

public class HalloweenLauncher : MonoBehaviour
{
    // Original scale.
    private float _scale = 12;

    // Use this for initialization
    void Start()
    {
        _scale = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, Mathf.Lerp(_scale, 5, Player.use.BallLaunchForce / Player.use.LaunchMaxForce), scale.z); 
    }
}
