using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerFactory
{
	

	public static Tower createTower(int idx, Vector2Int posInCell,Transform target){
		TowerTemplate tt = PlayerData.getInstance ().getTowerTemplate (idx);
		GameObject prefab = Resources.Load ("Prefabs/tower_base") as GameObject;

		//GameObject viewPrefab = Resources.Load ("Prefabs/towers/"+tt.tbase.tid) as GameObject;
		AnimatorOverrideController animCtrl =  Resources.Load ("OverrideAnimCtrl/towers/"+tt.tbase.tid) as AnimatorOverrideController;

		GameObject o = GameObject.Instantiate (prefab,target);

		Tower t = o.GetComponent<Tower> ();
		t.GetComponentInChildren<Animator>().runtimeAnimatorController = animCtrl;

		t.init (tt,posInCell);
		BattleManager.getInstance ().addTower(t);
		return t;
	}


}

