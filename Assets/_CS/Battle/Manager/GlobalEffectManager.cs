using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalEffectManager
{
	Dictionary<string,List<Tower>> globalEffects = new Dictionary<string,List<Tower>>();
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


	public void removeEffect(Tower source, string effectId){
		if (globalEffects.ContainsKey (effectId)) {
			globalEffects [effectId].Remove (source);
			if (globalEffects [effectId].Count == 0) {
				globalEffects.Remove (effectId);
			}
		}
	}

	public void addEffect(Tower source, string effectId){
		if (!globalEffects.ContainsKey (effectId)) {
			globalEffects.Add (effectId, new List<Tower> ());
		}
		if (!globalEffects [effectId].Contains (source)) {
			globalEffects [effectId].Add (source);
		}
	}

	public List<string> getAllGlobalEffect(){
		List<string> res = new List<string> ();

		foreach (string eid in globalEffects.Keys) {
			res.Add (eid);
		}
		return res;
	}


}

