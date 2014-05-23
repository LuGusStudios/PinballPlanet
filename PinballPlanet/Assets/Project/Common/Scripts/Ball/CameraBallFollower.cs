using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that makes the camera follow the ball up and down the field.
/// </summary>
public class CameraBallFollower : MonoBehaviour
{
    // If the ball goes past this point, the camera will start following it.
    public Transform CameraFollowStart;
    // If the ball reaches this point, it will stop following.
    public Transform CameraFollowEnd;

    // The point from which the camera will start.
    private Vector3 _cameraStartPos;
    // The furthest point to which the camera follows.
    public Transform CameraEndPos;

    // Use this for initialization.
    void Start()
	{
        // Store the camera start position.
        _cameraStartPos = transform.position;
	}

    // Update is called once per frame.
    void Update()
    {
        List<GameObject> balls = Player.use.BallsInPlay;
        //Debug.Log("--- Balls: " + balls.Count + " ---");
        
        // Put the camera at the original position if there are multiple balls or none.
        if (balls.Count != 1)
        {
            transform.position = _cameraStartPos;
        }
        // Move between the start and end position.
        else
        {
            // Distance between follow points.
            float dist = CameraFollowEnd.position.y - CameraFollowStart.position.y;
            // Distance of ball from start follow point.
            float relDist = balls[0].transform.position.y - CameraFollowStart.position.y;
            // New y position.
            transform.position = Vector3.Lerp(_cameraStartPos, CameraEndPos.position, relDist / dist);
        }
    }
}
