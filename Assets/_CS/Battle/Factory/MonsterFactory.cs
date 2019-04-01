using UnityEngine;
using System.Collections;

public class MonsterFactory
{
	public static GameLife createEnemy(string name, Vector2Int posInWorld,Transform target){
		GameObject prefab = Resources.Load ("Prefabs/enemy/enemy_base") as GameObject;
		//TextAsset enemyStatus = Resources.Load ("json/enemy/"+name+"_data") as TextAsset;
		EnemyData enemyData = GameStaticData.getInstance().getEnemyInfo(name);

		GameObject o = GameObject.Instantiate (prefab,target);

		GameObject viewPrefab = Resources.Load ("Prefabs/enemy/"+name) as GameObject;
		if (viewPrefab == null) {
			viewPrefab = Resources.Load ("Prefabs/enemy/default") as GameObject;
		}
		o.GetComponentInChildren<SpriteRenderer> ().sprite = viewPrefab.GetComponent<SpriteRenderer>().sprite;
		o.GetComponentInChildren<SpriteRenderer> ().color = viewPrefab.GetComponent<SpriteRenderer> ().color;

		GameLife gl = o.GetComponent<GameLife> ();
		gl.initEnemy (enemyData);
		gl.posXInt = posInWorld.x;
		gl.posYInt = posInWorld.y;
		BattleManager.getInstance ().enemies.Add (gl);
		return gl;
	}
}

