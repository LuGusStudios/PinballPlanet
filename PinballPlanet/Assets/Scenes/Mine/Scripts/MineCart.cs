using System.CodeDom;
using System.Reflection.Emit;
using UnityEngine;
using System.Collections;

public class MineCart : MonoBehaviour
{
    // iTween path names.
    private const string StartPath = "StartPath";
    private const string DefaultPath = "DefaultPath";
    private const string BridgeStartPath = "BridgeStartPath";
    private const string BridgeEndPath = "BridgeEndPath";

    // Transform of upside down cart. Used to put cart upside down for weird iTween behaviour.
    private Transform _upsideDownTransform;

    // How long the mine cart stays hidden.
    public float HiddenDelay = 1.0f;

    // Prefab of the crystal shards to drop.
    public GameObject CrystalShardPrefab = null;

    // How many crystal shard to drop.
    public int CrystalsToDropBarrier = 3;
    public int CrystalsToDropHit = 3;
    public int CrystalsToDropCrash = 15;
    public float CrystalsDropHitRadius = 50;

    // Sounds
    public AudioClip TravelSound = null;
    public AudioClip CrashSound = null;

    // Use this for initialization
    void Start()
    {
        _upsideDownTransform = transform.Find("MineCart02_UpsideDown").transform;

        // Play travel sound.
        LugusAudio.use.SFX().Play(TravelSound).Loop = true;

        // Play start path animation.
        StartPathMove();
    }

    // Called when this collider/rigidbody has begun touching another rigidbody/collider.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Ball")
            return;

        // Spawn crystal shard projectiles.
        for (int i = 0; i < CrystalsToDropBarrier; i++)
        {
            // Pick a random spot around the cart to spawn the crystals.
            Vector3 randPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f) * CrystalsDropHitRadius, transform.position.y + Random.Range(-0.5f, 0.5f) * CrystalsDropHitRadius);
            // Spawn crystal shard at cart and set projectile target to new position.
            GameObject crystal = Instantiate(CrystalShardPrefab, transform.position, Quaternion.identity) as GameObject;
            crystal.GetComponent<CrystalShard>().SetTarget(randPos);
            // Give random rotation.
            crystal.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }

        // Play shake animation.
        transform.Find("MineCart_Pivot/MineCart02").animation.Play("MineCartShake");
    }

    // Destroys the mine cart and notifies the Rails script.
    private void DestroyCart()
    {
        GameObject.Find("MineCart_Paths_02").GetComponent<MineCart_Rails>().OnMineCartDestroyed();
        Destroy(gameObject);
    }

    //------------------------------
    // Start path moving.
    //------------------------------
    // Make cart follow start path.
    private void StartPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(StartPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnStartPathEnded"));
    }

    // Called when the start path ends.
    private void OnStartPathEnded()
    {
        // Play default path animation if not following bridge path.
        if (GameObject.Find("MineCart_Paths_02").GetComponent<MineCart_Rails>().RailsSwitched)
            BridgeStartPathMove();
        else
            DefaultPathMove();
    }

    // Make cart follow start reversed path.
    private void StartPathRevMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPathReversed(StartPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 200
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnStartPathRevEnded"));
    }

    // Called when the start path reversed ends.
    private void OnStartPathRevEnded()
    {
        DestroyCart();
    }

    //------------------------------
    // Default path moving.
    //------------------------------
    // Make cart follow default path.
    private void DefaultPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(DefaultPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnDefaultPathEnded"));
    }

    // Called when the default path ends.
    private void OnDefaultPathEnded()
    {
        DestroyCart();
    }

    //------------------------------
    // Bridge start path moving.
    //------------------------------
    // Make cart follow bridge start path.
    private void BridgeStartPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(BridgeStartPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 80
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnBridgeStartPathEnded"));
    }

    // Called when the bridge start path ends.
    private void OnBridgeStartPathEnded()
    {
        // Play bridge broken animation if all TNT boxes are hit otherwise play bridge end animation.
        if (GameObject.Find("BridgeMultiObjective").GetComponent<TNTMultiObjective>().IsBroken)
            PlayCrashAnimation();
        else
            Invoke("BridgeEndPathMove", HiddenDelay);
    }

    // Make cart follow bridge start reversed path.
    private void BridgeStartPathRevMove()
    {
        // Set rotation upside down, to compensate for weird iTween behaviour.
        transform.Find("MineCart_Pivot").rotation = _upsideDownTransform.rotation;

        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPathReversed(BridgeStartPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 150
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnBridgeStartPathRevEnded"));
    }

    // Called when the bridge start reversed path ends.
    private void OnBridgeStartPathRevEnded()
    {
        StartPathRevMove();
    }

    //------------------------------
    // Bridge end path moving.
    //------------------------------
    // Make cart follow bridge path.
    private void BridgeEndPathMove()
    {
        transform.rotation = GameObject.Find("MineCart_BridgeEnd_Rotation").transform.rotation;

        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(BridgeEndPath)
                                            , "movetopath", false
                                            , "orienttopath", false
                                            , "looktime", 0.1f
                                            , "speed", 150
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnBridgeEndPathEnded"));
    }

    // Called when the bridge path ends.
    private void OnBridgeEndPathEnded()
    {
        // Play barrier hit animation.
        transform.Find("MineCart_Pivot/MineCart02").animation.Play("MineCartBarrierHit");

        // Spawn crystal shard projectiles.
        for (int i = 0; i < CrystalsToDropBarrier; i++)
        {
            // Find area to spawn in and pick a random spot.
            GameObject spawnArea = GameObject.Find("CrystalShardArea_Barrier");
            Vector3 randPos = new Vector3(Random.Range(-0.5f, 0.5f) * spawnArea.transform.localScale.x,
                                           Random.Range(-0.5f, 0.5f) * spawnArea.transform.localScale.y);
            randPos = spawnArea.transform.rotation * randPos;
            randPos += spawnArea.transform.position;
            // Spawn crystal shard at cart and move it to new position.
            GameObject crystal = Instantiate(CrystalShardPrefab, transform.position, Quaternion.identity) as GameObject;
            crystal.GetComponent<CrystalShard>().SetTarget(randPos);
            // Give random rotation.
            crystal.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }

        // Reverse cart.
        BridgeEndPathRevMove();
    }

    // Make cart follow bridge end reversed path.
    private void BridgeEndPathRevMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPathReversed(BridgeEndPath)
                                            , "movetopath", false
                                            , "orienttopath", false
                                            , "looktime", 0.1f
                                            , "speed", 70
                                            , "easetype", iTween.EaseType.easeInQuint
                                            , "oncomplete", "OnBridgeEndPathRevEnded"));
    }

    // Called when the bridge path end reversed ends.
    private void OnBridgeEndPathRevEnded()
    {
        // Play bridge start reverse animation with small delay when out of vision.
        Invoke("BridgeStartPathRevMove", HiddenDelay);
    }

    // Plays the crash animation and spawns a number of crystal shards.
    public void PlayCrashAnimation()
    {
        transform.position = GameObject.Find("MineCart_BridgeEnd_Rotation").transform.position;
        transform.rotation = GameObject.Find("MineCart_BridgeEnd_Rotation").transform.rotation;

        // Play minecart crash animation.
        transform.Find("MineCart_Pivot/MineCart02").animation.Play("MineCartCrash");
        //transform.Find("MineCart_Pivot/MineCart02").animation["MineCartCrash"].speed = 2;
        float destroyCartDelay = transform.Find("MineCart_Pivot/MineCart02").animation["MineCartCrash"].length + 2;
        Invoke("DestroyCart", destroyCartDelay);

        // Disable collision.
        collider.enabled = false;

        // Spawn crystals after a certain delay.
        Invoke("SpawnCrashCrystals", 0.7f);
    }

    private void SpawnCrashCrystals()
    {
        // Spawn crystal shard projectiles.
        for (int i = 0; i < CrystalsToDropCrash; i++)
        {
            // Find area to spawn in and pick a random spot.
            GameObject spawnArea = GameObject.Find("CrystalShardArea_Crash");
            Vector3 randPos = new Vector3( Random.Range(-0.5f, 0.5f) * spawnArea.transform.localScale.x,
                                           Random.Range(-0.5f, 0.5f) * spawnArea.transform.localScale.y);
            randPos = spawnArea.transform.rotation * randPos;
            randPos += spawnArea.transform.position;
            // Spawn crystal shard at cart and move it to new position.
            GameObject crystal = Instantiate(CrystalShardPrefab, transform.FindChild("MineCart_Pivot/MineCart02").position, Quaternion.identity) as GameObject;
            crystal.GetComponent<CrystalShard>().SetTarget(randPos);
            // Give random rotation.
            crystal.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }

        // Hide crystals in minecart itself.
        foreach ( Transform child in transform.FindChild("MineCart_Pivot/MineCart02"))
        {
            child.gameObject.SetActive(false);
        }

        // Play crash sound.
        if (CrashSound != null)
        {
            //audio.Stop();
            //audio.PlayOneShot(CrashSound);
            LugusAudio.use.SFX().Play(CrashSound).Loop = false;
        }
    }
}
