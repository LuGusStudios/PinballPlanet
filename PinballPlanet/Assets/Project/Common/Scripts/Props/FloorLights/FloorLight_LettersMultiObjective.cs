using UnityEngine;

public class FloorLight_LettersMultiObjective : BreakableMultiObjective
{
    public override void Activate()
    {
        GameObject.Find("FlowBackPreventer_Side").GetComponent<FlowBackPreventer_Side>().Enable();

        base.Activate();
    }
}
