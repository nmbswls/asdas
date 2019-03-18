using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class UsableHeroInfo{

	public string name;
	public string desp;
	public string[] tianfu = new string[4];
	public int maxHp;
	public int maxMp;
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
	}

	public List<UsableHeroInfo> heroes = new List<UsableHeroInfo>();
	public Dictionary<string, EncounterInfo> encounterDic = new Dictionary<string, EncounterInfo>();

	public Dictionary<string,TowerComponent> componentStaticInfo = new Dictionary<string,TowerComponent>();
	public Dictionary<string,string> potionStaticInfo = new Dictionary<string,string>();
	public Dictionary<string,TowerBase> towerBaseInfo = new Dictionary<string,TowerBase>();

	public Dictionary<string,MemoInfo> memoInfo = new Dictionary<string,MemoInfo>();
	public Dictionary<int,TowerSkill> towerSkills = new Dictionary<int,TowerSkill>();

	public List<MemoCombinationRule> memoCombineInfo = new List<MemoCombinationRule>();

	public void initHeroes(){
		
		UsableHeroInfo h1 = new UsableHeroInfo ();
		h1.name = "傻逼";
		h1.desp = "超级大傻逼";
		h1.maxHp = 100;
		h1.maxMp = 20;
		h1.tianfu [0] = "强力";

		UsableHeroInfo h2 = new UsableHeroInfo ();
		h2.name = "智障";
		h2.desp = "超级大智障";
		h2.maxHp = 100;
		h2.maxMp = 20;
		h2.tianfu [0] = "智商剥削";

		UsableHeroInfo h3 = new UsableHeroInfo ();
		h3.name = "白痴";
		h3.desp = "超级大白痴";
		h3.maxHp = 100;
		h3.maxMp = 20;
		h3.tianfu [0] = "智力归零";

		heroes.Add (h1);
		heroes.Add (h2);
		heroes.Add (h3);

	}

	public void loadEncounters(){
			
		{
			TextAsset encounterTxt = Resources.Load ("json/encounters/e00") as TextAsset;
			encounterDic = JsonConvert.DeserializeObject<Dictionary<string, EncounterInfo>> (encounterTxt.text);
		}
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
		towerSkills = JsonConvert.DeserializeObject<Dictionary<int,TowerSkill>> (ta.text);
	}



}

