using UnityEngine;

public class RumBottle : MonoBehaviour
{
    // Rotation Transforms.
    public Transform DrunkHalfTransform;
    public Transform DrunkFullTransform;

    // How long the player will be "drunk".
    public float DrunkTime = 3.0f;

    // How long till the bottle resets.
    // This might take longer if there is more than one ball.
    public float ResetTime = 10f;

    // How long player has been "drunk".
    private float _timeDrunk = 0;
    public bool IsDrunk = false;

    // Store the old rotation before player is "drunk".
    private Quaternion _oldRot;

    // Splash particles to spawn on hit.
    public GameObject SplashParticlesPrefab;
    private Vector3 _splashParticlesPos;

    // Sets the multiball active.
    public bool MultiBallActive
    {
        get { return GetComponent<MultiBall>().Activated; }
        set
        {
            if (value == false) return;
            if (_timeDrunk > ResetTime)
            {
                GetComponent<MultiBall>().ActivateMultiBall();
            }
        }
    }

    // Use this for initialization.
    void Start()
    {
        _splashParticlesPos = transform.FindChild("Cork").position;
    }

    // Update is called once per frame
    void Update()
    {
        // Play animation when drunk.
        if (IsDrunk)
        {
            //Debug.Log("--- Hic! ---");

            // Store camera.
            GameObject camera = GameObject.Find("MainCamera_Move");

            // Animate to half drunk transform for first half of animation
            if (_timeDrunk < DrunkTime / 2)
            {
                camera.transform.rotation = Quaternion.Lerp(_oldRot, GameObject.Find("MainCamera_DrunkHalf").transform.rotation, _timeDrunk / DrunkTime * 2);
            }
            else if (_timeDrunk > DrunkTime / 2)
            {
                camera.transform.rotation = Quaternion.Lerp(GameObject.Find("MainCamera_DrunkHalf").transform.rotation, GameObject.Find("MainCamera_DrunkFull").transform.rotation, ((_timeDrunk - DrunkTime / 2) / DrunkTime) * 2);
            }

            _timeDrunk += Time.deltaTime;
        }

        // Reset.
        if (_timeDrunk > DrunkTime)
        {
            GameObject.Find("MainCamera_Move").transform.rotation = _oldRot;
        }

        // Reactivate.
        if (_timeDrunk > ResetTime && Player.use.BallsInPlay.Count <= 1)
        {
            _timeDrunk = 0;
            IsDrunk = false;

            GetComponent<MultiBall>().ActivateMultiBall();
        }
    }

    // Called when another object collides.
    void OnCollisionEnter(Collision collision)
    {
        // Only so something if the collider is a ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;

        // Don't react when already drunk.
        if (IsDrunk)
            return;

        // Dont do anything unless multiball is active.
        if (!GetComponent<MultiBall>().Activated)
            return;

        // Play cork popping animation.
        GetComponent<Animation>().Play("BottlePop");
        transform.FindChild("Cork").GetComponent<Animation>().Play("CorkPop");

        // Store old camera rotation.
        _oldRot = GameObject.Find("MainCamera_Move").transform.rotation;

        // Set to drunk.
        IsDrunk = true;

        // Spawn splash particles.
        Invoke("SpawnSplash", 0.5f);
    }

    // Reset camera rotation.
    private void ResetCamera()
    {
        // Rotate the camera when the player is "drunk".
        GameObject.Find("MainCamera_Move").transform.rotation = _oldRot;
    }

    // Spawn splash particles.
    private void SpawnSplash()
    {
        Instantiate(SplashParticlesPrefab, _splashParticlesPos, Quaternion.identity);
    }
}
