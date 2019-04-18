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

	public int MAP_WIDTH = 50;
	public int MAP_HEIGHT = 30;


	public int MAP_MAX_WIDTH = 50;
	public int MAP_MAX_HEIGHT = 30;

	public static int TILE_WIDTH = 100;
	public static int TILE_HEIGHT = 100;



	Tile[] arrTiles;
	public Tile baseTile;//使用的最基本的Tile，我这里是白色块，然后根据数据设置不同颜色生成不同Tile

	public Tilemap tilemap;
	public SpriteRenderer smokeShadow;
	public Tilemap obcTilemap;
	public Tilemap speTilemap;

	public bool[][] dynamicBlocks;
	public bool[][] staticBlocks;

	public int[][] specialBlock;

	List<int[]> possibleSpawnerPos = new List<int[]>();
	int[] playerStartPos = new int[]{0,0};
	public List<GameObject> tiles = new List<GameObject>();
	public GameObject tilePrefabs;
	public Sprite[] images;
	public Sprite emptyImage;


	public List<MapZone> zones = new List<MapZone>();
	public static int MAP_ZONE_SIZE = 10;
	//int zoneNumX = ((MAP_WIDTH - 1) / MAP_ZONE_SIZE) + 1;
	//int zoneNumY = ((MAP_HEIGHT - 1) / MAP_ZONE_SIZE) + 1;

	int zoneNumX = 0;
	int zoneNumY = 0;

	//GameObject mapPrefab1;
	//GameObject mapPrefab2;

	//GameObject mapActiveArea1;

	public Transform obstacleLayer;
	public Transform activeLayer;

	void Start(){
		transform.position = Vector3.zero;
	}

	public bool getMapResource(string mapId){
		MapInfo mapInfo = (MapInfo)Resources.Load ("map/"+mapId+"/info");
		GameObject mapObcPrefab = (GameObject)Resources.Load ("map/" + mapId + "/obc");
		GameObject mapSpePrefab = (GameObject)Resources.Load ("map/" + mapId + "/spe");


		if (mapObcPrefab == null || mapSpePrefab == null) {
			return false;
		}

		smokeShadow.size = new Vector2(mapInfo.width,mapInfo.height);
		smokeShadow.transform.position = new Vector3 (mapInfo.width/2 + 0.5f,-mapInfo.height/2 - 0.5f,smokeShadow.transform.position.z);

		GameObject mapObc = null;
		GameObject mapSpe = null;

		mapObc = GameObject.Instantiate (mapObcPrefab, obstacleLayer);
		mapSpe = GameObject.Instantiate (mapSpePrefab,activeLayer);



		mapObc.transform.localPosition = new Vector3 (0, 0, 0);
		mapSpe.transform.localPosition = new Vector3 (0, 0, 0);


		this.obcTilemap = mapObc.GetComponent<Tilemap> ();
		this.speTilemap = mapSpe.GetComponent<Tilemap> ();
		this.MAP_WIDTH = mapInfo.width;
		this.MAP_HEIGHT = mapInfo.height;

		return true;
	}
	public void LoadMap(string mapId){
		if (obcTilemap != null) {
			GameObject.Destroy (obcTilemap.gameObject);
		}
		if (speTilemap != null) {
			GameObject.Destroy (speTilemap.gameObject);
		}

		bool res = getMapResource (mapId);
		if (!res)
			return;

		for(int i=0;i<MAP_MAX_HEIGHT;i++){
			for(int j=0;j<MAP_MAX_WIDTH;j++){
				
				if (i >= MAP_HEIGHT || j >= MAP_WIDTH) {
					tilemap.SetTile (new Vector3Int (j, -i, 0), null);
					continue;
				}
				if (obcTilemap.GetTile (new Vector3Int (j, -i, 0)) != null) {
					staticBlocks [i] [j] = true;
				} else {
					staticBlocks [i] [j] = false;
				}
				if (speTilemap.GetTile (new Vector3Int (j, -i, 0)) != null) {
					string blockType = speTilemap.GetTile (new Vector3Int (j, -i, 0)).name;

					switch (blockType) {
					case "BuildArea":
						specialBlock [i] [j] = 1;
						break;
					case "Spawner":
						specialBlock [i] [j] = 2;
						possibleSpawnerPos.Add (new int[]{i,j});
						speTilemap.SetTile (new Vector3Int (j, -i, 0), null);

						break;
					case "PlayerSpawner":
						playerStartPos = new int[]{ i, j };
						speTilemap.SetTile (new Vector3Int (j, -i, 0), null);
						break;
					}
				}
			}
		}


	}


	//上左下右
	void Awake(){
		if (instance != null) {
			Destroy (this);
			return;
		}
		instance = this;

	}

	public void updateOneBlock(Vector2Int posInCell){
		dynamicBlocks [posInCell.y] [posInCell.x] = true;
	}

	public void updateBlock(){
		
		foreach(Tower t in BattleManager.getInstance ().getAllTower()){
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
	public void Init ()
	{

		//mapActiveArea1 = Resources.Load ("map/Tilemap_active1") as GameObject;


		int numOfZone = (((MAP_HEIGHT - 1) / MAP_ZONE_SIZE) + 1) * (((MAP_WIDTH - 1) / MAP_ZONE_SIZE) + 1);
		for (int i = 0; i < numOfZone; i++) {
			zones.Add (new MapZone(i));
		}


		dynamicBlocks = new bool[MAP_HEIGHT][];
		staticBlocks = new bool[MAP_HEIGHT][];
		specialBlock = new int[MAP_HEIGHT][];
		for (int i = 0; i < MAP_HEIGHT; i++) {
			dynamicBlocks[i] = new bool[MAP_WIDTH];
			staticBlocks[i] = new bool[MAP_WIDTH];
			specialBlock[i] = new int[MAP_WIDTH];
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


//		int colorCount = 6;
//		arrTiles = new Tile[colorCount];
//		for(int i=0;i<colorCount;i++){
//			arrTiles[i] = ScriptableObject.CreateInstance<Tile>();
//			arrTiles[i].sprite = baseTile.sprite;
//			arrTiles[i].color = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f, 1f), 1);
//		}


		for(int i=0;i<MAP_HEIGHT;i++){
			for(int j=0;j<MAP_WIDTH;j++){
				if (tilemap.GetTile (new Vector3Int (j, -i, 0)) != null) {
					//tilemap.SetTile (new Vector3Int (j, -i, 0), arrTiles [Random.Range (0, arrTiles.Length)]);
				}
//				if (obcTilemap.GetTile (new Vector3Int (j, -i, 0)) != null) {
//					staticBlocks [i] [j] = true;
//				}
			}
			//yield return null;
		}
		PlayerPrefs.DeleteAll ();
		if (PlayerPrefs.GetInt ("isFirstBattle", 1) == 1) {
			LoadMap ("toturial");
		} else {
			LoadMap ("map01");
		}

	}


	
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
		foreach (GameLife gl in BattleManager.getInstance ().getTmpEnemyList()) {
			if (gl.gameObject == center)
				continue;
			int dis = (int)((center.transform.position - gl.transform.position).magnitude * 1000);
			if (dis < range) {
				res.Add (gl);
			}
		}
		return res;
	}

	public GameLife getClosestEnemy(MapObject center){
		int closest = int.MaxValue;
		GameLife closestEnemy = null;
		int idx = 0;

		foreach (GameLife gl in BattleManager.getInstance ().getTmpEnemyList()) {
			if (gl.gameObject == center)
				continue;
			if (!gl.IsAlive)
				continue;
			Vector3 Diff2d = (center.transform.position - gl.transform.position);
			Diff2d.z = 0;
			int dis = (int)(Diff2d.magnitude * 1000);
			if (dis < closest) {
				closest = dis;
				closestEnemy = gl;
			}
			idx++;
		}


		return closestEnemy;
	}



	public Vector2Int getRandomPosToGo(int i,int j){
		int time = 0;
		while (time<10) {
			int rIdx = Random.Range (0, 25);
			int col = rIdx % 5;
			int row = rIdx / 5;
			int nextI = i - 2 + row;
			int nextJ = j -2 + col;

			if (nextI >= 0 && nextI < staticBlocks.Length && nextJ >= 0 && nextJ < staticBlocks [0].Length && !staticBlocks [nextI] [nextJ]) {
				Vector2Int world = MapManager.getInstance ().CellToWorld (nextI,nextJ);
				return world;
			}
			time++;
		}
		return MapManager.getInstance ().CellToWorld (i,j);;
	}

	public Vector2Int getRandomPosToGo(Vector2Int center){
		Vector2Int posXY = MapManager.getInstance ().WorldToCell (center);
		return getRandomPosToGo (posXY.y,posXY.x);
	}


//	public Vector3Int worldPosToCellPos(Vector3 worldPos){
//		Vector3Int res = obcTilemap.WorldToCell (worldPos);
//		res.y = -res.y;
//		return res;
//
//	}
//
//	public Vector3 cellPosToWorldPos(int i, int j){
//		Vector3 res = obcTilemap.CellToWorld (new Vector3Int(j,-i,0));
//		return res;
//
//	}

	public Vector2Int CellToWorld(int i, int j){
		Vector2Int res = new Vector2Int (j*TILE_WIDTH*10,-i*TILE_HEIGHT*10);
		res += new Vector2Int (TILE_WIDTH * 5, -TILE_HEIGHT * 5);
		return res;
	}

	public Vector2Int CellToWorld(Vector2Int pos){
		return CellToWorld (pos.y, pos.x);
	}

	public Vector2Int WorldToCell(Vector3 worldPos){
		int x = (int)(worldPos.x * 1000 / (TILE_WIDTH * 0.01f));
		int y = (int)(-worldPos.y * 1000 / (TILE_HEIGHT * 0.01f));

		if (x >= 0)
			x = x / 1000;
		else
			x = -(-x / 1000) - 1;

		if (y >= 0)
			y = y / 1000;
		else
			y = -(-y / 1000) - 1;
		
		return new Vector2Int (x,y);
	}

	public Vector2Int WorldToCell(Vector2Int worldPosInt){
		int x = (int)(worldPosInt.x / (TILE_WIDTH * 0.01f));
		int y = (int)(-worldPosInt.y / (TILE_HEIGHT * 0.01f));
		if (x >= 0)
			x = x / 1000;
		else
			x = -(-x / 1000) - 1;

		if (y >= 0)
			y = y / 1000;
		else
			y = -(-y / 1000) - 1;
		
		return new Vector2Int (x,y);
	}

	public Vector2 WorldToCellWithTail(Vector2Int worldPosInt){
		

		int x = (int)(worldPosInt.x / (TILE_WIDTH * 0.01f));
		int y = (int)(-worldPosInt.y / (TILE_HEIGHT * 0.01f));
		return new Vector2 (x*0.001f,y*0.001f);
	}

//	public bool isWorldPosObc(Vector3 worldPos){
//		Vector3Int res = obcTilemap.WorldToCell (worldPos);
//		res.y = -res.y;
//		if (res.y < 0 || res.y >= staticBlocks.Length || res.x < 0 || res.x >= staticBlocks [0].Length)
//			return true;
//		if (staticBlocks [res.y] [res.x])
//			return true;
//		return false;
//	}

	public bool isWorldPosObc(Vector2Int worldPosInt){
		
		Vector3 res = WorldToCellWithTail (worldPosInt);
		if (res.y < 0 || res.y >= staticBlocks.Length || res.x < 0 || res.x >= staticBlocks [0].Length)
			return true;
		if (staticBlocks [(int)res.y] [(int)res.x])
			return true;
		return false;
	}



	public bool isCellObc(Vector3Int cellPos){
		
		if (cellPos.y < 0 || cellPos.y >= staticBlocks.Length || cellPos.x < 0 || cellPos.x >= staticBlocks [0].Length)
			return true;
		if (staticBlocks [cellPos.y] [cellPos.x])
			return true;
		return false;

	}

	public Vector3Int getValidBlinkPos(Vector3 worldBeforePos, Vector2 blinkDir, Rect box){
//		Vector3Int before = obcTilemap.WorldToCell (worldBeforePos);
//		before.y = -before.y;
//		if (before.y < 0 || before.y >= staticBlocks.Length || before.x < 0 || before.x >= staticBlocks [0].Length || staticBlocks [before.y] [before.x]) {
//			return before;
//		}
//
//		if (start.x > end.x) {
//			Vector2 tmp = start;
//			start = end;
//			end = tmp;
//		}
//
//
//		Vector3Int after = obcTilemap.WorldToCell (worldAfterPos);
//		after.y = -after.y;
//		if (after.y < 0 || after.y >= staticBlocks.Length || after.x < 0 || after.x >= staticBlocks [0].Length || staticBlocks [after.y] [after.x]) {
//			
//		} else {
//			return after;
//		
//		}
		return Vector3Int.zero;
	}


	public List<int[]> getRandomSpawnerPos(int amount){
		shuffleSpawnerPos ();

		if (amount > possibleSpawnerPos.Count) {
			return new List<int[]> (possibleSpawnerPos);
		} else {
			return possibleSpawnerPos.GetRange (0,amount);
		}
	}

	public void shuffleSpawnerPos(){
		for (int i = possibleSpawnerPos.Count-1; i > 0; i--) {
			int pos = Random.Range (0,i);
			var x = possibleSpawnerPos[i];
			possibleSpawnerPos[i] = possibleSpawnerPos[pos];
			possibleSpawnerPos[pos] = x;
		}
	}

	public void spawnPlayer(GameLife player){
		Vector2Int center = CellToWorld (playerStartPos[0],playerStartPos[1]);
		player.posXInt = center.x;
		player.posYInt = center.y;


	}
		
}

