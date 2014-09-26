using UnityEngine;
using System.Collections.Generic;

public delegate void HitEventHandler();

public class ScoreHit : MonoBehaviour
{
    public int score = 1000;
    public AudioClip sound = null;
    public Color color = Color.white;

    // Hit event.
    public event HitEventHandler Hit;

    // Use this for initialization
    void Start()
    {
        // Let all 'ObjectHitCondition' know this object was created.
        List<Condition> hitConditions = ChallengeManager.use.GetConditionsOfType<ObjectHitCondition>();
        foreach (Condition hitCond in hitConditions)
        {
            (hitCond as ObjectHitCondition).ScoreHitObjectCreated(this);
        }
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

        DoScore(collision.contacts[0].point);

        // Call hit event.
        if (Hit != null)
            Hit();
    }

    void OnTriggerEnter(Collider other)
    {
        // Only give score if the collider is a ball.
        if (other.gameObject.tag != "Ball")
            return;

        DoScore(other.transform.position);

        // Call hit event.
        if (Hit != null)
            Hit();
    }


    public void DoScore(Vector3 point)
    {
        ScoreManager.use.ShowScore(score, point.zAdd(Random.Range(10, 20)), 2.0f, sound, color, gameObject);
    }

    public void DoScore()
    {
        ScoreManager.use.ShowScore(score, transform.position.zAdd(Random.Range(10, 20)), 2.0f, sound, color, gameObject);
    }

}
