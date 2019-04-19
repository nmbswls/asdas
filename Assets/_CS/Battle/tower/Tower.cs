using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DamageData{
	public int atk;
	public eProperty property;
	public float crit;
}



public enum eAtkStage
{
	CANT = -1,
	READY = 0,
	PRE_YAO = 1,
	POST_YAO = 2,
}

public class Tower : MapObject
{

	public bool initialized = false;

	[SerializeField]
	public eAtkType atkType = eAtkType.MELLE_POINT;
	public eProperty property =eProperty.NONE; 
	public AtkInfo mainAtk = new AtkInfo();
	public List<AtkInfo> extraAtk = new List<AtkInfo>();
	public int mingzhong = 2;
	//list

	public int atkInterval = 2000;
	public int atkRange = 3000;
	public int atkPreTime = 300;
	public int atkPostTime = 700;

	//public TowerModel towerModel;

	public int castPreTime = 800;
	public int castPostTime = 700;



	public static int FIND_TARGET_INTERVAL = 250;
	public int findTargetTimer;

	public static int CHECK_SKILL_INTERVAL = 250;
	public int checkSkillTimer;

	public Vector2Int posInCell = Vector2Int.zero;

	private Animator anim;

	private TowerTemplate tt;

	TowerSkillComponent skillComponent;
	public TowerBuffComponent buffManager;


	[SerializeField]
	private GameLife hateTarget = null;


	private GameLife castTarget = null;




	void Awake(){
		anim = GetComponentInChildren<Animator> ();

		skillComponent = GetComponent<TowerSkillComponent> ();
		if (skillComponent == null) {
			skillComponent = gameObject.AddComponent<TowerSkillComponent> ();
		}
		skillComponent.Init(this);

		buffManager = GetComponent<TowerBuffComponent> ();
		if (buffManager == null) {
			buffManager = gameObject.AddComponent<TowerBuffComponent> ();
		}
		buffManager.Init(this);
	}


	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}

	public void init(TowerTemplate towerTemplate, Vector2Int posInCell){
		if (towerTemplate != null) {
			this.tt = towerTemplate;
			this.atkType = towerTemplate.tbase.atkType;
			this.mainAtk = towerTemplate.tbase.mainAtk;
			this.extraAtk = towerTemplate.tbase.extraAtk;
			this.atkInterval = towerTemplate.tbase.towerModel.atkInterval;
			this.atkRange = towerTemplate.tbase.towerModel.atkRange;
			this.atkPreTime = towerTemplate.tbase.towerModel.atkPreTime;
			this.mingzhong = towerTemplate.tbase.mingzhong;
			//this.tt = towerTemplate;

			for (int i = 0; i < towerTemplate.tbase.skills.Count; i++) {
				skillComponent.skills [i] = new SkillState (towerTemplate.tbase.skills[i].skillId,towerTemplate.tbase.skills[i].skillLevel);
				skillComponent.skillCoolDown [i] = 3000;
			}

		}
		this.posInCell = posInCell;
		Vector2Int posLt = MapManager.getInstance ().CellToWorld (posInCell.y,posInCell.x);
		//posLt += new Vector2Int (MapManager.TILE_WIDTH * 5,-MapManager.TILE_HEIGHT * 5);
		posXInt = posLt.x;
		posYInt = posLt.y;
		atkCoolDown = 1000;
		atkBhvTimer = 0;

		initialized = true;
	}

	void genBullet(string bulletStyle, GameLife target, bool isHoming, bool isBallistic){
		int originHeight = 0;
		if (isBallistic) {
			originHeight = 5000;
		}
		BulletManager.inst.EmitBullet (bulletStyle, this,target,isHoming, originHeight);
	}


	//private GameLife atkTarget = null;


	private eAtkStage castStage = eAtkStage.READY;

	private int castBhvTimer = 0;

	int castIdx = -1;

	void checkCastBehaviour(int dTime){
		
//		if (atkTarget != null && atkBhvTimer < atkPreTime) {
//			return;
//		}

		//攻击前摇时 不进行施法
		if (atkStage == eAtkStage.PRE_YAO) {
			return;
		}

		//效果结算
		if (castStage == eAtkStage.READY) {
			return;
		}

		castBhvTimer += dTime;

		bool isSkillEffect = false;

		if (castStage==eAtkStage.PRE_YAO && castBhvTimer > castPreTime) {
			castStage = eAtkStage.POST_YAO;
			isSkillEffect = true;
		} else if (castStage==eAtkStage.POST_YAO && castBhvTimer > castPreTime + castPostTime) {
			castStage = eAtkStage.READY;
			castBhvTimer = 0;
		}

		if (isSkillEffect) {
			SkillState skill = skillComponent.skills [castIdx];
			TowerSkill s = GameStaticData.getInstance ().getTowerSkillInfo (skill.skillId);
			if (s.tsType == eTowerSkillType.SELF_TARGET) {
				gainBuff ();
			} else if (s.tsType == eTowerSkillType.ACTIVE) {
				if (s.targetType == eAtkType.RANGED_HOMING) {
					genBullet (s.bulletStyle.bulletName,castTarget,true,false);
					castTarget = null;
				} else if (s.targetType == eAtkType.RANGED_INSTANT) {
					applyNormalAtk (castTarget,s.bulletStyle.bulletName);
					castTarget = null;
				}

			}else {
				
			}
		}





	}

	void checkAutoSkill(int dTime){
		
		if (checkSkillTimer > 0) {
			checkSkillTimer -= dTime;
		}
		if (checkSkillTimer > 0)
			return;

		if (castStage != eAtkStage.READY) {
			return;
		}
		checkSkillTimer = CHECK_SKILL_INTERVAL;

		List<int> readySkills = skillComponent.getReadySkill();

		GameLife closestOne = MapManager.getInstance().getClosestEnemy (this);

		List<int> readyUsableSkill = new List<int> ();

		if (readySkills.Count == 0) {
			return;
		}
		if (closestOne == null || !closestOne.IsAlive) {
			return;
		}
			
		for (int i = 0; i < readySkills.Count; i++) {
			SkillState skill = skillComponent.skills [readySkills [i]];

			int diss = (posXInt - closestOne.posXInt) * (posXInt - closestOne.posXInt) + (posYInt - closestOne.posYInt)*(posYInt - closestOne.posYInt);
			int dis = (int)Mathf.Sqrt (diss);


			//Vector2 diff = closestOne.transform.position - transform.position;

			TowerSkill s = GameStaticData.getInstance ().getTowerSkillInfo (skill.skillId);

			if (s.tsType == eTowerSkillType.SELF_TARGET) {
				readyUsableSkill.Add (readySkills[i]);
			} else {
				if (dis <= s.x[skill.skillLevel-1]) {
					readyUsableSkill.Insert (0,readySkills[i]);
				}
			}
		}

		if (readyUsableSkill.Count == 0) {
			return;
		}

		SkillState toUse = skillComponent.skills[readyUsableSkill [0]];
		TowerSkill ss = GameStaticData.getInstance ().getTowerSkillInfo (toUse.skillId);

		if (ss.tsType == eTowerSkillType.SELF_TARGET) {
			anim.SetTrigger ("buff");
		} else if (ss.tsType == eTowerSkillType.ACTIVE) {
			
			castTarget = closestOne;
			anim.SetTrigger ("skill");
			anim.SetFloat ("skill_style",Random.Range (0,2)*1.0f);

		}else {
			
		}

		skillComponent.setSkillCD(readyUsableSkill [0]);
		castBhvTimer = 0;
		castStage = eAtkStage.PRE_YAO;
		castIdx = readyUsableSkill [0];
	}

	void checkAtkTarget(int dTime){
		if (findTargetTimer > 0) {
			findTargetTimer -= dTime;
		}

		if (findTargetTimer > 0) {
			return;
		}

		if (hateTarget != null) {
			if (!hateTarget.IsAlive) {
				hateTarget = null;
			} else {
				int diss = (posXInt - hateTarget.posXInt) * (posXInt - hateTarget.posXInt) + (posYInt - hateTarget.posYInt)*(posYInt - hateTarget.posYInt);
				int dis = (int)Mathf.Sqrt (diss);
				if(dis > atkRange){
					hateTarget = null;
				}
			}
		}
		if (hateTarget == null) {
			hateTarget = MapManager.getInstance().getClosestEnemy (this);
			if (hateTarget!=null&&!hateTarget.IsAlive) {
				hateTarget = null;
			}
			if (hateTarget != null ) {
				int diss = (posXInt - hateTarget.posXInt) * (posXInt - hateTarget.posXInt) + (posYInt - hateTarget.posYInt)*(posYInt - hateTarget.posYInt);
				int dis = (int)Mathf.Sqrt (diss);

				if (dis > atkRange) {
					hateTarget = null;
				}
			}
			findTargetTimer = FIND_TARGET_INTERVAL;
		}

	}

	void checkBuff(int dTime){
		if (buffManager == null) {
			return;
		}
		buffManager.Tick (dTime);

	}

	void checkSkill(int dTime){
		if (skillComponent == null) {
			return;
		}

		skillComponent.Tick(dTime);
		checkCastBehaviour (dTime);


		checkAutoSkill (dTime);
	}


	private GameLife atkTarget = null;
	private int atkCoolDown = 0;
	private eAtkStage atkStage = eAtkStage.READY;
	private int atkBhvTimer = 0;


	void applyMeleeAtk(GameLife atkTarget){
		applyNormalAtk (atkTarget);
	}

	void applyHomingRangeAtk(GameLife atkTarget){
		string bulletStyle = tt.tbase.bulletStyle.bulletName;

		genBullet (bulletStyle,atkTarget,true,false);
	}

	void applyUnhomingRangeAtk(GameLife atkTarget){
		string bulletStyle = tt.tbase.bulletStyle.bulletName;

		genBullet (bulletStyle,atkTarget,false,false);
	}

	void applayMeleeAOE(GameLife atkTarget){
		
		Vector2 faceDir = (atkTarget.transform.position - transform.position);
		EffectManager.inst.EmitAtkSectorEffect (transform, faceDir);

		float cx = transform.position.x;
		float cy = transform.position.y;
		List<GameLife> validTarget = new List<GameLife> ();
		foreach (GameLife enemy in BattleManager.getInstance().getTmpEnemyList()) {
			float x = enemy.transform.position.x;
			float y = enemy.transform.position.y;

			Vector2 dir = new Vector2 (x - cx, y - cy);
			if (dir.magnitude * 1000 < atkRange && Vector2.Dot (faceDir.normalized, dir.normalized) > Mathf.Cos (60f * Mathf.Deg2Rad)) {
				validTarget.Add (enemy);
				//enemy.DoDamage (damage,property);
			}

		}
		foreach (GameLife target in validTarget) {
			applyNormalAtk (target);
		}
	}

	void checkBeforeAtk(int dTime){
		if (atkType == eAtkType.NONE) {
			return;
		}
		//处理冷却时间
		if (atkCoolDown > 0) {
			atkCoolDown -= dTime;
		}
		if(atkCoolDown>0){
			return;
		}

		//正在动作时无法攻击
		if (atkStage != eAtkStage.READY || castStage != eAtkStage.READY) {
			return;
		}

		if (hateTarget != null) {
			atkCoolDown = atkInterval;
			anim.SetTrigger ("atk");
			atkBhvTimer = 0;
			atkTarget = hateTarget;
			atkStage = eAtkStage.PRE_YAO;
		}
	}

	void checkAtkBehaviour(int dTime){
		
		//如果当前未攻击 不进行判定
		if (atkStage == eAtkStage.READY) {
			return;
		}

		//累加攻击计时器 并改变攻击状态
		atkBhvTimer += dTime;

		bool isAttackEffect = false;
		if (atkStage==eAtkStage.PRE_YAO && atkBhvTimer > atkPreTime) {
			atkStage = eAtkStage.POST_YAO;
			isAttackEffect = true;
		} else if (atkStage==eAtkStage.POST_YAO && atkBhvTimer > atkPreTime + atkPostTime) {
			atkStage = eAtkStage.READY;
			atkBhvTimer = 0;
		}
			
		//当攻击丢失时 重置攻击状态
		if (atkTarget == null || !atkTarget.IsAlive) {
			isAttackEffect = false;
			atkStage = eAtkStage.READY;
			atkBhvTimer = 0;
		}
		//当还未进行攻击时 返回
		if (!isAttackEffect) {
			return;
		}

		switch(atkType){
			case eAtkType.NONE  :
				return;
			case eAtkType.MELLE_POINT  :
				applyNormalAtk (atkTarget);
				break; 
			case eAtkType.RANGED_HOMING:
				applyHomingRangeAtk (atkTarget);
				break;
			case eAtkType.MELLE_AOE:
				applayMeleeAOE (atkTarget);
				break;
			case eAtkType.RANGED_INSTANT:
				applyNormalAtk (atkTarget);
				break;
			case eAtkType.RANGED_MULTI:
				break;
			case eAtkType.RANGED_UNHOMING:
				applyUnhomingRangeAtk (atkTarget);
				break;
			default : /* 可选的 */
				break; 
		}

		atkTarget = null;
	}


	protected override void Update ()
	{
		base.Update ();


		int dTime = (int)(Time.deltaTime*1000);
		checkAtkTarget (dTime);
		checkBuff (dTime);
		checkSkill (dTime);
		checkAtkBehaviour (dTime);
		checkBeforeAtk (dTime);
	}



	public void gainBuff(){
		//buffManager.addBuff (new Buff(1,50,5000));
		buffManager.addBuff (new Buff(1,50,2000));

	}
//	public bool checkOverlap(){
//		Collider2D[] cols = Physics2D.OverlapCircleAll (transform.position,r,1<<LayerMask.NameToLayer("enemy"));
//		if (cols.Length == 0) {
//			Debug.Log ("可建造");
//		}
//		return false;
//	}


	public void applyNormalAtk(GameLife hit,string damageEffect = "damaged01"){
		//hit.knock (atkTarget.transform.position - this.transform.position, 0.2f, 6f);

		List<Buff> attackEffect = new List<Buff> ();
		foreach (SkillState skill in skillComponent.skills) {
			if (skill == null || skill.skillId == null)
				continue;
			TowerSkill sinfo = GameStaticData.getInstance().getTowerSkillInfo(skill.skillId);
			if (sinfo.tsType == eTowerSkillType.PASSIVE && sinfo.checkPoint == ePassiveCheckPoint.ATK) {
				attackEffect.Add (new Buff (sinfo.x[skill.skillLevel-1], 50, 1000));
			}
		}
		List<AtkInfo> atk = new List<AtkInfo> ();
		atk.Add (mainAtk);
		atk.AddRange (extraAtk);
		hit.DoDamage (atk,mingzhong,property,attackEffect);

		EffectManager.inst.EmitFollowingEffect (damageEffect,500,hit);
	}

	public void applyMagicAtk(GameLife hit, SkillState skill, string damageEffect = "damaged01"){


		TowerSkill sinfo = GameStaticData.getInstance().getTowerSkillInfo(skill.skillId);

		int lv = skill.skillLevel;
		int damage = 0;
		switch (skill.skillId) {
			case "1004":
				damage = sinfo.x[lv-1];
				break;
			case "1005":
				damage = sinfo.x[lv-1];
				break;
			case "1006":
				damage = sinfo.x[lv-1];
				break;
		default:
			break;

		}


		hit.DoDamage (damage);

		EffectManager.inst.EmitFollowingEffect (damageEffect,500,hit);
	}
}

