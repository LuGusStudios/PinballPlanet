using UnityEngine;
using System.Collections;

public class LaunchPoint_Mine : MonoBehaviour
{
    private FlowBackPreventer_02 _flowBack = null;
    private Logs _logs = null;
    private FloorLight_Link _lightFirstLink = null;
    private FloorLight_Toggle _trainHandleLight = null;

    // Use this for initialization
    void Start()
    {
        _flowBack = GameObject.Find("FlowBackPreventer_02").GetComponent<FlowBackPreventer_02>();
        _logs = GameObject.Find("Logs").GetComponent<Logs>();
        _lightFirstLink = GameObject.Find("FloorLight_Launch_Link1").GetComponent<FloorLight_Link>();
        _trainHandleLight = GameObject.Find("MineCart_FloorLight_Toggle").GetComponent<FloorLight_Toggle>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Ball")
            return;

        // Reset flowback preventer.
        _flowBack.Reset();

        // Reset logs.
        _logs.Unbreak();

        // Reset launch lights.
        _lightFirstLink.Unbreak();

        // Reset track.
        _trainHandleLight.Unbreak();
    }
}
