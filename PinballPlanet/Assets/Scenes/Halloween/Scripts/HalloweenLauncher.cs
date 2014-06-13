using UnityEngine;
using System.Collections;

public class HalloweenLauncher : MonoBehaviour
{
    // Original scale.
    private float _scale = 1;

    // Use this for initialization
    void Start()
    {
        _scale = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
		transform.localScale = new Vector3(scale.x, scale.y, Mathf.Lerp(_scale, 1, Player.use.BallLaunchForce / Player.use.LaunchMaxForce)); 
    }
}
