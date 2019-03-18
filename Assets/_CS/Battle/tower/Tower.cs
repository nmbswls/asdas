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
	public int damage = 10000;
	public int mingzhong = 2;
	//list

	public int atkInterval = 2000;
	public int atkRange = 3000;
	public int atkPreanimTime = 300;

	public int castPreTime = 800;
	public int castTimeDuration = 1500;
	public bool skillEffect = false;

	public Vector3Int posInCell = Vector3Int.zero;


	private Animator anim;

	private TowerTemplate tt;

	SkillComponent skillComponent;
	public BuffComponent buffManager;
	public GameObject buffShow;

	[SerializeField]
	private GameLife hateTarget = null;
	private GameLife atkTarget = null;

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
	void Start ()
	{
		base.Start ();
	}

	public void init(TowerTemplate towerTemplate, Vector3Int posInCell){
		if (towerTemplate != null) {
			this.atkType = towerTemplate.tbase.atkType;
			this.damage = towerTemplate.tbase.damage;
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

	void genBullet(bool isHoming){

		BulletManager.inst.Emit (gameObject,atkTarget.gameObject,isHoming,5000);

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
			if (castTime > castPreTime&&!skillEffect) {
				Debug.Log ("use skill "+castIdx);
				gainBuff ();
				skillEffect = true;
			}
			if (castTime > castTimeDuration) {
				castIdx = -1;
				castTime = 0;
				skillEffect = false;
			}
			return;
		}


		int readySkill = skillComponent.getReadySkill();


		if (readySkill != -1) {
			if (skillComponent.skills [readySkill].skillId == 0) {
				

			}
			skillComponent.setSkillCD(readySkill);
			anim.SetTrigger ("buff");
			castTime = 0;
			castIdx = readySkill;
			skillEffect = false;
		}
	}


	void Update ()
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
				genBullet (true);
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
				genBullet (false);
				atkTarget = null;
			}
		}

		if (coolDown > 0) {
			return;
		}
		if (castIdx != -1)
			return;

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
		if (hateTarget == null) {
			hateTarget = MapManager.getInstance().getClosestEnemy (gameObject);
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
		}
		if (hateTarget == null) {
			
		} else {
			coolDown = atkInterval;
			anim.SetTrigger ("atk");
			atkTimer = 0;
			atkTarget = hateTarget;
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

		hit.DoDamage (damage,mingzhong,property,attackEffect);
	}
}

