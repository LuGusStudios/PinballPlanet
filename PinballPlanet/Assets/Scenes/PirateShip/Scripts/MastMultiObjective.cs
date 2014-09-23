using UnityEngine;

/// <summary>
/// Mast which will animate and give a score once its support subobjectives are broken.
/// </summary>
public class MastMultiObjective : BreakableMultiObjective
{
    // Particles to spawn when mast hits ground.
    public GameObject FallParticlesPrefab;

    // Chain connected to chest.
    public GameObject ChainPrefab;
    private GameObject _chain;

    // Chain Fading.
    public float FadeTime = 2.0f;
    private float _timeFading = 0;
    private float _alpha = 1;

    // Use this for initialization.
    protected override void Start()
    {
        // Create chain.
        _chain = (GameObject)Instantiate(ChainPrefab);
        // Attach chain to mast, so that it moves together.
        _chain.transform.FindChild("LockJoint_1_Lock").parent = GameObject.Find("Mast_Sail01").transform;

        // Call inherited base start.
        base.Start();
    }

    // Called once every frame.
    void Update()
    {
        // Fade away chain once it's broken.
        if (IsBroken && _chain != null)
        {
            foreach (Transform child in _chain.transform)
            {
                if (child.name == "Chain_Link" || child.name == "Chain_BreakLink")
                {
                    //Color matColor = ;
                    child.renderer.material.color = new Color(1, 1, 1, _alpha);
                }
            }

            _alpha = Mathf.Lerp(1, 0, _timeFading / FadeTime);
            _timeFading += Time.deltaTime;

            // Destroy chain.
            if (_timeFading >= FadeTime)
            {
                Destroy(_chain);
            }
        }
    }

    // Activates the objective once all breakable subobjectives are broken.
    public override void Activate()
    {
        // Play mast falling animation.
        transform.FindChild("Mast_Sail01").GetComponent<Animation>().Play("MastBreaking");

        // Play camera shake after falling animation.
        Invoke("ShakeCamera", transform.FindChild("Mast_Sail01").animation["MastBreaking"].length);

        // Give bonus score for completing all objectives.
        GameObject scorePopup = ScoreManager.use.ShowScore(BonusPoints, transform.position.zAdd(20), 1.5f, null, Color.white, gameObject);
        scorePopup.GetComponent<TextMesh>().characterSize = 2.5f;
    }

    // Lower score of the mast foot object.
    protected override void LowerScore()
    {
        transform.FindChild("Mast_Foot01").GetComponent<ScoreHit>().score = BrokenScore;
    }

    // Reset.
    public override void Unbreak()
    {
        // Reverse mast falling animation.
        transform.FindChild("Mast_Sail01").GetComponent<Animation>().Play("MastBreaking");
        AnimationState mastFall = transform.FindChild("Mast_Sail01").animation["MastBreaking"];
        mastFall.time = mastFall.length;
        mastFall.speed = -0.5f;

        // Reset completed after animation is done.
        Invoke("UnbreakBase", mastFall.length * 2);
    }

    // Resets inherited base.
    private void UnbreakBase()
    {
        ResetAnimation();

        // Reset chain.
        _chain = (GameObject)Instantiate(ChainPrefab);
        _timeFading = 0;
        _alpha = 1;
        // Attach chain to mast, so that it moves together.
        _chain.transform.FindChild("LockJoint_1_Lock").parent = GameObject.Find("Mast_Sail01").transform;

        // Call inherited unbreak function.
        base.Unbreak();
    }

    // Plays a shake animation on the camera.
    private void ShakeCamera()
    {
        // Play camera shake animation.
        GameObject.Find("MainCamera").GetComponent<Animation>().Play("CameraShake");

        // Spawn particles.
        Instantiate(FallParticlesPrefab, transform.position, FallParticlesPrefab.transform.rotation);
    }

    // Resets the animation.
    private void ResetAnimation()
    {
        // Reset mast falling animation.
        AnimationState mastFall = transform.FindChild("Mast_Sail01").animation["MastBreaking"];
        mastFall.time = 0;
        mastFall.speed = 1;

        transform.FindChild("Mast_Sail01").GetComponent<Animation>().Sample();
        transform.FindChild("Mast_Sail01").GetComponent<Animation>().Stop();
    }
}
