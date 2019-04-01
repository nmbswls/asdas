using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseSkillComponent : MonoBehaviour
{

	public static int MAX_SKILL = 4;
	public int[] skillCoolDown = new int[MAX_SKILL];
	public SkillState[] skills= new SkillState[MAX_SKILL];

	public static int CHECK_PERMANENT_INTERVAL = 500;
	public int lastInteval = 0;

	public void Tick (int timeInt)
	{
		checkCoolDown (timeInt);
		checkLastingSkill (timeInt);
	}

	public void checkCoolDown(int timeInt){

		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null)
				continue;
			if (skillCoolDown [i] > 0)
				skillCoolDown [i] -= timeInt;	
		}
	}

	public void checkLastingSkill(int timeInt){
		if (lastInteval > 0)
			lastInteval -= timeInt;
		if (lastInteval > 0) {
			return;
		}
		lastInteval = CHECK_PERMANENT_INTERVAL;
		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null)
				continue;
			if (!isPermanentSkill (skills [i].skillId)) {
				continue;
			}
			int range = 3000;
			List<GameLife> enemyCopy = new List<GameLife> (BattleManager.getInstance ().enemies);
			foreach (GameLife e in enemyCopy) {
				Vector2 a = transform.position;
				Vector2 b = e.transform.position;
				if ((a - b).magnitude * 1000 < range) {
					e.showFollowingEffect (1);
					e.DoDamage (1000);
				}
			}

		}
	}

	public List<int> getReadySkill(){
		List<int> possibleSkills = new List<int> ();
		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null)
				continue;
			if(!isPassiveSkill(skills [i].skillId)&&skillCoolDown[i] <= 0) {
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

	public virtual void setSkillCD(int readySkill){
		skillCoolDown [readySkill] = 5000;
	}
}

