using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;

public class BattleManager : Singleton<BattleManager> {

	public GameObject playerPrefab;
	public GameObject spawnerPrefab;
	public Transform spawnerLayer;

	public Camera battleMainCamera;
	public Transform enemySpawnerLayer;
	public Transform mapObjLayer;

	public List<EnemySpawner> enemySpawners = new List<EnemySpawner> ();

	List<GameLife> enemies = new List<GameLife>();
	private List<Tower> towers = new List<Tower>();

	public PlayerModule player;

	public MainUIManager mainUIManager;
	public MapItemManager mapItemManager;

	public MonsterDropManager dropManager;

	public PlayerSkillManager playerSkillManager;

	public Potion[] potionInBattle = new Potion[3];

	public int battleTime = 0;

	public int killLeft = 10;
	public int hp;
	public int maxHP;
	public int san;
	public int defend;
	bool battleWillFinish = false;

	public int[] money = new int[4];

	public List<TowerTemplate> buildableTowers = new List<TowerTemplate> ();

	public int Hp {
		get {
			return this.hp;
		}
		set {
			if (hp != value) {
				hp = value;
				mainUIManager.updateHp ();
			}
		}
	}

	public void getDamaged(int amount){
		Hp -= amount;
		PlayerData.getInstance ().hp = Hp;
		if (Hp <= 0) {
			showBattleFinish (false);
			PlayerData.getInstance ().hp = 20;
		}
	}
	void Start(){

		initStatus ();
		mainUIManager.Init ();
		lockUI ();

		mapItemManager = GetComponent<MapItemManager> ();
		dropManager = GetComponent<MonsterDropManager> ();


		MapManager.getInstance ().Init ();
		spawnPlayer ();
		initEnemySpawner ();
		initBattle ();

		mainUIManager.updateHp ();
		mainUIManager.updatePotions ();
		mainUIManager.updateCoin ();
		mainUIManager.updateLeftEnemy ();
		StartCoroutine (fadeIn());
		unlockUI ();

	}

	void initStatus(){
		buildableTowers = PlayerData.getInstance ().ownedTowers;
		money = new int[4];
		money[0] = 200;
		money[1] = 10;
		money[2] = 10;
		money[3] = 10;


		hp = PlayerData.getInstance().hp;
		maxHP = PlayerData.getInstance ().maxHP;
		san = 40;
	}

	IEnumerator fadeIn(){
		pause ();
		yield return new WaitForSecondsRealtime (1.5f);
		unPause ();
		if (PlayerPrefs.GetInt ("isFirstBattle", 1) == 1) {
			GuideManager.getInstance ().showGuidePanel ();
		}
	}

	public void spawnEnemy(string enemyId,Vector2Int pos){
		GameLife enemy = MonsterFactory.createEnemy (enemyId,pos,mapObjLayer);
		enemy.OnDieCallback += delegate() {
			enemyDie(enemy);
		};
	}

	void initEnemySpawner(){
		List<EnemyCombo> enemies = PlayerData.getInstance ().chasingEnemies;
		{
			List<int[]> spawners = MapManager.getInstance ().getRandomSpawnerPos (enemies.Count);

			for (int i = 0; i < spawners.Count; i++) {
				Vector2Int center = MapManager.getInstance ().CellToWorld (spawners [i] [0], spawners [i] [1]);
				spawnEnemy (enemies [i].enemyId [0], center);
			}
			PlayerData.getInstance ().chasingEnemies.Clear ();
		}
		if (PlayerPrefs.GetInt ("isFirstBattle", 1) == 1) {
			//first time
			{
				List<int[]> spawners = MapManager.getInstance ().getRandomSpawnerPos (1);

				Vector2Int center = MapManager.getInstance ().CellToWorld (spawners[0][0],spawners[0][1]);
				GameObject o = GameObject.Instantiate(spawnerPrefab,spawnerLayer);
				EnemySpawner spanwer = o.GetComponent<EnemySpawner> ();
				spanwer.posXInt = center.x;
				spanwer.posYInt = center.y;
				enemySpawners.Add (spanwer);
				spanwer.enemyName="toturial";
			}

		} else {
			{

				List<int[]> spawners = MapManager.getInstance ().getRandomSpawnerPos (3);
				for (int i = 0; i < spawners.Count; i++) {
					Vector2Int center = MapManager.getInstance ().CellToWorld (spawners[i][0],spawners[i][1]);
					GameObject o = GameObject.Instantiate(spawnerPrefab,spawnerLayer);
					EnemySpawner spanwer = o.GetComponent<EnemySpawner> ();
					spanwer.posXInt = center.x;
					spanwer.posYInt = center.y;
					enemySpawners.Add (spanwer);
					spanwer.enemyName="10005";
				}
			}
		}
	}
	public void initBattle(){
		//怪物由两部分组成，背景怪物 无限刷新 固定怪物 由怪物卡召唤
		killLeft = 10;

		{
			EncounterBattleInfo binfo = PlayerData.getInstance ().battleInfo;
			if (binfo == null) {
				binfo = new EncounterBattleInfo ();
			}
			if (binfo.liveTime > 0) {
			
			}
			if (binfo.killEnemy > 0) {
				killLeft = binfo.killEnemy;
			} else {
				killLeft = -1000;
			}
		}
		//PlayerPrefs.DeleteAll ();

		for (int i = 0; i < 3; i++) {
			if(i>=PlayerData.getInstance ().potions.Count)break;
			potionInBattle[i] = PlayerData.getInstance ().potions [i];
		}
		//if(BattleManager)
	}

	GameLife highlightEnemy = null;
	float highlightTime = 0f;
	public void placeHPOnTopLayer(GameLife enemy){
		if (highlightEnemy != null) {
			highlightEnemy.isHighLight = false;
		}
		highlightEnemy = enemy;
		enemy.isHighLight = true;

	}

	public void checkCancelHighLight(float dtime){
		if (highlightEnemy == null)
			return;
		highlightTime += dtime;
		if (dtime > 0.3f) {
			if (highlightEnemy != null) {
				highlightEnemy.isHighLight = false;
			}
		}
	}

//	public static BattleManager getInstance(){
//		return instance;
//	}
//



	public void spawnPlayer(){
		


		GameObject o = GameObject.Instantiate (playerPrefab,mapObjLayer);
		player = o.GetComponent<PlayerModule> ();

		battleMainCamera.GetComponent<TargetFollower> ().Target = o.transform;

		MapManager.getInstance ().spawnPlayer (player.gl);
		player.gl.updatePosImmediately ();

		string heroName = "hero"+((PlayerData.getInstance().heroIdx+1)+"").PadLeft (2,'0');
		AnimatorOverrideController animCtrl =  Resources.Load ("OverrideAnimCtrl/player/"+heroName) as AnimatorOverrideController;
		o.GetComponentInChildren<Animator>().runtimeAnimatorController = animCtrl;

		battleMainCamera.transform.position = new Vector3(player.transform.position.x,player.transform.position.y,battleMainCamera.transform.position.z);
	}


	public void enemyDie(GameLife toDie){
		enemies.Remove(toDie);
		List<int> dropss = new List<int> ();
		dropss.Add (10);
		dropss.Add (3);
		dropss.Add (10);
		dropss.Add (3);
		dropManager.createDrops (dropss,toDie.transform.position);
		killLeft--;
		mainUIManager.updateLeftEnemy ();
		if (killLeft>-1000&&killLeft <= 0) {
			showBattleFinish (true);
		}
	}

	bool isShowingBattleFinishPanel = false;
	void showBattleFinish(bool isWin){
		if (isShowingBattleFinishPanel)
			return;
		pause ();
		isShowingBattleFinishPanel = true;
		mainUIManager.showBattleFinishPanel ();
	}


	//	private AsyncOperation async = null;
	//	IEnumerator UnloadBattle()
	//	{
	//		//GameManager.getInstance ().finishBattle();
	//		async = SceneManager.LoadSceneAsync("Explore");
	//		yield return async;
	//
	//	}
	public void battleFinish(){
		if (battleWillFinish)
			return;

		battleWillFinish = true;
		battleMainCamera.gameObject.SetActive (false);
		mainUIManager._mainView.visible = false;
		PlayerData.getInstance ().finishBattle (true);
		unPause ();
		Load.SceneName = "Explore";//B场景的名字 
		SceneManager.LoadScene("Loading"); 
		//StartCoroutine (UnloadBattle());
	}
		

	public void addTower(Tower t){
		towers.Add (t);
		MapManager.getInstance ().updateBlock ();
	}

	public void removeTower(Tower t){
		towers.Remove (t);
		MapManager.getInstance ().updateBlock ();
	}

	public List<Tower> getAllTower(){
		return towers;
	}



	public PlayerModule getPlayer(){
		return player;
	}

	public void lockAllEnemies(){
		
	}

	public void gainCoin(int num){
		money[0] += num;
		mainUIManager.updateCoin ();
	}

	public void unlockAllEnemies(){
		
	}




	void Update(){
		int timeInt = (int)(Time.deltaTime * 1000f);
		battleTime += timeInt;
		foreach (EnemySpawner spawner in enemySpawners) {
			spawner.Tick (timeInt,battleTime);
		}
		if (Input.GetKeyDown ("z")) {
			showBattleFinish (true);
		}

		if (Input.GetKeyDown ("c")) {
			dropManager.createDrops (new List<int>(),player.transform.position+Vector3.right*3f);
		}

		playerSkillManager.Tick (timeInt);
		dropManager.Tick (timeInt);

		checkCancelHighLight (Time.deltaTime);
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			if (!UILayer.activeSelf) {
//				UILayer.SetActive (true);
//				Time.timeScale = 0.0f;
//				playerAC.lockActor ();
//				lockAllEnemies ();
//			} else {
//				UILayer.SetActive (false);
//				Time.timeScale = 1.0f;
//				playerAC.unlockActor ();
//				unlockAllEnemies ();
//			}
//
//		}
	}
//	private void checkGameObject(){
//		if (tag == "GM") {
//			return;
//		}
//		Destroy (this);
//	}
//
//	private void checkSingle(){
//		if (instance == null) {
//			instance = this;
//			DontDestroyOnLoad (gameObject);
//			return;
//		}
//		Destroy (this);
//	}


	public bool useItem(int idx){
		if (idx < 0 || idx >= 3)
			return false;
		if (potionInBattle [idx] == null)
			return false;
		
		bool res = PlayerData.getInstance ().usePotion (potionInBattle [idx]);
		Debug.Log ("gain hp ");
		potionInBattle [idx] = null;
		mainUIManager.updatePotions ();
		return res;
	}

	int uiLockNum = 0;


	public List<GameLife> getTmpEnemyList(){
		return new List<GameLife>(enemies);
	}

	public void registerEnemy(GameLife newEnemy){
		if(!enemies.Contains(newEnemy))enemies.Add (newEnemy);
	}



	public void lockUI(){
		uiLockNum++;
		if (uiLockNum > 0) {
			GRoot.inst.touchable = false;
			mainUIManager._mainView.touchable = false;
		}
	}

	public void unlockUI(){
		uiLockNum--;
		if (uiLockNum == 0) {
			GRoot.inst.touchable = true;
			mainUIManager._mainView.touchable = true;
		}
	}

	int pauseCount = 0;
	public void pause(){
		pauseCount++;
		if (pauseCount > 0) {
			Time.timeScale = 0;
		}
	}

	public void unPause(){
		pauseCount--;
		if (pauseCount == 0) {
			Time.timeScale = 1;
		}
	}
}
