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

    // Use this for initialization
    void Start()
    {
        _upsideDownTransform = transform.FindChild("MineCart02_UpsideDown").transform;

        // Play start path animation.
        StartPathMove();
    }

    // Update is called once per frame
    void Update()
    {

    }

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
        // Play bridge end animation with small delay when out of vision.
        Invoke("BridgeEndPathMove", HiddenDelay);
    }

    // Make cart follow bridge start reversed path.
    private void BridgeStartPathRevMove()
    {
        // Set rotation upside down, to compensate for weird iTween behaviour.
        transform.FindChild("MineCart02").rotation = _upsideDownTransform.rotation;
        
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
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(BridgeEndPath)
                                            , "movetopath", false
                                            , "orienttopath", false
                                            , "looktime", 0.1f
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnBridgeEndPathEnded"));
    }

    // Called when the bridge path ends.
    private void OnBridgeEndPathEnded()
    {
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

}
