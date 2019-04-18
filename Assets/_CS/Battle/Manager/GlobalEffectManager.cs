using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalEffectManager
{
	Dictionary<string,List<GameObject>> globalEffects = new Dictionary<string,List<GameObject>>();
	static GlobalEffectManager _instance;

	public static GlobalEffectManager inst
	{
		get
		{
			if (_instance == null)
				_instance = new GlobalEffectManager();
			return _instance;
		}
	}


	public void removeEffect(GameObject source, string effectId){
		
	}


}

