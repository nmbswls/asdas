using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AssemblyCSharp;
using Newtonsoft.Json;
using FairyGUI;


[System.Serializable]
public class EncounterInfo{
	public string eId;
	public int type;
	public string desp = "迷雾呈现出你似曾相识的污浊颜色，能听见不知名的声音。";
	public Dictionary<int,EncounterStage> stages = new Dictionary<int,EncounterStage>();
}

[System.Serializable]
public class EncounterStage{
	public int sId;
	public string desp = "...............";
	//public bool isFinishStage = false;
	//public bool isBattleStage = false;
	public eStageType stageType = eStageType.NORMAL;
	public string extra;
	public EncounterRes res = null;
	public int randomValue = 0;
	public int battleRes = 0;
	public List<EncounterConvert> converts = new List<EncounterConvert>();
}



[System.Serializable]
public class EncounterRes{
	public eFinishType type = eFinishType.NORMAL;
	public string finishEffect = "none";
	public List<EncounterReward> rewords = new List<EncounterReward> ();
}

[System.Serializable]
public class EncounterReward{
	public string rewardName = "hp";
	public int rewardAmount = 30;
	public int chance = 100;
}

[System.Serializable]
public class EncounterConvert{
	public int nextStageIdx;
	public string choiceDesp = "...";
	public List<EncounterConvertCheck> checks = new List<EncounterConvertCheck>();
}

[System.Serializable]
public class EncounterConvertCheck{
	public string key;
	public eCheckOpt fuhao = eCheckOpt.eq;
	public string value;
}

[System.Serializable]
public enum eCheckOpt{
	eq = 1,
	gt = 2,
	lt = 3,
	elt = 4,
	egt = 5,
	has = 6,
}


[System.Serializable]
public enum eStageType{
	NORMAL = 0,
	BATTLE = 1,
	CHECK = 2,
	FINISH = 3,
	RANDOM = 4,
}

[System.Serializable]
public enum eFinishType{
	NORMAL = 0,
	ANOTHER_ENCOUNTER = 1,
	NEXT_LEVEK = 2,
}

public class SelectionBranchManager : Window
{

	public static int MAX_BRANCH_NUM = 4;

	public List<EncounterConvert> converters = new List<EncounterConvert> ();

	public EncounterInfo encounter;

	GList _branches;
	GLoader _loader;
	GTextField _text;

	int stageIndex = -1;

	// Use this for initialization
	void Start ()
	{
		//init (5);
	}
	protected override void OnInit()
	{
		

		this.contentPane = UIPackage.CreateObject("UIMain", "DialogWindow").asCom;
		this.Center();
		this.modal = true;

		_branches = this.contentPane.GetChild("tabs").asList;
		_loader = this.contentPane.GetChild ("pic").asLoader;
		_text = this.contentPane.GetChild ("desp").asTextField;

		_branches.foldInvisibleItems = true;

		_branches.onClickItem.Add(clickItem);

		_branches.itemRenderer = RenderListItem;
		_branches.EnsureBoundsCorrect();


		for (int i = 0; i < MAX_BRANCH_NUM; i++)
		{
			SelectionBranch item = (SelectionBranch)_branches.GetChildAt (i).asButton;
			item.idx = i;
		}
		//		float interval = 300/(_list.numChildren>8?8:_list.numChildren);
		//		_list.columnGap = (int)interval;
		//_list.numItems = 3;
	}

	public void initEncounter(string eid = null,int stageIdx = 0){

		if (eid == null) {
			encounter = GameStaticData.getInstance().getEncounterInfo("empty");
		} else {
			encounter = GameStaticData.getInstance().getEncounterInfo(eid);
		}
		//通过静态信息得到文字表
		this.stageIndex = stageIdx;
		this.Show ();
	}

	protected override void OnShown ()
	{
		base.OnShown ();
		this.alpha = 0;
		this.TweenFade (1, 0.3f);

		changeView (stageIndex);
	}

	void RenderListItem(int index, GObject obj){
	
	}

	void clickItem(EventContext context){
		SelectionBranch item = (SelectionBranch)context.data;
		chooseBranch (item.idx);
	}

	// Update is called once per frame
	void Update ()
	{

	}

	void Awake(){
//		for (int i = 0; i < MAX_BRANCH_NUM; i++) {
//			int index = i;
//
//			UIEventListener clickListener = branches[index].AddComponent<UIEventListener> ();
//			clickListener.ClickEvent += delegate(GameObject gb,PointerEventData eventData) {
//				Debug.Log("click"+index);
//				chooseBranch(index);
//			};
//		}

	}



	public void chooseBranch(int choice){

		if (choice < 0 || choice >= converters.Count) {
			return;
		}
		stageIndex = converters [choice].nextStageIdx;
		changeView (stageIndex);
	}

	public void getRes(EncounterRes res){
		if (res == null)
			return;
		if (res.type == eFinishType.NORMAL) {
			if (res.rewords.Count > 0) {
				GameManager.getInstance ().getItemManager.initAndShow (new string[]{ "大宝剑", "藏宝图", "火箭" });
			} else {
				GameManager.getInstance ().finishItemGet ();
			}
			panelHide ();
			//GameManager.getInstance ().showConfirm (panelHide);

		} else if (res.type == eFinishType.NEXT_LEVEK) {
			
			GameManager.getInstance ().nextLevel ();
		}
	}

	void panelHide(){
		this.TweenFade (0, 0.3f).OnComplete(delegate() {
			this.Hide ();
		});
	}
	void changeView(int stageIndex){
		EncounterStage stage = null;
		converters.Clear ();
		if (!encounter.stages.TryGetValue (stageIndex, out stage)) {
			return;
		}
		if (stage.extra == "monster") {
			EnemyCombo ec = GameStaticData.getInstance ().getEnemyWithValue (5);
			GameManager.getInstance().chaseByEnemy (ec);
		}

		if (stage.stageType == eStageType.FINISH) {
			getRes (stage.res);
			return;
		} else if (stage.stageType == eStageType.BATTLE) {
			Debug.Log ("battle");
			PlayerData.getInstance ().initBattle (encounter.eId, stageIndex);
			GameManager.getInstance ().enterBattle ();
			this.Hide ();
			return;
		} else if (stage.stageType == eStageType.CHECK) {
			Debug.Log ("check");

			int nextStage = -1;
			for (int i=0;i<stage.converts.Count-1;i++) {
				EncounterConvert convert = stage.converts [i];
				bool canConvert = checkConditions (convert.checks);
				if (canConvert) {
					nextStage = convert.nextStageIdx;
					break;
				}
			}
			if (nextStage == -1) {
				nextStage = stage.converts [stage.converts.Count - 1].nextStageIdx;
			}
			if (nextStage != -1) {
				stageIndex = nextStage;
				changeView (stageIndex);
			} else {
				Debug.Log ("error");
				panelHide ();
			}
		} else if(stage.stageType == eStageType.RANDOM){
		
			int randomInt = Random.Range (0, 99);
			int nextStage = -1;
			for (int i=0;i<stage.converts.Count-1;i++) {
				EncounterConvert convert = stage.converts [i];
				bool canConvert = checkConditions (convert.checks,randomInt);
				if (canConvert) {
					nextStage = convert.nextStageIdx;
					break;
				}
			}
			if (nextStage == -1) {
				nextStage = stage.converts [stage.converts.Count - 1].nextStageIdx;
			}
			if (nextStage != -1) {
				stageIndex = nextStage;
				changeView (stageIndex);
			} else {
				Debug.Log ("error");
				panelHide ();
			}

		}else {
			_text.text = stage.desp;
			int idx = 0;
			for (int i = 0; i < stage.converts.Count; i++) {
				EncounterConvert convert = stage.converts [i];
				bool canShow = checkConditions (convert.checks);
				//EncounterConvert convert = stage.converts [i];
				if (canShow) {
					_branches.GetChildAt(idx).asButton.title = convert.choiceDesp;
					_branches.GetChildAt(idx).visible = true;
					converters.Add (convert);
					idx++;
				}
			}
			for (int i = idx; i < MAX_BRANCH_NUM; i++) {
				_branches.GetChildAt(i).asButton.title ="";
				_branches.GetChildAt(i).visible = false;
			}

			_loader.url = "Explore/default_bg";
		}
	}


	bool checkConditions(List<EncounterConvertCheck> checks, int randInt = 0){
		foreach (EncounterConvertCheck check in checks) {
			int toCompare = 0;
			if (check.key == "hp") {
				toCompare = PlayerData.getInstance ().hp;
			} else if (check.key == "san") {
				toCompare = PlayerData.getInstance ().san;
			} else if (check.key == "money") {
				toCompare = PlayerData.getInstance ().money;
			} else if (check.key == "random") {
				toCompare = randInt;
			} else {
				continue;
			}
			int toCompareTo = 0;
			if (!int.TryParse (check.value, out toCompareTo)) {
				return false;
			}
			if (check.fuhao == eCheckOpt.eq && toCompare != toCompareTo
				|| check.fuhao == eCheckOpt.egt && toCompare < toCompareTo
				|| check.fuhao == eCheckOpt.elt && toCompare > toCompareTo
				|| check.fuhao == eCheckOpt.gt && toCompare <= toCompareTo
				|| check.fuhao == eCheckOpt.lt && toCompare >= toCompareTo) {
				return false;
			}

		}
		return true;
	}
}
