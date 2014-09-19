using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour
{
    // Target to jump to.
    public GameObject JumpTarget;
    private Vector3 _originalPos;

    // Time jumping.
    public float JumpTime = 1.0f;
    private float _timeJumping;

    // Time stuck to camera.
    public float StickTime = 1.0f;

    // Animator.
    private Animator _animator;

    void Start()
    {
        _originalPos = transform.position;

        _animator = transform.GetChild(0).GetComponent<Animator>();

        //JumpTarget = GameObject.Find("Spider_Target");
        //Stick();

        // Play idle animation.
        _animator.Play("OpenLegs", 0);
    }

    void Update()
    {
        if (_timeJumping > 0)
        {
            _timeJumping += Time.deltaTime;

            transform.position = Vector3.Lerp(_originalPos, JumpTarget.transform.position, _timeJumping / JumpTime);
        }
    }

    // Make spider jump to target.
    public void Jump()
    {
        _timeJumping = Time.deltaTime;

        iTween.RotateTo(gameObject, JumpTarget.transform.rotation.eulerAngles, JumpTime);

        Invoke("Stick", JumpTime);
    }

    // Make spider stick to target.
    void Stick()
    {
        _timeJumping = 0;

        // Set position to camera target.
        transform.parent = JumpTarget.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // Remove spider after delay.
        Destroy(gameObject, StickTime);

        // Play animation.
        _animator.Play("OnFace", 0);
    }
}
