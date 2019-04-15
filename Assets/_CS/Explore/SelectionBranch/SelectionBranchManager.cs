using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AssemblyCSharp;
using Newtonsoft.Json;
using FairyGUI;




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


//		for (int i = 0; i < MAX_BRANCH_NUM; i++)
//		{
//			SelectionBranch item = (SelectionBranch)_branches.GetChildAt (i).asButton;
//			item.idx = i;
//		}
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
		chooseBranch (item.getIndex());
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
				List<string> l = GameStaticData.getInstance().getRandomComponents (3);
				GameManager.getInstance ().getItemManager.initAndShow (l);
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
			GameManager.getInstance ().chaseByEnemy (ec);
		} else if (stage.extra == "toturial_monster") {
			EnemyCombo ec = GameStaticData.getInstance ().getSpecifiedEnemy ("toturial");
			GameManager.getInstance ().chaseByEnemy (ec);
		}

		if (stage.stageType == eStageType.FINISH) {
			getRes (stage.res);
			return;
		} else if (stage.stageType == eStageType.BATTLE) {
			EncounterBattleInfo battleInfo = stage.battleInfo;

			PlayerData.getInstance ().initBattle (encounter.eId, stageIndex,battleInfo);
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


			_branches.ClearSelection();
			_branches.RemoveChildrenToPool();


//			for(int i=0;i<items.Count;i++)
//			{
//				NewItem item = (NewItem)_new_item_list.AddItemFromPool();
//				item.init (items[i]);
//				item.onClick.Add (delegate() {
//					if(_new_item_list.selectedIndex!=-1){
//						_confirm.visible = true;
//					}
//					if (PlayerData.getInstance ().guideStage == 10) {
//						GuideManager.getInstance ().showGuideConfirmChooseItem ();
//						PlayerData.getInstance ().guideStage = 11;
//					}
//				});
//				item.GetChild ("detail").onTouchBegin.Add (delegate() {
//					//Debug.Log("Show Detail");
//				});
//			}

			int numOfBranch = 0;


			for (int i = 0; i < stage.converts.Count; i++) {
				EncounterConvert convert = stage.converts [i];
				bool canShow = checkConditions (convert.checks);
				//EncounterConvert convert = stage.converts [i];
				if (canShow) {

					SelectionBranch item = (SelectionBranch)_branches.AddItemFromPool ().asButton;
					item.init (numOfBranch ++,convert.choiceDesp);
					item.visible = true;
					converters.Add (convert);
				}
			}
			_branches.ResizeToFit (numOfBranch);

//			for (int i = 0; i < stage.converts.Count; i++) {
//				EncounterConvert convert = stage.converts [i];
//				bool canShow = checkConditions (convert.checks);
//				//EncounterConvert convert = stage.converts [i];
//				if (canShow) {
//
//					SelectionBranch item = (SelectionBranch)_branches.GetChildAt (i).asButton;
//					item.idx = numOfBranch ++ ;
//
//					_branches.GetChildAt(idx).asButton.title = convert.choiceDesp;
//					_branches.GetChildAt(idx).visible = true;
//					converters.Add (convert);
//					idx++;
//
//
//				}
//			}
//			for (int i = idx; i < MAX_BRANCH_NUM; i++) {
//				_branches.GetChildAt(i).asButton.title ="";
//				_branches.GetChildAt(i).visible = false;
//			}

			_loader.url = "Explore/default_bg";
		}

		if (PlayerData.getInstance ().guideStage == 2) {
			PlayerData.getInstance ().guideStage = 3;
			GuideManager.getInstance ().showGuideChooseBranch ();
		}else if (PlayerData.getInstance ().guideStage == 3) {
			PlayerData.getInstance ().guideStage = 4;
			GuideManager.getInstance ().showGuideChooseBranch ();
		}else if (PlayerData.getInstance ().guideStage == 4) {
			GuideManager.getInstance ().hideGuide ();
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


	public Rect getFirstBranch(){
		GObject firstItem = _branches.GetChildAt (0);
		Rect rect = firstItem.TransformRect(new Rect(0, 0, firstItem.width, firstItem.height), GRoot.inst);
		_branches.EnsureBoundsCorrect ();

		Vector2 center = _branches.LocalToGlobal(new Vector3(firstItem.position.x+firstItem.width/2,firstItem.position.y+firstItem.height/2,0));
		Rect trueRect = new Rect (center.x-rect.size.x/2-4,center.y-rect.size.y/2,rect.size.x+4,rect.size.y+4);
		//GuideManager.getInstance ()._guideLayer.setMark (GuideManager.getInstance ()._guideLayer.GlobalToLocal(center));
		return trueRect;
	}

//	public GObject getFirstBranch(){
//		
//		return _branches;
//	}
}
