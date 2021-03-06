﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;
using Newtonsoft.Json;





[System.Serializable]
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
	public int san = 200;
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
		//PlayerPrefs.DeleteAll ();
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

		int totalLeft = gridNum-2;
		int encounterLeft = levelInfo.NumOfEncounter;

		if (encounterLeft > totalLeft) {
			encounterLeft = totalLeft;
		}

		grids = new EncounterState[GridManager.GRID_HEIGHT][];
		for (int i = 0; i < GridManager.GRID_HEIGHT; i++) {
			grids[i] = new EncounterState[GridManager.GRID_WIDTH];
		}
		for (int i = 0; i < ls.activePos.Count; i++) {
			if (ls.activePos [i] == ls.endPos || ls.activePos [i] == ls.spawnPos) {
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
		grids [ls.spawnPos.x] [ls.spawnPos.y] = new EncounterState ("empty");

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

		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t01");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t02");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t03");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
//		{
//			TowerTemplate tt = new TowerTemplate ();
//			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t04");
//
//			tt.tbase = tb;
//			ownedTowers.Add (tt);
//		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t05");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t06");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t07");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t08");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t10");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t11");

			tt.tbase = tb;
			ownedTowers.Add (tt);
		}
		{
			TowerTemplate tt = new TowerTemplate ();
			TowerBase tb = GameStaticData.getInstance ().getTowerBase ("t20");

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

