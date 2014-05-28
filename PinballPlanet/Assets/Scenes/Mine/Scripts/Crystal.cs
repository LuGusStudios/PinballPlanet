using UnityEngine;

public class Crystal : Breakable
{
    // Seperate shard subobjects.
    private GameObject _shard1, _shard2, _shard3, _particles, _light;

    // Index of currently broken subobject.
    private int _breakIndex = 0;

    // Time till reset.
    public float ResetTime = 5;

    // Use this for initialization
    protected override void Start()
    {
        // Store child gameobjects.
        _shard1 = transform.FindChild("CrystalShard01").gameObject;
        _shard2 = transform.FindChild("CrystalShard02").gameObject;
        _shard3 = transform.FindChild("CrystalShard03").gameObject;
        _particles = transform.FindChild("CrystalParticles").gameObject;
        _light = transform.FindChild("Point light").gameObject;

        base.Start();
    }

    // Breaks the game object.
    public override void Break()
    {
        // Return if already broken.
        if(IsBroken)
            return;

        // Break each part seperately with each consecutive hit.
        switch (_breakIndex)
        {
            case 0:
                // Disable crystal part.
                _shard1.renderer.enabled = false;
                ++_breakIndex;
                break;
            case 1:
                // Disable crystal part.
                _shard2.renderer.enabled = false;
                ++_breakIndex;
                break;
            case 2:
                // Disable crystal parts.
                _shard3.renderer.enabled = false;
                _particles.GetComponent<ParticleSystem>().enableEmission = false;
                _light.GetComponent<Light>().enabled = false;
                // Disable collider.
                collider.enabled = false;
                // Reset after a delay.
                Invoke("Unbreak", ResetTime);
                // Call base Break.
                base.Break();
                break;
        }
    }

    // Restores the game object to its unbroken state.
    public override void Unbreak()
    {
        // Reset index.
        _breakIndex = 0;
        // Enable crystal parts.
        _shard1.renderer.enabled = true;
        _shard2.renderer.enabled = true;
        _shard3.renderer.enabled = true;
        _particles.GetComponent<ParticleSystem>().enableEmission = true;
        _light.GetComponent<Light>().enabled = true;
        // Enable collision.
        collider.enabled = true;

        // Play grow animation.
        animation.Play("CrystalGrow");

        // Call base Unbreak.
        base.Unbreak();
    }
}
