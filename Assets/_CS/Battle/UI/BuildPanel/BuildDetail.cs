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

	public static string getAtkSpdTxt(int atkInterval){
		if (atkInterval < 500) {
			return "Very Fast";
		} else if (atkInterval < 1000) {
			return "Fast";
		} else if (atkInterval < 2000) {
			return "Middle";
		} else if (atkInterval < 2000) {
			return "Slow";
		} else {
			return "Very Slow";
		}
	}

	public static string getAtkRangeTxt(int atkRange){
		if (atkRange < 500) {
			return "Very Fast";
		} else if (atkRange < 1000) {
			return "Fast";
		} else if (atkRange < 2000) {
			return "Middle";
		} else if (atkRange < 2000) {
			return "Slow";
		} else {
			return "Very Slow";
		}
	}


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
		
		TowerBattleProperty property = TowerBattleProperty.genTowerBattleProperty (tt);

		TowerBase tb = tt.tbase;

		_name.text = tb.tname;

		_cost.text = tb.cost [0] + " " + tb.cost [1] + " " + tb.cost [2];

		if (tb.atkType == eAtkType.MELLE_AOE || tb.atkType == eAtkType.MELLE_POINT) {
			string s = "Melee";
			_range.text = s;
		} else {
			string s = getAtkRangeTxt (property.atkRange);
			_range.text = s;
		}

		int atkInteval = tb.towerModel.atkInterval;
		_range.text = getAtkSpdTxt (atkInteval);




		setHit(tb.mingzhong);
		//setAtkRange(tb.towerModel.atkRange+"");
		//setAtkSpd(tb.towerModel.atkInterval+"");

		setDamage (property.mainAtk,property.extraAtk);
		setSkill (property.originSkills);
	}


	public void setDamage(AtkInfo mainAtk, List<AtkInfo> extraAtk){
		_damage_list.RemoveChildrenToPool ();
		{
			TowerDamageItem damage = (TowerDamageItem)_damage_list.AddItemFromPool ();
			damage.setDamage (mainAtk.property,mainAtk.damage/1000);
		}
		for (int i = 0; i < extraAtk.Count; i++) {
			TowerDamageItem damage = (TowerDamageItem)_damage_list.AddItemFromPool ();
			damage.setDamage (extraAtk[i].property,extraAtk[i].damage/1000);
		}
	}
	public void setSkill(List<SkillState> skills){
		_skill_list.RemoveChildrenToPool ();

		for (int i = 0; i < skills.Count; i++) {
			TowerSkillItem skill = (TowerSkillItem)_skill_list.AddItemFromPool ();
			skill.touchable = false;
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

