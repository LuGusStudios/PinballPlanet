using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StepPowerupSelector : IMenuStep 
{
	protected Vector3 originalPosition = Vector3.zero;
	protected Button upButton = null;
	protected Button downButton = null;
	protected Button powerup1Button = null;
	protected Button powerup2Button = null;
	protected Button exitButton = null;
	
	protected SpriteRenderer powerup1Icon = null;
	protected SpriteRenderer powerup2Icon = null;
	
	protected TextMeshWrapper powerup1Text = null;
	protected TextMeshWrapper powerup2Text = null;
	protected TextMeshWrapper powerup1Description = null;
	protected TextMeshWrapper powerup2Description = null;
	
	
	protected List<Powerup> permanentPowerups = null;
	protected List<Powerup> temporaryPowerups = null;
	
	private int _startIndex = 0;
	public bool updatingPermanentPowerups = true;
	
	public override void SetupLocal()
	{
		Debug.Log ("Setup Local");
		upButton = gameObject.FindComponentInChildren<Button>(true, "ArrowUp");
		downButton = gameObject.FindComponentInChildren<Button>(true, "ArrowDown");
		
		powerup1Button = gameObject.FindComponentInChildren<Button>(true, "Powerup1");
		powerup2Button = gameObject.FindComponentInChildren<Button>(true, "Powerup2");
		
		exitButton = gameObject.FindComponentInChildren<Button>(true, "ExitButton");
		
		powerup1Icon = gameObject.FindComponentInChildren<SpriteRenderer>(true, "Icon_PU1");
		powerup2Icon = gameObject.FindComponentInChildren<SpriteRenderer>(true, "Icon_PU2");
		
		powerup1Text = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU1");
		powerup2Text = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU2");
		
		powerup1Description = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU1_Description");
		powerup2Description = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU2_Description");
		
		originalPosition = transform.position;
		
		permanentPowerups = new List<Powerup>();
		temporaryPowerups = new List<Powerup>();
	}
	
	public void SetupGlobal()
	{
		Debug.Log ("Setup Global");
		FillPowerupLists();
		UpdatePowerups();
	}
	
	protected void Start()
	{
		SetupGlobal();
	}
	
	protected void Update()
	{
		if (!activated)
			return;
		
		if(powerup1Button.pressed)
		{
			if (updatingPermanentPowerups)
			{
				PlayerData.use.permanentPowerup = permanentPowerups[_startIndex];
			}
			else 
			{
				PlayerData.use.temporaryPowerup = temporaryPowerups[_startIndex];
			}
			MenuManager.use.DeactivateOverlayMenu(this, false);
			PlayerData.use.Save();
		}
		else if(powerup2Button.pressed)
		{
			if (updatingPermanentPowerups)
			{
				PlayerData.use.permanentPowerup = permanentPowerups[GetNextIndex()];
			}
			else 
			{
				PlayerData.use.temporaryPowerup = temporaryPowerups[GetNextIndex()];
			}
			MenuManager.use.DeactivateOverlayMenu(this, false);
			PlayerData.use.Save();
		}
		else if (upButton.pressed)
		{
			_startIndex = GetPreviousIndex();
			UpdatePowerups();
		}
		else if (downButton.pressed)
		{
			_startIndex = GetNextIndex();
			UpdatePowerups();
		}
		else if (exitButton.pressed)
		{
			MenuManager.use.DeactivateOverlayMenu(this, false);
		}
	}
	
	protected void UpdatePowerups()
	{
		Powerup pu1 = null;
		Powerup pu2 = null;
		
		if (updatingPermanentPowerups)
		{
			pu1 = permanentPowerups[_startIndex];
			pu2 = permanentPowerups[GetNextIndex()];
		} 
		else 
		{
			pu1 = temporaryPowerups[_startIndex];
			pu2 = temporaryPowerups[GetNextIndex()];
		}
		
		// update PU1
		if (pu1 != null){
			powerup1Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/" + pu1.iconName);
			powerup1Text.SetText(pu1.name);
			powerup1Description.SetText(pu1.description);
		} 
		else 
		{
			powerup1Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/Icon_NoPower01");
			powerup1Text.SetText("None");
			powerup1Description.SetText("");
		}
		
		// update PU2
		if (pu2 != null)
		{
			powerup2Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/" + pu2.iconName);
			powerup2Text.SetText(pu2.name);
			powerup2Description.SetText(pu2.description);
		}
		else 
		{
			powerup2Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/Icon_NoPower01");
			powerup2Text.SetText("None");
			powerup2Description.SetText("");
		}
	}
	
	protected int GetNextIndex()
	{
		int index = _startIndex;
		index ++;
		if (updatingPermanentPowerups)
		{
			if (index >= permanentPowerups.Count)
			{
				index = 0;
			}
		}
		else 
		{
			if (index >= temporaryPowerups.Count)
			{
				index = 0;
			}
		}
		return index;
	}
	
	protected int GetPreviousIndex()
	{
		int index = _startIndex;
		index --;
		if (updatingPermanentPowerups)
		{
			if (index < 0)
			{
				index = permanentPowerups.Count - 1;
			}
		}
		else 
		{
			if (index < 0)
			{
				index = temporaryPowerups.Count - 1;
			}
		}
		return index;
	}
	
	public void FillPowerupLists()
	{
		List<int> PermanentPowerupIndices = new List<int>();
		List<int> TemporaryPowerupIndices = new List<int>();
		temporaryPowerups = new List<Powerup>();
		permanentPowerups = new List<Powerup>();
		
		foreach (int value in Enum.GetValues(typeof(PowerupKey)))
		{
			if (value == 0)
			{
				temporaryPowerups.Add(PowerupManager.use.GetNewPowerupOfType((PowerupKey)value));
				permanentPowerups.Add(PowerupManager.use.GetNewPowerupOfType((PowerupKey)value));
			} 
			else if (value < 100)
			{
				temporaryPowerups.Add(PowerupManager.use.GetNewPowerupOfType((PowerupKey)value));
			}
			else if (value > 100)
			{
				permanentPowerups.Add(PowerupManager.use.GetNewPowerupOfType((PowerupKey)value));
			}
		}
	}
	
	public override void Activate(bool animate = true)
	{
		activated = true;
		gameObject.SetActive(true);
		_startIndex = 0;
		FillPowerupLists();
		UpdatePowerups();
	}
	
	public override void Deactivate(bool animate = true)
	{
		activated = false;
		gameObject.SetActive(false);
	}
}
