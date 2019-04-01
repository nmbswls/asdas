using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;


public enum ePassiveCheckPoint{
	ATK = 0,
	DANAGED = 1,
}


public class TowerSkillComponent : BaseSkillComponent
{
	public Dictionary<string,TowerSkill> staticSkillInfo;

	void Start ()
	{
		staticSkillInfo = GameStaticData.getInstance ().towerSkills;
	}

	public override void setSkillCD(int readySkill){
		skillCoolDown[readySkill] = staticSkillInfo[skills[readySkill].skillId].cooldown;

	}


	protected override bool isPermanentSkill(string skillId){
		TowerSkill ts = staticSkillInfo[skillId];
		if (ts.isPassive && ts.isPermanent) {
			return true;
		}
		return false;
	}
	protected override bool isPassiveSkill(string skillId){
		TowerSkill ts = staticSkillInfo[skillId];
		if (ts.isPassive) {
			return true;
		}
		return false;
	}
}

