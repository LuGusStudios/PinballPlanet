using UnityEngine;
using System.Collections;

public class MineCart : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        iTweener newiTweener = new iTweener();
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("MineCart_StartPath"), "movetopath", false, "orienttopath", true, "speed", 150, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
