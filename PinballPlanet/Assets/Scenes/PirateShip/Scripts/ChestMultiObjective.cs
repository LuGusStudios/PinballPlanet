using UnityEngine;

public class ChestMultiObjective : BreakableMultiObjective
{
    // How many times a score pops up.
    private int _scoreRepeat = 10;

    // Prefab of a coin to spawn.
    public GameObject CoinPrefab;
    public int CoinsAmount = 10;

    // Sounds
    public AudioClip CoinsSound;

    // Use this for initialization.
    protected override void Start()
    {
        base.Start();

        // Test
        //Activate();

        // Disable gold particle effect.
        transform.FindChild("GoldShine").gameObject.SetActive(false);
    }

    // Activates the objective once all breakable subobjectives are broken.
    public override void Activate()
    {
        // Hide lock.
        transform.FindChild("TreasureChestLock").renderer.enabled = false;
        transform.FindChild("TreasureChestPadLock").renderer.enabled = false;

        // Play chest open animation.
        transform.FindChild("TreasureChestLid").GetComponent<Animation>().Play("ChestOpening");

        //AnimationState lidOpen = transform.FindChild("TreasureChestLid").animation["ChestOpening"];
        //Debug.Log("--- Animating: " + name + ", speed: " + lidOpen.speed + ", time: " + lidOpen.time + " ---");

        // Give bonus points.
        float delay = 0.2f;
        for (int i = 0; i < _scoreRepeat; i++)
        {
            Invoke("ShowScore", i * delay);
        }

        // Spawn coins.
        for (int i = 0; i < CoinsAmount; i++)
        {
            // Find area to spawn in and pick a random spot.
            GameObject spawnArea = GameObject.Find("CoinSpawn");
            Vector3 randPos = new Vector3(spawnArea.transform.position.x + Random.Range(-0.5f, 0.5f) * spawnArea.transform.localScale.x, spawnArea.transform.position.y + Random.Range(-0.5f, 0.5f) * spawnArea.transform.localScale.y);
            randPos.z = 10;
            // Spawn coin at chest and move it to new position.
            GameObject coin = Instantiate(CoinPrefab, transform.position, Quaternion.identity) as GameObject;
            coin.GetComponent<Coin>().SetTarget(randPos);
            // Give random rotation.
            coin.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }

        // Play coins falling sound.
        LugusAudio.use.SFX().Play(CoinsSound).Loop = false;

        // Activate gold particle effect.
        transform.FindChild("GoldShine").gameObject.SetActive(true);

        // Reset after a while.
        Invoke("Unbreak", ResetDelay);
    }

    // Give a score instance.
    private void ShowScore()
    {
        // Offset position randomly.
        float randX = Random.Range(-30, 30);
        float randY = Random.Range(-30, 30);
        float randZ = Random.Range(-15, 15);

        GameObject scorePopup = ScoreManager.use.ShowScore(BonusPoints / _scoreRepeat, transform.position.zAdd(60) + new Vector3(randX, randY, randZ), 1.0f, null, Color.yellow);
        scorePopup.GetComponent<TextMesh>().characterSize = 2f;
    }

    // Reset.
    public override void Unbreak()
    {
        //Debug.Log("--- Unbreaking chest. ---");

        // Reverse chest open animation.
        transform.FindChild("TreasureChestLid").GetComponent<Animation>().Play("ChestOpening");
        AnimationState lidOpen = transform.FindChild("TreasureChestLid").animation["ChestOpening"];
        lidOpen.time = lidOpen.length;
        lidOpen.speed = -0.5f;

        // Disable gold particle effect.
        transform.FindChild("GoldShine").gameObject.SetActive(false);

        // Show lock again after close animation.
        Invoke("ShowLock", lidOpen.length);

        // Reset completed after animation is done.
        Invoke("UnbreakBase", lidOpen.length + 0.1f);
    }

    // Resets inherited base.
    private void UnbreakBase()
    {
        ResetAnimation();

        // Call inherited unbreak function.
        base.Unbreak();
    }

    // Resets the lock to be shown.
    private void ShowLock()
    {
        // Unhide lock.
        transform.FindChild("TreasureChestLock").renderer.enabled = true;
        transform.FindChild("TreasureChestPadLock").renderer.enabled = true;
    }

    // Resets the animation.
    private void ResetAnimation()
    {
        // Reset TreasureChestLid opening animation.
        AnimationState lidOpen = transform.FindChild("TreasureChestLid").animation["ChestOpening"];
        lidOpen.time = 0;
        lidOpen.speed = 1;

        transform.FindChild("TreasureChestLid").GetComponent<Animation>().Sample();
        transform.FindChild("TreasureChestLid").GetComponent<Animation>().Stop();

        //Debug.Log("--- Resseting Animation: TreasureChestLid, speed: " + lidOpen.speed + ", time: " + lidOpen.time + " ---");
    }
}
