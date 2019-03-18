using UnityEngine;
using System.Collections;

public class MonsterFactory
{
	public static GameLife createEnemy(string name, Vector3 posInWorld,Transform target){
		GameObject prefab = Resources.Load ("Prefabs/enemy/"+name) as GameObject;
		TextAsset enemyStatus = Resources.Load ("json/enemy/"+name+"_data") as TextAsset;
		EnemyData enemyData = JsonUtility.FromJson<EnemyData> (enemyStatus.text);

		GameObject o = GameObject.Instantiate (prefab,target);
		o.transform.position = posInWorld;
		GameLife gl = o.GetComponent<GameLife> ();
		gl.initEnemy (enemyData);
		BattleManager.getInstance ().enemies.Add (gl);
		return gl;
	}
}

