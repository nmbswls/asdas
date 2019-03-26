using UnityEngine;
using System.Collections;
using FairyGUI;

public class TowerSkillItem : GComponent
{


	TowerSkillState skill;

	GLoader _icon;
	GTextField _desp;


	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		_icon = this.GetChild ("icon").asLoader;
		_desp = this.GetChild ("txt").asTextField;
		this.onTouchBegin.Add (delegate() {
			Debug.Log("click");
			GameManager.getInstance().showDetailAmplifier("tower_skill",skill);
		});
	}

	public void setInfo(TowerSkillState skill){
		this.skill = skill;
		_desp.text = "ssss";
		_icon.url = "image/TowerSkill/"+skill.skillId;
	}
}

