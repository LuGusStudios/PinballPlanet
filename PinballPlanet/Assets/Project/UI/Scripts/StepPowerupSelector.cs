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

	protected TextMeshWrapper powerup1StarCost = null;
	protected TextMeshWrapper powerup2StarCost = null;

	protected Transform powerup1Frame = null;
	protected Transform powerup2Frame = null;

	protected TextMeshWrapper playerStarCount = null;

	protected Transform starCost1 = null;
	protected Transform starCost2 = null;

	protected Transform lock1 = null; 
	protected Transform lock2 = null;
	
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

		powerup1StarCost = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU1_Cost");
		powerup2StarCost = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU2_Cost");

		powerup1Frame = transform.FindChildRecursively("Selection_PU1");
		powerup2Frame = transform.FindChildRecursively("Selection_PU2");

		playerStarCount = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_StarCount");

		starCost1 = transform.FindChildRecursively("StarCost1");
		starCost2 = transform.FindChildRecursively("StarCost2");
		lock1 = transform.FindChildRecursively("Lock1");
		lock2 = transform.FindChildRecursively("Lock2");
				
		originalPosition = transform.position;
		
		permanentPowerups = new List<Powerup>();
		temporaryPowerups = new List<Powerup>();
	}
	
	public void SetupGlobal()
	{
		Debug.Log ("Setup Global");
		FillPowerupLists();
		UpdatePowerups();
		playerStarCount.SetText(PlayerData.use.Stars + "");
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
				// Set new powerup
				Powerup pu = permanentPowerups[_startIndex];
				if (pu == null)
				{
					// Deactivate old powerup
					PowerupManager.use.DeactivatePermanentPowerup();

					PlayerData.use.permanentPowerup = permanentPowerups[_startIndex];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
				else if(PlayerData.use.GetLevel() >= pu.unlockLevel && PlayerData.use.Stars >= pu.starCost)
				{
					// Deactivate old powerup
					PowerupManager.use.DeactivatePermanentPowerup();

					PlayerData.use.Stars -= pu.starCost;
					PlayerData.use.permanentPowerup = permanentPowerups[_startIndex];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
			}
			else 
			{
				Powerup pu = temporaryPowerups[_startIndex];
				if (pu == null)
				{
					PlayerData.use.temporaryPowerup = temporaryPowerups[_startIndex];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
				else if(PlayerData.use.GetLevel() >= pu.unlockLevel && PlayerData.use.Stars >= pu.starCost)
				{
					PlayerData.use.Stars -= pu.starCost;
					PlayerData.use.temporaryPowerup = temporaryPowerups[_startIndex];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
			}
		}
		else if(powerup2Button.pressed)
		{
			if (updatingPermanentPowerups)
			{
				// Set new powerup
				Powerup pu = permanentPowerups[GetNextIndex()];
				if (pu == null)
				{
					// Deactivate old powerup
					PowerupManager.use.DeactivatePermanentPowerup();

					PlayerData.use.permanentPowerup = permanentPowerups[GetNextIndex()];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
				else if(PlayerData.use.GetLevel() >= pu.unlockLevel && PlayerData.use.Stars >= pu.starCost)
				{
					// Deactivate old powerup
					PowerupManager.use.DeactivatePermanentPowerup();

					PlayerData.use.Stars -= pu.starCost;
					PlayerData.use.permanentPowerup = permanentPowerups[GetNextIndex()];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
			}
			else 
			{
				Powerup pu = temporaryPowerups[GetNextIndex()];
				if (pu == null)
				{
					PlayerData.use.temporaryPowerup = temporaryPowerups[GetNextIndex()];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
				else if(PlayerData.use.GetLevel() >= pu.unlockLevel && PlayerData.use.Stars >= pu.starCost)
				{
					PlayerData.use.Stars -= pu.starCost;
					PlayerData.use.temporaryPowerup = temporaryPowerups[GetNextIndex()];
					MenuManager.use.DeactivateOverlayMenu(this, false);
					PlayerData.use.Save();
				}
			}

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
		else if (exitButton.pressed || LugusInput.use.KeyDown(KeyCode.Escape))
		{
			MenuManager.use.DeactivateOverlayMenu(this, false);
		}
	}
	
	protected void UpdatePowerups()
	{
		Powerup pu1 = null;
		Powerup pu2 = null;

		powerup1Frame.gameObject.SetActive(false);
		powerup2Frame.gameObject.SetActive(false);

		if (updatingPermanentPowerups)
		{
			pu1 = permanentPowerups[_startIndex];
			pu2 = permanentPowerups[GetNextIndex()];

			if (pu1 == null && PlayerData.use.permanentPowerup == null)			
				powerup1Frame.gameObject.SetActive(true);			
			else if (pu1 != null && PlayerData.use.permanentPowerup != null && (PowerupKey)pu1.id == (PowerupKey)PlayerData.use.permanentPowerup.id)
				powerup1Frame.gameObject.SetActive(true);

			if (pu2 == null && PlayerData.use.permanentPowerup == null)			
				powerup2Frame.gameObject.SetActive(true);			
			else if (pu2 != null && PlayerData.use.permanentPowerup != null && (PowerupKey)pu2.id == (PowerupKey)PlayerData.use.permanentPowerup.id)
				powerup2Frame.gameObject.SetActive(true);

		} 
		else 
		{
			pu1 = temporaryPowerups[_startIndex];
			pu2 = temporaryPowerups[GetNextIndex()];

			if (pu1 == null && PlayerData.use.temporaryPowerup == null)			
				powerup1Frame.gameObject.SetActive(true);			
			else if (pu1 != null && PlayerData.use.temporaryPowerup != null && (PowerupKey)pu1.id == (PowerupKey)PlayerData.use.temporaryPowerup.id)
				powerup1Frame.gameObject.SetActive(true);
			
			if (pu2 == null && PlayerData.use.temporaryPowerup == null)			
				powerup2Frame.gameObject.SetActive(true);			
			else if (pu2 != null && PlayerData.use.temporaryPowerup != null && (PowerupKey)pu2.id == (PowerupKey)PlayerData.use.temporaryPowerup.id)
				powerup2Frame.gameObject.SetActive(true);
		}

		// update PU1
		if (pu1 != null){

			bool levelReqOK = pu1.unlockLevel <= PlayerData.use.GetLevel();
			bool starCost = pu1.starCost != 0;

			powerup1Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/" + pu1.iconName);
			powerup1Text.SetText(pu1.name);
			// If level requirement ok -> show
			if (levelReqOK)
			{
				powerup1Icon.color = powerup1Icon.color.a(1.0f);
				lock1.gameObject.SetActive(false);
				powerup1Description.SetText(pu1.description);
			}
			// If level requirement not ok -> show lock and level requirement
			else 
			{
				powerup1Icon.color = powerup1Icon.color.a(0.5f);
				lock1.gameObject.SetActive(true);
				string t = LugusResources.use.Localized.GetText("UnlockedAtLevelPrefix") +
						pu1.unlockLevel +
						LugusResources.use.Localized.GetText("UnlockedAtLevelSuffix");
				powerup1Description.SetText(t);
			}

			// If there is a star cost, show it. 
			if (starCost)
			{
				starCost1.gameObject.SetActive(true);
				powerup1StarCost.SetText(pu1.starCost + "");
			}
			else 
			{
				starCost1.gameObject.SetActive(false);
			}
		} 
		else 
		{
			powerup1Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/Icon_NoPower01");
			powerup2Icon.color = powerup2Icon.color.a(1.0f);
			powerup1Text.SetText("None");
			powerup1Description.SetText("");
			starCost1.gameObject.SetActive(false);
			lock1.gameObject.SetActive(false);
		}

		// update PU2
		if (pu2 != null)
		{			
			bool levelReqOK = pu2.unlockLevel <= PlayerData.use.GetLevel();
			bool starCost = pu2.starCost != 0;

			powerup2Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/" + pu2.iconName);
			powerup2Text.SetText(pu2.name);
			// If level requirement ok -> show
			if (levelReqOK)
			{
				powerup2Icon.color = powerup2Icon.color.a(1.0f);
				lock2.gameObject.SetActive(false);
				powerup2Description.SetText(pu2.description);
			}
			// If level requirement not ok -> show lock and level requirement
			else 
			{
				powerup2Icon.color = powerup2Icon.color.a(0.5f);
				lock2.gameObject.SetActive(true);
				string t = LugusResources.use.Localized.GetText("UnlockedAtLevelPrefix") +
						pu2.unlockLevel +
						LugusResources.use.Localized.GetText("UnlockedAtLevelSuffix");
				powerup2Description.SetText(t);
			}
			
			// If there is a star cost, show it. 
			if (starCost)
			{
				starCost2.gameObject.SetActive(true);
				powerup2StarCost.SetText(pu2.starCost + "");
			}
			else 
			{
				starCost2.gameObject.SetActive(false);
			}
		}
		else 
		{
			powerup2Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/Icon_NoPower01");
			powerup2Icon.color = powerup2Icon.color.a(1.0f);
			powerup2Text.SetText("None");
			powerup2Description.SetText("");
			starCost2.gameObject.SetActive(false);
			lock2.gameObject.SetActive(false);
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

		//temporaryPowerups.Sort(delegate(Powerup p1, Powerup p2) { return p1.unlockLevel > p2.unlockLevel; });
		permanentPowerups.Sort(delegate(Powerup p1, Powerup p2) { 

			if (p1 == null)
				return -1;
			if (p2 == null)
				return 1;

			return p1.unlockLevel - p2.unlockLevel; 
		});
	}
	
	public override void Activate(bool animate = true)
	{
		activated = true;
		gameObject.SetActive(true);
		_startIndex = 0;
		FillPowerupLists();
		UpdatePowerups();
		playerStarCount.SetText(PlayerData.use.Stars + "");
	}
	
	public override void Deactivate(bool animate = true)
	{
		activated = false;
		gameObject.SetActive(false);
	}
}
