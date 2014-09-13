using UnityEngine;
using System.Collections;

public class Candle : Breakable
{
    // Use this for initialization
    protected override void Start()
    {
        // Disable flame.
        transform.FindChild("Flame").GetComponent<ParticleSystem>().enableEmission = false;
        transform.FindChild("Point light").GetComponent<Light>().enabled = false;
    }

    public override void Break()
    {
        // Enable flame.
        transform.FindChild("Flame").GetComponent<ParticleSystem>().enableEmission = true;
        transform.FindChild("Point light").GetComponent<Light>().enabled = true;

        base.Break();
    }

    public override void Unbreak()
    {
        // Disable flame.
        transform.FindChild("Flame").GetComponent<ParticleSystem>().enableEmission = false;
        transform.FindChild("Point light").GetComponent<Light>().enabled = false;

        base.Unbreak();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Ball")
            return;

        if (!IsBroken)
            Break();
        else
        {
            transform.FindChild("Flame").GetComponent<ParticleSystem>().enableEmission = false;
            Invoke("EnableParticles", 0.3f);
        }
    }

    void EnableParticles()
    {
        transform.FindChild("Flame").GetComponent<ParticleSystem>().enableEmission = true;
    }
}
