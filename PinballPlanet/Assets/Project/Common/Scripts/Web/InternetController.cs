using UnityEngine;
using System.Collections;

public class InternetController : MonoBehaviour 
{
	string txt = "";
	string total = "";

	// Use this for initialization
	void Start () 
	{
		StartCoroutine( TestDownload() );
	}
	
	public IEnumerator TestDownload()
	{
		/*
		WWW www = new WWW("http://www.ketnet.be/ws/user");
		yield return www;
		
		total = "done";
		
		//Debug.LogError( www.text );
		txt = www.text + " -> " + www.bytes.Length;
		*/
		
		
		//BrowserCommunicator.TestCommunication();
		
		//Application.ExternalCall("calledFromActionScript", "TESTING THE JS CALLZ");
		
		/*
        ActionScript.Import("flash.external.ExternalInterface");

        bool available = ActionScript.Expression<bool>("ExternalInterface.available");

        if(available){

            var functionName = "calledFromActionScript";

            var arg0 = "Hello From Unity!";

            ActionScript.Statement("ExternalInterface.call({0},{1});", functionName,arg0);

        }else{

            Debug.Log("ExternalInterface not available");   

        }
        */
		
		
		/*
		WWWCaller wwwCaller = new WWWCaller();
		wwwCaller.DoWebRequest("http://www.ketnet.be/ws/user");
		
		bool waiting = true;
		
		while( waiting )
		{
			if( WWWCaller.response != "" )
				waiting = false;
			
			yield return new WaitForSeconds(0.5f);
		}
		
		total = "done";
		txt = WWWCaller.response + " -> ";// + www.bytes.Length;
		*/
		
		
		
		/*
		
		WWWPost wwwPost = new WWWPost();
		//wwwPost.Post("http://accept.ketnet.be/ws/game/login", "");
		wwwPost.Post("http://ketnet.be/ketnet_com/login", "username=Annatar&password=robin");
		
		bool waiting = true;
		
		while( waiting )
		{
			if( WWWPost.response != "" )
				waiting = false;
			
			yield return new WaitForSeconds(0.5f);
		}
		
		total = "done";
		txt = WWWPost.response + " -> ";// + www.bytes.Length;
		*/
		
		//	Debug.Log("Starting PostRoutine");
		
		StartCoroutine( PostRoutine( "http://www.ketnet.be/ketnet_com/login", "game_id=50741f7871483&name=Annatar&pass=robin" ) );
		//StartCoroutine( PostRoutine( "http://accept.ketnet.be/ws/game/addscore", "game_id=50741f7871483&score=50" ) );
		
		
		yield return null;
		
	}
	
		protected IEnumerator PostRoutine(string url, string parameters)
		{
			Debug.Log("Getting the url shizzles! DICK");

			WWWForm form = new WWWForm();
			
			string[] paramList = parameters.Split('&');
			foreach(string paramTotal in paramList)
			{
				string[] paramParts = paramTotal.Split('=');
				form.AddField( paramParts[0], paramParts[1] );
				Debug.Log( "Added parameter: " + paramParts[0] + " = " + paramParts[1] );
			}
			
			
			WWW www = new WWW(url, form);
			
			Debug.Log(Time.frameCount + " first");
			
			yield return www;
			
			Debug.Log(Time.frameCount + " second = " + www.text);
			
			total = "done";
			txt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
			yield return null;
		}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	void OnGUI()
	{
		if( txt != "" )
			GUI.TextArea( new Rect(0, 0, 300, 300), txt );
		
		if( total != "" )
			GUI.TextArea( new Rect(0, 300, 300, 300), total );
	}
	*/
	
	
}
