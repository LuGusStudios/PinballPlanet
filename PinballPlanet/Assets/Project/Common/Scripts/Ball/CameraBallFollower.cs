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

        Vector3 targetPos;

        // Put the camera at the original position if there are multiple balls or none.
        if (balls.Count != 1)
        {
            if (PlayerData.use.camMode == CameraMode.Smooth)
            {
                targetPos = _cameraStartPos;
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5);
            }
            else
            {
                transform.position = _cameraStartPos;
            }
            
        }
        // Move between the start and end position.
        else
        {
            if (PlayerData.use.camMode == CameraMode.Instant)
            {
                MoveInstant(balls[0]);
            }
            else if (PlayerData.use.camMode == CameraMode.Smooth)
            {
                MoveSmooth(balls[0]);
            }
            else if (PlayerData.use.camMode == CameraMode.Fixed)
            {
                MoveFixed();
            }
            else
            {
                MoveInstant(balls[0]);
            }

            //// Distance between follow points.
            //float dist = CameraFollowEnd.position.y - CameraFollowStart.position.y;
            //// Distance of ball from start follow point.
            //float relDist = balls[0].transform.position.y - CameraFollowStart.position.y;
            //// New y position.            
            ////transform.position = Vector3.Lerp(_cameraStartPos, CameraEndPos.position, relDist / dist);
            //targetPos = Vector3.Lerp(_cameraStartPos, CameraEndPos.position, relDist / dist);
            
        }

        //transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*5);
    }


    void MoveInstant(GameObject ball)
    {
        float dist = CameraFollowEnd.position.y - CameraFollowStart.position.y;             
        float relDist = ball.transform.position.y - CameraFollowStart.position.y;
        transform.position = Vector3.Lerp(_cameraStartPos, CameraEndPos.position, relDist / dist);        
    }

    void MoveSmooth(GameObject ball)
    {
        float dist = CameraFollowEnd.position.y - CameraFollowStart.position.y;
        float relDist = ball.transform.position.y - CameraFollowStart.position.y;        
        Vector3 targetPos = Vector3.Lerp(_cameraStartPos, CameraEndPos.position, relDist / dist);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5);
    }

    void MoveFixed()
    {
        transform.position = _cameraStartPos;
    }
}
