using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour
{
    // The end and start point where the player was dragging.
    private Vector3 _dragStart;
    private Vector3 _dragEnd;

    // How far the player needs to drag to launch with max force.
    public float MaxDragDistance = 250;

    // Transforms of often accessed game objects.
    private Transform _leftFlipperTransform;
    private Transform _rightFlipperTransform;
    private Transform _leftFlipperClickBoxTransform;
    private Transform _rightFlipperClickBoxTransform;
    private Transform _mainCameraTransform;

    // Initialization.
    void Start()
    {
        _leftFlipperTransform = GameObject.Find("LeftFlipper").transform;
        _rightFlipperTransform = GameObject.Find("RightFlipper").transform;
        _leftFlipperClickBoxTransform = GameObject.Find("ClickBox_LeftFlipper").transform;
        _rightFlipperClickBoxTransform = GameObject.Find("ClickBox_RightFlipper").transform;
        _mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    // Called every fixed frame.
    void FixedUpdate()
    {
        // Check if mouse/touch is being held.
        if (LugusInput.use.down || LugusInput.use.dragging)
        {
            // Store the clicked/touched object.
            Transform clickedObject = LugusInput.use.RayCastFromMouse(_mainCameraTransform.GetComponent<Camera>());

            if (clickedObject != null)
            {
                // Move flippers if touched/clicked.
                if (clickedObject == _leftFlipperClickBoxTransform)
                    _leftFlipperTransform.GetComponent<Flipper>().TouchPressed = true;
                if (clickedObject == _rightFlipperClickBoxTransform)
                    _rightFlipperTransform.GetComponent<Flipper>().TouchPressed = true;
            }
        }
    }

    // Called every frame.
    void Update()
    {
        // Only allow launch when ball is ready.
        if (!Player.use.IsSingleBallReadyForLaunch())
            return;

        // Store where player started dragging.
        if (LugusInput.use.down)
        {
            _dragStart = LugusInput.use.lastPoint;
        }

        // Store where player started dragging.
        if (LugusInput.use.dragging)
        {
            // Calculate how far was dragged.
            float dragDist = LugusInput.use.lastPoint.y - _dragStart.y;

            // Only check for downwards dragging.
            if (dragDist < 0)
            {
                // Set launch force.
                Player.use.BallLaunchForce = Mathf.Lerp(0, Player.use.LaunchMaxForce, Mathf.Abs(dragDist) / MaxDragDistance);
            }
        } 

        // Calculate how hard to launch the ball.
        if (LugusInput.use.up)
        {
            _dragEnd = LugusInput.use.lastPoint;

            // Calculate how far was dragged.
            float dragDist = _dragEnd.y - _dragStart.y;

            // Only check for downwards dragging.
            if (dragDist < 0)
            {
                // Set launch force.
                Player.use.BallLaunchForce = Mathf.Lerp(0, Player.use.LaunchMaxForce, Mathf.Abs(dragDist) / MaxDragDistance);
            }

            //// Launch the ball.
            //Player.use.LaunchBall();
        }
    }
}
