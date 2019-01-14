using UnityEngine;
using System.Collections;

public class SpawnTowerEffect : BaseEffect
{
	Vector3Int toSpawnPosInCell;
	GameObject sprite;

	public static float Gg = 50f;
	public float OFFSET = 0.05f;
	private int height = 0;
	private int vHeight = 0;

	int spawingTimeLeft = 300;
	Vector3 v = Vector3.zero;

	void Start(){
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
			transform.position += v * timeInt * 0.001f;
		}
	}



	public void init(Vector3Int toSpawnPosInCell,Vector3 playerPosInWorld, float toSpawnTime = 0.3f){
		initialized = true;
		gameObject.SetActive (true);
		this.toSpawnPosInCell = toSpawnPosInCell;
		spawingTimeLeft = (int)(toSpawnTime * 1000);
		transform.position = playerPosInWorld;

		vHeight = (int)(toSpawnTime * 1000 / 2 * Gg);


		Vector3 toSpawnPosInWorld = MapManager.getInstance ().obcTilemap.CellToWorld (toSpawnPosInCell);
		toSpawnPosInWorld.x += MapManager.TILE_WIDTH * 0.5f * 0.01f;
		toSpawnPosInWorld.y += MapManager.TILE_HEIGHT * 0.5f * 0.01f;
		toSpawnPosInWorld.z = 0;

		v = (toSpawnPosInWorld - playerPosInWorld) / toSpawnTime;
	}

	protected override void OnRelease(){
		TowerFactory.createTower ("tower01", toSpawnPosInCell, transform);
		sprite = null;
		v = Vector3.zero;
		toSpawnPosInCell = Vector3Int.zero;
		Destroy (gameObject);
	}
}

