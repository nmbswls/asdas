using System;
using System.Collections.Generic;

public class TowerTemplate{
	public TowerBase tbase;
	public TowerComponent[] components = new TowerComponent[5];
}

public class TowerBattleProperty{

	public int atkRange;
	public int atkSpd;
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

		//List<SkillState> extraSkills = new List<SkillState> ();

		foreach (TowerComponent tc in tt.components) {
			foreach (TowerComponentEffect effect in tc.effects) {
				switch(effect.type){
				case eTowerComponentEffectType.ATK_CHANGE:
					mainAtk.damage += effect.x;
					break;
				case eTowerComponentEffectType.EXTRA_ABILITY:
//					
					string skillId = effect.extra;
					int skillLevel = effect.x;
					TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo (skillId);

					{
						bool found = false;
						for (int i = 0; i < skills.Count; i++) {
							if (skills [i].skillId == skillId) {
								found = true;
								skills [i].skillLevel += skillLevel;
								if (skills [i].skillLevel > ts.maxLv) {
									skills [i].skillLevel = ts.maxLv;
								}
								break;
							}
						}
						if (!found) {
							SkillState skill = new SkillState ();
							skill.skillId = skillId;
							skill.skillLevel = skillLevel;
							skills.Add (skill);
						}
					}
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
		res.atkRange = atkRange;
		res.atkSpd = atkSpeed;
		res.mainAtk = mainAtk;
		res.extraAtk = extraAtk;
		res.originSkills = skills;
		return res;
	}
}

