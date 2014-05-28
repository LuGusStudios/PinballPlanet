using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{
    public Vector3 rotation = new Vector3(0, 0, 1);
    public bool pingPongRotation = false;
    public Vector3 hoverDistance = new Vector3(0, 1, 0);
    public float time = 1f;



	// Use this for initialization
	void Start () 
    {
        if (pingPongRotation)
        {
            iTween.RotateBy(this.gameObject, iTween.Hash(
            "amount", rotation,
            "time", time,
            "easetype", iTween.EaseType.easeInOutQuad,
            "looptype", iTween.LoopType.pingPong,
            "space", Space.World));
        }

        iTween.MoveBy(this.gameObject, iTween.Hash(
        "amount", hoverDistance,
        "time", time,
        "easetype", iTween.EaseType.easeInOutQuad,
        "looptype", iTween.LoopType.pingPong,
        "space", Space.World));
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!pingPongRotation)
            transform.Rotate(rotation);
	}
}
