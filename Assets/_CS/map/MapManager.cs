using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

	private static MapManager instance;

	public static MapManager getInstance(){
		return instance;
	}

	public static int MAP_WIDTH = 50;
	public static int MAP_HEIGHT = 30;

	public static int TILE_WIDTH = 50;
	public static int TILE_HEIGHT = 50;

	public bool[][] dynamicBlocks;
	public bool[][] staticBlocks;
	public List<GameObject> tiles = new List<GameObject>();
	public GameObject tilePrefabs;
	public Sprite[] images;
	public Sprite emptyImage;


	public List<MapZone> zones = new List<MapZone>();
	public static int MAP_ZONE_SIZE = 10;
	int zoneNumX = ((MAP_WIDTH - 1) / MAP_ZONE_SIZE) + 1;
	int zoneNumY = ((MAP_HEIGHT - 1) / MAP_ZONE_SIZE) + 1;
	//上左下右
	void Awake(){
		if (instance != null) {
			Destroy (this);
			return;
		}
		instance = this;

	}

	public void updateBlock(){
		
		foreach(Tower t in GameManager.getInstance ().getAllTower()){
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					int checkI = -t.posInCell.y + i;
					int checkJ = t.posInCell.x + j;
					if (checkI < 0 || checkI >= MAP_HEIGHT || checkJ < 0 || checkJ >= MAP_WIDTH) {
						continue;
					}
					dynamicBlocks [checkI] [checkJ] = true;
				}
			}
		}
	}


	public MapZone getZoneByPosition(Vector3 posInWorld){
		Vector3Int posInCell = tilemap.WorldToCell (posInWorld);
		int x = posInCell.x / MAP_ZONE_SIZE;
		int y = posInCell.y / MAP_ZONE_SIZE;
		if (zoneNumX * y + x < zones.Count) {
			return zones [(((MAP_WIDTH - 1) / MAP_ZONE_SIZE) + 1) * y + x];
		}
		return null;
	}

	//九宫格
	public List<MapZone> getNearbyZones(Vector3 posInWorld){
		MapZone res = getZoneByPosition (posInWorld);
		int idx = res.idx;
		int idxY = idx / zoneNumX;
		int idxX = idx % zoneNumX;
		for (int di = -1; di <= 1; di++) {
			for (int dj = -1; dj <= 1; dj++) {
				
			}
		}
		return null;
	}



	// Use this for initialization
	void Start ()
	{
		int numOfZone = (((MAP_HEIGHT - 1) / MAP_ZONE_SIZE) + 1) * (((MAP_WIDTH - 1) / MAP_ZONE_SIZE) + 1);
		for (int i = 0; i < numOfZone; i++) {
			zones.Add (new MapZone(i));
		}


		dynamicBlocks = new bool[MAP_HEIGHT][];
		staticBlocks = new bool[MAP_HEIGHT][];
		for (int i = 0; i < MAP_HEIGHT; i++) {
			dynamicBlocks[i] = new bool[MAP_WIDTH];
			staticBlocks[i] = new bool[MAP_WIDTH];
		}
//		bg [0] [1] = 1;
//
//		bg [0] [2] = 1;
//		bg [0] [3] = 1;
//		bg [0] [4] = 1;
//		bg [0] [5] = 1;
//		bg [0] [6] = 1;
//		bg [0] [7] = 1;
		//GenerateMap ();


		int colorCount = 6;
		arrTiles = new Tile[colorCount];
		for(int i=0;i<colorCount;i++){
			arrTiles[i] = ScriptableObject.CreateInstance<Tile>();//创建Tile，注意，要使用这种方式
			arrTiles[i].sprite = baseTile.sprite;
			arrTiles[i].color = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f, 1f), 1);
		}
		for(int i=0;i<MAP_HEIGHT;i++){//这里就是设置每个Tile的信息了
			for(int j=0;j<MAP_WIDTH;j++){
				if (tilemap.GetTile (new Vector3Int (j, -i, 0)) == null) {
					tilemap.SetTile (new Vector3Int (j, -i, 0), arrTiles [Random.Range (0, arrTiles.Length)]);
				}
				if (obcTilemap.GetTile (new Vector3Int (j, -i, 0)) != null) {
					staticBlocks [i] [j] = true;
				}
			}
			//yield return null;
		}


		Debug.Log (tilemap.CellToWorld (new Vector3Int(1,-1,0)));
		Debug.Log (tilemap.CellToWorld (new Vector3Int(0,0,0)));
	}

	Tile[] arrTiles;
	public Tile baseTile;//使用的最基本的Tile，我这里是白色块，然后根据数据设置不同颜色生成不同Tile

	public Tilemap tilemap;
	public Tilemap obcTilemap;
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void Recalculate(){
		
	}

	//左上右下
	public static int[][] dir = new int[4][]{new int[]{0,-1},new int[]{-1,0},new int[]{0,1},new int[]{1,0}};

	void GenerateMap () {
		//ScriptableObject.CreateInstance ();
//		for (int i = 0; i < bg.Length; i++) {
//			for (int j = 0; j < bg [i].Length; j++) {
//				if (bg [i] [j] == 0) {
//					continue;
//				}
//				int mask = 0;
//				for (int k = 0; k < 4; k++) {
//					int nextI = i + dir [k] [0];
//					int nextJ = j + dir [k] [1];
//					if (posValid (nextI, nextJ) && bg[nextI][nextJ] == bg[i][j] && bg[nextI][nextJ] != 0) {
//						mask |= 1 << k;
//					}
//				}
//				mask = ~mask & 15;
//				GameObject p = GameObject.Instantiate (tilePrefabs,transform);
//				p.GetComponent<SpriteRenderer>().sprite = images[mask];
//				p.transform.localPosition = new Vector3 ((TILE_WIDTH * j + 0.5f * TILE_WIDTH)*0.01f, -(TILE_HEIGHT * i + 0.5f * TILE_HEIGHT)*0.01f,0);
//
//			}
//		}
	}

	bool posValid(int i, int j){
		if (i < 0 || i >= MAP_HEIGHT || j < 0 || j >= MAP_WIDTH) {
			return false;
		}
		return true;
	}

	public List<GameLife> getEnemiesInRange(GameLife center, int range){
		List<GameLife> res = new List<GameLife> ();
		foreach (GameLife gl in GameManager.getInstance ().enemy) {
			if (gl.gameObject == center)
				continue;
			int dis = (int)((center.transform.position - gl.transform.position).magnitude * 1000);
			if (dis < range) {
				res.Add (gl);
			}
		}
		return res;
	}

	public GameLife getClosestEnemy(GameObject center){
		int closest = int.MaxValue;
		int minidx = -1;
		int idx = 0;

		foreach (GameLife gl in GameManager.getInstance ().enemy) {
			if (gl.gameObject == center)
				continue;
			int dis = (int)((center.transform.position - gl.transform.position).magnitude * 1000);
			if (dis < closest) {
				closest = dis;
				minidx = idx;
			}
			idx++;
		}
		if (minidx == -1) {
			return null;
		}

		return GameManager.getInstance ().enemy[minidx];
	}
}

