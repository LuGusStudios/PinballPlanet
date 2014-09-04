using UnityEngine;
using System.Collections;

public class LookAtBall : MonoBehaviour
{
    void Update()
    {
        if(Player.use.BallsInPlay.Count > 0)
            GetComponent<LookAt>().Target = Player.use.BallsInPlay[0].transform ;
        else
            GetComponent<LookAt>().Target = null;
    }
}
