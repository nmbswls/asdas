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

	public List<GameLife> enemies = new List<GameLife>();
	private List<Tower> towers = new List<Tower>();

	public PlayerModule player;

	public MainUIManager mainUIManager;
	public MapItemManager mapItemManager;

	public MonsterDropManager dropManager;

	public PlayerSkillManager playerSkillManager;

	public Potion[] potionInBattle = new Potion[3];

	public int killLeft = 10;
	int hp;
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
		mapItemManager = GetComponent<MapItemManager> ();
		dropManager = GetComponent<MonsterDropManager> ();
		spawnPlayer ();
		MapManager.getInstance ().Init ();
		initBattle ();
	}

	public void spawnEnemy(string enemyId,Vector3 pos){
		GameLife enemy = MonsterFactory.createEnemy (enemyId,pos,mapObjLayer);
		enemy.OnDieCallback += delegate() {
			enemyDie(enemy);
		};
	}

	public void initBattle(){
		//怪物由两部分组成，背景怪物 无限刷新 固定怪物 由怪物卡召唤


		{
			List<EnemyCombo> enemies = PlayerData.getInstance ().chasingEnemies;
			List<int[]> spawners = MapManager.getInstance ().getRandomSpawnerPos (enemies.Count);

			for (int i = 0; i < spawners.Count; i++) {
				Vector3 center = MapManager.getInstance ().cellPosToWorldPos (spawners[i][0],spawners[i][1]);
				spawnEnemy(enemies [i].enemyId [0],center);
			}
			PlayerData.getInstance ().chasingEnemies.Clear ();
		}

		{
			List<int[]> spawners = MapManager.getInstance ().getRandomSpawnerPos (3);
			for (int i = 0; i < 3; i++) {
				Vector3 center = MapManager.getInstance ().cellPosToWorldPos (spawners[i][0],spawners[i][1]);
				GameObject o = GameObject.Instantiate(spawnerPrefab,center,Quaternion.identity,spawnerLayer);
				EnemySpawner spanwer = o.GetComponent<EnemySpawner> ();
				enemySpawners.Add (spanwer);
				spanwer.enemyName="10005";
			}
		}

			


		for (int i = 0; i < 3; i++) {
			if(i>=PlayerData.getInstance ().potions.Count)break;
			potionInBattle[i] = PlayerData.getInstance ().potions [i];
		}

		buildableTowers = PlayerData.getInstance ().ownedTowers;
		money = new int[4];
		money[0] = 20;
		money[1] = 20;
		money[2] = 20;
		money[3] = 20;

		killLeft = 10;
		hp = PlayerData.getInstance().hp;
		maxHP = PlayerData.getInstance ().maxHP;
		san = 40;
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
//	void Awake(){
//		checkGameObject ();
//		checkSingle ();
//	}


	public void spawnPlayer(){
		GameObject o = GameObject.Instantiate (playerPrefab,mapObjLayer);
		o.transform.position = new Vector3 (1f, -1f, 0);
		player = o.GetComponent<PlayerModule> ();
		battleMainCamera.GetComponent<TargetFollower> ().Target = o.transform;
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
		mainUIManager._enemy_left.text = (killLeft > 0 ? killLeft : 0)+"";
		if (killLeft <= 0) {
			showBattleFinish (true);
		}
	}

	bool isShowingBattleFinishPanel = false;
	void showBattleFinish(bool isWin){
		if (isShowingBattleFinishPanel)
			return;
		Time.timeScale = 0;
		isShowingBattleFinishPanel = true;
		mainUIManager.showBattleFinishPanel ();
	}

	public void battleFinish(){
		if (battleWillFinish)
			return;

		battleWillFinish = true;
		battleMainCamera.gameObject.SetActive (false);
		mainUIManager._mainView.visible = false;
		PlayerData.getInstance ().finishBattle (true);
		StartCoroutine (UnloadBattle());
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
		mainUIManager._coins.text = money[0]+"";
	}

	public void unlockAllEnemies(){
		
	}

	private AsyncOperation async = null;
	IEnumerator UnloadBattle()
	{
		//GameManager.getInstance ().finishBattle();
		async = SceneManager.LoadSceneAsync("Explore");
		yield return async;

	}


	void Update(){
		if (Input.GetKeyDown ("z")) {
			showBattleFinish (true);
		}
		if (Input.GetKeyDown ("x")) {
			Time.timeScale = Time.timeScale==0?1:0;
		}
		if (Input.GetKeyDown ("c")) {
			dropManager.createDrops (new List<int>(),player.transform.position+Vector3.right*3f);
		}
		int timeInt = (int)(Time.deltaTime * 1000f);

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


}
