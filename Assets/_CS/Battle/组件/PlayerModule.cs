using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModule : MonoBehaviour
{
	public GameLife gl;


	public GameObject trapPrefab;

	public int godModeTime = 1000;

	public GameObject shield;
	// Use this for initialization
	void Awake ()
	{
		gl = GetComponent<GameLife> ();
		trapPrefab = Resources.Load ("Prefabs/traps/trap01") as GameObject;
		shield = transform.Find ("shield").gameObject;
	}

	// Update is called once per frame
	void Update ()
	{

		Tick ((int)(Time.deltaTime*1000f));
	}

	//public bool canDamaged = true;
	public int shieidTime = 2000;

	void Tick(int timeInd){
		if (godModeTime > 0)
			godModeTime -= timeInd;
		if(shieidTime > 0)
			shieidTime -= timeInd;
		if(shieidTime<=0)shield.SetActive (false);
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
//					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().obcTilemap.GetTile(newPos)==null && MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
//						possible.Add (newPos);
//					}
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[-newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector3Int newPos = new Vector3Int (posXY.x - offset,posXY.y - r,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[-newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector3Int newPos = new Vector3Int (posXY.x + r,posXY.y - offset,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[-newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector3Int newPos = new Vector3Int (posXY.x - r,posXY.y + offset,0);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[-newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
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

	public bool tryBuildTower(int towerIdx){
		if (BattleManager.getInstance ().money [0] < BattleManager.getInstance ().buildableTowers [towerIdx].tbase.cost) {
			return false;	
		}
		if (!SpawnTower (towerIdx)) {
			return false;
		}
		BattleManager.getInstance ().money [0] -= BattleManager.getInstance ().buildableTowers [towerIdx].tbase.cost;
		return true;
	}

	public bool SpawnTower(int towerIdx){
		Vector3Int closestPos = Vector3Int.zero;
		if (getClosestEmptyGrid (transform.position, out closestPos)) {
			//StartCoroutine (spawnTowerDelay(name,0.3f,closestPos));
			//TowerFactory.createTower (name, closestPos, transform.parent);
			int checkI = -closestPos.y;
			int checkJ = closestPos.x;

			MapManager.getInstance ().updateOneBlock (closestPos);
			//dynamicBlocks [checkI] [checkJ] = true;
			EffectManager.inst.EmitSpawnTowerEffect (towerIdx,closestPos,transform,0.3f);
			return true;
		} else {
			Debug.Log ("no pos");
			return false;
		}
	}
//	IEnumerator spawnTowerDelay(string name, float delay,Vector3Int closestPos){
//		yield return new WaitForSecondsRealtime (delay);
//		TowerFactory.createTower (name, closestPos, transform.parent);
//	}



//	public void useItem(int idx){
//		BattleManager.getInstance ().useItem (idx);
//	}

	public void setTrap(){
		GameObject.Instantiate (trapPrefab,transform.position,Quaternion.identity,BattleManager.getInstance().mapObjLayer);
	}

	public void useShield(){
		shieidTime = 2000;
		shield.SetActive (true);
		//canDamaged = false;
	}
		
	public void useBlink(){
		Vector2 d = gl.faceDirVector;
		Vector3 target = new Vector2(gl.transform.position.x,gl.transform.position.y) + d * 1f;
		if (!MapManager.getInstance ().isWorldPosObc (target)) {
			transform.position = target;
		} else {
			
		}
	}


	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag != "enemy") {
			return;
		}
		if (godModeTime > 0)
			return;
		if (shieidTime>0)
			return;
		
		EmitManager.inst.Emit (transform,0,5,true);
		BattleManager.getInstance().getDamaged (5);
		godModeTime = 1000;
	}
}

