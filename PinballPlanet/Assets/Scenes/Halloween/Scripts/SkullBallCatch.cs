using UnityEngine;
using System.Collections;

public class SkullBallCatch : MonoBehaviour
{
    private OpeningSkull _skull;
    private AudioClip _eatSound;

    void Start()
    {
        _skull = GameObject.Find("Skull_Opening").GetComponent<OpeningSkull>();

        _eatSound = LugusResources.use.Shared.GetAudio("SkullBallMunch01");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ball")
            return;

		if (_skull.Ball != null)
			return;

        _skull.CloseSkull();
        _skull.Ball = other.gameObject.GetComponent<Ball>();

        other.transform.position = new Vector3(transform.position.x, transform.position.y, other.transform.position.z);
        other.rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Play eat sound.
        LugusAudio.use.SFX().Play(_eatSound);
    }
}
