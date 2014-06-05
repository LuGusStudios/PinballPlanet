using UnityEngine;

public class ScoreHit : MonoBehaviour
{
    public int score = 1000;
    public AudioClip sound = null;
    public Color color = Color.white;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnCollisionEnter(Collision collision)
    {
        // Only give score if the collider is a ball.
        if (collision.collider.gameObject.tag != "Ball")
            return;
        //Debug.Log("" + Time.frameCount + " -> " + this.name + " Collided with " + collision.collider.name );

        DoScore(collision.contacts[0].point);


        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
        //if (collision.relativeVelocity.magnitude > 2)
        //    audio.Play();


        //.bounds.Contains()
    }

    void OnTriggerEnter(Collider other)
    {
        // Only give score if the collider is a ball.
        if (other.gameObject.tag != "Ball")
            return;

        DoScore(other.transform.position);
    }


    public void DoScore(Vector3 point)
    {
        ScoreManager.use.ShowScore(score, point.zAdd(Random.Range(10, 20)), 2.0f, sound, color);
    }

    public void DoScore()
    {
        ScoreManager.use.ShowScore(score, transform.position.zAdd(Random.Range(10, 20)), 2.0f, sound, color);
    }

}
