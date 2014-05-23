using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{

    public string CommandKey;	// The key to activate the flipper
    public float RestPosition = 0; // The z target rotation when at rest
    public float PressedPosition = -1; // The z target rotation when pressed
    public ConfigurableJoint ConfigurableJoint; // The configurable joint of the flipper

    public bool _isGoingToPressedPosition = false;

    /// Storing the list of balls touching the flipper collider this frame and last frame after flipper should have hit them.
    /// If balls that were touching last frame after the flipper hit are still touching this frame it means they're stuck.
    // Balls touching flipper this update.
    public List<GameObject> _ballsTouching = new List<GameObject>();
    // Balls touching last update.
    public List<GameObject> _ballsLastTouching = new List<GameObject>();

    public bool IsGoingToPressedPosition()
    {
        return _isGoingToPressedPosition;
    }

    public bool IsAtRest()
    {
        return (Vector3.Magnitude(rigidbody.angularVelocity) < 0.0001) ? true : false;
    }

    void GoToPressedPosition()
    {
        if (!_isGoingToPressedPosition)
        {
            Quaternion newRot = ConfigurableJoint.targetRotation;
            newRot.z = PressedPosition;
            ConfigurableJoint.targetRotation = newRot;
            _isGoingToPressedPosition = true;

            //if (_ballsTouching.Count > 0 && _ballsLastTouching.Count > 0)
            //{
            //    //Debug.Log("*** Balls: " + _ballsTouching.Count + " Balls Last: " + _ballsLastTouching.Count + " ***");

            //    // Check if any touching balls this frame were also touching last frame.
            //    foreach (GameObject ball in _ballsTouching)
            //    {
            //        if (_ballsLastTouching.Contains(ball))
            //            Debug.Log("*** Ball still touching?! ***");
            //    }
            //}
        }
    }

    void GoToRestPosition()
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
        if (Input.GetKey(CommandKey))
        {
            GoToPressedPosition();
        }
        else
        {
            GoToRestPosition();
        }

        _ballsLastTouching = new List<GameObject>(_ballsTouching);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Ball")
            return;

        _ballsTouching.Add(collision.collider.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name != "Ball")
            return;

        _ballsTouching.Remove(collision.collider.gameObject);
    }
}
