using UnityEngine;
using System.Collections;

public class BallShooter : MonoBehaviour
{
    // Animation start/end transform.
    private Transform _animStartTransform;
    private Transform _animEndTransform;

    // Animation Speed.
    public float AnimPos = 0;
    public float AnimSpeed = 0.1f;
    public float AnimAcceleration = 0.01f;
    public float AnimMaxSpeed = 10;
    
    // Use this for initialization
    void Start()
    {
        _animStartTransform = transform.parent.FindChild("BallLaunch_Left");
        _animEndTransform = transform.parent.FindChild("BallLaunch_Right");

        Debug.Log(transform.parent.name);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = Vector3.Lerp(_animStartTransform.position, _animEndTransform.position, Mathf.PingPong(AnimPos, 1));

        AnimSpeed += AnimSpeed * Time.deltaTime * AnimAcceleration;
        AnimPos += AnimSpeed;

        if (AnimSpeed > AnimMaxSpeed)
            AnimSpeed = AnimMaxSpeed;
    }
}
