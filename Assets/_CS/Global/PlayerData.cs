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

	public string bulletStyle = "default";
	public int[] cost = new int[3];

	public eAtkType atkType = eAtkType.MELLE_POINT;
	public AtkInfo mainAtk = new AtkInfo();
	public List<AtkInfo> extraAtk = new List<AtkInfo>();
	public int mingzhong;
	public int atkInteval = 2000;
	public int atkRange = 3000;
	public int atkPreanimTime = 300;
	public List<SkillState> skills = new List<SkillState>();
}

[System.Serializable]
public class TowerSkill{
	public string skillId;
	public string skillName;
	public string skillDesp;

	public string bulletStyle; 

	public int maxLv;
	public int cooldown = 20000;
	public bool isSelfTarget = false;
	public eAtkType atkType = eAtkType.MELLE_POINT;
	public bool isPassive = false;
	public bool isPermanent = false;
	public ePassiveCheckPoint checkPoint = ePassiveCheckPoint.ATK;
	public List<int> x;
	public List<int> y;
	public List<int> z;
	public TowerSkill(){
	}
	public TowerSkill(string skillId){
		this.skillId = skillId;
	}
}

[System.Serializable]
public class SkillState{
	public string skillId;
	public int skillLevel;
	public SkillState(){
	}
	public SkillState(string skillId,int skillLevel){
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

	public string getEffects(){
		string s = "";
		foreach(TowerComponentEffect effect in effects){
			if (effect.type == eTowerComponentEffectType.ATK_CHANGE) {
				s += "atk " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.ATK_RANGE_CHANGE) {
				s += "range " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.ATK_SPD_CHANGE) {
				s += "atk spd " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.EXTRA_ATK) {
				s += effect.extra + " atk " + effect.x;
			} else if (effect.type == eTowerComponentEffectType.EXTRA_ABILITY) {
				s += effect.extra;
			}
			s += "\n";
		}
		return s;
	}
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

	public string nowLevelId;

	public EncounterState[][] grids;
	public Vector2Int playerPos = Vector2Int.zero;


	public int guideStage = -1;


	public bool isWaitingBattle = false;

	public EncounterBattleInfo battleInfo;
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
	//public string nowLevelId = "toturial";

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
		PlayerPrefs.DeleteAll ();
		if (PlayerPrefs.GetInt ("isFirstGame",1)==1) {
			nowLevelId = "toturial";
			guideStage = 0;
		} else {
			nowLevelId = "l0";
			guideStage = -1;
		}
		generateDungeon (nowLevelId);
		initTowers ();
		initBag ();
		intPotions ();
		initScar ();
		initMemoState ();
	}
	
	public void generateDungeon(string levelId){

		LevelEntry levelInfo = GameStaticData.getInstance ().getLevelInfo (levelId);
		int gridNum = levelInfo.NumOfGrid;

		List<EncounterEntry> toChooseFrom = new List<EncounterEntry>(levelInfo.PossibleEncounters);


		LevelShape ls = GameStaticData.getInstance ().pickRandomShape (gridNum);
		if (nowLevelId == "toturial") {
			ls = GameStaticData.getInstance ().getToturialShape ();

			grids = new EncounterState[GridManager.GRID_HEIGHT][];
			for (int i = 0; i < GridManager.GRID_HEIGHT; i++) {
				grids[i] = new EncounterState[GridManager.GRID_WIDTH];
			}

			EncounterEntry entry1 = toChooseFrom [0];
			EncounterEntry entry2 = toChooseFrom [1];
			grids [ls.activePos[0].x] [ls.activePos[0].y] = new EncounterState ("empty");
			grids [ls.activePos[1].x] [ls.activePos[1].y] = new EncounterState (entry1.eid);
			grids [ls.activePos[2].x] [ls.activePos[2].y] = new EncounterState (entry2.eid);

			playerPos = ls.spawnPos;
			grids [ls.endPos.x] [ls.endPos.y] = new EncounterState ("next_level_01");
			return;
		}

		int totalLeft = gridNum;
		int encounterLeft = levelInfo.NumOfEncounter;

		if (encounterLeft > totalLeft) {
			encounterLeft = totalLeft;
		}

		grids = new EncounterState[GridManager.GRID_HEIGHT][];
		for (int i = 0; i < GridManager.GRID_HEIGHT; i++) {
			grids[i] = new EncounterState[GridManager.GRID_WIDTH];
		}
		for (int i = 0; i < ls.activePos.Count; i++) {
			if (ls.activePos [i] == ls.endPos) {
				continue;
			}
			int ranInt = Random.Range (0,totalLeft);
			if (ranInt < encounterLeft) {
				totalLeft--;
				encounterLeft --;
				EncounterEntry entry = toChooseFrom [Random.Range (0, toChooseFrom.Count)];
				grids [ls.activePos[i][0]] [ls.activePos[i][1]] = new EncounterState (entry.eid);
			} else {
				totalLeft--;
				grids [ls.activePos[i][0]] [ls.activePos[i][1]] = new EncounterState ();
			}
		}
		//grids [2] [3] = new EncounterState ("shop");
		playerPos = ls.spawnPos;
		grids[ls.endPos.x][ls.endPos.y] = new EncounterState ("next_level_01");
	}
	
	public void initPlayerData(){
		
	}



	public void initBattle(string eid,int stageIdx,EncounterBattleInfo bInfo){
		isWaitingBattle = true;
		beforeEid = eid;
		beforeStage = stageIdx;
		battleInfo = bInfo;
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
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t0"+i);

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
	}
	public void initBag(){
		for (int i = 0; i < 20; i++) {



			TowerComponent tc = GameStaticData.getInstance ().getComponentInfo ("c0"+(i%6));
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
			potions.Add(new Potion("00"));
		}
	}

	void initScar(){
		for (int i = 0; i < 3; i++) {
			scars.Add(new Scar("00",2+i));
		}
	}

	public void gainComponent(string componentId){
		TowerComponent tc = GameStaticData.getInstance ().getComponentInfo (componentId);
		bagComponents.Add (tc);
	}

	public void gainTowerBase(string tid){
		TowerBase tb = GameStaticData.getInstance ().getTowerBase (tid);
		TowerTemplate tt = new TowerTemplate ();
		tt.tbase = tb;
		ownedTowers.Add (tt);
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

	public void enterNextLevel(){
		LevelEntry entry = GameStaticData.getInstance ().getLevelInfo (nowLevelId);
		string nextLevelId = entry.NextLevel;
		nowLevelId = nextLevelId;
		if(nextLevelId == "end"){
			return;
		}
		generateDungeon (nextLevelId);
	}



}

