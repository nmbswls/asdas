﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLife : MapObject
{
	[HideInInspector]
	public IUnitController pCtrl;

	[HideInInspector]
	public EnemyBuffComponent buffManager;

	[HideInInspector]
	public TowerSkillComponent skillComponent;

	[HideInInspector]
	public BuffEffectComponent effectManager;

	[HideInInspector]
	public GameObject emoji;

//	public CharPhysicsController charPhysicsCtrl;

	//固有属性
	[HideInInspector]
	public int id;
	[HideInInspector]
	public Animator anim;
	[HideInInspector]
	public SpriteRenderer image;
	[HideInInspector]
	public PhysicsComponent pc;

	public bool isPlayer = true;

	//可变属性
	public float maxHP = 1000f;
	public float hp = 1000f;
	public float mp;
	public int def = 5000;
	public int evade = 5;

	public eProperty property;

	public int Def {
		get {
			int defReduce = buffManager.getDefChange ();
			return this.def - defReduce;
		}
	}

	public int Atk {
		get {
			return this.atk;
		}
	}
	public int atk;

	public void initEnemy(EnemyData ed){
		this.maxHP = ed.hp;
		this.hp = ed.hp;
		this.atk = ed.atk;
		this.def = ed.def;
	}

	Vector3Int lastPosInCell;
	public List<PathSeekBehaviour> followers = new List<PathSeekBehaviour>();
	void OnGridChange(){
		foreach (PathSeekBehaviour follower in followers) {
			follower.reSearchPath ();
		}
	}
	//状态相关

	public bool isMoving = false;
	public bool isAttcking = false;

	//-1 normal attack
	//0 - 3 special attack
	//-2 damaged
	int actingMode = -1;


	public bool IsAlive
	{
		get { return hp > 0; }
	}

	bool isPushingBack = false;



	void Awake(){
		pCtrl = GetComponent<IUnitController> ();
		anim = GetComponentInChildren<Animator> ();
		//rBody = GetComponent<Rigidbody2D>();


		pc = GetComponent<PhysicsComponent> ();
		if (pc == null) {
			pc = gameObject.AddComponent<PhysicsComponent>();
		}

		image = GetComponentInChildren<SpriteRenderer> ();



		buffManager = GetComponent<EnemyBuffComponent> ();
		if (buffManager == null) {
			buffManager = gameObject.AddComponent<EnemyBuffComponent>();
		}


		effectManager = GetComponent<BuffEffectComponent> ();
		if (effectManager == null) {
			effectManager = gameObject.AddComponent<BuffEffectComponent>();
		}

	}




	public Vector2 faceDirVector
	{
		get { 
			if (anim == null) {
				return Vector2.zero;
			}
			Vector2 dir = new Vector2(anim.GetFloat ("inputX"),anim.GetFloat ("inputY")).normalized;
			//return new Vector2(dirVec[faceDir][0],dirVec[faceDir][1]); 
			return dir;
		}
	}


	public void changeDir(int x, int y){
		if (x < 0) {
			faceDir = 1;
		} else if (x > 0) {
			faceDir = 3;
		} else if (y < 0) {
			faceDir = 2;
		} else {
			faceDir = 0;
		}
	}

	public int faceDir = 2;
	public int[][] dirVec = new int[][]{new int[]{0,1}, new int[]{-1,0}, new int[]{0,-1}, new int[]{1,0}};



	private bool lockPanel;
	public Vector2 vReal;

	public float WalkSpeed {
		get {
			float rate = buffManager.getSpeedRate ();
			if (isPlayer) {
				rate *= GetComponent<PlayerModule> ().godModeTime > 0 ? 1.5f : 1f;
			}
			return this.m_originWalkSpeed * rate;
		}
	}


	public Vector2 thrustVec;
	public int force;
	//public int knockLast;

	[SerializeField]
	private float m_originWalkSpeed = 3f;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		isMoving = false;
		isAttcking = false;

		//characterSizeY = image.bounds.size.y;
		//characterSizeX = image.bounds.size.x;

		faceDir = 2;
		lastPosInCell = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
	}




	public bool canMove(){
		return !(isAttcking || isPushingBack);
	}

	// Update is called once per frame
	protected override void Update ()
	{			
		base.Update ();

		if (buffManager!=null) {
			buffManager.Tick ((int)(Time.deltaTime * 1000f));
		}

		if (!pCtrl) {
			return;
		}
		anim.SetFloat("speed",pCtrl.Dmag);

		if (canMove() && pCtrl.Dmag > 0.1f) {
			anim.SetFloat ("inputX",pCtrl.Dvec.x);
			anim.SetFloat ("inputY",pCtrl.Dvec.y);
		}
		if (isPlayer) {
			
			if (!lockPanel) {
				vReal = /*pCtrl.Dmag*/1 * pCtrl.Dvec * WalkSpeed;
			}
		}

//		if (knockLast > 0) {
//			knockLast -= (int)Mathf.Round (Time.deltaTime * 1000f);
//			if (knockLast <= 0) {
//				thrustVec = Vector3.zero;
//				anim.SetLayerWeight (anim.GetLayerIndex ("DamagedLayer"), 0);
//			}
//		}


		Vector3Int nowPos = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
		if (isPlayer && nowPos != lastPosInCell) {
			OnGridChange ();
		}
		lastPosInCell = nowPos;
	}

	void FixedUpdate(){
		//rBody.position += deltaPos;
		//rBody.position += new Vector3(thrustVec.x * Time.fixedDeltaTime,thrustVec.y * ,0);
		if (isPlayer) {
			pc.doMove = true;
			pc.moveBy = vReal * Time.fixedDeltaTime;
		}

//		if (knockLast > 0) {
//			rBody.velocity = new Vector3 (thrustVec.x, thrustVec.y, 0);
//		} else {
//			rBody.velocity = new Vector3 (vReal.x, vReal.y, 0);
//			if (!isPlayer) {
//				//rBody.velocity += pCtrl.offsetV;
//			}
//		}

		//thrustVec = Vector3.zero;
		//deltaPos = Vector3.zero;
	}
		

//	public void knock(Vector3 dir, float last, float force){
//		if (isPlayer && knockLast > 0) {
//			return;
//		}
//		this.thrustVec = /*VInt*/dir.normalized * force;
//		this.knockLast = (int)Mathf.Round (last * 1000);
//		anim.SetLayerWeight (anim.GetLayerIndex ("DamagedLayer"), 1);
//	}

	//上 右 下 左

	public Vector3[] atkPointOffset = new Vector3[]{new Vector3(0,0.1f,0),new Vector3(-0.1f,0,0),new Vector3(0,-0.2f,0),new Vector3(0.1f,0,0)};

	public delegate void Callback();
	public event Callback OnDieCallback;
	public void OnDie(){
		//Destroy (gameObject);
		gameObject.SetActive(false);
		if (OnDieCallback != null) {
			OnDieCallback ();
		}
	}

	public void DoDamage(int damage){
		hp -= damage;
		EmitManager.inst.Emit(transform, 0, (int)(damage*0.001f),isPlayer);
		if (hp <= 0) {
			OnDie ();
		}
		BattleManager.getInstance().placeHPOnTopLayer (this);
		EffectManager.inst.EmitNormalEffectOnFixedPos ("damaged01",new Vector2Int(posXInt,posYInt),500);
	}

	public void DoDamage(List<AtkInfo> atk,int mingzhong,eProperty type,List<Buff> attachedEffect){
		int chance = mingzhong - evade;

		int damage = 0;
		for (int i = 0; i < atk.Count; i++) {
			damage += atk [i].damage - def > 0 ? atk [i].damage - def : 1000; 
		}

		if (type == property) {
			damage /= 2;
		}
		for (int i = 0; i < attachedEffect.Count; i++) {
			//Debug.Log ("shang buff "+attachedEffect[i].buffId);
			buffManager.addBuff (attachedEffect[i]);
		}
		hp -= damage;
		EmitManager.inst.Emit(transform, 0, (int)(damage*0.001f), UnityEngine.Random.Range(0, 10) == 5);
		if (hp <= 0) {
			OnDie ();
		}
		BattleManager.getInstance().placeHPOnTopLayer (this);

	}



	public void showEmoji(int type){
		transform.Find ("emoji").gameObject.SetActive(true);
		StartCoroutine (hideEmoji());
	}

	IEnumerator hideEmoji(){
		yield return new WaitForSeconds (1f);
		transform.Find ("emoji").gameObject.SetActive(false);
	}
}



