using UnityEngine;
using System.Collections;

public class MonsterFactory
{
	public static GameLife createEnemy(string name, Vector3 posInWorld,Transform target){
		GameObject prefab = Resources.Load ("Prefabs/enemy/enemy_base") as GameObject;
		//TextAsset enemyStatus = Resources.Load ("json/enemy/"+name+"_data") as TextAsset;
		EnemyData enemyData = GameStaticData.getInstance().getEnemyInfo(name);

		GameObject o = GameObject.Instantiate (prefab,target);

		GameObject viewPrefab = Resources.Load ("Prefabs/enemy/"+name) as GameObject;


		o.GetComponentInChildren<SpriteRenderer> ().sprite = viewPrefab.GetComponent<SpriteRenderer>().sprite;
		o.GetComponentInChildren<SpriteRenderer> ().color = viewPrefab.GetComponent<SpriteRenderer> ().color;


		o.transform.position = posInWorld;
		GameLife gl = o.GetComponent<GameLife> ();
		gl.initEnemy (enemyData);
		BattleManager.getInstance ().enemies.Add (gl);
		return gl;
	}
}

