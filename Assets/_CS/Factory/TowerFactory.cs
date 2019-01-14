using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerFactory
{

	public static Tower createTower(string name, Vector3Int posInCell,Transform target){
		GameObject prefab = Resources.Load ("Prefabs/"+name) as GameObject;
		TextAsset towerAsset = Resources.Load ("json/"+name+"_data") as TextAsset;
		AnimatorOverrideController animCtrl =  Resources.Load ("OverrideAnimCtrl/"+name) as AnimatorOverrideController;
		TowerData towerData = JsonUtility.FromJson<TowerData> (towerAsset.text);

		GameObject o = GameObject.Instantiate (prefab,target);
		Tower t = o.GetComponent<Tower> ();
		t.GetComponentInChildren<Animator>().runtimeAnimatorController = animCtrl;
		t.init (towerData,posInCell);
		GameManager.getInstance ().addTower(t);
		return t;
	}


}

