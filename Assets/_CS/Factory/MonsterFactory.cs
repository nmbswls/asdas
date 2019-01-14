using UnityEngine;
using System.Collections;

public class MonsterFactory
{
	public static GameLife createEnemy(string name, Vector3 posInWorld,Transform target){
		GameObject prefab = Resources.Load ("Prefabs/enemy01") as GameObject;
		GameObject o = GameObject.Instantiate (prefab,target);
		o.transform.position = posInWorld;
		GameLife gl = o.GetComponent<GameLife> ();
		GameManager.getInstance ().enemy.Add (gl);
		return gl;
	}
}

