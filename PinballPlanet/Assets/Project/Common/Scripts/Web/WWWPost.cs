using UnityEngine;

[NotConverted]
[NotRenamed]
public class WWWPost
{
	[NotRenamed]
	public static string response = "No Response";
	
	/*
	[NotRenamedAttribute]
	public void AddScore()
	{
		//Do nothing in this case.
	}
	*/
	
	[NotRenamedAttribute]
	public void Post(string url, string parameters)
	{
		//StartCoroutine( PostRoutine(url, parameters) );
		
		//Debug.Log("Getting the url shizzles!");
		
		//PostRoutine(url, parameters);
	}
	/*
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
		
		Debug.Log(Time.frameCount + " second");
		
		response = www.text;
	}
	*/
	

}
