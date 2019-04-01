using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class BuildDetail : GComponent
{
	GTextField _name;
	GTextField _cost;

	GTextField _hit;
	GTextField _range;
	GTextField _spd;


	GList _damage_list;
	GList _skill_list;

	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);

		_name = this.GetChild ("name").asTextField;
		_cost = this.GetChild ("cost").asTextField;

		_hit = this.GetChild ("hit").asTextField;
		_range = this.GetChild ("range").asTextField;
		_spd = this.GetChild ("spd").asTextField;

		_damage_list = this.GetChild ("damage").asList;
		_skill_list = this.GetChild ("skills").asList;



	}

	public void updateView(TowerTemplate tt){
		TowerBase tb = tt.tbase;


		int atkInteval = tb.atkInteval;
		int atkRange = tb.atkRange;

		List<AtkInfo> atks = new List<AtkInfo> ();

		AtkInfo mainAtk = new AtkInfo(tb.mainAtk);

		for (int i = 0; i < tb.extraAtk.Count; i++) {
			atks.Add (new AtkInfo(tb.extraAtk[i]));
		}

		List<SkillState> skills = new List<SkillState> ();
		skills.AddRange (tb.skills);

		List<SkillState> extraSkills = new List<SkillState> ();


		foreach (TowerComponent tc in tt.components) {
			if (tc == null)
				continue;
			foreach (TowerComponentEffect effect in tc.effects) {
				if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
					mainAtk.damage += effect.x;
				}else if(effect.type == eTowerComponentEffectType.EXTRA_ABILITY){
					//extras.Add (effect.extra);
					SkillState skill = new SkillState();
					skill.skillId = effect.extra;
					skill.skillLevel = effect.x;
					extraSkills.Add (skill);

				}else if(effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE){

				}else if(effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE){

				}else if(effect.type == eTowerComponentEffectType.EXTRA_ATK){

					if (mainAtk.property == (eProperty)effect.x) {
						mainAtk.damage += effect.y;
					} else {
						bool found = false;
						for (int i = 0; i < atks.Count; i++) {
							if (atks [i].property == (eProperty)effect.x) {
								found = true;
								atks [i].damage += effect.y;
								break;
							}
						}
						if (!found) {
							atks.Add (new AtkInfo(effect.x,effect.y));
						}
					}
				}
			}
		}
		atks.Insert (0,mainAtk);
		skills.AddRange (extraSkills);

		setHit(tb.mingzhong);
		setAtkRange(tb.atkRange+"");
		setAtkSpd(tb.atkInteval+"");

		setDamage (atks);
		setSkill (skills);
	}


	public void setDamage(List<AtkInfo> atk){
		_damage_list.RemoveChildrenToPool ();
		for (int i = 0; i < atk.Count; i++) {
			TowerDamageItem damage = (TowerDamageItem)_damage_list.AddItemFromPool ();
		}
	}
	public void setSkill(List<SkillState> skills){
		_skill_list.RemoveChildrenToPool ();

		for (int i = 0; i < skills.Count+1; i++) {
			TowerSkillItem skill = (TowerSkillItem)_skill_list.AddItemFromPool ();
			skill.onTouchEnd.Set (delegate() {
				Debug.Log("ss");
			});
		}
	}
	public void setHit(int hit){
		_hit.text = hit + "";
	}

	public void setAtkRange(string range){
		_range.text = range;
	}
	public void setAtkSpd(string atkSpd){
		_spd.text = atkSpd;
	}




}

