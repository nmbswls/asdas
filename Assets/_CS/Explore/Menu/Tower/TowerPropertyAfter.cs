using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class TowerPropertyAfter : GComponent
{
	GTextField _tname;
	GTextField _hit;
	GTextField _range;
	GTextField _spd;


	GList _damage_list;
	GList _skill_list;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		_hit = this.GetChild ("hit").asTextField;
		_range = this.GetChild ("range").asTextField;
		_spd = this.GetChild ("spd").asTextField;

		_tname = this.GetChild ("tname").asTextField;

		_damage_list = this.GetChild ("damage").asList;
		_skill_list = this.GetChild ("skills").asList;

		this.onTouchBegin.Add (delegate() {
			//GameManager.getInstance().showDetailAmplifier("减少生命上限");
		});


	}

	public void setDamage(List<AtkInfo> atk){
		_damage_list.RemoveChildrenToPool ();
		for (int i = 0; i < atk.Count; i++) {
			TowerDamageItem damage = (TowerDamageItem)_damage_list.AddItemFromPool ();
		}
	}
	public void setSkill(List<TowerSkillState> skills){
		_skill_list.RemoveChildrenToPool ();

		for (int i = 0; i < skills.Count; i++) {
			TowerSkillItem skill = (TowerSkillItem)_skill_list.AddItemFromPool ();
			skill.setInfo (skills[i]);
			skill.onClick.Set (delegate() {
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

