using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager
{

	public static int MAX_BULLET_IDLE_NUMBER = 50;
	static EffectManager _instance;

	public GameObject effectPrefab;

	public GameObject spawnTowerEffectPrefab;
	public GameObject atkHintEffectPrefab;
	public GameObject atkCircleEffectPrefab;

	public static EffectManager inst
	{
		get
		{
			if (_instance == null)
				_instance = new EffectManager();
			return _instance;
		}
	}



	private readonly Stack<GameObject> _componentPool = new Stack<GameObject>();

	public EffectManager()
	{
		effectPrefab = Resources.Load ("Prefabs/effect/effect01") as GameObject;
		spawnTowerEffectPrefab = Resources.Load ("Prefabs/effect/effect_tower_spawn") as GameObject;
		atkHintEffectPrefab = Resources.Load ("Prefabs/effect/atk_hint") as GameObject;
		atkCircleEffectPrefab = Resources.Load ("Prefabs/effect/atk_hint_circle") as GameObject;
	}

	public void Emit(Transform pos)
	{
		GameObject b;
		if (_componentPool.Count > 0)
			b = _componentPool.Pop ();
		else
			b = GameObject.Instantiate (effectPrefab,pos.parent);
		ItemGottenEffect effect = b.GetComponent<ItemGottenEffect> ();
		effect.init (pos.position);
	}


	public void EmitSpawnTowerEffect(Vector3Int toSpawnPosInCell, Transform player,float time){
		
		GameObject effect = GameObject.Instantiate (spawnTowerEffectPrefab,player.parent);
		SpawnTowerEffect e = effect.GetComponent<SpawnTowerEffect> ();
		e.init (toSpawnPosInCell,player.position,time);
	}

	public void EmitAtkSectorEffect(Transform pos, Vector3 dir,int time = 300){

		GameObject effect = GameObject.Instantiate (atkHintEffectPrefab,pos.parent);
		AtkSectorEffect e = effect.GetComponent<AtkSectorEffect> ();
		e.init (pos.position,dir,time);
	}

	public void EmitAtkCircleEffect(Transform pos, float range, int time = 3000){
		GameObject effect = GameObject.Instantiate (atkCircleEffectPrefab,pos.parent);
		AtkCircleEffect e = effect.GetComponent<AtkCircleEffect> ();
		e.init (pos.position,range,time);
	}

	public void ReturnComponent(GameObject com)
	{
		if (_componentPool.Count < MAX_BULLET_IDLE_NUMBER) {
			_componentPool.Push (com);
		}
	}
}

