using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLife : MonoBehaviour
{
	public IUnitController pCtrl;


//	public CharPhysicsController charPhysicsCtrl;

	//固有属性
	public int id;
	public Animator anim;
	public SpriteRenderer image;
//	public AttackEffect attackEffect;
	public Rigidbody2D rBody;
//	public GameLifeStats stats;

	public float characterSizeX;
	public float characterSizeY;
	public bool isPlayer = true;

	//可变属性
	public float maxHP = 1000f;
	public float hp = 1000f;
	public float mp;
	public int def = 5000;
	public int atk;


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

	bool isUsingSkill = false;
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
		rBody = GetComponent<Rigidbody2D>();
		image = GetComponentInChildren<SpriteRenderer> ();

		Application.targetFrameRate = 60;
	}




	public Vector2 faceDirVector
	{
		get { return new Vector2(dirVec[faceDir][0],dirVec[faceDir][1]); }
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
	public float walkSpeed = 3.0f;
	public Vector2 thrustVec;
	public int force;
	public int knockLast;

	// Use this for initialization
	void Start ()
	{
		isMoving = false;
		isAttcking = false;



		characterSizeY = image.bounds.size.y;
		characterSizeX = image.bounds.size.x;

		faceDir = 2;
		lastPosInCell = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
	}




	public bool canMove(){
		return !(isAttcking || isPushingBack);
	}

	// Update is called once per frame
	void Update ()
	{			



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
				vReal = pCtrl.Dmag * pCtrl.Dvec * walkSpeed;
			}
		}

		if (knockLast > 0) {
			knockLast -= (int)Mathf.Round (Time.deltaTime * 1000f);
			if (knockLast <= 0) {
				thrustVec = Vector3.zero;
				anim.SetLayerWeight (anim.GetLayerIndex ("DamagedLayer"), 0);
			}
		}


		Vector3Int nowPos = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
		if (isPlayer && nowPos != lastPosInCell) {
			OnGridChange ();
		}
		lastPosInCell = nowPos;
	}

	void FixedUpdate(){
		//rBody.position += deltaPos;
		//rBody.position += new Vector3(thrustVec.x * Time.fixedDeltaTime,thrustVec.y * ,0);
		if (knockLast > 0) {
			rBody.velocity = new Vector3 (thrustVec.x, thrustVec.y, 0);
		} else {
			rBody.velocity = new Vector3 (vReal.x, vReal.y, 0);
			if (!isPlayer) {
				rBody.velocity += pCtrl.offsetV;
			}
		}

		//thrustVec = Vector3.zero;
		//deltaPos = Vector3.zero;
	}
		

	public void knock(Vector3 dir, float last, float force){
		if (isPlayer && knockLast > 0) {
			return;
		}
		this.thrustVec = /*VInt*/dir.normalized * force;
		this.knockLast = (int)Mathf.Round (last * 1000);
		anim.SetLayerWeight (anim.GetLayerIndex ("DamagedLayer"), 1);
	}

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

	public void DoDamage(int atk){
		Debug.Log ("damage"+name);
		int damage = (atk - def) > 0 ? atk - def : 1;
		hp -= damage * 0.001f;
		EmitManager.inst.Emit(transform, 0, (int)(damage*0.001f), UnityEngine.Random.Range(0, 10) == 5);
		if (hp < 0) {
			OnDie ();
		}
	}

	public bool getClosestEmptyGrid(Vector3 posInWorld, out Vector3Int closestPos, int range = 3){
		closestPos = Vector3Int.zero;
		Vector3Int posXY = MapManager.getInstance ().obcTilemap.WorldToCell (posInWorld);
		List<Vector3Int> possible;
		for (int r = 1; r <= range; r++) {
			possible = new List<Vector3Int> ();
			for (int offset = -r; offset < r; offset++) {
				{
					Vector3Int newPos = new Vector3Int (posXY.x + offset,posXY.y + r,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().obcTilemap.GetTile(newPos)==null && MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector3Int newPos = new Vector3Int (posXY.x - offset,posXY.y - r,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().obcTilemap.GetTile(newPos)==null && MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector3Int newPos = new Vector3Int (posXY.x + r,posXY.y - offset,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().obcTilemap.GetTile(newPos)==null && MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector3Int newPos = new Vector3Int (posXY.x - r,posXY.y + offset,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().obcTilemap.GetTile(newPos)==null && MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
			}
			if (possible.Count > 0) {
				closestPos = possible[Random.Range (0, possible.Count)];
				return true;
			}
		}
		return false;
	}

	bool checkPos(int x, int y){
		
		y = -y;
		if (x < 0 || x >= MapManager.MAP_WIDTH || y < 0 || y >= MapManager.MAP_HEIGHT) {
			return false;
		}
		return true;
	}

	public void SpawnTower(string name){
		Vector3Int closestPos = Vector3Int.zero;
		if (getClosestEmptyGrid (transform.position, out closestPos)) {
			TowerFactory.createTower (name, closestPos, transform.parent);
			EffectManager.inst.EmitSpawnTowerEffect (closestPos,transform,0.3f);
		} else {
			Debug.Log ("no pos");
		}
	}

}



