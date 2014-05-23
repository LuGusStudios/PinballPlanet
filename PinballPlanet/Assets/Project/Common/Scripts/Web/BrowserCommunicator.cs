using UnityEngine;

[NotConverted]
[NotRenamed]
public class BrowserCommunicator
{
	/*
   [NotRenamed]
   public static void TestCommunication()
   {
		//GameObject.Find("JESUS").GetComponent<UIGameController>().ShowGameoverGUI();
   }
	*/
	
	[NotRenamed]
	public static void showLoginform(string redirectURL)
	{
		Debug.LogError("LoginForm shown: redirecting to " + redirectURL);
	}
	
	[NotRenamed]
	public static string GetURL()
	{
		return "key1@test1$url@/parent/redirect/griezelflipperkast/$key2@test2$";
	}
}
