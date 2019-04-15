using UnityEngine;
using System.Collections;

public class SpawnTowerEffect : BaseEffect
{
	Vector2Int toSpawnPosInCell;
	GameObject sprite;

	public static float Gg = 50f;
	public float OFFSET = 0.05f;
	private int height = 0;
	private int vHeight = 0;

	int spawingTimeLeft = 300;
	Vector3Int v = Vector3Int.zero;

	int towerIdx;


	protected override void Start(){
		base.Start ();
		sprite = transform.Find ("sprite").gameObject;
	}

	protected override void effectAction(int timeInt){

		if (height >= 0) {
			vHeight -= (int)Gg * timeInt;
			height += (int)(vHeight * 0.001f) * timeInt;
		}
		sprite.transform.localPosition = new Vector3 (0,height * 0.001f * 5+OFFSET,0);

		spawingTimeLeft -= timeInt;
		if (spawingTimeLeft <= 0) {
			Release ();
		} else {
			posXInt += v.x * timeInt / 1000;
			posYInt += v.y * timeInt / 1000;
		}
	}



	public void init(int towerIdx,Vector2Int toSpawnPosInCell,Vector2Int playerPosInWorld, float toSpawnTime = 0.3f){
		initialized = true;
		gameObject.SetActive (true);
		this.toSpawnPosInCell = toSpawnPosInCell;
		spawingTimeLeft = (int)(toSpawnTime * 1000);
		posXInt = playerPosInWorld.x;
		posYInt = playerPosInWorld.y;

		vHeight = (int)(toSpawnTime * 1000 / 2 * Gg);

		this.towerIdx = towerIdx;


		Vector2Int toSpawnPosInWorld = MapManager.getInstance ().CellToWorld (toSpawnPosInCell);
		Vector2Int diff = (toSpawnPosInWorld - playerPosInWorld);
		v = new Vector3Int( (int)(diff.x/toSpawnTime),(int)(diff.y/toSpawnTime),0);
	}

	protected override void OnRelease(){
		TowerFactory.createTower (towerIdx, toSpawnPosInCell, GameObject.Find ("MapLayer").transform);
		sprite = null;
		v = Vector3Int.zero;
		toSpawnPosInCell = Vector2Int.zero;
		Destroy (gameObject);
	}
}

