using UnityEngine;
using System.Collections;

public abstract class IMenuStep : MonoBehaviour
{
	protected bool activated = false;
	
	public abstract void Activate(bool animate = true);
	public abstract void Deactivate(bool animate = true);

    public abstract void SetupLocal();

	public bool IsActive()
	{
		return activated;
	}

    public void ScaleElements()
    {
        foreach (Transform child in transform)
        {
            child.localPosition = MenuManager.use.CalculateUIPos(child.localPosition);
        }
    }
}

// Menu class template below
//public class NewMenuStep : IMenuStep 
//{
//	public void SetupLocal()
//	{
//	}
//	
//	public void SetupGlobal()
//	{
//	}
//	
//	protected void Awake()
//	{
//		SetupLocal();
//	}
//	
//	protected void Start () 
//	{
//		SetupGlobal();
//	}
//	
//	protected void Update () 
//	{
//	}
//	
//	public void Activate()
//	{
//		
//	}
//	
//	public void Deactivate()
//	{
//		
//	}
//}