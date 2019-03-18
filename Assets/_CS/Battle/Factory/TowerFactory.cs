using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerFactory
{

	public static Tower createTower(int idx, Vector3Int posInCell,Transform target){
		TowerTemplate tt = PlayerData.getInstance ().getTowerTemplate (idx);
		GameObject prefab = Resources.Load ("Prefabs/towers/tower") as GameObject;
		GameObject viewPrefab = Resources.Load ("Prefabs/towers/"+tt.tbase.tid) as GameObject;
		//GameObject prefab = Resources.Load ("Prefabs/towers/"+tt.tbase.tid) as GameObject;
		AnimatorOverrideController animCtrl =  Resources.Load ("OverrideAnimCtrl/"+tt.tbase.tid) as AnimatorOverrideController;

		GameObject o = GameObject.Instantiate (prefab,target);
		o.GetComponentInChildren<SpriteRenderer> ().sprite = viewPrefab.GetComponent<SpriteRenderer>().sprite;
		o.GetComponentInChildren<SpriteRenderer> ().color = viewPrefab.GetComponent<SpriteRenderer> ().color;
		//GameObject view = GameObject.Instantiate (viewPrefab,o.transform);
		Tower t = o.GetComponent<Tower> ();
		t.GetComponentInChildren<Animator>().runtimeAnimatorController = animCtrl;
		t.init (tt,posInCell);
		BattleManager.getInstance ().addTower(t);
		return t;
	}


}

