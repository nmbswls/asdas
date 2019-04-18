using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;




public class TowerSkillComponent : BaseSkillComponent
{

	Tower owner;
	public void Init(Tower owner){
		this.owner = owner;
	}

	Dictionary<string, int> localTimers = new Dictionary<string, int> ();

	protected override void tickLogic(int timeInt){
		List<string> keys = new List<string> ();
		foreach (var k in localTimers.Keys) {
			keys.Add (k);
		}

		foreach (string k in keys) {
			localTimers [k] += timeInt;
		}

	}

	public override void setSkillCD(int readySkill){
		SkillState s = skills [readySkill];
		TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo (s.skillId);
		skillCoolDown[readySkill] = ts.cooldown[s.skillLevel-1];
	}

	protected override void calcuAuraSkill(SkillState skillState){
		TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo (skillState.skillId);
		int level = skillState.skillLevel;

		int range = ts.x[level-1];
		if (level - 1 < ts.y.Count) {
		
		}
		int y = ts.y [level-1];
		int z = ts.z [level-1];
		if (ts.debuff) {
			List<GameLife> enemyCopy = BattleManager.getInstance ().getTmpEnemyList ();
			Vector2 a = new Vector2 (owner.posXInt, owner.posYInt);
			foreach (GameLife e in enemyCopy) {

				Vector2 b = new Vector2 (e.posXInt, e.posYInt);
				if ((a - b).magnitude < range) {
					switch (ts.skillId) {
						case "1003":
							EffectManager.inst.EmitFollowingEffect ("burning",300,e);
							e.DoDamage (y);
							break;

						default:
							break;
					}

				}
			}
		} else {
			if(!localTimers.ContainsKey(ts.skillId)){
				localTimers [ts.skillId] = 0;
			}
			int timer = localTimers[ts.skillId];

			switch (ts.skillId) {
				case "1009":
					if (timer > 5000) {
						localTimers[ts.skillId] = 0;
						BattleManager.getInstance ().player.gl.DoDamage (-5000);
					}
					break;

				default:
					break;
			}
		}

	}

	protected override bool isPermanentSkill(string skillId){
		TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo(skillId);
		if (ts.tsType == eTowerSkillType.AURA) {
			return true;
		}
		return false;
	}
	protected override bool isPassiveSkill(string skillId){
		TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo(skillId);
		if (ts.tsType == eTowerSkillType.PASSIVE) {
			return true;
		}
		return false;
	}
	protected override bool isActiveSkill(string skillId){
		TowerSkill ts = GameStaticData.getInstance ().getTowerSkillInfo(skillId);
		if (ts.tsType == eTowerSkillType.ACTIVE) {
			return true;
		}
		return false;
	}
}

