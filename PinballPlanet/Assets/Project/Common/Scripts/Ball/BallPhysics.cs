using UnityEngine;

/// <summary>
/// This script controls the ball physics. We do a lot of things
/// each frame to try to guarantee that the ball won't go through
/// the flippers. The tactics we use include:
/// 
/// 1. Checking in advance for collisions
/// http://unifycommunity.com/wiki/index.php?title=DontGoThroughThings
/// 
/// 2. Limit velocity
/// http://vonlehecreative.wordpress.com/2010/02/02/unity-resource-velocitylimiter/
/// 
/// 3. If the ball is in a "flipper zone", cast a ray from behind the ball to the tangent 
/// of the flipper. If it strikes the flipper, that means it's behind the flipper and we
/// need to shoot it back up.
/// </summary>
public class BallPhysics : MonoBehaviour
{

    // The velocity at which drag should begin being applied.  
    public float DragStartVelocity;
    // The velocity at which drag should equal maxDrag.  
    public float DragMaxVelocity;
    // The maximum allowed velocity. The velocity will be clamped to keep  
    // it from exceeding this value. (Note: this value should be greater than  
    // or equal to dragMaxVelocity.)  
    public float MaxVelocity;
    // The maximum drag to apply. This is the value that will  
    // be applied if the velocity is equal or greater  
    // than dragMaxVelocity. Between the start and max velocities,  
    // the drag applied will go from 0 to maxDrag, increasing  
    // the closer the velocity gets to dragMaxVelocity.  
    public float MaxDrag = 1.0f;
    // The original drag of the object, which we use if the velocity  
    // is below dragStartVelocity.  
    private float _originalDrag;
    // Cache the rigidbody to avoid GetComponent calls behind the scenes.
    private Rigidbody _rb;
    // Cached values used in FixedUpdate  
    private float _sqrDragStartVelocity;
    private float _sqrDragVelocityRange;
    private float _sqrMaxVelocity;
    ////////////////////////////////////////////////////////////////

    // Variables from DontGoThroughThings.js /////////////////////////
    public LayerMask LayerMask; //make sure we aren't in this layer 
    public float SkinWidth = 0.1f; //probably doesn't need to be changed 
    private float _minimumExtent;
    private float _partialExtent;
    private float _sqrMinimumExtent;
    private Vector3 _previousPosition;
    private Rigidbody _myRigidbody;
    ////////////////////////////////////////////////////////////////

    private Collider _leftFlipperBuffer;
    private Collider _rightFlipperBuffer;
    private FlipperNew _leftFlipper;
    private FlipperNew _rightFlipper;
    private Transform _objTransform; // Cache the transform (optimization)

    private void DontGoThroughThings_Awake()
    {
        _myRigidbody = rigidbody;
        _previousPosition = _myRigidbody.position;
        _minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z);
        _partialExtent = _minimumExtent * (1.0f - SkinWidth);
        _sqrMinimumExtent = _minimumExtent * _minimumExtent;
    }

    private void DontGoThroughThings_FixedUpdate()
    {
        //have we moved more than our minimum extent? 
        Vector3 movementThisStep = _myRigidbody.position - _previousPosition;
        float movementSqrMagnitude = movementThisStep.sqrMagnitude;
        if (movementSqrMagnitude > _sqrMinimumExtent)
        {
            float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
            RaycastHit hitInfo;
            //check for obstructions we might have missed 
            if (Physics.Raycast(_previousPosition, movementThisStep, out hitInfo, movementMagnitude, LayerMask.value))
            {
                Debug.Log("DontGoThroughThings: Obstruction missed");
                _myRigidbody.position = hitInfo.point - (movementThisStep / movementMagnitude) * _partialExtent;
            }
        }
        _previousPosition = _myRigidbody.position;
    }

    private void VelocityLimiter_Awake()
    {
        _originalDrag = rigidbody.drag;
        _rb = rigidbody;
        VelocityLimiter_Initialize(DragStartVelocity, DragMaxVelocity, MaxVelocity, MaxDrag);
    }

    // Sets the threshold values and calculates cached variables used in FixedUpdate.  
    // Outside callers who wish to modify the thresholds should use this function. Otherwise,  
    // the cached values will not be recalculated.  
    private void VelocityLimiter_Initialize(float dragStartVelocity, float dragMaxVelocity, float maxVelocity, float maxDrag)
    {
        this.DragStartVelocity = dragStartVelocity;
        this.DragMaxVelocity = dragMaxVelocity;
        this.MaxVelocity = maxVelocity;
        this.MaxDrag = maxDrag;

        // Calculate cached values  
        _sqrDragStartVelocity = dragStartVelocity * dragStartVelocity;
        _sqrDragVelocityRange = (dragMaxVelocity * dragMaxVelocity) - _sqrDragStartVelocity;
        _sqrMaxVelocity = maxVelocity * maxVelocity;
    }

    private void VelocityLimiter_FixedUpdate()
    {
        var v = _rb.velocity;
        // We use sqrMagnitude instead of magnitude for performance reasons.  
        var vSqr = v.sqrMagnitude;

        if (vSqr > _sqrDragStartVelocity)
        {
            _rb.drag = Mathf.Lerp(_originalDrag, MaxDrag, Mathf.Clamp01((vSqr - _sqrDragStartVelocity) / _sqrDragVelocityRange));

            // Clamp the velocity, if necessary  
            if (vSqr > _sqrMaxVelocity)
            {
                // Vector3.normalized returns this vector with a magnitude  
                // of 1. This ensures that we're not messing with the  
                // direction of the vector, only its magnitude.  
                _rb.velocity = v.normalized * MaxVelocity;
            }
        }
        else
        {
            _rb.drag = _originalDrag;
        }
    }

    void Awake()
    {
        // Initialize DontGoThroughThings
        DontGoThroughThings_Awake();
        // Initialize VelocityLimiter
        VelocityLimiter_Awake();
        // Initialize our mandatory variables
        _leftFlipperBuffer = GameObject.Find("LeftFlipperBuffer").collider;
        _rightFlipperBuffer = GameObject.Find("RightFlipperBuffer").collider;
        Transform t = GameObject.Find("LeftFlipper").transform;
        _leftFlipper = t.GetComponent<FlipperNew>();
        t = GameObject.Find("RightFlipper").transform;
        _rightFlipper = t.GetComponent<FlipperNew>();
        _objTransform = transform;
    }

    void FixedUpdate()
    {
        if (rigidbody.isKinematic)
            return;

        // Throttle the velocity of the ball
        VelocityLimiter_FixedUpdate();
        // Check ahead for objects we may go through before the next frame, and reset our position
        // if necessary
        DontGoThroughThings_FixedUpdate();

        // Keep the ball at z=0 (I haven't been able to get a custom joint to do this adequately)
        //objTransform.position.z = 0;
        //_rb.velocity.z(0);

//        // If the ball is going downards, then we're OK to begin flipper correction checking
//        if (_rb.velocity.y != 0) // TODO WHUT
//        {
//            // Begin by seeing if the ball is within the "flipper buffer" which is a rectangular region of space near the flipper.
//            // No point in doing correction calculations if the ball is far away.	
//            int layerMask = 0;
//
//            if (_leftFlipperBuffer.bounds.Contains(_objTransform.position))
//            {
//                //if ((_leftFlipper.IsGoingToPressedPosition || _leftFlipper.IsAtRest) && Mathf.Abs(_leftFlipper.rigidbody.angularVelocity.z) > 2.0)
//                if (Mathf.Abs(_leftFlipper.rigidbody.angularVelocity.z) > 2.0f) 
//                {
//                    // We're near the left flipper and it's in motion. Set the layer mask to that of the correction tangents of the left flipper
//                    layerMask = _leftFlipper.transform.FindChild("FlipperTangent").gameObject.layer;
//                }
//            }
//            else if (_rightFlipperBuffer.bounds.Contains(_objTransform.position))
//            {
//                //if ((_rightFlipper.IsGoingToPressedPosition || _rightFlipper.IsAtRest) && Mathf.Abs(_rightFlipper.rigidbody.angularVelocity.z) > 2.0)
//                if (Mathf.Abs(_rightFlipper.rigidbody.angularVelocity.z) > 2.0f)
//                {
//                    // We're near the right flipper and it's in motion. Set the layer mask to that of the correction tangents of the right flipper
//                    layerMask = _rightFlipper.transform.FindChild("FlipperTangent").gameObject.layer;
//                }
//            }
//            //else if()
//
//            if (layerMask != 0)
//            {
//				layerMask = 1 << layerMask;
//				//Debug.Log("Layer " + layerMask);
//                //Debug.Log("*** Correction might be required. ***");
//                RaycastHit hitInfo;
//                // Cast a ray from behind the ball toward the tangent. If it hits, then try to put the ball above the flipper
//                if (Physics.Raycast(_previousPosition, Vector3.Normalize(_rb.velocity), out hitInfo, Mathf.Infinity, layerMask))
//                {
//                    Debug.Log(/*"Correction required. Ball at " + objTransform.position + */" collided with " + hitInfo.transform.name + " at " + hitInfo.point /*+ " v = " + rb.velocity*/);
//
//                    // Move the ball up to the tangent point
//                    // (c.haag 2011-02-28) - If you uncomment this out, sometimes the ball is jerked to a place it shouldn't
//                    // be. This is because the ball may have already started going in the opposite direction by a regular
//                    // Unity collision and the flipper tangent may not be in the direction it was when the ball actually penetrated
//                    // it by this juncture.
//                    //transform.position = hitInfo.point;
//
//                    // Now calculate the ball's new velocity. It should be a value that causes it to defelect off the normal
//                    // of the flipper.
//                    Vector3 surfaceNormal = hitInfo.normal;
//                    Vector3 ballRay = Vector3.Normalize(_rb.velocity);
//                    Vector3 angleOfReflection = Vector3.Reflect(ballRay, surfaceNormal);
//
//                    // Also apply an extra velocity so it doesn't stick or loiter around the boundary of the flipper
//                    //Vector3 extraVelocity = (_rb.velocity.y < 0 && _rb.velocity.y > -60) ? new Vector3(0, 500, 0) : new Vector3(0, 100, 0);
//
//					// Edit By Tom: This caused a bug making the ball launch when releasing an "up" flipper
//					Vector3 extraVelocity = new Vector3(0, 0, 0); 
//
//                    _rb.velocity = angleOfReflection * Vector3.Magnitude(_rb.velocity);
//                    // Ensure the current velocity is always positive
//                    if (_rb.velocity.y < 0) { 
//						_rb.velocity.y(-_rb.velocity.y); 
//					}
//                    // Now apply the extra velocity
//                    _rb.velocity += extraVelocity;
//                    //Debug.Log("New velocity: " + rb.velocity + " (Extra = " + extraVelocity + ")");
//                }
//
//            }
//        }
    }
}
