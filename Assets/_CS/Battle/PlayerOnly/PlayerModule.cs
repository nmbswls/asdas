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
	public bool getClosestEmptyGrid(Vector2Int posInWorld, out Vector2Int closestPos, int range = 3){
		closestPos = Vector2Int.zero;
		Vector2Int posXY = MapManager.getInstance ().WorldToCell (posInWorld);
		List<Vector2Int> possible;
		for (int r = 1; r <= range; r++) {
			possible = new List<Vector2Int> ();
			for (int offset = -r; offset < r; offset++) {
				{
					Vector2Int newPos = new Vector2Int (posXY.x + offset,posXY.y + r);
//					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().obcTilemap.GetTile(newPos)==null && MapManager.getInstance().dynamicBlocks[-newPos.y][newPos.x] == false){
//						possible.Add (newPos);
//					}
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector2Int newPos = new Vector2Int (posXY.x - offset,posXY.y - r);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector2Int newPos = new Vector2Int (posXY.x + r,posXY.y - offset);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[newPos.y][newPos.x] == false){
						possible.Add (newPos);
					}
				}
				{
					Vector2Int newPos = new Vector2Int (posXY.x - r,posXY.y + offset);
					if(checkPos(newPos.x,newPos.y)&&MapManager.getInstance ().specialBlock[newPos.y][newPos.x]==1&&MapManager.getInstance().dynamicBlocks[newPos.y][newPos.x] == false){
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

		//y = -y;
		if (x < 0 || x >= MapManager.MAP_WIDTH || y < 0 || y >= MapManager.MAP_HEIGHT) {
			return false;
		}
		return true;
	}

	public bool tryBuildTower(int towerIdx){
		if (BattleManager.getInstance ().money [0] < BattleManager.getInstance ().buildableTowers [towerIdx].tbase.cost[0]) {
			return false;	
		}
		if (!SpawnTower (towerIdx)) {
			return false;
		}
		BattleManager.getInstance ().money [0] -= BattleManager.getInstance ().buildableTowers [towerIdx].tbase.cost[0];
		return true;
	}

	public bool SpawnTower(int towerIdx){
		Vector2Int closestPos = Vector2Int.zero;
		if (getClosestEmptyGrid (new Vector2Int(gl.posXInt,gl.posYInt),out closestPos)) {
			//StartCoroutine (spawnTowerDelay(name,0.3f,closestPos));
			//TowerFactory.createTower (name, closestPos, transform.parent);
			int checkI = -closestPos.y;
			int checkJ = closestPos.x;

			MapManager.getInstance ().updateOneBlock (closestPos);
			//dynamicBlocks [checkI] [checkJ] = true;
			EffectManager.inst.EmitSpawnTowerEffect (towerIdx,closestPos,gl,0.3f);
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
		Vector2Int d = new Vector2Int((int)(gl.faceDirVector.x*1000f),(int)(gl.faceDirVector.y*1000f));
		Vector2Int target = new Vector2Int(gl.posXInt,gl.posYInt) + d * 1;
		if (!MapManager.getInstance ().isWorldPosObc (target)) {
			gl.posXInt = target.x;
			gl.posYInt = target.y;
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

