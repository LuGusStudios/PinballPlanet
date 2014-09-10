using UnityEngine;
using System.Collections;

public class TrainHandle : Triggerable
{
    public string AnimationName;

    private bool _setDefault = false;

    public AudioClip Sound;

    protected override void Start()
    {
        // Automatically set to default.
        SetDefault();

        base.Start();
    }

    protected override void TriggerHit(GameObject trigger, GameObject other)
    {
        if (other.name != "Ball")
            return;

        if (_setDefault)
            SetBridge();
        else
            SetDefault();

        if (Sound != null)
            //audio.PlayOneShot(Sound);
            LugusAudio.use.SFX().Play(Sound).Loop = false;
    }

    public void SetDefault()
    {
        _setDefault = true;

        AnimationState animState = animation[AnimationName];
        animState.time = 0;
        animState.speed = 1;

        animation.Sample();
        animation.Play(AnimationName);

    }

    public void SetBridge()
    {
        _setDefault = false;

        AnimationState animState = animation[AnimationName];
        animState.time = animState.length;
        animState.speed = -1;

        animation.Sample();
        animation.Play(AnimationName);
    }
}
