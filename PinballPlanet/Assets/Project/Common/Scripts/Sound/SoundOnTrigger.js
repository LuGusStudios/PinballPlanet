var SkullSound : AudioClip;

function OnTriggerEnter (other : Collider) {
    audio.clip = SkullSound;
    audio.Play();

}
@script RequireComponent(AudioSource)








