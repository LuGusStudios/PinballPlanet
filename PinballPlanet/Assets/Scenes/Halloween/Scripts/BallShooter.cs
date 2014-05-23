using UnityEngine;
using System.Collections;

public class BallShooter : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShootBall(GameObject ball)
    {
        StartCoroutine(ShootBallRoutine(ball));
    }

    public IEnumerator ShootBallRoutine(GameObject ball)
    {
        //iTween.RotateTo( exit, new Vector3(40, 0, 0), 2.0f );

        iTween.Stop(this.gameObject);

        iTween.RotateAdd(this.gameObject, iTween.Hash("amount", new Vector3(-50, 0, 0), "time", 2.0f, "isLocal", true));

        /*	
        iTween.RotateTo( exit, 
                iTween.Hash("rotation",new Vector3(40, exit.transform.localRotation.eulerAngles.y, exit.transform.localRotation.eulerAngles.z),
                            "time", 2.0f,
                            "islocal",true));
        */

        yield return new WaitForSeconds(2.0f);

        iTween.Stop(this.gameObject);
        //iTween.RotateTo( exit, new Vector3(90, 0, 0), 2.0f );

        iTween.RotateAdd(this.gameObject,
                iTween.Hash("amount", new Vector3(50, 0, 0),
                            "time", 2.0f,
                            "isLocal", true));

        /*
        iTween.RotateTo( exit, 
                iTween.Hash("rotation",new Vector3(90, exit.transform.localRotation.eulerAngles.y, exit.transform.localRotation.eulerAngles.z),
                            "time", 2.0f,
                            "islocal",true));
        */
        ball.rigidbody.isKinematic = false;

        ball.rigidbody.AddForce(this.transform.forward * 7000);

        yield return null;
    }
}
