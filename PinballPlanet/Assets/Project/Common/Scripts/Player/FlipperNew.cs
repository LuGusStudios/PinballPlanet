using UnityEngine;
using System.Collections;

public class FlipperNew : MonoBehaviour
{
    public string InputKey;

    private JointMotor _motor;
    private JointMotor _reverseMotor;
    private float _force;

    public bool Pressed = false;
    private bool _up = false;

    // Use this for initialization
    void Start()
    {
        _motor = GetComponent<HingeJoint>().motor;
        _force = _motor.force;

        _reverseMotor = new JointMotor();
        _reverseMotor.force = _force;
        _reverseMotor.targetVelocity = -_motor.targetVelocity;
        _reverseMotor.freeSpin = _motor.freeSpin;
    }

    // FixedUpdate is called once per physics calculation.
    void FixedUpdate()
    {
		if (MenuManager.use.ActiveMenu != MenuManagerDefault.MenuTypes.GameMenu)
			return;


        if (Input.GetKey(InputKey) || Pressed)
        {
            GetComponent<HingeJoint>().motor = _reverseMotor;

            if (!_up)
            {
                if (name.Contains("Left"))
                    Player.use.PlayLeftFlipperSound();
                else
                    Player.use.PlayRightFlipperSound();
                _up = true;
            }
        }
        else
        {
            GetComponent<HingeJoint>().motor = _motor;
            _up = false;
        }
 
        Pressed = false;
    }

    public void SetPressed()
    {
        Pressed = true;

        if (!_up)
        {
            if (name.Contains("Left"))
                Player.use.PlayLeftFlipperSound();
            else
                Player.use.PlayRightFlipperSound();

            _up = true;
        }
    }
}
