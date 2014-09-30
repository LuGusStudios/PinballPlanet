using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PopupManager : LugusSingletonExisting<PopupManager> 
{
	public Popup boxPrefab = null;
	public List<Popup> boxes = new List<Popup>();

	public Popup CreateBox( string text, Sprite icon = null )
	{
		Popup output = null;
		foreach( Popup box in boxes )
		{
			if( box.available )
			{
				output = box;
				break;
			}
		}
		
		if( output == null )
		{
			output = (Popup) GameObject.Instantiate( boxPrefab );
			output.transform.position = boxPrefab.transform.position;
			boxes.Add( output );
		}

		print (output.transform.Path());
		
		output.available = false;
		output.boxType = Popup.PopupType.Notification;
		
		output.text = text;
		output.icon.sprite = icon; 

		output.transform.parent = boxPrefab.transform.parent; 

		return output;
	}

	public void HideOthers(Popup keep)
	{
		foreach( Popup box in boxes )
		{
			if( box != keep )
				box.Hide();
		}
	}

	public void HideAll()
	{
		HideOthers( null );
	}



	public void SetupLocal()
	{
		// assign variables that have to do with this class only
		if( boxPrefab == null )
		{
			boxPrefab = gameObject.FindComponentInChildren<Popup>(true);
		}

		if( boxPrefab == null )
		{
			Debug.LogError( transform.Path () + " : no BoxPrefab known for DialogueManager!");
		}
		else
		{
			boxes.Add( boxPrefab ); // use the prefab itself as well: no waste!
	
			boxPrefab.SetupLocal();
			boxPrefab.SetupGlobal();
			boxPrefab.gameObject.SetActive(false);
		}
	}
	
	public void SetupGlobal()
	{
		// lookup references to objects / scripts outside of this script
	}
	
	protected void Awake()
	{
		SetupLocal();
	}

	protected void Start () 
	{
		SetupGlobal();
	}
}
