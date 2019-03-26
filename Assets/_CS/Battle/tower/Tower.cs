using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DamageData{
	public int atk;
	public eProperty property;
	public float crit;
}

public enum eProperty{
	NONE=-1,
	FIRE = 0,
	WATER = 1,
	WIND = 2,
	ICE = 3,
	LIGHT = 4,
	DARK = 5,
	VOID = 6,
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
	public int atkPreanimTime = 300;

	public int castPreTime = 800;
	public int castTimeDuration = 1500;
	public bool skillHasTriggered = false;

	public static int findTargetInteval = 250;
	public int findTargetTimer;

	public static int checkSkillInteval = 250;
	public int checkSkillTimer;

	public Vector3Int posInCell = Vector3Int.zero;


	private Animator anim;

	private TowerTemplate tt;

	SkillComponent skillComponent;
	public BuffComponent buffManager;
	public GameObject buffShow;

	[SerializeField]
	private GameLife hateTarget = null;
	private GameLife atkTarget = null;

	private GameLife castTarget = null;

	public GameObject bulletPrefab;
	[SerializeField]
	private int coolDown = 0;
	[SerializeField]
	private int atkTimer = 0;


	void Awake(){
		anim = GetComponentInChildren<Animator> ();
		skillComponent = GetComponent<SkillComponent> ();
		if (skillComponent == null) {
			skillComponent = gameObject.AddComponent<SkillComponent> ();
		}

		buffManager = GetComponent<BuffComponent> ();
	}


	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}

	public void init(TowerTemplate towerTemplate, Vector3Int posInCell){
		if (towerTemplate != null) {
			this.tt = towerTemplate;
			this.atkType = towerTemplate.tbase.atkType;
			this.mainAtk = towerTemplate.tbase.mainAtk;
			this.extraAtk = towerTemplate.tbase.extraAtk;
			this.atkInterval = towerTemplate.tbase.atkInteval;
			this.atkRange = towerTemplate.tbase.atkRange;
			this.atkPreanimTime = towerTemplate.tbase.atkPreanimTime;
			this.mingzhong = towerTemplate.tbase.mingzhong;
			//this.tt = towerTemplate;

			for (int i = 0; i < towerTemplate.tbase.skills.Count; i++) {
				skillComponent.skills [i] = new TowerSkillState (towerTemplate.tbase.skills[i].skillId,towerTemplate.tbase.skills[i].skillLevel);
				skillComponent.skillCoolDown [i] = 3000;
			}

		}
		this.posInCell = posInCell;
		Vector3 pos = MapManager.getInstance ().obcTilemap.CellToWorld (posInCell);
		pos.x += MapManager.TILE_WIDTH * 0.5f * 0.01f;
		pos.y += MapManager.TILE_HEIGHT * 0.5f * 0.01f;
		pos.z = 0;
		transform.position = pos;

		coolDown = 1000;
		atkTimer = 0;

		initialized = true;
	}

	void genBullet(string style, bool isHoming, GameObject target,int height = 5000){

		BulletManager.inst.Emit (style,gameObject,target,isHoming,height);

//		GameObject o = GameObject.Instantiate (bulletPrefab,transform.parent);
//		o.transform.position = transform.position;
//		o.GetComponent<Bullet> ().target = atkTarget.gameObject;
	}

	int castTime = 0;
	int castIdx = -1;

	void checkSkill(int dTime){
		
		if (atkTarget != null && atkTimer < atkPreanimTime) {
			return;
		}
		if (castIdx != -1) {
			castTime += dTime;
			if (castTime > castPreTime&&!skillHasTriggered) {
				Debug.Log ("use skill "+castIdx);

				TowerSkillState skill = skillComponent.skills [castIdx];
				TowerSkill s = GameStaticData.getInstance ().towerSkills [skill.skillId];
				if (s.isSelfTarget) {
					gainBuff ();
				} else {
					genBullet (s.bulletStyle, true,castTarget.gameObject,0);
					castTarget = null;
				}
					
				skillHasTriggered = true;
			}
			if (castTime > castTimeDuration) {
				castIdx = -1;
				castTime = 0;
				skillHasTriggered = false;
			}
			return;
		}

		if (checkSkillTimer > 0)
			return;
		
		List<int> readySkills = skillComponent.getReadySkill();


		GameLife closestOne = MapManager.getInstance().getClosestEnemy (this);

		List<int> readyUsableSkill = new List<int> ();

		if (readySkills.Count > 0) {
			for (int i = 0; i < readySkills.Count; i++) {
				TowerSkillState skill = skillComponent.skills [readySkills [i]];
				Vector2 diff = closestOne.transform.position - transform.position;
				TowerSkill s = GameStaticData.getInstance ().towerSkills [skill.skillId];

				if (s.isSelfTarget) {
					readyUsableSkill.Add (readySkills[i]);
				} else {
					if (diff.magnitude * 1000f <= s.x[skill.skillLevel]) {
						readyUsableSkill.Insert (0,readySkills[i]);
					}
				}
			}
		}

		if (readyUsableSkill.Count > 0) {
			TowerSkillState toUse = skillComponent.skills[readyUsableSkill [0]];


			TowerSkill ss = GameStaticData.getInstance ().towerSkills [toUse.skillId];
			if (ss.isSelfTarget) {
				Debug.Log ("自家buff");
				anim.SetTrigger ("buff");
			} else {
				
				Debug.Log ("攻击");
				castTarget = closestOne;
				anim.SetTrigger ("skill");
				Random.Range (0,2);
				anim.SetFloat ("skill_style",Random.Range (0,2)*1.0f);
			}

			skillComponent.setSkillCD(readyUsableSkill [0]);
			castTime = 0;
			castIdx = readyUsableSkill [0];
			skillHasTriggered = false;
		}
		checkSkillTimer = checkSkillInteval;
	}


	protected override void Update ()
	{
		base.Update ();


		int dTime = (int)(Time.deltaTime*1000);

		if (buffManager!=null) {
			buffManager.Tick ((int)(Time.deltaTime * 1000f));
			if (buffShow != null) {
				if (buffManager.buffs.Count > 0) {
					buffShow.SetActive (true);
				} else {
					buffShow.SetActive (false);
				}
			}

		}
		if(findTargetTimer>0)findTargetTimer -= dTime;
		if(checkSkillTimer>0)checkSkillTimer -= dTime;
		atkTimer += dTime;
		if(skillComponent!=null){
			skillComponent.Tick(dTime);
			checkSkill (dTime);
		}
		if (coolDown > 0) {
			coolDown -= dTime;
		}
		if (atkType == eAtkType.MELLE_POINT) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {
				applyAtk (atkTarget);
				//atkTarget.knock (atkTarget.transform.position - this.transform.position, 0.2f, 6f);
				//atkTarget.DoDamage (damage,property);
				atkTarget = null;
			}
		} else if (atkType == eAtkType.RANGED_HOMING) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {
				genBullet (tt.tbase.bulletStyle,true,atkTarget.gameObject);
				atkTarget = null;
			}
		} else if (atkType == eAtkType.MELLE_AOE) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {

				Vector2 faceDir = (atkTarget.transform.position - transform.position);
				EffectManager.inst.EmitAtkSectorEffect (transform, faceDir);

				float cx = transform.position.x;
				float cy = transform.position.y;
				List<GameLife> validTarget = new List<GameLife> ();
				foreach (GameLife enemy in BattleManager.getInstance().enemies) {
					float x = enemy.transform.position.x;
					float y = enemy.transform.position.y;

					Vector2 dir = new Vector2 (x - cx, y - cy);
					if (dir.magnitude * 1000 < atkRange && Vector2.Dot (faceDir.normalized, dir.normalized) > Mathf.Cos (60f * Mathf.Deg2Rad)) {
						validTarget.Add (enemy);
						//enemy.DoDamage (damage,property);
					}

				}
				foreach (GameLife target in validTarget) {
					applyAtk (target);
				}
				//atkTarget.DoDamage (damage);
				atkTarget = null;
			}

		} else if (atkType == eAtkType.RANGED_INSTANT) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {
				applyAtk (atkTarget);
				//atkTarget.DoDamage (damage,property);
				atkTarget = null;
			}
		} else if (atkType == eAtkType.RANGED_MULTI) {
//			if (atkTarget != null && atkTimer > atkPreanimTime) {
//				atkTarget.DoDamage (damage);
//				atkTarget = null;
//			}
		} else if (atkType == eAtkType.RANGED_UNHOMING) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {
				genBullet (tt.tbase.bulletStyle,false,atkTarget.gameObject);
				atkTarget = null;
			}
		}

		if (coolDown > 0) {
			return;
		}
		if (castIdx != -1)
			return;
		updateTarget ();

		if (hateTarget != null) {
			coolDown = atkInterval;
			anim.SetTrigger ("atk");
			atkTimer = 0;
			atkTarget = hateTarget;
		}
	}


	public void updateTarget(){
		if (hateTarget != null) {
			if (!hateTarget.IsAlive) {
				hateTarget = null;
			} else {
				int dis = (int)((transform.position - hateTarget.transform.position).magnitude * 1000);
				if(dis > atkRange){
					hateTarget = null;
				}
			}
		}
		if (hateTarget == null&&findTargetTimer<=0) {
			hateTarget = MapManager.getInstance().getClosestEnemy (this);
			if (hateTarget!=null&&!hateTarget.IsAlive) {
				hateTarget = null;
			}
			if (hateTarget != null ) {
				Vector3 Diff2d = (transform.position - hateTarget.transform.position);
				Diff2d.z = 0;
				int dis = (int)(Diff2d.magnitude * 1000);
				if (dis > atkRange) {
					hateTarget = null;
				}
			}
			findTargetTimer = findTargetInteval;
		}

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


	public void applyAtk(GameLife hit){
		//hit.knock (atkTarget.transform.position - this.transform.position, 0.2f, 6f);

		List<Buff> attackEffect = new List<Buff> ();
		foreach (TowerSkillState skill in skillComponent.skills) {
			if (skill == null)
				continue;
			TowerSkill sinfo = GameStaticData.getInstance().towerSkills[skill.skillId];
			if (sinfo.isPassive && sinfo.checkPoint == ePassiveCheckPoint.ATK) {
				attackEffect.Add (new Buff (sinfo.x[skill.skillLevel], 50, 1000));
			}
		}
		List<AtkInfo> atk = new List<AtkInfo> ();
		atk.Add (mainAtk);
		atk.AddRange (extraAtk);
		hit.DoDamage (atk,mingzhong,property,attackEffect);
	}
}

