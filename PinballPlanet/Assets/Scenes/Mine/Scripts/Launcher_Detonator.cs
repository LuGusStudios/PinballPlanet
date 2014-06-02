using UnityEngine;
using System.Collections;

public class Launcher_Detonator : MonoBehaviour
{
    private Transform _topTransform;
    private Transform _botTransform;

    // Use this for initialization
    void Start()
    {
        _topTransform = transform.parent.FindChild("DetonatorPlunger_Top");
        _botTransform = transform.parent.FindChild("DetonatorPlunger_Bot");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(_botTransform.position, _topTransform.position,
            Player.use.BallLaunchForce/Player.use.LaunchMaxForce);
    }
}
