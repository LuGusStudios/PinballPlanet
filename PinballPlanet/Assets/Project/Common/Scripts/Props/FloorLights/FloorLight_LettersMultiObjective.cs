using UnityEngine;

public class FloorLight_LettersMultiObjective : BreakableMultiObjective
{
    public override void Activate()
    {
        GameObject sideFlowBack = GameObject.Find("FlowBackPreventer_Side");
        if (sideFlowBack != null)
            sideFlowBack.GetComponent<FlowBackPreventer_Side>().Enable();

        base.Activate();
    }
}
