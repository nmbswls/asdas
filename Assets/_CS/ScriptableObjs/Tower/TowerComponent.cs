using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TowerComponentEffect{
	public eTowerComponentEffectType type;
	public string extra;
	public int x;
	public int y;
	public int z;
}

[System.Serializable]
public enum eTowerComponentEffectType{
	ATK_CHANGE = 0,
	ATK_RANGE_CHANGE = 1,
	EXTRA_ATK = 2,
	ATK_SPD_CHANGE = 3,
	CRIT_CHANGE = 4,
	EXTRA_ABILITY = 5,
}

public class TowerComponentInList{
	public string cid;
	public int slot = -1;
}


[CreateAssetMenu(fileName="New Tower Component",menuName="cs526/tower/tower component")]
[System.Serializable]
public class TowerComponent:ScriptableObject{

	[SerializeField]
	public string cid = "0";

	[SerializeField]
	public string cname = "default";

	[SerializeField]
	public string cdesp = "default";

	[SerializeField]
	public List<TowerComponentEffect> effects = new List<TowerComponentEffect>();

	public string getEffects(){
		string s = "";
		foreach(TowerComponentEffect effect in effects){
			if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
				s += "atk " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE) {
				s += "range " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE) {
				s += "atk spd " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.EXTRA_ATK) {
				s += effect.extra + " atk " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.EXTRA_ABILITY) {
				s += effect.extra;
			}
			s += "\n";
		}
		return s;
	}
}

