using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class PlayerSkill{
	public string name;
	public string effect;
	public bool isPassive;

}
public class PlayerSkillManager : MonoBehaviour
{

	public List<PlayerSkill> pcSkills = new List<PlayerSkill>();
	public List<int> pcSkillCd = new List<int>();
	public PlayerModule player;
	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < 3; i++) {
			pcSkills.Add (new PlayerSkill());
			pcSkillCd.Add (0);
		}
		player = BattleManager.getInstance ().player;
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void Tick (int timeInt)
	{
		for (int i = 0; i < pcSkillCd.Count; i++) {
			if(pcSkillCd [i]>0)pcSkillCd [i] -= timeInt;
		}
	}

	public void useSkill(int idx){
		if (pcSkills [idx].isPassive)
			return;
		if (pcSkillCd [idx] > 0) {
			return;
		}
		pcSkillCd [idx] = 5000;

		if (idx == 0) {
			player.setTrap ();
		} else if (idx == 1) {
			player.useShield ();
		} else if (idx == 2) {
			player.useBlink ();
		}
	}

}

