using UnityEngine;
using System.Collections;

public class StarButton : Button
{

	protected Transform parentTransform = null;
	protected ILugusCoroutineHandle coroutineHandle = null;

    // Update is called once per frame
    protected override IEnumerator PressRoutine()
    {
        // Add star.
        ++PlayerData.use.Stars;

        // Stop animation and destroy coroutine
		StopAnimation();
		coroutineHandle.StopRoutine();

		//TODO Animation!
		LugusAudio.use.SFX().Play(LugusResources.use.Shared.GetAudio("Crystal_01"));

		// Destroy button.
        Destroy(transform.parent.gameObject);

        return base.PressRoutine();
    }

	public void StopAnimation()
	{
		parentTransform.gameObject.StopTweens();
	}

	public void StartAnimation()
	{
		Vector3 startPoint = new Vector3();
		Vector3[] path = new Vector3[]{
			new Vector3( 0.0f,  0.0f, -2.0f),
			new Vector3( 0.0f,  3.0f, -2.0f),
			new Vector3(-2.0f,  3.0f, -2.0f),
			new Vector3( 3.0f,  9.0f, -2.0f)
		};

		parentTransform = transform.parent;

		parentTransform.localPosition = path[0];
		parentTransform.localScale = Vector3.zero;

		parentTransform.gameObject.MoveTo(path).MoveToPath(false).IsLocal(true).Time(4.0f).Delay(2.0f).Execute();
		parentTransform.gameObject.ScaleTo(Vector3.one*3).Time(1.5f).Execute();
		parentTransform.gameObject.ScaleTo(Vector3.one).Time(3.5f).Delay(1.6f).Execute();

		coroutineHandle = LugusCoroutines.use.StartRoutine(DestroyRoutine(6.5f));
	}

	protected IEnumerator DestroyRoutine(float delay)
	{
		yield return new WaitForSeconds(delay);
		StopAnimation ();
		Destroy(parentTransform.gameObject);
	}

}
