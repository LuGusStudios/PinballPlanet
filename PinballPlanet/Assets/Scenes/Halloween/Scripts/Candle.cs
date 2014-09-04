using UnityEngine;
using System.Collections;

public class Candle : Breakable
{
    // Use this for initialization
    void Start()
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
}
