using UnityEngine;

public class LoginMessage : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//GetComponent<TextMesh>().text = "Je moet inloggen om \n scores te kunnen bekijken \n en delen.";	
		if( GameObject.Find("GOD").GetComponent<KetnetController>().logged_in )
			gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
