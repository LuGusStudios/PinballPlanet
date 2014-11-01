using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StepPowerupSelectMenu : IMenuStep 
{
	protected Vector3 originalPosition = Vector3.zero;
	protected Button powerup1Button = null;
	protected Button powerup2Button = null;
	protected Button playButton = null;

	protected SpriteRenderer powerup1Icon = null;
	protected SpriteRenderer powerup2Icon = null;

	protected TextMeshWrapper powerup1Text = null;
	protected TextMeshWrapper powerup2Text = null;

	private Powerup _selectedPU1 = null;
	private Powerup _selectedPU2 = null;
	
	public override void SetupLocal()
	{
		powerup1Button = gameObject.FindComponentInChildren<Button>(true, "Powerup1Time");
		powerup2Button = gameObject.FindComponentInChildren<Button>(true, "PowerupPerm");

		playButton = gameObject.FindComponentInChildren<Button>(true, "PlayButton");

		powerup1Icon = gameObject.FindComponentInChildren<SpriteRenderer>(true, "Icon_PU1Time");
		powerup2Icon = gameObject.FindComponentInChildren<SpriteRenderer>(true, "Icon_PUPerm");

		powerup1Text = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PU1Time");
		powerup2Text = gameObject.FindComponentInChildren<TextMeshWrapper>(true, "Text_PUPerm");
		
		originalPosition = transform.position;
	}
	
	public void SetupGlobal()
	{	
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

		// Update powerups when they are changed
		if (_selectedPU1 != PowerupManager.use.GetTemporaryPowerup() ||
		    _selectedPU2 != PowerupManager.use.GetPermanentPowerup() )
		{
			_selectedPU1 = PowerupManager.use.GetTemporaryPowerup();
			_selectedPU2 = PowerupManager.use.GetPermanentPowerup();
			UpdatePowerups();
		}

		if(powerup1Button.pressed)
		{
			transform.parent.gameObject.FindComponentInChildren<StepPowerupSelector>(true, "PowerupSelector").updatingPermanentPowerups = false;
			MenuManager.use.ActivateOverlayMenu(MenuManagerDefault.MenuTypes.PowerupSelector, false);
		}
		else if(powerup2Button.pressed)
		{
			transform.parent.gameObject.FindComponentInChildren<StepPowerupSelector>(true, "PowerupSelector").updatingPermanentPowerups = true;
			MenuManager.use.ActivateOverlayMenu(MenuManagerDefault.MenuTypes.PowerupSelector, false);
		}
		else if (playButton.pressed)
		{
			MenuManager.use.ActivateMenu(MenuManagerDefault.MenuTypes.GameMenu, false);

			PowerupManager.use.ActivatePowerups();
			Player.use.ReleaseBall();
		}
	}

	protected void UpdatePowerups()
	{

		Powerup pu1 = PowerupManager.use.GetTemporaryPowerup();
		Powerup pu2 = PowerupManager.use.GetPermanentPowerup();

		// update PU1
		if (pu1 != null){
			powerup1Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/" + pu1.iconName);
			powerup1Text.SetText(pu1.name);
		} 
		else 
		{
			powerup1Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/Icon_NoPower01");
			powerup1Text.SetText("None");
		}
		
		// update PU2
		if (pu2 != null)
		{
			powerup2Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/" + pu2.iconName);
			powerup2Text.SetText(pu2.name);
		}
		else 
		{
			powerup2Icon.sprite = LugusResources.use.Shared.GetSprite("Powerups/Icon_NoPower01");
			powerup2Text.SetText("None");
		}
	}

	public override void Activate(bool animate = true)
	{
		activated = true;
		UpdatePowerups();
		gameObject.SetActive(true);
	}
	
	public override void Deactivate(bool animate = true)
	{
		activated = false;
		gameObject.SetActive(false);
	}
}
