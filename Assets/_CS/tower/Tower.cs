using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{

	public bool initialized = false;

	[SerializeField]
	public eAtkType atkType = eAtkType.MELLE;
	public int damage = 10000;
	public int atkInterval = 2000;
	public int atkRange = 3000;
	public int atkPreanimTime = 300;


	public Vector3Int posInCell = Vector3Int.zero;


	private Animator anim;

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
	}
	// Use this for initialization
	void Start ()
	{
	
	}

	public void init(TowerData towerData, Vector3Int posInCell){
		if (towerData != null) {
			this.atkType = towerData.atkType;
			this.damage = towerData.damage;
			this.atkInterval = towerData.atkInteval;
			this.atkRange = towerData.atkRange;
			this.atkPreanimTime = towerData.atkPreanimTime;
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

	void genBullet(){
		BulletManager.inst.Emit (gameObject,atkTarget.gameObject,5000);

//		GameObject o = GameObject.Instantiate (bulletPrefab,transform.parent);
//		o.transform.position = transform.position;
//		o.GetComponent<Bullet> ().target = atkTarget.gameObject;
	}
	

	void Update ()
	{
		int dTime = (int)(Time.deltaTime*1000);
		atkTimer += dTime;
		if (coolDown > 0) {
			coolDown -= dTime;
		}
		if (atkType == eAtkType.MELLE) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {
				atkTarget.knock (atkTarget.transform.position - this.transform.position, 0.2f, 6f);
				atkTarget.DoDamage (damage);
				atkTarget = null;
			}
		} else if (atkType == eAtkType.RANGE) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {
				genBullet ();
				atkTarget = null;
			}
		} else if (atkType == eAtkType.MELLE_SECTOR) {
			if (atkTarget != null && atkTimer > atkPreanimTime) {

				Vector2 faceDir = (atkTarget.transform.position - transform.position);
				EffectManager.inst.EmitAtkSectorEffect (transform, faceDir);

				float cx = transform.position.x;
				float cy = transform.position.y;

				foreach (GameLife enemy in GameManager.getInstance().enemy) {
					float x = enemy.transform.position.x;
					float y = enemy.transform.position.y;

					Vector2 dir = new Vector2(x - cx,y-cy);
					if (dir.magnitude * 1000 < atkRange && Vector2.Dot(faceDir.normalized,dir.normalized) > Mathf.Cos(60f * Mathf.Deg2Rad)) {
						enemy.DoDamage (damage);
					}

				}

				//atkTarget.DoDamage (damage);
				atkTarget = null;
			}

		} else {
		
		}

		if (coolDown > 0) {
			return;
		}

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
			if (!hateTarget.IsAlive) {
				hateTarget = null;
			}
			if (hateTarget != null ) {
				int dis = (int)((transform.position - hateTarget.transform.position).magnitude * 1000);
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


//	public bool checkOverlap(){
//		Collider2D[] cols = Physics2D.OverlapCircleAll (transform.position,r,1<<LayerMask.NameToLayer("enemy"));
//		if (cols.Length == 0) {
//			Debug.Log ("可建造");
//		}
//		return false;
//	}
}

