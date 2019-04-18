using UnityEngine;
using System.Collections;
using FairyGUI;

public class TowerSkillItem : GComponent
{


	SkillState skill;

	GLoader _icon;
	GTextField _desp;
	GTextField _lv_num;


	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		_icon = this.GetChild ("icon").asLoader;
		_desp = this.GetChild ("txt").asTextField;
		_lv_num = this.GetChild ("lv").asTextField;
		this.onClick.Add (delegate() {
			GameManager.getInstance().showDetailAmplifier("tower_skill",skill);
		});
	}

	public void setInfo(SkillState skill){
		this.skill = skill;
		TowerSkill sinfo = GameStaticData.getInstance ().getTowerSkillInfo (skill.skillId);
		_desp.text = sinfo.skillDesp;
		_lv_num.text = skill.skillLevel + "";
		_icon.url = "image/TowerSkill/"+skill.skillId;
	}
}

