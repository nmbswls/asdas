using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySkillComponent : BaseSkillComponent
{

	public Dictionary<string,EnemySkill> staticSkillInfo;

	void Start ()
	{
		staticSkillInfo = GameStaticData.getInstance ().enemySkills;
	}

	public override void setSkillCD(int readySkill){
		skillCoolDown[readySkill] = staticSkillInfo[skills[readySkill].skillId].cooldown;
	}


	protected override bool isPermanentSkill(string skillId){
		EnemySkill ts = staticSkillInfo[skillId];
		if (ts.isPassive && ts.isPermanent) {
			return true;
		}
		return false;
	}
	protected override bool isPassiveSkill(string skillId){
		EnemySkill ts = staticSkillInfo[skillId];
		if (ts.isPassive) {
			return true;
		}
		return false;
	}
}

