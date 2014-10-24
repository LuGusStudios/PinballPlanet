using UnityEngine;
using System.Collections;

public class SpiderWeb : Breakable 
{
    public GameObject SpiderPrefab;
    private Spider _spider;

    public float SpiderRespawnTime = 5.0f;
	public ParticleSystem particles = null;

    protected override void Start()
    {
        // Spawn spider.
        _spider = (Instantiate(SpiderPrefab) as GameObject).GetComponent<Spider>();
		_spider.particles = particles;

        base.Start();
    }

    public override void Break()
    {
        // Make spider jump.
        _spider.JumpTarget = GameObject.Find("Spider_Target");
        _spider.Jump();

        // Disable collision.
        collider.enabled = false;

        base.Break();

        Invoke("Unbreak", SpiderRespawnTime);
    }

    public override void Unbreak()
    {
        // Spawn spider.
        _spider = (Instantiate(SpiderPrefab) as GameObject).GetComponent<Spider>();
		_spider.particles = particles;

        // Enable collision.
        collider.enabled = true;

        base.Unbreak();
    }
}
