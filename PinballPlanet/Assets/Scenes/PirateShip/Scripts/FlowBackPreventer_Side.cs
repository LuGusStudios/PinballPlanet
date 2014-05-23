using UnityEngine;

/// <summary>
/// Flow back preventer for the sidelanes.
/// Gets disabled the first time it's hit.
/// </summary>
public class FlowBackPreventer_Side : MonoBehaviour
{
    void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.tag != "Ball")
            return;

        Invoke("Disable", 0.5f);
    }

    public void Disable()
    {
        renderer.enabled = false;
        collider.enabled = false;
    }

    public void Enable()
    {
        renderer.enabled = true;
        collider.enabled = true;
    }
}
