using UnityEngine;

public class LoginPopup : MonoBehaviour 
{
	public Transform loginButton = null;
	public Transform cancelButton = null;
	
	public TextMesh usernameInput = null;
	public TextMesh passwordInput = null;
	
	protected Camera uiCamera = null;

	// Use this for initialization
	void Start () 
	{
		uiCamera = GameObject.Find("UICamera").camera;
	}
	
	// Update is called once per frame
	void Update () 
	{
		usernameInput.text += Input.inputString;
	
		Transform t = LugusInput.use.RayCastFromMouseDown(uiCamera);
		
		if( t == loginButton )
		{
			Debug.LogError("Login with " + username + " -> " + password );
			KetnetController kc = GameObject.Find("GOD").GetComponent<KetnetController>();
		
			//kc.onStatusAdded += GoToSuccess;
			kc.Login(username, password);
		}
		else if( t == cancelButton )
		{
			Debug.LogError("Cancelling login");
		}
	}
	
	protected string username = "";
	protected string password = "";
	
	/*
	void OnGUI()
	{
		username = GUI.TextField( new Rect(345, 390, 175, 30), username);
		password = GUI.PasswordField( new Rect(345, 445, 175, 30), password, '*');
	}
	*/
}
