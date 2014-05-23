using UnityEngine;
using System.Collections;

public class KetnetController : MonoBehaviour 
{
	public string baseURL = "http://www.ketnet.be/";
	public string gameID = "50741b60ebece";
	public string gameKEY = "682fe574968afc2162d67ba6411dec2f";
	
	public string session_id = "";
	public bool logged_in = false;
	
	public bool live = true;
	
	public string staging_baseURL = "http://accept.ketnet.be/";
	public string staging_gameID = "50741f7871483";
	public string staging_gameKEY = "a7a54eb5a9ccf776ac2ec77e1b82db04";
	
	public string live_baseURL = "http://www.ketnet.be/";
	public string live_gameID = "50741b60ebece";
	public string live_gameKEY = "682fe574968afc2162d67ba6411dec2f";
	
	
	public delegate void OnLoggedIn();
	public OnLoggedIn onLoggedIn;
	
	public delegate void OnLeaderBoardLoaded();
	public OnLeaderBoardLoaded onLeaderBoardLoaded;
	
	public delegate void OnFriendLeaderBoardLoaded();
	public OnFriendLeaderBoardLoaded onFriendLeaderBoardLoaded;
	
	public delegate void OnStatusAdded();
	public OnStatusAdded onStatusAdded;
	
	public delegate void OnUserFetched();
	public OnUserFetched onUserFetched;
	
	/*
	public Hashtable leaderBoardGlobal = new Hashtable();
	public Hashtable leaderBoardMe = new Hashtable();
	public Hashtable friendLeaderBoard = new Hashtable();
	public Hashtable friendLeaderBoardMe = new Hashtable();
	*/
	
	public ArrayList leaderBoardGlobal = new ArrayList();
	//public ArrayList leaderBoardMe = new ArrayList();
	public ArrayList friendLeaderBoard = new ArrayList();
	//public ArrayList friendLeaderBoardMe = new ArrayList();
	
	void Awake()
	{
		if( GameObject.FindObjectsOfType(typeof(KetnetController)).Length > 1 )
		{
			gameObject.SetActive(false);
			Reset();
			GameObject.Destroy(this.gameObject);
			return;
		}
		else
			GameObject.DontDestroyOnLoad(this.gameObject);
	}
	
	// Use this for initialization
	void Start () 
	{
		
		SwitchServers();
		
		GetUser();
		
		//Login("Annatar","robin");
	}
	
	public void Reset()
	{
		onLoggedIn = null;
		onLeaderBoardLoaded = null;
		onFriendLeaderBoardLoaded = null;
		onStatusAdded = null;
		onUserFetched = null;
	}
	
	public void SwitchServers()
	{
		if( live )
		{
			baseURL = live_baseURL;
			gameID = live_gameID;
			gameKEY = live_gameKEY;
		}
		else
		{
			baseURL = staging_baseURL;
			gameID = staging_gameID;
			gameKEY = staging_gameKEY;
		}
	}
	
	protected bool loggingIn = false;
	
	public void Login(string username, string password)
	{
		if( loggingIn )
			return;
		
		loggingIn = true;
		
		StartCoroutine( LoginRoutine(username, password) );
	}
	
	public string debugTxt = "";
	
	protected IEnumerator LoginRoutine(string username, string password)
	{
		Debug.Log("LoginRoutine : " + username + " - " + password);
		
		WWWForm form = new WWWForm();
		
		form.AddField("game_id", gameID);
		form.AddField("name", username);
		form.AddField("pass", password);
		
		string url = baseURL + "ketnet_com/login";
			
			
		WWW www = new WWW(url, form);
			
		yield return www;
		
		Debug.LogError("Login returned : " + www.text);
		
		debugTxt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
		JSONObject objects = new JSONObject(www.text);
		
		if( objects.GetBool("success") )
		{
			session_id = objects.GetField("session_id").str;
			logged_in = true;
			
			if( onLoggedIn != null )
				onLoggedIn();
		}
		else
		{
			Debug.LogError("LoginRoutine: something went wrong... " + www.text);
		}
		
		loggingIn = false;
		
		yield return null;
	}
	
	public void GetUser()
	{
		StartCoroutine( GetUserRoutine() );
	}
	
	public IEnumerator GetUserRoutine()
	{
		string url = baseURL + "ws/user?";
		url = AddCommonParameters(url);
		//url += "session_id=" + session_id;
		
		Debug.Log("GetUserRoutine : " + url);
		
		WWW www = new WWW(url);
			
		yield return www;
		
		Debug.LogError("GetUserRoutine returned : " + www.text);
		
		debugTxt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
		JSONObject objects = new JSONObject(www.text);
		
		if( objects.GetBool("success") )
		{
			//int count = (int) objects.GetNumber("leaderboard_count");
			logged_in = true;
			
			if( onLoggedIn != null )
				onLoggedIn();
		}
		else
		{
			Debug.LogError("GetUserRoutine: something went wrong... " + www.text);
		}
		
		
		//Debug.Log("returned : " + objects["succes"] + " -> " + ((bool)objects["success"] == true) + " -> " + objects["leaderboard"]);
		
		yield return null;
	}
	
	
	public void GetLeaderboards()
	{
		StartCoroutine( LeaderboardsRoutine() );
	}
	
	protected IEnumerator LeaderboardsRoutine()
	{
		string url = baseURL + "ws/game/leaderboard?";
		url = AddCommonParameters(url);
		
		Debug.Log("LeaderBoardRoutine : " + url);
		
		WWW www = new WWW(url);
			
		yield return www;
		
		Debug.LogError("Leaderboard returned : " + www.text);
		
		debugTxt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
		JSONObject objects = new JSONObject(www.text);
		
		if( objects.GetBool("success") )
		{
			//int count = (int) objects.GetNumber("leaderboard_count");
			
			leaderBoardGlobal = new ArrayList();
			
			JSONObject entries = objects.GetField("leaderboard"); // leaderboard
			int counter = 0;
			for( counter = 0; counter < entries.list.Count; ++counter )
			{
				JSONObject entry = (JSONObject) entries.list[counter];
				string entryNr = (string) entries.keys[counter];
				
				JSONObject user = entry.GetField("user");
				
				leaderBoardGlobal.Add("" + entryNr + " : " + user.GetString("name") + "%" + entry.GetNumber("score") );
				
				Debug.Log("LEADERBOARD ENTRY : " + "" + entryNr + " : " + user.GetString("name") + " -> " + entry.GetNumber("score"));
			}
			
			ShowLeaderBoards();
			
			if( onLeaderBoardLoaded != null )
				onLeaderBoardLoaded();
		}
		else
		{
			Debug.LogError("LeaderboardsRoutine: something went wrong... " + www.text);
		}
		
		
		//Debug.Log("returned : " + objects["succes"] + " -> " + ((bool)objects["success"] == true) + " -> " + objects["leaderboard"]);
		
		yield return null;
		
	}
	
	public void GetFriendLeaderboards()
	{
		StartCoroutine( FriendLeaderboardsRoutine() );
	}
	
	protected IEnumerator FriendLeaderboardsRoutine()
	{
		string url = baseURL + "ws/game/friendleaderboard?";
		url = AddCommonParameters(url);
		
		Debug.Log("FriendLeaderboardsRoutine : " + url);
		//url += 
		
		WWW www = new WWW(url);
			
		yield return www;
		
		Debug.LogError("FriendsLeaderboard returned : " + www.text);
		
		debugTxt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
		JSONObject objects = new JSONObject(www.text);
		
		if( objects.GetBool("success") )
		{
			friendLeaderBoard = new ArrayList();
			
			JSONObject entries = objects.GetField("leaderboard"); // leaderboard
			int counter = 0;
			for( counter = 0; counter < entries.list.Count; ++counter )
			{
				JSONObject entry = (JSONObject) entries.list[counter];
				string entryNr = (string) entries.keys[counter];
				
				JSONObject user = entry.GetField("user");
				
				friendLeaderBoard.Add("" + entryNr + " : " + user.GetString("name") + "%" + entry.GetNumber("score") );
				
				Debug.Log("FRIENDLEADERBOARD ENTRY : " + "" + entryNr + " : " + user.GetString("name") + " -> " + entry.GetNumber("score"));
			}
			
			ShowFriendLeaderBoards();
			
			if( onFriendLeaderBoardLoaded != null )
				onFriendLeaderBoardLoaded();
		}
		else
		{
			Debug.LogError("FriendsLeaderboardRoutine: something went wrong... " + www.text);
		}
		
		
		//Debug.Log("returned : " + objects["succes"] + " -> " + ((bool)objects["success"] == true) + " -> " + objects["leaderboard"]);
		
		yield return null;
		
	}
	
	protected string AddCommonParameters(string url)
	{
		url += "game_id=" + gameID + "&";
		url += "apikey=" + gameKEY + "&";
		url += "session_id=" + session_id;
		return url;
	}
	
	protected bool addingScore = false;
	
	public void AddScore(int score)
	{
		if( addingScore )
			return;
		
		addingScore = true;
		
		StartCoroutine( AddScoreRoutine(score) );
	}
	
	protected IEnumerator AddScoreRoutine(int score)
	{
		Debug.Log("AddScoreRoutine : " + score);
		
		WWWForm form = new WWWForm();
		
		form.AddField("game_id", gameID);
		form.AddField("session_id", session_id);
		form.AddField("apikey", gameKEY);
		form.AddField("score", score);
		
		string url = baseURL + "ws/game/addscore";
		
			
		WWW www = new WWW(url, form);
			
		yield return www;
		
		Debug.LogError("Addscore returned : " + www.text);
		
		debugTxt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
		JSONObject objects = new JSONObject(www.text);
		
		if( objects.GetBool("success") )
		{
			Debug.Log("Addscore success!!");
			
			GetLeaderboards();
			GetFriendLeaderboards();
		}
		else
		{
			Debug.LogError("Addscore: something went wrong... " + www.text);
		}
		
		//Debug.Log("returned : " + objects["success"] + " -> " + ((bool)objects["success"] == true) + " -> " + objects["session_id"]);
		
		addingScore = false;
		
		yield return null;
	}
	
	protected bool addingStatus = false;
	
	public void AddStatus(int score)
	{
		if( addingStatus )
			return;
		
		addingStatus = true;
		
		StartCoroutine( AddStatusRoutine(score) );
	}
	
	public IEnumerator AddStatusRoutine(int score)
	{
		Debug.Log("AddStatusRoutine");
		
		WWWForm form = new WWWForm();
		
		form.AddField("game_id", gameID);
		form.AddField("session_id", session_id);
		form.AddField("apikey", gameKEY);
		form.AddField("update_message", "heeft " + score + " punten behaald in De Griezelflipperkast!");
		
		string url = baseURL + "ws/status/add";
		
			
		WWW www = new WWW(url, form);
			
		yield return www;
		
		Debug.LogError("AddStatusRoutine returned : " + www.text);
		
		debugTxt = www.text + " -> USING THE C# VERSION BABY!";// + www.bytes.Length;
		
		JSONObject objects = new JSONObject(www.text);
		
		if( objects.GetBool("success") )
		{
			Debug.Log("AddStatusRoutine success!!");
			
			if( onStatusAdded != null )
				onStatusAdded();
			
			/*
			ShareScoreButton[] buttons = Object.FindObjectsOfType( typeof(ShareScoreButton) ) as ShareScoreButton[];
			foreach( ShareScoreButton button in buttons )
				button.GoToSuccess();
			*/
		}
		else
		{
			Debug.LogError("AddStatusRoutine: something went wrong... " + www.text);
		}
		
		//Debug.Log("returned : " + objects["success"] + " -> " + ((bool)objects["success"] == true) + " -> " + objects["session_id"]);
		
		
		addingStatus = false;
		
		yield return null;
	}
	
	protected bool showGUI = false;
	
	
	void OnGUI()
	{
		if( !showGUI )
			return;
		
		if( debugTxt != "" )
			GUI.TextArea( new Rect(0, 0, 300, 250), debugTxt );
		
		if( GUI.Button( new Rect(0, 250, 200, 50), "Live " + live ) )
		{
			live = !live;
			session_id = "";
			SwitchServers();
		}
		
		if( GUI.Button( new Rect(0, 350, 200, 50), "Login" ) )
			Login("Annatar", "robin");
		
		if( GUI.Button( new Rect(0, 300, 200, 50), "Leaderboards" ) )
			GetLeaderboards();
		
		if( GUI.Button( new Rect(0, 400, 200, 50), "FriendLeaderboards" ) )
			GetFriendLeaderboards();
		
		if( GUI.Button( new Rect(0, 450, 200, 50), "AddScore" ) )
			AddScore(50000);
		
		
		if( GUI.Button( new Rect(0, 500, 200, 50), "GetUser" ) )
			GetUser();
		
		if( GUI.Button( new Rect(0, 550, 200, 50), "AddStatus" ) )
			AddStatus(50000);
		
		
	}
	
	
	public void ShowLeaderBoard(GameObject parent)
	{
		//if( leaderBoardGlobal.Count == 0 )
		//	return;
		
		int counter = 1;
		
		/*
		foreach( DictionaryEntry de in leaderBoardGlobal )
		{
			TextMesh header = parent.transform.FindChild("header"+counter).GetComponent<TextMesh>();
			TextMesh score = parent.transform.FindChild("score"+counter).GetComponent<TextMesh>();
			
			header.text = (string) de.Key;
			score.text = "" + de.Value;
			
			counter++;
		}
		*/
		
		foreach( object obj in leaderBoardGlobal )
		{
			TextMesh header = parent.transform.FindChild("header"+counter).GetComponent<TextMesh>();
			TextMesh score = parent.transform.FindChild("score"+counter).GetComponent<TextMesh>();
			
			string objString = (string) obj;
			//string[] splitters = new string[1];
			//splitters[0] = "@@@";
			//string[] split = objString.Split(splitters, System.StringSplitOptions.None);
			
			
			string[] split = objString.Split('%');
			
			header.text = split[0];
			score.text = split[1];
				
			counter++;
		}
			
		for( ; counter <= 6; ++counter )
		{
			
			TextMesh header = parent.transform.FindChild("header"+counter).GetComponent<TextMesh>();
			TextMesh score = parent.transform.FindChild("score"+counter).GetComponent<TextMesh>();
			
			header.text = "";
			score.text = "";
		}
	}
	
	public void ShowLeaderBoards()
	{
		GameObject[] globalScores = GameObject.FindGameObjectsWithTag("ScoresGlobal");
		foreach( GameObject score in globalScores )
			ShowLeaderBoard( score );
	}
	
	public void ShowFriendLeaderBoards()
	{
		GameObject[] meScores = GameObject.FindGameObjectsWithTag("ScoresME");
		foreach( GameObject score in meScores )
			ShowFriendLeaderBoard( score );
	}
	
	public void ShowFriendLeaderBoard(GameObject parent)
	{
		//if( friendLeaderBoard.Count == 0 )
		//	return;
		
		int counter = 1;
		/*
		foreach( DictionaryEntry de in friendLeaderBoard )
		{
			TextMesh header = parent.transform.FindChild("header"+counter).GetComponent<TextMesh>();
			TextMesh score = parent.transform.FindChild("score"+counter).GetComponent<TextMesh>();
			
			header.text = (string) de.Key;
			score.text = "" + de.Value;
			
			counter++;
		}
		*/
		
		
		foreach( object obj in friendLeaderBoard )
		{
			TextMesh header = parent.transform.FindChild("header"+counter).GetComponent<TextMesh>();
			TextMesh score = parent.transform.FindChild("score"+counter).GetComponent<TextMesh>();
			
			string objString = (string) obj;
			//string[] splitters = new string[1];
			//splitters[0] = "@@@";
			string[] split = objString.Split('%');
			
			header.text = split[0];
			score.text = split[1];
				
			counter++;
		}
		
			
		for( ; counter <= 6; ++counter )
		{
			
			TextMesh header = parent.transform.FindChild("header"+counter).GetComponent<TextMesh>();
			TextMesh score = parent.transform.FindChild("score"+counter).GetComponent<TextMesh>();
			
			header.text = "";
			score.text = "";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if( Input.GetKeyDown(KeyCode.Tab) )
		//	showGUI = !showGUI;
	}
	
}
