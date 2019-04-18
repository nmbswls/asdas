using UnityEngine;
using System.Collections;

public class EnemyBuffComponent : BaseBuffComponent
{

	public float getSpeedRate(){
		float rate = 1f;
		foreach (Buff buff in buffs) {
			if (buff.buffId == 1) {
				int level = buff.buffLevel;
				rate *= (1-level / 100f);
			}
		}
		return rate;
	}

	public int getAtkChange(){
		int change = 0;
		foreach (Buff buff in buffs) {
			if (buff.buffId == 10) {
				int level = buff.buffLevel;
				change += level;
			}else if (buff.buffId == 11) {
				int level = buff.buffLevel;
				change -= level;
			}

		}
		return change;
	}

	public int getDefChange(){
		int change = 0;
		foreach (Buff buff in buffs) {
			if (buff.buffId == 20) {
				int level = buff.buffLevel;
				change += level;
			}else if (buff.buffId == 21) {
				int level = buff.buffLevel;
				change -= level;
			}

		}
		return change;
	}
}

