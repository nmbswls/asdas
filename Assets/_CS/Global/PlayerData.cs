using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using Newtonsoft.Json;



public class TowerTemplate{
	public TowerBase tbase;
	public TowerComponent[] components = new TowerComponent[5];
}

public class AtkInfo{
	public int damage = 1000;
	public eProperty property = eProperty.NONE;

	public AtkInfo(){
	}

	public AtkInfo(AtkInfo other){
		this.damage = other.damage;
		this.property = other.property;
	}
	public AtkInfo(int property,int damage){
		this.damage = damage;
		this.property = (eProperty)property;
	}
}

[System.Serializable]
public class TowerBase{
	public string tid;
	public string tname;
	public string tdesp;
	public eAtkType atkType = eAtkType.MELLE_POINT;
	public AtkInfo mainAtk = new AtkInfo();
	public List<AtkInfo> extraAtk = new List<AtkInfo>();
	public int mingzhong;
	public int atkInteval = 2000;
	public int atkRange = 3000;
	public int atkPreanimTime = 300;
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
	public string extra;
	public int x;
	public int y;
	public int z;
}

[System.Serializable]
public enum eTowerComponentEffectType{
	ATK_CHANGE = 0,
	ATK_RANGE_CHANGE = 1,
	EXTRA_ATK = 2,
	ATK_SPD_CHANGE = 3,
	CRIT_CHANGE = 4,
	EXTRA_ABILITY = 5,
}

public class TowerComponentInList{
	public string cid;
	public int slot = -1;
}

public class Potion{
	public string pid;
	public string pname;

	public Potion(){
	}

	public Potion(string pid){
		this.pid = pid;
	}
}

public class Scar{
	public string scarId;
	public int value;

	public Scar(){
		
	}

	public Scar(string scarId,int value){
		this.scarId = scarId;
		this.value = value;
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
	public List<EnemyCombo> fixedMonsters;

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

	public List<EnemyCombo> chasingEnemies = new List<EnemyCombo>();

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
		toChooseFrom.Add ("20");
		toChooseFrom.Add ("0");
//		foreach(var item in GameStaticData.getInstance().encounterDic){
//			toChooseFrom.Add (item.Key);
//		}

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
		fixedMonsters = new List<EnemyCombo> ();
	}

	public void initBattle(){
		isWaitingBattle = true;
		isFixedBattle = false;
		fixedMonsters = new List<EnemyCombo> ();
	}

	public void finishBattle(bool win){
		battleWin = win;
	}



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
		for (int i = 0; i < 5; i++) {
			potions.Add(new Potion("0"));
		}
	}

	void initScar(){
		for (int i = 0; i < 3; i++) {
			scars.Add(new Scar("0",2+i));
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

	public void addMonster(EnemyCombo enemyAbstract){
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


	public void getPossibleEnemyCombo(){
		foreach (var kv in GameStaticData.getInstance().enemyCombos) {
			
		}
	}




}

