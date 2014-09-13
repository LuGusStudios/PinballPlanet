using UnityEngine;
using System.Collections;

public class GhostPathFollower : Breakable
{
    private const string ExitPath = "ExitPath";
    private const string LoopPath = "LoopPath";

    public float Speed = 80;

    // Use this for initialization
    protected override void Start()
    {
        // Play exit path animation.
        ExitPathMove();

        base.Start();
    }

    public override void Break()
    {
        // Enable flame.
        transform.FindChild("Flame").GetComponent<ParticleSystem>().enableEmission = true;
        transform.FindChild("Point light").GetComponent<Light>().enabled = true;

        // Call inherited break function.
        base.Break();
    }

    // Make ghost follow exit path.
    private void ExitPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(ExitPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "speed", Speed
                                            , "easetype", iTween.EaseType.linear
                                            , "oncomplete", "LoopPathMove"));
    }

    // Make ghost follow loop path.
    private void LoopPathMove()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(LoopPath)
                                            , "movetopath", false
                                            , "orienttopath", true
                                            , "looptype", iTween.LoopType.loop
                                            , "speed", Speed
                                            , "easetype", iTween.EaseType.linear));
    }

    //// Makes ghost go to wall.
    //public void WallMove()
    //{
    //    iTween.Stop(gameObject);

    //    iTween.MoveTo(gameObject, iTween.Hash("position", GameObject.Find("Ghost_HoverPos").transform.position
    //                                        , "movetopath", false
    //                                        , "orienttopath", true
    //                                        , "speed", Speed
    //                                        , "easetype", iTween.EaseType.easeInOutCubic));
    //}
}
