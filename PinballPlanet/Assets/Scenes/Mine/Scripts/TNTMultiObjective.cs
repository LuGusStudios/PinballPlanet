using UnityEngine;
using System.Collections;

public class TNTMultiObjective : BreakableMultiObjective
{
    // The two different versions of the bridge.
    private GameObject _bridge, _bridgeDestroyed;

    // Explosion particle spawned on activation.
    public GameObject ExplosionPrefab;

    // Sounds
    public AudioClip ExplosionSound;

    protected override void Start()
    {
        // Replace bridge with broken version.
        _bridge = GameObject.Find("Bridge");
        _bridgeDestroyed = GameObject.Find("BridgeDestroyed");
        _bridgeDestroyed.SetActive(false);

        base.Start();
    }

    public override void Activate()
    {
        _bridgeDestroyed.SetActive(true);
        _bridge.SetActive(false);

        // Play coins falling sound.
        if (audio != null)
            audio.PlayOneShot(ExplosionSound);

        Instantiate(ExplosionPrefab);

        // Reset after a while.
        Invoke("Unbreak", ResetDelay);
    }

    public override void Unbreak()
    {
        _bridgeDestroyed.SetActive(false);
        _bridge.SetActive(true);

        base.Unbreak();
    }
}
