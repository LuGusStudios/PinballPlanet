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

    void Start()
    {
        _originalPos = transform.position;
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

        transform.parent = JumpTarget.transform;
        transform.localPosition = Vector3.zero;

        Destroy(gameObject, StickTime);
    }
}
