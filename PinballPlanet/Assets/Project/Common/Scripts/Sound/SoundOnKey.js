var FlipperSoundLeft : AudioClip;
var FlipperSoundRight : AudioClip;
//var LaunchSound : AudioClip;
//var ReleaseSound : AudioClip;

function Update() {

if (Input.GetKeyDown("left"))
{
	audio.PlayOneShot(FlipperSoundLeft);
}

if (Input.GetKeyDown("right"))
{
	audio.PlayOneShot(FlipperSoundRight);
}

/*
if (Input.GetKeyDown("down"))
{
	audio.clip = LaunchSound;
	audio.loop = true;
	audio.Play();
	//audio.PlayOneShot(LaunchSound);
}

if (Input.GetKeyUp("down"))
{
	audio.loop = false;
	audio.Stop();
	
	audio.PlayOneShot(ReleaseSound);
}
*/

}
@script RequireComponent(AudioSource)