﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class UsableHeroInfo{

	public string name;
	public string desp;
	public HeroTalent[] talent = new HeroTalent[3];
	public int maxHp;
	public int maxMp;
}

public class HeroTalent{
	public string talentId;
	public string talentName;
	public string talentDesp;
}

public class PotionStaticInfo{
	public string pid;
	public string pname;
	public string pdesp = "This is a potion.";
}

public class ScarStaticInfo{
	public string scarId;
	public string scarName;
	public string scarDesp;
}


public class EnemyCombo{
	public string comboId = "";
	public List<string> enemyId = new List<string>();
	public List<int> enemyNum = new List<int> ();
}


public class EnemyData
{
	public string enemyId;
	public string enemyName = "不可名状之物";
	public int difficulty = 1;
	public int hp = 10000;
	public int speed = 2000;
	public int atk = 3000;
	public int def = 300;
}

public class GameStaticData
{

	private static GameStaticData instance;

	public static GameStaticData getInstance(){
		if (instance == null) {
			instance = new GameStaticData ();
		}
		return instance;
	}

	GameStaticData(){
		loadEncounters ();
		initHeroes ();
		loadTowerInfo ();
		loadComponentInfo ();
		loadTowerSkillInfo ();
		loadMemoInfo ();
		loadMemoCombinationRule ();

		loadEnemyInfo ();

		loadScarInfo ();
		loadPotionInfo ();
	}

	public List<UsableHeroInfo> heroes = new List<UsableHeroInfo>();
	public Dictionary<string, EncounterInfo> encounterDic = new Dictionary<string, EncounterInfo>();

	public Dictionary<string,TowerComponent> componentStaticInfo = new Dictionary<string,TowerComponent>();

	public Dictionary<string,PotionStaticInfo> potionStaticInfo = new Dictionary<string,PotionStaticInfo>();
	public Dictionary<string,ScarStaticInfo> scarStaticInfo = new Dictionary<string,ScarStaticInfo>();

	public Dictionary<string,TowerBase> towerBaseInfo = new Dictionary<string,TowerBase>();

	public Dictionary<string,MemoInfo> memoInfo = new Dictionary<string,MemoInfo>();
	public Dictionary<string,TowerSkill> towerSkills = new Dictionary<string,TowerSkill>();

	public List<MemoCombinationRule> memoCombineInfo = new List<MemoCombinationRule>();

	public Dictionary<string,EnemyData> enemyStaticInfo = new Dictionary<string,EnemyData>();

	public Dictionary<string,EnemyCombo> enemyCombos = new Dictionary<string,EnemyCombo>();

	public Dictionary<int,List<string>> enemyPerLv = new Dictionary<int,List<string>>();

	public List<HeroTalent> talents = new List<HeroTalent>();

	public void initHeroes(){

		{
			HeroTalent talent = new HeroTalent ();
			talent.talentId = "00";
			talent.talentName = "Strong";
			talent.talentDesp = "You are so strong, that all your melee towers will cause 10% more damage.";
			talents.Add (talent);
		}
		{
			HeroTalent talent = new HeroTalent ();
			talent.talentId = "01";
			talent.talentName = "Continuation";
			talent.talentDesp = "You will come back.";
			talents.Add (talent);
		}
		{
			HeroTalent talent = new HeroTalent ();
			talent.talentId = "02";
			talent.talentName = "Shield";
			talent.talentDesp = "Bugs will not hurt you.";
			talents.Add (talent);
		}
		{
			HeroTalent talent = new HeroTalent ();
			talent.talentId = "03";
			talent.talentName = "Steal Will";
			talent.talentDesp = "No one can destory your will.";
			talents.Add (talent);
		}

		UsableHeroInfo h1 = new UsableHeroInfo ();
		h1.name = "Soldier";
		h1.desp = "He is a soldier.\nHe is pretty strong and fearless.\nHe is good at battle.";
		h1.maxHp = 100;
		h1.maxMp = 20;

		h1.talent [0] = talents[0];
		h1.talent [1] = talents[1];
		h1.talent [2] = talents[2];

		UsableHeroInfo h2 = new UsableHeroInfo ();
		h2.name = "Wizard";
		h2.desp = "He is a powerful wizard with strong will and fragile body.\nHe can use magic to play his enemy.\nHe is used to darkness.";
		h2.maxHp = 100;
		h2.maxMp = 20;
		h2.talent [0] = talents[0];
		h2.talent [1] = talents[0];
		h2.talent [2] = talents[0];

		UsableHeroInfo h3 = new UsableHeroInfo ();
		h3.name = "Scavenger";
		h3.desp = "He is searching for something.\nHe is good at protect himself.\nHe is an sb.";
		h3.maxHp = 100;
		h3.maxMp = 20;
		h3.talent [0] = talents[3];
		h2.talent [1] = talents[3];
		h2.talent [2] = talents[3];

		heroes.Add (h1);
		heroes.Add (h2);
		heroes.Add (h3);

	}

	public void loadEncounters(){
			
//		{
//			TextAsset encounterTxt = Resources.Load ("json/encounters/20") as TextAsset;
//			encounterDic = JsonConvert.DeserializeObject<Dictionary<string, EncounterInfo>> (encounterTxt.text);
//		}
	}

	public void loadTowerInfo(){
		
		//TowerData towerData = JsonUtility.FromJson<TowerData> (towerAsset.text);
		TextAsset ta = Resources.Load ("json/tower_base/towers") as TextAsset;
		towerBaseInfo = JsonConvert.DeserializeObject<Dictionary<string,TowerBase>> (ta.text);

	}

	public void loadComponentInfo(){
		//componentStaticInfo.Add ("0", new TowerComponent ());
		TextAsset ta = Resources.Load ("json/tower_component/components") as TextAsset;
		List<TowerComponent> ll = JsonConvert.DeserializeObject<List<TowerComponent>> (ta.text);
		//Dictionary<string,TowerComponent> dic = new Dictionary<string,TowerComponent> ();
		foreach (TowerComponent tc in ll) {
			componentStaticInfo.Add (tc.cid, tc);
		}
	}

	public void loadPotionInfo(){
		string[] potionNames = new string[]{"生命药剂","遗忘药剂","苦痛药剂","装甲药剂","空瓶"};
		for (int i = 0; i < potionNames.Length; i++) {
			PotionStaticInfo p = new PotionStaticInfo ();
			p.pid = (i+"").PadLeft(2,'0');
			p.pname = potionNames[i];
			potionStaticInfo [p.pid] = p;
		}
		{
			PotionStaticInfo p = new PotionStaticInfo ();
			p.pid = "default";
			p.pname = "None";
			potionStaticInfo ["default"] = p;
		}

	}
	public void loadScarInfo(){

		string[] scarNames = new string[]{"撕裂","重度撕裂","致命","痛苦","虚无"};
		string[] scarDesps = new string[]{"你的生命上限减少了","你的生命上限大幅度减少，治疗效果也同时下降","你从死亡的边缘爬了回来，但死神还是在你身上留下了记好。","你的灵魂无时无刻不处于折磨之中。","你能听到虚空的互换。"};
		for (int i = 0; i < scarNames.Length; i++) {
			ScarStaticInfo p = new ScarStaticInfo ();
			p.scarId = (i+"").PadLeft(2,'0');
			p.scarName = scarNames[i];
			p.scarDesp = scarDesps [i];
			scarStaticInfo [p.scarId] = p;
		}

		{
			ScarStaticInfo p = new ScarStaticInfo ();
			p.scarId = "default";
			p.scarName = "None";
			scarStaticInfo ["default"] = p;
		}

	}

	public void loadMemoInfo(){
		TextAsset ta = Resources.Load ("json/memo/memos") as TextAsset;
		memoInfo = JsonConvert.DeserializeObject<Dictionary<string,MemoInfo>> (ta.text);


	}

	public void loadMemoCombinationRule(){
		TextAsset ta = Resources.Load ("json/memo/memo_combine_rule") as TextAsset;
		memoCombineInfo = JsonConvert.DeserializeObject<List<MemoCombinationRule>> (ta.text);
	}

	public string getPossibleCombination(List<MemoItem> toCombine){
		List<string> toCombineIdx = new List<string> ();
		foreach(MemoItem item in toCombine){
			toCombineIdx.Add (item.memoId);
		}

		foreach(MemoCombinationRule rule in GameStaticData.getInstance().memoCombineInfo){
			if (toCombine.Count != rule.toCombine.Count)
				continue;
			bool flag = true;
			foreach (string idx in rule.toCombine) {
				if (!toCombineIdx.Contains (idx)) {
					flag = false;
					break;
				}
			}
			if (flag) {
				return rule.resId;
			}
		}
		return "";
	}

	void loadTowerSkillInfo(){
		TextAsset ta = Resources.Load ("json/tower_skill/100") as TextAsset;
		towerSkills = JsonConvert.DeserializeObject<Dictionary<string,TowerSkill>> (ta.text);
	}


	void loadEnemyInfo(){
		TextAsset ta = Resources.Load ("json/enemy/all") as TextAsset;
		enemyStaticInfo = JsonConvert.DeserializeObject<Dictionary<string,EnemyData>> (ta.text);

		foreach (EnemyData ed in enemyStaticInfo.Values) {
			if (!enemyPerLv.ContainsKey (ed.difficulty)) {
				enemyPerLv [ed.difficulty] = new List<string> ();
			}
			enemyPerLv [ed.difficulty].Add (ed.enemyId);
		}

		{
			EnemyCombo combo = new EnemyCombo ();
			enemyCombos ["0"] = combo;
		}
	}

	public EnemyData getEnemyInfo(string eid){
		EnemyData data = null;
		if (!enemyStaticInfo.TryGetValue(eid,out data)) {
			data = enemyStaticInfo ["default"];
		}
		return data;
	}

	public TowerBase getTowerBase(string tid){
		TowerBase data = null;
		if (!towerBaseInfo.TryGetValue(tid,out data)) {
			data = towerBaseInfo ["default"];
		}
		return data;
	}

	public ScarStaticInfo getScarInfo(string sid){
		ScarStaticInfo data = null;
		if (!scarStaticInfo.TryGetValue(sid,out data)) {
			data = scarStaticInfo ["default"];
		}
		return data;
	}
	public PotionStaticInfo getPotionInfo(string pid){
		PotionStaticInfo data = null;
		if (!potionStaticInfo.TryGetValue(pid,out data)) {
			data = potionStaticInfo ["default"];
		}
		return data;
	}


	public TowerComponent getComponentInfo(string cid){
		TowerComponent data = null;
		if (!componentStaticInfo.TryGetValue(cid,out data)) {
			data = componentStaticInfo ["default"];
		}
		return data;
	}

	public TowerSkill getTowerSkillInfo(string skillId){
		TowerSkill data = null;
		if (!towerSkills.TryGetValue(skillId,out data)) {
			data = towerSkills ["default"];
		}
		return data;
	}

	public EncounterInfo getEncounterInfo(string eid){
		if (encounterDic.ContainsKey (eid)) {
			return encounterDic[eid];
		}
		TextAsset ta = Resources.Load ("json/encounters/"+eid) as TextAsset;
		if (ta == null)
			return null;
		EncounterInfo einfo = JsonConvert.DeserializeObject<EncounterInfo> (ta.text);
		encounterDic [einfo.eId] = einfo;
		return einfo;
	}




	public EnemyCombo getEnemyWithValue(int v){
		EnemyCombo ec = new EnemyCombo ();
		if (v <= 8) {
			int a = enemyPerLv [1].Count;
			int randInt = Random.Range (0,a-1);
			string eId = enemyPerLv [1] [randInt];
			ec.enemyId.Add (eId);
			ec.enemyNum.Add (v);
		}
		return ec;
	}

	public List<string> getRandomComponents(int num){
		List<string> tt = new List<string> ();
		foreach (var k in componentStaticInfo.Keys) {
			tt.Add (k);
		}
		List<string> res = new List<string> ();
		for (int i = 0; i < num; i++) {
			res.Add (tt [(int)Random.Range (0, tt.Count)]);
		}
		return res;
	}
}

