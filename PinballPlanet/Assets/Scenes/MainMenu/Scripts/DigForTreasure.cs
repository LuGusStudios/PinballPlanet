using UnityEngine;
using System.Collections;

public class DigForTreasure : MonoBehaviour {


	public ParticleSystem dustParticles = null;
	public int numberOfDigs = 5;
	private Button button_mark = null;
	public Button button_treasure = null;
	private Vector3 treasureScale;

	// Use this for initialization
	void Start () {
		button_mark = gameObject.FindComponentInChildren<Button>(true, "XMark");
		button_mark.gameObject.SetActive(true);
		button_treasure = gameObject.FindComponentInChildren<Button>(true, "HiddenTreasure");
		treasureScale = button_treasure.transform.localScale;
		LugusCoroutines.use.StartRoutine(SetInActiveDelayed());
	}

	// This makes sure that the button script can fetch the original scale
	// before the gameobject can go inactive. 
	IEnumerator SetInActiveDelayed ()
	{
 		yield return null;
		button_treasure.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (button_mark.pressed)
		{
			numberOfDigs -= 1;

			GameObject dustObject = Instantiate(dustParticles.gameObject, dustParticles.transform.position, dustParticles.transform.rotation) as GameObject;
			dustObject.transform.parent = transform;
			dustObject.GetComponent<ParticleSystem>().Play();
			Destroy(dustObject, 2.0f);

			if (numberOfDigs <= 0)
			{
				button_mark.gameObject.SetActive(false);
				button_treasure.gameObject.SetActive(true);
				button_treasure.transform.localScale = Vector3.zero;
				button_treasure.gameObject.ScaleTo(treasureScale).Time(1.0f).EaseType(iTween.EaseType.easeOutBounce).IsLocal(true).Execute();
			}
		}
	}
}
