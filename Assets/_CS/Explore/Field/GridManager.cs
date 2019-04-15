using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;






public class GridManager : MonoBehaviour {

	public static int GRID_HEIGHT = 7;
	public static int GRID_WIDTH = 7;// guding kuandu 

	public static float GRID_INTERVAL_WIDTH = 3f;
	public static float GRID_INTERVAL_HEIGHT = 3f;

	public PlayerSymbol playerSymbol;
	public GameObject markGO;

	public Transform tr;
	public GameObject gridPrefab;
	ExploreGrid[] grids = new ExploreGrid[GRID_HEIGHT*GRID_WIDTH];

	public GameObject map;


	public GameObject indicatorPrefab;

	// Use this for initialization


	public void initGrid(){




		nowIndex = PlayerData.getInstance().playerPos[1]+ GRID_WIDTH * PlayerData.getInstance().playerPos[0];
		PlayerData.getInstance ().grids [nowIndex / GRID_WIDTH] [nowIndex % GRID_WIDTH].isFinish = true;
		//grids[nowIndex].reveal();
		if (grids != null) {
			for (int i = 0; i < GRID_HEIGHT; i++) {
				for (int j = 0; j < GRID_WIDTH; j++) {
					if(grids[i*GRID_WIDTH+j]!=null)
					GameObject.Destroy (grids[i*GRID_WIDTH+j].gameObject);
				}
			}
		}

		Vector2 picSize = gridPrefab.transform.GetChild (0).GetComponent<SpriteRenderer> ().sprite.bounds.size;
		for (int i = 0; i < GRID_HEIGHT; i++) {
			for (int j = 0; j < GRID_WIDTH; j++) {
				if (PlayerData.getInstance().grids[i][j] != null) {
					GameObject o = GameObject.Instantiate (gridPrefab,tr);
					ExploreGrid exploreGrid = o.GetComponent<ExploreGrid> ();
					o.transform.localPosition = new Vector3 (j*GRID_INTERVAL_WIDTH,-i*GRID_INTERVAL_HEIGHT,0);
					grids[i*GRID_WIDTH+j] = exploreGrid;
					int finalI = i;
					int finalJ = j;
					if (PlayerData.getInstance ().grids [i] [j].isFinish) {
						exploreGrid.reveal ();
					}

					FieldEventlistener tListener = o.AddComponent<FieldEventlistener> ();
					tListener.ClickEvent += delegate(GameObject gb,Vector3 clickpos) {
						chooseEncounter(finalI * GRID_WIDTH + finalJ);
					};

				}
			}
		}


		playerSymbol.transform.localPosition = new Vector3 (PlayerData.getInstance().playerPos[1]*GRID_INTERVAL_WIDTH,-PlayerData.getInstance().playerPos[0]*GRID_INTERVAL_HEIGHT,0);
		markGO.transform.localPosition = playerSymbol.transform.localPosition;

		initCameraControl ();
		SetMap ();

		Vector3 startPos = playerSymbol.transform.position;
		//startPos.x = 0;
		startPos.z = mainCamera.transform.position.z;
		{
			if (startPos.y < (cameraBound [1] + cameraHalfHeight)) {
				startPos.y = (cameraBound [1] + cameraHalfHeight);
			}
			if (startPos.y > (cameraBound [3] - cameraHalfHeight)) {
				startPos.y = (cameraBound [3] - cameraHalfHeight);
			}
			if (startPos.x < (cameraBound [0] + cameraHalfWidth)) {
				startPos.x = (cameraBound [0] + cameraHalfWidth);
			}
			if (startPos.x > (cameraBound [2] - cameraHalfWidth)) {
				startPos.x = (cameraBound [2] - cameraHalfWidth);
			}
		}
		mainCamera.transform.position = startPos;


	}

	// Update is called once per frame
	void Update () {
		
	}
	void LateUpdate(){

		//cameraHalfHeight = mainCamera.pixelHeight * 0.5f / 100f;
		//cameraHalfWidth = mainCamera.pixelWidth * 0.5f / 100f;
		MoveMap ();




	}

	public static float MAX_MAP_SPEED = 20f;
	void MoveMap ()
	{
		if(!isMovingMap && !isContinueMovingMap)
			return;
		
		if (toMove.y < (cameraBound [1] + cameraHalfHeight)) {
			toMove.y = (cameraBound [1] + cameraHalfHeight);
		}
		if (toMove.y > (cameraBound [3] - cameraHalfHeight)) {
			toMove.y = (cameraBound [3] - cameraHalfHeight);
		}
		if (toMove.x < (cameraBound [0] + cameraHalfWidth)) {
			toMove.x = (cameraBound [0] + cameraHalfWidth);
		}
		if (toMove.x > (cameraBound [2] - cameraHalfWidth)) {
			toMove.x = (cameraBound [2] - cameraHalfWidth);
		}
		toMove.z = mainCamera.transform.localPosition.z;
		if ((toMove - mainCamera.transform.localPosition).magnitude < 0.01f) {
			mainCamera.transform.localPosition = toMove;
		} else {
			mainCamera.transform.localPosition = Vector3.Lerp (mainCamera.transform.localPosition,toMove,0.5f);
		}

//		Vector3 moveDir = toMove - mainCamera.transform.localPosition;moveDir.z = 0;
//		Vector3 newPos;
//
//		if (moveDir.magnitude < Time.deltaTime * MAX_MAP_SPEED) {
//			newPos = toMove;
//			if (isContinueMovingMap) {
//				isContinueMovingMap = false;
//			}
//		} else {
//			if (isContinueMovingMap) {
//				newPos = mainCamera.transform.localPosition + moveDir.normalized * Time.deltaTime * MAX_MAP_SPEED;
//			
//			} else {
//				newPos = mainCamera.transform.localPosition + moveDir.normalized * Time.deltaTime * MAX_MAP_SPEED;
//			}
//
//		}
//		Debug.Log (newPos);
//
//
//		if (newPos.y < (cameraBound [1] + cameraHalfHeight)) {
//			newPos.y = (cameraBound [1] + cameraHalfHeight);
//		}
//		if (newPos.y > (cameraBound [3] - cameraHalfHeight)) {
//			newPos.y = (cameraBound [3] - cameraHalfHeight);
//		}
		//Debug.Log (newPos);
		//mainCamera.transform.localPosition = newPos;
	}


	bool isMovingMap = false;
	bool isContinueMovingMap = false;
	Vector3 toMove = Vector3.zero;


	public void moveMap(Vector3 dragDir){
		
		Vector3 moveDir = dragDir/15f; //blend
		moveDir.z = 0;
		toMove = mainCamera.transform.localPosition - moveDir;
		//toMove = Input.mousePosition;

	}

	public void SetMap(){
		FieldEventlistener mapListener = map.AddComponent<FieldEventlistener> ();
		mapListener.BeginDragEvent += delegate(GameObject gb,Vector3 dragDir) {
			isMovingMap = true;
			isContinueMovingMap = false;
			toMove = mainCamera.transform.localPosition;
		};

		mapListener.OnDragEvent += delegate(GameObject gb,Vector3 dragDir) {
			moveMap(dragDir);
		};
		mapListener.EndDragEvent += delegate(GameObject gb,Vector3 dragDir) {
			isContinueMovingMap = true;
			isMovingMap = false;
		};
	}

	float[] cameraBound = new float[4];
	float cameraHalfHeight;
	float cameraHalfWidth;

	SpriteRenderer activeArea;

	public Camera mainCamera;

	int markedIndex;
	int nowIndex;
	bool isMoving = false;

	int[,] dir = new int[,]{{-1,0},{1,0},{0,1},{0,-1}};

	public void chooseEncounter(int index){
		int iIndex = index/GRID_WIDTH;
		int jIndex = index%GRID_WIDTH;
		if (iIndex < 0 || iIndex>= GRID_HEIGHT || jIndex<0 || jIndex >= GRID_WIDTH) {
			return;
		}
		if (isMoving)
			return;

		if (markedIndex != index) {
			StartCoroutine (moveMark(index));
		} else {
			if (nowIndex == index) {
				GameManager.getInstance ().hideDetailPanel ();
				return;
			}
			int nowI = nowIndex/GRID_WIDTH;
			int nowJ = nowIndex%GRID_WIDTH;
			for (int i = 0; i < 4; i++) {
				int nextI = nowI + dir [i,0];
				int nextJ = nowJ + dir [i,1];
				if (nextI >= 0 && nextI < GRID_HEIGHT && nextJ >= 0 && nextJ < GRID_WIDTH && PlayerData.getInstance().grids[nextI][nextJ] !=null && (nextI == iIndex && nextJ == jIndex)) {
					PlayerData.getInstance ().playerPos = new Vector2Int(nextI,nextJ);
					StartCoroutine (movingPlayer(index));
				}
			}
		}

	}

	IEnumerator moveMark(int index){




		GameManager.getInstance ().hideDetailPanel ();

		isMoving = true;
		markGO.SetActive (true);
		Vector3 target = new Vector3 ((index%GRID_WIDTH)*GRID_INTERVAL_WIDTH,-(index/GRID_WIDTH)*GRID_INTERVAL_HEIGHT,0);
		//Vector3 target = encounterGo [bg[totalPattern[index][0]][totalPattern[index][1]]].transform.localPosition;
		while (true) {
			Vector3 diff = (target - markGO.transform.localPosition);
			if (diff.magnitude < Time.deltaTime * 8f) {
				markGO.transform.localPosition = target;
				break;
			} else {
				markGO.transform.localPosition = Vector3.Lerp (markGO.transform.localPosition,target,0.15f);
				//markGO.transform.localPosition += dir.normalized * Time.deltaTime * 8f;
			}
			yield return null;
		}
		isMoving = false;

		if (PlayerData.getInstance ().guideStage == 0) {
			PlayerData.getInstance ().guideStage = 1;
			GuideManager.getInstance ().showGuideMovePlayer (getToturialGridPos ());
		}

		markedIndex = index;
		changeEncounterDetail (markedIndex);
	}

	IEnumerator movingPlayer(int index){
		GameManager.getInstance().hideDetailPanel ();
		isMoving = true;
		Vector3 target = new Vector3 ((index%GRID_WIDTH)*GRID_INTERVAL_WIDTH,-(index/GRID_WIDTH)*GRID_INTERVAL_HEIGHT,0);

		if (PlayerData.getInstance ().guideStage == 1) {
			PlayerData.getInstance ().guideStage = 2;
			GuideManager.getInstance ().hideGuide ();
		}

		while (true) {
			Vector3 dir = (target - playerSymbol.transform.localPosition);
			if (dir.magnitude < Time.deltaTime * 3f) {
				playerSymbol.transform.localPosition = target;
				break;
			} else {
				playerSymbol.transform.localPosition = Vector3.Lerp (playerSymbol.transform.localPosition,target,0.15f);
				//playerSymbol.transform.localPosition += dir.normalized * Time.deltaTime * 3f;
			}
			{
				Vector3 startPos = playerSymbol.transform.position;
				startPos.x = 0;
				startPos.z = mainCamera.transform.position.z;
				isMovingMap = true;
				toMove = startPos;
			}
			yield return null;
		}
		markGO.SetActive (false);



		isMoving = false;
		nowIndex = index;
		triggerEncounter (index);

	}


	public void triggerEncounter(int index){
//		if (PlayerData.getInstance().bg[index].isFinished) {
//			return;
//		}
		EncounterState es = PlayerData.getInstance ().grids [index / GRID_WIDTH] [index % GRID_WIDTH];

		if (es.isFinish) {
			return;
		}

		//Debug.Log (PlayerData.getInstance ().grids [index / GRID_WIDTH] [index % GRID_WIDTH].eid);
		if (es.eid == "shop") {
			GameManager.getInstance ().enterShop ();
		} else {
			GameManager.getInstance ().sbManager.initEncounter(es.eid);
		}

		PlayerData.getInstance ().grids [index / GRID_WIDTH] [index % GRID_WIDTH].isFinish = true;
		grids[index].reveal();
	}

	public void changeEncounterDetail(int index){
		if (index < 0 || index > grids.Length) {
			return;
		}
		EncounterState encounterState = PlayerData.getInstance ().grids [index / GRID_WIDTH] [index % GRID_WIDTH];
		string eid = encounterState.eid;
		EncounterInfo einfo = new EncounterInfo ();
		if (eid != "") {
			einfo = GameStaticData.getInstance ().getEncounterInfo (eid);
		}
		GameManager.getInstance().showDetailPanel (einfo.desp);
		//detailText.text = eList [index].eid + "号遭遇";
	}

	public void initCameraControl(){
		SpriteRenderer activeArea = map.GetComponent<SpriteRenderer> ();
		cameraBound[0] = -activeArea.bounds.size.x/2;
		//cameraBound [0] = mainCamera.WorldToScreenPoint;
		cameraBound[1] = -activeArea.bounds.size.y/2;

		cameraBound[2] = activeArea.bounds.size.x/2;
		cameraBound[3] = activeArea.bounds.size.y/2;

		Vector2 cameraBoundInWorld = mainCamera.ScreenToWorldPoint (new Vector3 (mainCamera.pixelWidth, mainCamera.pixelHeight, 0));

		//cameraHalfHeight = mainCamera.pixelHeight / 200f;
		//cameraHalfWidth = mainCamera.pixelWidth / 200f;
		cameraHalfHeight = cameraBoundInWorld.y;
		cameraHalfWidth = cameraBoundInWorld.x;

		//Debug.Log(new Vector3 (mainCamera.pixelWidth, mainCamera.pixelHeight, 0));
		//Debug.Log(mainCamera.ScreenToWorldPoint (new Vector3 (0, 0, 0)));
		//Debug.Log(mainCamera.ScreenToWorldPoint (new Vector3 (0, 0, 10)));
		//Debug.Log(mainCamera.ScreenToWorldPoint (new Vector3 (mainCamera.pixelWidth, mainCamera.pixelHeight, 0)));

		//Debug.Log ("height:" + buildCamera.pixelHeight + ";" + "wid" + buildCamera.pixelWidth);

	}
		

	public Vector2 getToturialGridPos(){
		return Camera.main.WorldToScreenPoint (grids[15].transform.position);
	}
}
