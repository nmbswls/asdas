using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	
	private static T instance;

	public static T getInstance(){
		return instance;
	}


	public static bool instanceExists
	{
		get { return instance != null; }
	}


	protected virtual void Awake()
	{
		if (instanceExists)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = (T) this;
		}
	}


	protected virtual void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}
}

