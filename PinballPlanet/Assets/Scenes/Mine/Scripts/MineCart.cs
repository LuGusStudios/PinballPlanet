using System.CodeDom;
using System.Reflection.Emit;
using UnityEngine;
using System.Collections;

public class MineCart : MonoBehaviour
{
    // iTween path names.
    private const string StartPath =    "StartPath";
    private const string DefaultPath =  "DefaultPath";
    private const string BridgeStartPath = "BridgeStartPath";
    private const string BridgeEndPath = "BridgeEndPath";

    // Transform of upside down cart. Used to put cart upside down for weird iTween behaviour.
    public Transform _upsideDownTransform;

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

    // Make cart follow start path.
    void StartPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(StartPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnStartPathEnded"));
    }

    // Called when the start path ends.
    void OnStartPathEnded()
    {
        // Play default path animation if not following bridge path.
        if (GameObject.Find("MineCart_Paths").GetComponent<MineCart_Rails>().RailsSwitched)
            BridgeStartPathMove();
        else
            DefaultPathMove();
    }

    // Make cart follow start path.
    void DefaultPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(DefaultPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnDefaultPathEnded"));
    }

    // Called when the start path ends.
    void OnDefaultPathEnded()
    {
        GameObject.Find("MineCart_Paths").GetComponent<MineCart_Rails>().OnMineCartDestroyed();
        Destroy(gameObject);
    }

    // Make cart follow bridge start path.
    void BridgeStartPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(BridgeStartPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnBridgeStartPathEnded"));
    }

    // Called when the bridge start path ends.
    void OnBridgeStartPathEnded()
    {
        // Play start path animation.
        float delay = 2.0f;
        Invoke("BridgeEndPathMove", delay);
    }

    // Make cart follow bridge path.
    void BridgeEndPathMove()
    {
        // Set rotation upside down, to compensate for weird iTween behaviour.
        transform.FindChild("MineCart02").rotation = _upsideDownTransform.rotation;

        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(BridgeEndPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "looktime", 0.1f
                                            , "speed", 100
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "OnBridgeEndPathEnded"));
    }

    // Called when the bridge path ends.
    void OnBridgeEndPathEnded()
    {
        GameObject.Find("MineCart_Paths").GetComponent<MineCart_Rails>().OnMineCartDestroyed();
        Destroy(gameObject);
    }
}
