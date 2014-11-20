using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FacebookBasic {

	// Make this a singleton
	protected static FacebookBasic _instance;
	private FacebookBasic() {}
	public static FacebookBasic use {
		get {
			if (_instance == null){
				_instance = new FacebookBasic();		
			}
			return _instance;
		}
	}

	private bool _isInitialized = false;
	const string AppId = "817815064936088";
	const string ShareUrl = "http://www.facebook.com/dialog/feed";

	protected Texture2D lastResponseTexture;
	protected string lastResponse = "";

	private bool _shouldStillPost = false;
	private string _shouldStillPostMessage = ""; 

	public void Initialize()
	{
		CallFBInit();
	}

	public void Share (string message)
	{
		if (FB.IsLoggedIn)
		{
			PostToFeed(message);
		}
		else 
		{
			CallFBLogin();
			_shouldStillPost = true;
			_shouldStillPostMessage = message;
		}
	}

	private void PostToFeed(string message) 
	{
		FB.Feed(
			link: "https://www.facebook.com/pinballplanet/",
			linkName: "Pinball Planet",
			linkCaption: "",
			linkDescription: message,
			picture: "http://www.pinball-planet.com/media/avatarsmall.png"
		);
	}

	void LoginCallback(FBResult result)
	{
		if (result.Error != null)
			lastResponse = "Error Response:\n" + result.Error;
		else if (!FB.IsLoggedIn)
		{
			lastResponse = "Login cancelled by Player";
		}
		else
		{
			lastResponse = "Login was successful!";

			if (_shouldStillPost)
				PostToFeed(_shouldStillPostMessage);

			_shouldStillPost = false;
		}
	}

	protected void Callback(FBResult result)
	{
		lastResponseTexture = null;
		// Some platforms return the empty string instead of null.
		if (!String.IsNullOrEmpty (result.Error))
		{
			lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!String.IsNullOrEmpty (result.Text))
		{
			lastResponse = "Success Response:\n" + result.Text;
		}
		else if (result.Texture != null)
		{
			lastResponseTexture = result.Texture;
			lastResponse = "Success Response: texture\n";
		}
		else
		{
			lastResponse = "Empty Response\n";
		}
	}

	void LogCallback(string response) {
		Debug.Log(response.ToString());
	}

	private void CallFBInit()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}
	
	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		_isInitialized = true;
	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}
	
	private void CallFBLogin()
	{
		FB.Login("email,publish_actions", LoginCallback);
	}
	

	
	private void CallFBLogout()
	{
		FB.Logout();
	}

}
