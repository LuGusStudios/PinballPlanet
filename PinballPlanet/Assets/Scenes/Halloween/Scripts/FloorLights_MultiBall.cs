using System.Collections.Generic;
using UnityEngine;

public class FloorLights_MultiBall : MonoBehaviour
{
    // Cannon script, used to get it's target.
    public MultiBall MultiBall;

    // Lights to animate.
    public List<FloorLight_Toggle> FloorLights;

    // Animation.
    public float AnimationTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // Animate lights if cannon target is in right area.
        if (MultiBall.Activated)
        {
            for (int i = 0; i < FloorLights.Count; ++i)
            {
                float min = AnimationTime / FloorLights.Count * i;
                float max = AnimationTime / FloorLights.Count * (i + 1);
                if (Time.time % AnimationTime > min && Time.time % AnimationTime < max)
                {
                    if (!FloorLights[i].IsBroken)
                        FloorLights[i].Break();
                }
                else
                {
                    if (FloorLights[i].IsBroken)
                        FloorLights[i].Unbreak();
                }
            }
        }
    }
}
