using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterDropManager : MonoBehaviour
{
	public GameObject dropPrefab;
	public Transform mapPanel;
	List<MonsterDrop> monsterDroppings = new List<MonsterDrop>();
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	public void Tick (int timeInt)
	{
		MapObject player = BattleManager.getInstance ().player.gl;
		foreach (MonsterDrop drop in monsterDroppings) {
			if (drop.triggered||drop.isMovingOrigin)
				continue;
			Vector2 p1 = drop.transform.position;
			Vector2 p2 = player.transform.position;
			if ((p1 - p2).magnitude < 2f) {
				drop.trigger ();
			}
		}
	}

	public void createDrops(List<int> drops,Vector3 pos){
		foreach (int dropNum in drops) {
			GameObject o = GameObject.Instantiate (dropPrefab,mapPanel);
			o.transform.position = pos;
			MonsterDrop drop = o.GetComponent<MonsterDrop> ();
			drop.dropManager = this;
			monsterDroppings.Add (drop);
			drop.moveToRandomNearby ();
		}

	}

//	public void addDrop(List<MonsterDrop> newDrops){
//		monsterDroppings.AddRange(newDrops);
//	}

	public void removeDrop(MonsterDrop toRemove){
		monsterDroppings.Remove (toRemove);
	}
}

