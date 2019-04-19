using System;
using System.Collections.Generic;

public class TowerTemplate{
	public TowerBase tbase;
	public TowerComponent[] components = new TowerComponent[5];
}

public class TowerBattleProperty{

	public int atkRange;
	public int atkInterval;
	public eAtkType atkType;
	public List<AtkInfo> extraAtk;
	public AtkInfo mainAtk;
	public List<SkillState> originSkills;
	public List<SkillState> extraSkills;

	public static TowerBattleProperty genTowerBattleProperty(TowerTemplate tt){
		TowerBase tb = tt.tbase;
		List<TowerComponent> components = new List<TowerComponent>();
		foreach (TowerComponent tc in tt.components) {
			if (tc == null || tc.cid == null)
				continue;
			components.Add (tc);
		}



		int atkRange = tb.towerModel.atkRange;
		int atkSpeed = tb.atkSpd;


		List<AtkInfo> extraAtk = new List<AtkInfo> ();

		AtkInfo mainAtk = new AtkInfo(tb.mainAtk);
		for (int i = 0; i < tb.extraAtk.Count; i++) {
			extraAtk.Add (new AtkInfo(tb.extraAtk[i]));
		}

		List<SkillState> skills = new List<SkillState> ();
		skills.AddRange (tb.skills);

		List<SkillState> extraSkills = new List<SkillState> ();

		foreach (TowerComponent tc in tt.components) {
			foreach (TowerComponentEffect effect in tc.effects) {
				switch(effect.type){
				case eTowerComponentEffectType.ATK_CHANGE:
					mainAtk.damage += effect.x;
					break;
				case eTowerComponentEffectType.EXTRA_ABILITY:
					SkillState skill = new SkillState();
					skill.skillId = effect.extra;
					skill.skillLevel = effect.x;
//					TowerSkill ts = 
//					bool found = false;
//					for (int i = 0; i < skills.Count; i++) {
//						if (skills [i].skillId == skill.skillId) {
//							found = true;
//							skills [i].skillLevel += skill.skillLevel;
//							if(skills [i].skillLevel > )
//							break;
//						}
//					}
//					if (!found) {
//						extraSkills.Add (skill);
//					}
					break;
				case eTowerComponentEffectType.ATK_SPD_CHANGE:
					atkSpeed += effect.x;
					break;
				case eTowerComponentEffectType.ATK_RANGE_CHANGE:
					atkRange += effect.x;
					break;
				case eTowerComponentEffectType.EXTRA_ATK:
					if (mainAtk.property == (eProperty)effect.x) {
						mainAtk.damage += effect.y;
					} else {
						bool found = false;
						for (int i = 0; i < extraAtk.Count; i++) {
							if (extraAtk [i].property == (eProperty)effect.x) {
								found = true;
								extraAtk [i].damage += effect.y;
								break;
							}
						}
						if (!found) {
							extraAtk.Add (new AtkInfo(effect.x,effect.y));
						}
					}
					break;
				default:
					break;
				}

			}
		}
		TowerBattleProperty res = new TowerBattleProperty ();

		return res;
	}
}

