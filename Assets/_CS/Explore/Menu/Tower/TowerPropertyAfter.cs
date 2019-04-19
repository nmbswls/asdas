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

	public void setName(string tname){
		_tname.text = tname;
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
			skill.setInfo (skills[i]);
			skill.onClick.Set (delegate() {
			});
		}
	}




	public void setHit(int hit){
		_hit.text = hit + "";
	}

	public void setAtkRange(string range){
		_range.text = range;
	}
	public void setAtkSpd(string actualInterval, string atkSpd){
		_spd.text = actualInterval + "  (+"+atkSpd+")";
	}

}

