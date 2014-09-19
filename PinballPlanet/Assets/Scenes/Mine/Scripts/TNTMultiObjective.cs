using UnityEngine;
using System.Collections;

public class TNTMultiObjective : BreakableMultiObjective
{
    // The two different versions of the bridge.
    private GameObject _bridge, _bridgeDestroyed;

    // Explosion particle spawned on activation.
    public GameObject ExplosionPrefab;

    // Sounds
    private AudioClip ExplosionSound;

    protected override void Start()
    {
        // Hide bridge broken version.
        _bridge = GameObject.Find("Bridge");
        _bridgeDestroyed = GameObject.Find("BridgeDestroyed");
        _bridgeDestroyed.SetActive(false);

        // Load audio.
        ExplosionSound = Resources.Load<AudioClip>("Shared/Audio/BridgeTNT_01");

        base.Start();
    }

    public override void Activate()
    {
        // Replace bridge with broken version.
        _bridgeDestroyed.SetActive(true);
        _bridge.SetActive(false);

        // Play explosion sound.
        if (audio != null)
            //audio.PlayOneShot(ExplosionSound);
            LugusAudio.use.SFX().Play(ExplosionSound).Loop = false;

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
