using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour
{
    private bool _dragging = false;
    private Vector3 _dragStart;
    private Vector3 _dragEnd;

    public float MaxDragDistance = 250;

    private void FixedUpdate()
    {
        // Check if mouse/touch is being held.
        if (LugusInput.use.down || LugusInput.use.dragging)
        {
            // Check if one of flipper click boxes is clicked.
            Transform clickedObject =
                LugusInput.use.RayCastFromMouse(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>());

            if (clickedObject != null)
            {
                // Move flippers if touched.
                if (clickedObject.name == "ClickBox_LeftFlipper")
                    GameObject.Find("LeftFlipper").GetComponent<Flipper>().TouchPressed = true;
                if (clickedObject.name == "ClickBox_RightFlipper")
                    GameObject.Find("RightFlipper").GetComponent<Flipper>().TouchPressed = true;
            }
        }
    }

    void Update()
    {
        // Only allow launch when ball is ready.
        if (!Player.use.IsSingleBallReadyForLaunch())
            return;

        // Store where player started dragging.
        if (LugusInput.use.down)
        {
            _dragging = true;
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
            _dragging = false;
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
