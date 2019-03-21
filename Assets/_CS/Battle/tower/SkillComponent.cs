using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;


public enum ePassiveCheckPoint{
	ATK = 0,
	DANAGED = 1,
}


public class SkillComponent : MonoBehaviour
{
	public static int MAX_SKILL = 4;
	public int[] skillCoolDown = new int[MAX_SKILL];
	public TowerSkillState[] skills= new TowerSkillState[MAX_SKILL];
	public Dictionary<int,TowerSkill> staticSkillInfo;
	// Use this for initialization
	void Start ()
	{
		staticSkillInfo = GameStaticData.getInstance ().towerSkills;
	}
	public static int CHECK_PERMANENT_INTERVAL = 500;
	public int lastInteval = 0;
	// Update is called once per frame
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
			TowerSkill ts = staticSkillInfo[skills [i].skillId];
			if (ts.isPassive&&ts.isPermanent) {
				int range = 3000;
				foreach (GameLife e in BattleManager.getInstance ().enemies) {
					Vector2 a = transform.position;
					Vector2 b = e.transform.position;
					if ((a - b).magnitude * 1000 < range) {
						e.showFollowingEffect (1);
					}
				}
			}
		}
	}

	public List<int> getReadySkill(){
		List<int> possibleSkills = new List<int> ();
		for (int i = 0; i < MAX_SKILL; i++) {
			if (skills [i] == null)
				continue;
			TowerSkill ts = staticSkillInfo[skills [i].skillId];
			if (!ts.isPassive&&skillCoolDown[i] <= 0) {
				possibleSkills.Add (i);
			}
		}
		return possibleSkills;
//		if (possibleSkills.Count == 0) {
//
//			return -1;
//		}
//		for (int i = 0; i < possibleSkills.Count; i++) {
//			
//		}
	}

	public void setSkillCD(int readySkill){
		
		skillCoolDown[readySkill] = staticSkillInfo[skills[readySkill].skillId].cooldown;

	}
}

