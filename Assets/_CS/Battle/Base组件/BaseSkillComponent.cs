using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseSkillComponent : MonoBehaviour
{

	public static int MAX_SKILL = 4;

	[HideInInspector]
	public int[] skillCoolDown = new int[MAX_SKILL];
	[HideInInspector]
	public SkillState[] skills= new SkillState[MAX_SKILL];

	public static int CHECK_PERMANENT_INTERVAL = 500;
	public int lastInteval = 0;

	public void Tick (int timeInt)
	{
		checkCoolDown (timeInt);
		checkLastingSkill (timeInt);

		tickLogic (timeInt);
	}

	public bool hasSkill(string sid){
		for (int i = 0; i < skills.Length; i++) {
			if (skills [i] == null || skills [i].skillId == null) {
				continue;
			}
			if (skills [i].skillId == sid) {
				return true;
			}
		}
		return false;
	}

	protected virtual void tickLogic(int timeInt){
	
	}
	public void checkCoolDown(int timeInt){

		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null || skills [i].skillId==null)
				continue;
			if (skillCoolDown [i] > 0)
				skillCoolDown [i] -= timeInt;	
		}
	}
	protected virtual void calcuAuraSkill(SkillState skillState){
	
	}

	public void checkLastingSkill(int timeInt){
		if (lastInteval > 0)
			lastInteval -= timeInt;
		if (lastInteval > 0) {
			return;
		}
		lastInteval = CHECK_PERMANENT_INTERVAL;
		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null || skills [i].skillId==null)
				continue;
			if (!isPermanentSkill (skills [i].skillId)) {
				continue;
			}
			calcuAuraSkill (skills[i]);
		}
	}

	public List<int> getReadySkill(){
		List<int> possibleSkills = new List<int> ();
		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null || skills [i].skillId==null)
				continue;
			if(isActiveSkill(skills [i].skillId) && skillCoolDown[i] <= 0) {
				possibleSkills.Add (i);
			}
		}
		return possibleSkills;
	}

	protected virtual bool isPermanentSkill(string skillId){
		return false;
	}
	protected virtual bool isPassiveSkill(string skillId){
		return false;
	}
	protected virtual bool isActiveSkill(string skillId){
		return false;
	}

	public virtual void setSkillCD(int readySkill){
		skillCoolDown [readySkill] = 5000;
	}
}

