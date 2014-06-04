using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public string CommandKey;	// The key to activate the flipper
    public float RestPosition = 0; // The z target rotation when at rest
    public float PressedPosition = -1; // The z target rotation when pressed
    public ConfigurableJoint ConfigurableJoint; // The configurable joint of the flipper

    ///// Storing the list of balls touching the flipper collider this frame and last frame after flipper should have hit them.
    ///// If balls that were touching last frame after the flipper hit are still touching this frame it means they're stuck.
    //// Balls touching flipper this update.
    //public List<GameObject> BallsTouching = new List<GameObject>();
    //// Balls touching last update.
    //public List<GameObject> BallsLastTouching = new List<GameObject>();

    private bool _isGoingToPressedPosition = false;
    public bool IsGoingToPressedPosition
    {
        get
        { return _isGoingToPressedPosition; }
    }

    public bool IsAtRest
    {
        get 
        { return (Vector3.Magnitude(rigidbody.angularVelocity) < 0.0001) ? true : false; }
    }

    public bool TouchPressed = false;

    public void GoToPressedPosition()
    {
        if (!_isGoingToPressedPosition)
        {
            Quaternion newRot = ConfigurableJoint.targetRotation;
            newRot.z = PressedPosition;
            ConfigurableJoint.targetRotation = newRot;
            _isGoingToPressedPosition = true;

            if(name.Contains("Left"))
                Player.use.PlayLeftFlipperSound();
            else
                Player.use.PlayRightFlipperSound();
        }
    }

    public void GoToRestPosition()
    {
        if (_isGoingToPressedPosition)
        {
            Quaternion newRot = ConfigurableJoint.targetRotation;
            newRot.z = RestPosition;
            ConfigurableJoint.targetRotation = newRot;
            _isGoingToPressedPosition = false;
        }
    }

    void FixedUpdate()
    {
        // Respond to keystrokes.
        if (Input.GetKey(CommandKey) || TouchPressed)
        {
            GoToPressedPosition();
        }
        else
        {
            GoToRestPosition();
        }

        TouchPressed = false;

        //BallsLastTouching = new List<GameObject>(BallsTouching);
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.name != "Ball")
    //        return;

    //    BallsTouching.Add(collision.collider.gameObject);
    //}

    //void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider.name != "Ball")
    //        return;

    //    BallsTouching.Remove(collision.collider.gameObject);
    //}
}
