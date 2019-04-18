using UnityEngine;
using System.Collections;

public class TowerBuffComponent : BaseBuffComponent
{

	Tower owner;
	public GameObject buffShow;

	public void Init(Tower tower){
		owner = tower;
		withUI = false;
		buffShow = transform.Find ("BuffEffect").gameObject;
	}

	protected override void afterTick(){

		if (buffShow != null) {
			if (buffs.Count > 0) {
				buffShow.SetActive (true);
			} else {
				buffShow.SetActive (false);
			}
		}
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
}

