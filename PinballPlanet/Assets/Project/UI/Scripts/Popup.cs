using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Popup : MonoBehaviour 
{
	public enum PopupType
	{
		NONE = -1,

		Notification = 1, // no buttons, auto-hide or hidden by script
		Continue = 2, // just an OK button
		ConfirmCancel = 3
	}

	public bool available = true;
	public string text = "";
	public SpriteRenderer icon = null;
	public PopupType boxType = PopupType.Notification;
	public Button continueButton = null;
	public Button confirmButton = null;
	public Button cancelButton = null;

	public delegate void OnContinueButtonClicked(Popup box);
	public OnContinueButtonClicked onContinueButtonClicked = null;
	
	public delegate void OnConfirmButtonClicked(Popup box);
	public OnConfirmButtonClicked onConfirmButtonClicked = null;
	
	public delegate void OnCancelButtonClicked(Popup box);
	public OnCancelButtonClicked onCancelButtonClicked = null;
	
	public delegate void OnBoxClicked(Popup box);
	public OnBoxClicked onBoxClicked = null;
	
	public bool blockInput = false;

	protected Transform background = null;
	protected TextMeshWrapper textWithIcon = null;
	protected TextMeshWrapper textWithoutIcon = null;
	protected TextMeshWrapper textNoButtons = null;
	protected BoxCollider inputBlockCollider = null;
	
	public void UpdateText()
	{
		// update the text-wrapping
		TextMeshWrapper chosenText = null;

		if ((boxType == PopupType.Notification || boxType == PopupType.NONE) && this.icon.sprite == null)
		{
			this.textWithoutIcon.gameObject.SetActive(false);
			this.textWithIcon.gameObject.SetActive(false);
			this.textNoButtons.gameObject.SetActive(true);

			chosenText = this.textNoButtons;
		}
		else
		{
			if( this.icon.sprite != null )
			{
				this.textWithoutIcon.gameObject.SetActive(false);
				this.textWithIcon.gameObject.SetActive(true);
				this.textNoButtons.gameObject.SetActive(false);

				chosenText = this.textWithIcon;
			}
			else
			{
				this.textWithoutIcon.gameObject.SetActive(true);
				this.textWithIcon.gameObject.SetActive(false);
				this.textNoButtons.gameObject.SetActive(false);

				chosenText = this.textWithoutIcon;
			}
		}

		chosenText.SetText(text);

	}
	
	protected ILugusCoroutineHandle autoHideHandle = null;

	protected void ShowButtons()
	{
		//Debug.LogError("RepositionButtons " + boxType);

		confirmButton.transform.parent.gameObject.SetActive(false);

		if( boxType == PopupType.NONE )
		{
			continueButton.transform.parent.gameObject.SetActive(false);
			confirmButton.transform.parent.gameObject.SetActive(false);
		}
		else if( boxType == PopupType.Notification )
		{
			continueButton.transform.parent.gameObject.SetActive(false);
			confirmButton.transform.parent.gameObject.SetActive(false);
		}
		else if( boxType == PopupType.Continue )
		{
			continueButton.transform.parent.gameObject.SetActive(true);
			confirmButton.transform.parent.gameObject.SetActive(false);
		}
		else if ( boxType == PopupType.ConfirmCancel )
		{
			continueButton.transform.parent.gameObject.SetActive(false);
			confirmButton.transform.parent.gameObject.SetActive(true);
		}
	}


	public void Show(float autoHideDelay, bool hideOthers = true)
	{
		if (autoHideHandle != null && autoHideHandle.Running)
		{
			autoHideHandle.StopRoutine();
		}

		autoHideHandle = LugusCoroutines.use.StartRoutine( AutoHideRoutine(autoHideDelay) );

		Show ( hideOthers ); 
	}

	protected IEnumerator AutoHideRoutine(float autoHideDelay)
	{
		yield return new WaitForSeconds( autoHideDelay );

		Hide ();
	}

	public void Show(bool hideOthers = true)
	{
		if( hideOthers )
		{
			PopupManager.use.HideOthers(this);
		}
				
		this.gameObject.SetActive(true);

		inputBlockCollider.gameObject.SetActive(blockInput);

		available = false;

		UpdateText();

		ShowButtons();
	}

	public void Hide()
	{
		if( autoHideHandle != null && autoHideHandle.Running )
		{
			autoHideHandle.StopRoutine();
		}

		autoHideHandle = null;
		available = true;
  
		this.gameObject.SetActive(false);

		Reset ();
	}

	public void Reset()
	{
		inputBlockCollider.gameObject.SetActive(false);
        text = "";
	}

	public void SetupLocal()
	{
		// assign variables that have to do with this class only
		if( background == null )
		{
			background = this.transform.FindChild("Background");
		}
		if( background == null )
		{
			Debug.LogError( transform.Path () + " : No background found!" );
		}

		if( textWithIcon == null )
		{
			textWithIcon = this.transform.FindChild("TextWithoutIcon").GetComponent<TextMeshWrapper>();
		}
		if( textWithIcon == null )
		{
			Debug.LogError( transform.Path () + " : No textSmall found!" );
		}
		
		if( textWithoutIcon == null )
		{
			textWithoutIcon = this.transform.FindChild("TextWithIcon").GetComponent<TextMeshWrapper>();
		}
		if( textWithoutIcon == null )
		{
			Debug.LogError( transform.Path () + " : No textLarge found!" );
		}

		if( textNoButtons == null )
		{
			textNoButtons = this.transform.FindChild("TextFull").GetComponent<TextMeshWrapper>();
		}
		if( textNoButtons == null )
		{
			Debug.LogError( transform.Path () + " : No textFull found!" );
		}

		if( icon == null )
		{
			icon = this.transform.FindChild("Icon").GetComponent<SpriteRenderer>();
		}
		if( icon == null )
		{
			Debug.LogError( transform.Path () + " : No icon found!" );
		}
		
		if( continueButton == null )
		{
			continueButton = this.transform.FindChild("ContinueButtonContainer").GetComponentInChildren<Button>();
		}
		if( continueButton == null )
		{
			Debug.LogError( transform.Path () + " : No ContinueButton found!" );
		}

		if( confirmButton == null )
		{
			confirmButton = this.transform.FindChild("ConfirmButtonContainer/ButtonConfirm").GetComponent<Button>();
		}
		if( confirmButton == null )
		{
			Debug.LogError( transform.Path () + " : No confirm button found!" );
		}

		if( cancelButton == null )
		{
			cancelButton = this.transform.FindChild("ConfirmButtonContainer/ButtonCancel").GetComponent<Button>();
		}
		if( cancelButton == null )
		{
			Debug.LogError( transform.Path () + " : No cancel button found!" );
		}

		if (inputBlockCollider == null)
		{
			inputBlockCollider = this.transform.FindChild("InputBlock").GetComponent<BoxCollider>();
		}

		if (inputBlockCollider == null)
		{
			Debug.LogError( transform.Path () + " : No input blocker found!" );
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
	
	protected void Update () 
	{
		if( available ) // we're not currently in active use: no interaction allowed
			return;

		if (LugusInput.use.up)
		{
			// There's various other colliders before the popup background. Instead of a complex setup with layers, we'll just iterate over all the colliders the ray hit.
			RaycastHit[] hits = Physics.RaycastAll(LugusCamera.ui.ScreenPointToRay(LugusInput.use.lastPoint));

			foreach(RaycastHit hit in hits)
			{
				if (hit.collider.transform == background)
				{
					if (onBoxClicked != null)
						onBoxClicked(this);

					break;
                }
			}
		}

		if( continueButton.pressed )
		{
			if( onContinueButtonClicked != null )
				onContinueButtonClicked(this);
		}

		if (confirmButton.pressed)
		{
			if( onConfirmButtonClicked != null )
				onConfirmButtonClicked(this);
		}

		if (cancelButton.pressed)
		{
			if( onCancelButtonClicked != null )
				onCancelButtonClicked(this);
		}
	}
}
