using UnityEngine;
using System.Collections;

public class StarButton : Button
{
    // Update is called once per frame
    protected override IEnumerator PressRoutine()
    {
        // Add star.
        ++PlayerData.use.Stars;

        // Destroy button.
        Destroy(gameObject);
        Destroy(transform.parent.gameObject);

        LugusAudio.use.SFX().Play(Resources.Load<AudioClip>("Shared/Audio/Crystal_01"));

        return base.PressRoutine();
    }
}
