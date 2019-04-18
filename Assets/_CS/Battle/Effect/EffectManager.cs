using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager
{


	public static int MAX_EFFECT_IDLE_NUMBER = 10;
	static EffectManager _instance;

	public GameObject effectBasePrefab;
	public GameObject effectPrefab;

	public GameObject spawnTowerEffectPrefab;
	public GameObject atkHintEffectPrefab;
	public GameObject atkCircleEffectPrefab;

	public GameObject damagedEffectPrefab;

	public static EffectManager inst
	{
		get
		{
			if (_instance == null)
				_instance = new EffectManager();
			return _instance;
		}
	}

	Transform effectLayer;

	private readonly Stack<GameObject> _componentPool = new Stack<GameObject>();


	public Dictionary<string,AnimatorOverrideController> effectDic = new Dictionary<string,AnimatorOverrideController>();


	public EffectManager()
	{

		effectBasePrefab = Resources.Load ("Prefabs/effect_base") as GameObject;

		effectPrefab = Resources.Load ("Prefabs/effect/effect01") as GameObject;
		spawnTowerEffectPrefab = Resources.Load ("Prefabs/effect/effect_tower_spawn") as GameObject;
		atkHintEffectPrefab = Resources.Load ("Prefabs/effect/atk_hint") as GameObject;
		atkCircleEffectPrefab = Resources.Load ("Prefabs/effect/atk_hint_circle") as GameObject;
		damagedEffectPrefab = Resources.Load ("Prefabs/effect/damage_effect") as GameObject;

		effectLayer = GameObject.Find ("EffectLayer").transform;

		AnimatorOverrideController anim = Resources.Load ("OverrideAnimCtrl/effect/default") as AnimatorOverrideController;
		effectDic ["default"] = anim;

	}

	public AnimatorOverrideController getEffect(string effectName){
		if (!effectDic.ContainsKey (effectName)) {
			AnimatorOverrideController anim = Resources.Load ("OverrideAnimCtrl/effect/"+effectName) as AnimatorOverrideController;
			if (anim != null) {
				effectDic [effectName] = anim;
			} else {
				effectDic [effectName] = effectDic ["default"];
			}
		}

		return effectDic [effectName];
	}

	public void Emit(Transform pos)
	{
		GameObject b;
		//if (_componentPool.Count > 0)
		//	b = _componentPool.Pop ();
		//else
			b = GameObject.Instantiate (effectPrefab,pos.parent);
		ItemGottenEffect effect = b.GetComponent<ItemGottenEffect> ();
		effect.init (pos.position);
	}


	public void EmitSpawnTowerEffect(int towerIdx,Vector2Int toSpawnPosInCell, GameLife player,float time){
		
		GameObject effect = GameObject.Instantiate (spawnTowerEffectPrefab,player.transform.parent);
		SpawnTowerEffect e = effect.GetComponent<SpawnTowerEffect> ();
		e.init (towerIdx,toSpawnPosInCell,new Vector2Int(player.posXInt,player.posYInt),time);
	}

	public void EmitAtkSectorEffect(Transform pos, Vector3 dir,int time = 300){

		GameObject effect = GameObject.Instantiate (atkHintEffectPrefab,pos.parent);
		AtkSectorEffect e = effect.GetComponent<AtkSectorEffect> ();
		e.init (pos.position,dir,time);
	}

	public void EmitAtkCircleEffect(Transform pos, float range, int time = 300){
		GameObject effect = GameObject.Instantiate (atkCircleEffectPrefab,pos.parent);
		AtkCircleEffect e = effect.GetComponent<AtkCircleEffect> ();
		e.init (pos.position,range,time);
	}

//	public void EmitDamageEffect(Transform target, int time = 300){
//		GameObject effect = GameObject.Instantiate (damagedEffectPrefab,effectLayer);
//		DamagedEffect e = effect.GetComponent<DamagedEffect> ();
//		e.init (target,time);
//	}

	public void EmitNormalEffectOnFixedPos(string effectName, Vector2Int targetPos, int lasting){

		GameObject effect;
		if (_componentPool.Count > 0)
			effect = _componentPool.Pop ();
		else
			effect = GameObject.Instantiate (effectBasePrefab,effectLayer);

		AnimatorOverrideController animator = getEffect (effectName);
		BaseEffect be = effect.GetComponent<BaseEffect> ();
		effect.GetComponentInChildren<SpriteRenderer> ().sprite = null;
		effect.GetComponentInChildren<Animator>().runtimeAnimatorController = animator;
		be.init (lasting,targetPos);
	}

	public void EmitFollowingEffect(string effectName, int lasting, MapObject owner){
		GameObject effect;
		if (_componentPool.Count > 0)
			effect = _componentPool.Pop ();
		else
			effect = GameObject.Instantiate (effectBasePrefab,effectLayer);

		AnimatorOverrideController animator = getEffect (effectName);
		BaseEffect be = effect.GetComponent<BaseEffect> ();
		effect.GetComponentInChildren<SpriteRenderer> ().sprite = null;


		effect.GetComponentInChildren<Animator>().runtimeAnimatorController = animator;

		be.init ((int)(animator["effect_default"].length*1000),owner);
	}

	public void ReturnComponent(GameObject com)
	{
		
		if (_componentPool.Count < MAX_EFFECT_IDLE_NUMBER) {
			_componentPool.Push (com);
		} else {
			GameObject.Destroy (com);
		}
	}
}

