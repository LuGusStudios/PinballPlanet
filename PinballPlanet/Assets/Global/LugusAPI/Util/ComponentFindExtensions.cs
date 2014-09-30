using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ComponentFindExtensions 
{
	public static T FindComponent<T>(this GameObject go) where T : Component
	{
		T retrieved = go.GetComponent<T>();

		if (retrieved == null)
		{
			Debug.LogError("FindComponent: GameObject " + go.name + " did not have a component of type: " + typeof(T).ToString() + ".", go);
		}

		return retrieved;
	}

	// Same as FindComponent, but receives the containing object by reference, so the lookup will be avoided if it's already cached.
	// Returns a boolean indicating success, which can be used for custom actions when the component is not found.
	public static bool CacheComponent<T>(this GameObject go, ref T cache) where T : Component
	{
		if (cache != null)
			return true;

		cache = FindComponent<T>(go);

		return cache != null;
	}

	public static T[] FindComponents<T>(this GameObject go) where T : Component
	{
		T[] retrieved = go.GetComponents<T>();
	
		if (retrieved == null || retrieved.Length <= 0)
		{
			Debug.LogError("FindComponents: GameObject " + go.name + " did not have any components of type: " + typeof(T).ToString() + ".", go);
		}
	
		return retrieved;
	}

	public static T FindComponentInChildren<T>(this GameObject go, bool includeInactive = false, string childName = "") where T : Component
	{
		T[] retrievedArray = go.GetComponentsInChildren<T>(includeInactive);
		T retrieved = null;

		if (retrievedArray == null || retrievedArray.Length <= 0)
		{
			Debug.LogError("FindComponentInChildren: GameObject " + go.name + " or its children did not have a component of type " + typeof(T).ToString() + ".", go);
			return retrieved;
		}

		if (string.IsNullOrEmpty(childName))
		{
			retrieved = retrievedArray[0];
		}
		else
		{
			foreach(T t in retrievedArray)
			{
				if (t.gameObject.name == childName)
				{
					retrieved = t;
					break;
				}
			}

			if (retrieved == null)
			{
				Debug.LogError("FindComponentInChildren: GameObject " + go.name + " or its children did not have a component of type: " + typeof(T).ToString() + " on a child object named " + childName + ".", go);
			}
		}

		return retrieved;
	}

	public static T[] FindComponentsInChildren<T>(this GameObject go, bool includeInactive = false, string childName = "") where T : Component
	{
		T[] retrievedArray = go.GetComponentsInChildren<T>(includeInactive);
		
		if (retrievedArray == null || retrievedArray.Length <= 0)
		{
			Debug.LogError("FindComponentsInChildren: GameObject " + go.name + " or its children did not have any components of type " + typeof(T).ToString() + ".", go);
		}

		if (!string.IsNullOrEmpty(childName))
		{
			List<T> filteredList = new List<T>();

			foreach(T t in retrievedArray)
			{
				if (t.gameObject.name == childName)
				{
					filteredList.Add(t);
				}
			}

			retrievedArray = filteredList.ToArray();

			if (retrievedArray == null || retrievedArray.Length <= 0)
			{
				Debug.LogError("FindComponentsInChildren: GameObject " + go.name + " or its children did not have any components of type " + typeof(T).ToString() + " on a child object named " + childName + ".", go);
			}
		}
		
		return retrievedArray;
	}

	public static T FindComponentInParent<T>(this GameObject go, bool includeInactive = false, string parentName = "") where T : Component
	{
		T[] retrievedArray = go.GetComponentsInParent<T>(includeInactive);
		T retrieved = null;
		
		if (retrievedArray == null || retrievedArray.Length <= 0)
		{
			Debug.LogError("FindComponentInParent: GameObject " + go.name + " or its parent objects did not have a component of type " + typeof(T).ToString() + ".", go);
			return retrieved;
		}
		
		if (string.IsNullOrEmpty(parentName))
		{
			retrieved = retrievedArray[0];
		}
		else
		{
			foreach(T t in retrievedArray)
			{
				if (t.gameObject.name == parentName)
				{
					retrieved = t;
					break;
				}
			}
			
			if (retrieved == null)
			{
				Debug.LogError("FindComponentInParent: GameObject " + go.name + " or its parent objects did not have a component of type: " + typeof(T).ToString() + " on a parent named " + parentName + ".", go);
			}
		}
		
		return retrieved;
	}

	public static T[] FindComponentsInParent<T>(this GameObject go, bool includeInactive = false, string parentName = "") where T : Component
	{
		T[] retrievedArray = go.GetComponentsInParent<T>(includeInactive);
		
		if (retrievedArray == null || retrievedArray.Length <= 0)
		{
			Debug.LogError("FindComponentsInParent: GameObject " + go.name + " or its parent objects did not have any components of type " + typeof(T).ToString() + ".", go);
		}
		
		if (!string.IsNullOrEmpty(parentName))
		{
			List<T> filteredList = new List<T>();
			
			foreach(T t in retrievedArray)
			{
				if (t.gameObject.name == parentName)
				{
					filteredList.Add(t);
				}
			}
			
			retrievedArray = filteredList.ToArray();
			
			if (retrievedArray == null || retrievedArray.Length <= 0)
			{
				Debug.LogError("FindComponentsInParent: GameObject " + go.name + " or its parent objects did not have any components of type " + typeof(T).ToString() + " on a parent named " + parentName + ".", go);
			}
		}
		
		return retrievedArray;
	}
}