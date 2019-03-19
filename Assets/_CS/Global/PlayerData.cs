using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using Newtonsoft.Json;



public class TowerTemplate{
	public TowerBase tbase;
	public TowerComponent[] components = new TowerComponent[5];
}

[System.Serializable]
public class TowerBase{
	public string tid;
	public string tname;
	public string tdesp;
	public eAtkType atkType = eAtkType.MELLE_POINT;
	public int damage = 10000;
	public int mingzhong;
	public int atkInteval = 2000;
	public int atkRange = 3000;
	public int atkPreanimTime = 300;
	public eProperty property = eProperty.NONE;
	public List<TowerSkillState> skills = new List<TowerSkillState>();
}

[System.Serializable]
public class TowerSkill{
	public int skillId;
	public int maxLv;
	public int cooldown = 20000;
	public bool isPassive = false;
	public bool isPermanent = false;
	public ePassiveCheckPoint checkPoint = ePassiveCheckPoint.ATK;
	public List<int> x;
	public List<int> y;
	public List<int> z;
	public TowerSkill(){
	}
	public TowerSkill(int skillId){
		this.skillId = skillId;
	}
}

[System.Serializable]
public class TowerSkillState{
	public int skillId;
	public int skillLevel;
	public TowerSkillState(){
	}
	public TowerSkillState(int skillId,int skillLevel){
		this.skillId = skillId;
		this.skillLevel = skillLevel;
	}
}


[System.Serializable]
public class TowerComponent{
	public string cid = "0";
	public string cname = "default";
	public string cdesp = "default";
	public List<TowerComponentEffect> effects = new List<TowerComponentEffect>();
}

[System.Serializable]
public class TowerComponentEffect{
	public eTowerComponentEffectType type;
	public int bonus;
	public string extra;
}

[System.Serializable]
public enum eTowerComponentEffectType{
	ATK_CHANGE = 0,
	ATK_RANGE_CHANGE = 1,
	ATK_PROPERTY_CHANGE = 2,
	ATK_INTERVAL_CHANGE = 3,
	CRIT_CHANGE = 4,
	EXTRA_ABILITY = 5
}

public class TowerComponentInList{
	public string cid;
	public int slot = -1;
}

public class Potion{
	public string pname;
}

public class Scar{
	public string scarName;
	public int type;
	public int v;
	public Scar(){
	}

	public Scar(string scarName){
		this.scarName = scarName;
	}
}

public class PlayerData
{

	private static PlayerData instance;

	public static PlayerData getInstance(){
		if (instance == null) {
			instance = new PlayerData ();
		}
		return instance;
	}


	public int heroIdx = 0;
	public int[] info;

	public EncounterState[][] grids;
	public int[] playerPos = new int[]{2,2};




	public bool isWaitingBattle = false;
	public int beforeStage = 1;
	public string beforeEid = "";
	public bool battleWin;
	public bool isFixedBattle = false;


	//数据

	public int hp = 20;
	public int maxHP = 40;
	public int san = 2000;
	public int money = 200;
	public int level = 15;

	public List<Potion> potions = new List<Potion>();
	public List<Scar> scars = new List<Scar>();

	public List<int> heroSkills = new List<int>();

	public List<TowerTemplate> ownedTowers = new List<TowerTemplate>();
	public List<TowerComponent> bagComponents = new List<TowerComponent> ();

	public List<ChasingEnemyAbstract> chasingEnemies = new List<ChasingEnemyAbstract>();

	public List<string> ownedMemos = new List<string> ();

	public Dictionary<string,int> memoState = new Dictionary<string,int>();
	// Use this for initialization
	private PlayerData ()
	{

		info = new int[10];


		generateDungeon (20);
		initTowers ();
		initBag ();
		intPotions ();
		initScar ();
		initMemoState ();
	}

	public void generateDungeon(int encounterNum){
		List<string> toChooseFrom = new List<string> ();
		foreach(var item in GameStaticData.getInstance().encounterDic){
			toChooseFrom.Add (item.Key);
		}

		int totalLeft = GridManager.GRID_HEIGHT * GridManager.GRID_WIDTH;
		int encounterLeft = encounterNum;
		if (encounterNum > totalLeft) {
			encounterLeft = totalLeft;
		}
		grids = new EncounterState[GridManager.GRID_HEIGHT][];
		for (int i = 0; i < GridManager.GRID_HEIGHT; i++) {
			grids[i] = new EncounterState[GridManager.GRID_WIDTH];
			for (int j = 0; j < GridManager.GRID_WIDTH; j++) {
				int ranInt = Random.Range (0,totalLeft);
				if (ranInt < encounterLeft) {
					totalLeft--;
					encounterLeft --;
					grids [i] [j] = new EncounterState (toChooseFrom[Random.Range(0,toChooseFrom.Count)]);
				} else {
					totalLeft--;
					grids [i] [j] = new EncounterState ();
				}
			}
		}
		grids [2] [3] = new EncounterState ("shop");
		playerPos = new int[]{2,2};
	}
	
	public void initPlayerData(){
		
	}



	public void initBattle(string eid,int stageIdx){
		isWaitingBattle = true;
		beforeEid = eid;
		beforeStage = stageIdx;
		isFixedBattle = true;
	}

	public void initBattle(){
		isWaitingBattle = true;
		isFixedBattle = false;
	}

	public void finishBattle(bool win){
		battleWin = win;
	}


	string[] names = new string[]{"贪欲之壶","布袋熊","三年高考","针筒","仪式剪纸","Gameboy","钢笔","奥特人偶"};
	string[] desps = new string[]{"禁忌的壶，拥有吐纳天地的无限容量","模样可爱？","我们都拥有过的东西","童年噩梦","它会动吗？","痛击你的敌人！","除了装逼还有别的用处","变身！"};

	public void initTowers(){
		for (int i = 1; i < 8; i++) {
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = null;
			GameStaticData.getInstance ().towerBaseInfo.TryGetValue ("t0"+i,out tb);
			if(tb==null)tb=GameStaticData.getInstance ().towerBaseInfo["default"];
			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
	}
	public void initBag(){
		for (int i = 0; i < 20; i++) {
			
			TowerComponent tc = null;
			GameStaticData.getInstance().componentStaticInfo.TryGetValue(i%3+"",out tc);
			if (tc == null) {
				tc = GameStaticData.getInstance().componentStaticInfo["default"];
			}
			bagComponents.Add (tc);
		}
	}

	public bool usePotion(Potion toUse){
		
		return potions.Remove (toUse);
	}


	public void gainPotion(){
	
	}

	void intPotions(){
		for (int i = 0; i < 8; i++) {
			potions.Add(new Potion());
		}
	}

	void initScar(){
		for (int i = 0; i < 3; i++) {
			scars.Add(new Scar("受伤"));
		}
	}

	public void gainComponent(string componentId){
		TowerComponent tc = null;
		if (GameStaticData.getInstance ().componentStaticInfo.TryGetValue (componentId, out tc)) {
			bagComponents.Add (tc);
		} else {
			bagComponents.Add (GameStaticData.getInstance ().componentStaticInfo["0"]);
		}
	}

	public void addMonster(ChasingEnemyAbstract enemyAbstract){
		chasingEnemies.Add (enemyAbstract);
	}

	public TowerTemplate getTowerTemplate(int idx){
		if (idx < 0 || idx >= ownedTowers.Count) {
			return null;
		}
		return ownedTowers [idx];
	}

	public void initMemoState(){
		foreach(var kv in GameStaticData.getInstance().memoInfo){
			if(kv.Key!="default"&&int.Parse(kv.Key)<10)
				memoState [kv.Key] = 1;
		}
	}

	public void removeScar(Scar toRemove){
		scars.Remove (toRemove);
	}
}

