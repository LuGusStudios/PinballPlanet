using UnityEngine;

public class Lock : Breakable
{
    // Chain connected to chest.
    public GameObject ChainPrefab;
    private GameObject _chain;

    // Chain Fading.
    public float FadeTime = 2.0f;
    private float _timeFading = 0;
    private float _alpha = 1;

    // Initialization.
    protected override void Start()
    {
        // Create chain.
        _chain = (GameObject) Instantiate(ChainPrefab);

        base.Start();
    }

    // Called once every frame.
    void Update()
    {
        // Fade away chain once it's broken.
        if(IsBroken && _chain != null)
        {
            foreach (Transform child in _chain.transform)
            {
                if(child.name == "Chain_Link" || child.name == "Chain_BreakLink")
                {
                    //Color matColor = ;
                    child.renderer.material.color = new Color(1, 1, 1, _alpha);
                }
            }

            _alpha = Mathf.Lerp(1, 0, _timeFading/FadeTime);
            _timeFading += Time.deltaTime;

            // Destroy chain.
            if (_timeFading >= FadeTime)
            {
                Destroy(_chain);
            }
        }
    }

    // Breaks the game object by opening the lock.
    public override void Break()
    {
        // Play animation on child lock.
        gameObject.transform.FindChild("Lock_Lock").GetComponent<Animation>().Play("LockOpen");

        // Break the chain.
        _chain.transform.FindChild("Chain_BreakLink").hingeJoint.breakForce = 0;

        // Call inherited break function.
        base.Break();
    }

    // Reset.
    public override void Unbreak()
    {
        // Reverse lock open animation.
        transform.FindChild("Lock_Lock").GetComponent<Animation>().Play("LockOpen");
        AnimationState lockOpen = transform.FindChild("Lock_Lock").animation["LockOpen"];
        lockOpen.time = lockOpen.length;
        lockOpen.speed = -0.5f;

        // Reset completed after animation is done.
        Invoke("UnbreakBase", lockOpen.length * 2);
    }

    // Resets inherited base.
    private void UnbreakBase()
    {
        ResetAnimation();

        // Reset chain.
        _timeFading = 0;
        _alpha = 1;
        _chain = (GameObject)Instantiate(ChainPrefab);

        // Call inherited unbreak function.
        base.Unbreak();
    }

    // Resets the animation.
    private void ResetAnimation()
    {
        // Reset Lock opening animation.
        AnimationState lockOpen = transform.FindChild("Lock_Lock").animation["LockOpen"];
        lockOpen.time = 0;
        lockOpen.speed = 1;

        transform.FindChild("Lock_Lock").GetComponent<Animation>().Sample();
        transform.FindChild("Lock_Lock").GetComponent<Animation>().Stop();
    }
}
