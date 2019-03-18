﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;


public class ChasingEnemyAbstract{
	public List<int> enemyId = new List<int>();
	public List<int> enemyNum = new List<int>();


}


public delegate void PopupCallback();

public class GameManager : Singleton<GameManager> {

	bool inBattle = false;
	public Camera mainCamera;
	public GridManager gridManager;

	public GetItemManager getItemManager;
	public EnterBattlePanel enterBattlePanel;

	public ShopInterface shopPanel;

	//FGUI components
	GComponent _main;

	GTextField hpTextView;
	GTextField moneyTextView;
	GTextField sanTextView;
	GTextField levelTextView;
	GLoader detail;

	GComponent monsterContianer;

	public SelectionBranchManager sbManager;
	public ExploreMenu eMenu;

	GComponent detailPanel;


	GComponent popupWindow;
	UnlockThingWindow unlockThingWindow;

	ItemDetailAmplifier itemDetailAmplifier;
	GGraph _mask;

	//game state

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (this);
		//DontDestroyOnLoad (mainCamera.gameObject);
		initUI ();
		Time.timeScale = 1;
		GameStaticData.getInstance ();
	}

	private AsyncOperation async = null;
	IEnumerator LoadBattle()
	{
		async = SceneManager.LoadSceneAsync("A1");
		yield return async;
		//_main.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.B)) {
			chaseByEnemy (new ChasingEnemyAbstract());
		}
	}

	void resetStats(){
		hpTextView.text = PlayerData.getInstance ().hp+"";
		moneyTextView.text = PlayerData.getInstance ().money+"";
		sanTextView.text = PlayerData.getInstance ().san+"";
		levelTextView.text = "Level."+PlayerData.getInstance ().level;
	}

	public void enterBattle(){
		if (!inBattle) {
			StartCoroutine (LoadBattle());
			inBattle = true;
			mainCamera.gameObject.SetActive (false);
		}
	}

	public void finishBattle(){
		inBattle = false;
		//_main.visible = true;
		mainCamera.gameObject.SetActive (true);
		initUI ();
	}

	public void initUI(){
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/MonsterCard", typeof(MonsterCardComponent));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerPanel", typeof(TowerPanel));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/RolePanel", typeof(RolePanel));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/MemoPanel", typeof(MemoPanel));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/MemoWindow", typeof(MemoInternalWindow));
		//UIObjectFactory.SetPackageItemExtension("ui://UIMain/MemoWindow", typeof(MemoInternalWindow));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/BranchItem", typeof(SelectionBranch));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/ComponentView", typeof(ComponentView));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerViewInList", typeof(GButton));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/NewItem", typeof(NewItem));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/EnterBattlePanel", typeof(EnterBattlePanel));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/MemoItem", typeof(MemoItem));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/RoleClickable", typeof(ItemWithDetail));
		//UIObjectFactory.SetPackageItemExtension("ui://UIMain/AccesoryView", typeof(AccesoryView));


		_main = GameObject.Find ("UIPanel").GetComponent<UIPanel> ().ui;
		_main.GetChild ("n3").onClick.Add (enterBattle);

		Vector3 detailPanelPos = mainCamera.ScreenToWorldPoint (new Vector3 (-100f,mainCamera.pixelHeight/2, 0));
		detailPanelPos.z = 0;
		GameObject.Find ("EncounterDetailPanel").transform.position = detailPanelPos;
		detailPanel = GameObject.Find ("EncounterDetailPanel").GetComponent<UIPanel>().ui;
		detailPanel.visible = false;
		monsterContianer = _main.GetChild ("monsters").asCom;

		hpTextView = _main.GetChild ("hp").asTextField;
		moneyTextView = _main.GetChild ("money").asTextField;
		sanTextView = _main.GetChild ("san").asTextField;
		levelTextView = _main.GetChild ("level").asTextField;

		detail = _main.GetChild ("detail").asLoader;
		detail.url="detail";
		detail.onClick.Add (delegate(EventContext context) {
			eMenu.Show();
		});

		_mask = _main.GetChild ("mask").asGraph;

//		for (int i = 0; i < 3; i++) {
//			addMonster ("9");
//		}
		//detailPanel.TweenFade (0,2);

		sbManager = new SelectionBranchManager ();
		eMenu = new ExploreMenu ();
		popupWindow = UIPackage.CreateObject ("UIMain", "PopupConfirmWindow").asCom;
		unlockThingWindow = new UnlockThingWindow();

		shopPanel = new ShopInterface ();

		enterBattlePanel = new EnterBattlePanel ();
		getItemManager = new GetItemManager ();
		itemDetailAmplifier = new ItemDetailAmplifier ();
		resetStats ();

		checkBattleRes ();
	}

	void checkBattleRes(){
		if (!PlayerData.getInstance ().isWaitingBattle||!PlayerData.getInstance().isFixedBattle) {
			return;
		}
		Debug.Log (PlayerData.getInstance ().beforeEid);
		EncounterInfo einfo = GameStaticData.getInstance ().encounterDic [PlayerData.getInstance ().beforeEid];
		EncounterStage es = einfo.stages [PlayerData.getInstance ().beforeStage];
		int showIdx = -1;
		if (PlayerData.getInstance ().battleWin) {
			showIdx = es.converts [0].nextStageIdx;
		} else {
			showIdx = es.converts [1].nextStageIdx;
		}
		if (showIdx != -1) {
			sbManager.initEncounter (einfo.eId,showIdx);
		}
		PlayerData.getInstance ().isWaitingBattle = false;
	}

	public void showDetailPanel(string info){
		detailPanel.visible = true;
		detailPanel.alpha = 0;
		detailPanel.TweenFade (1,0.3f);
		detailPanel.GetChild ("desp").text = info;
	}
	public void hideDetailPanel(){
		detailPanel.TweenFade (0,0.1f).OnComplete(delegate() {
			detailPanel.visible = false;
		});
	}

	public void showConfirm(PopupCallback callback){
		GRoot.inst.ShowPopup(popupWindow);
		popupWindow.GetChild ("ok").onClick.Clear();
		popupWindow.GetChild ("ok").onClick.Add (delegate(EventContext context) {
			if(callback!=null)callback.Invoke();
			GRoot.inst.HidePopup(popupWindow);
		});
		popupWindow.SetXY(GRoot.inst.width/2, GRoot.inst.height/2);
	}

	public void showUnlockWindow(){
		unlockThingWindow.Show ();
	}



//	for (int i = 0; i < 3; i++) {
//		addMonster ("9");
//	}

	public void nextLevel(){
		sbManager.Hide();
		_mask.visible = true;
		_mask.alpha = 0;
		PlayerData.getInstance().generateDungeon(10);
		_mask.TweenFade (1, 0.5f).OnComplete (delegate() {
			gridManager.initGrid();
			_mask.TweenFade(0,0.5f).OnComplete(delegate() {
				_mask.visible = false;
			});
		});

	}


	public void showGetItemEffect(Vector2 originPosInGlobal){
		NewItem copyView = (NewItem)UIPackage.CreateObject ("UIMain", "NewItem").asCom;
		//GTextField copyView = new GTextField();
		copyView.text = "assasasaaas";
		//Vector2 f = _new_item_list.GetChildAt (idx).position;
		copyView.position = _main.GlobalToLocal(originPosInGlobal);
		copyView.sortingOrder = 100;
		//GRoot.inst.AddChild (copyView);
		_main.AddChild (copyView);
		copyView.TweenMove (new Vector2(0,GRoot.inst.height-copyView.height*0.4f),0.3f).OnUpdate(delegate(GTweener tweener) {
			copyView.InvalidateBatchingState();
			float r = (tweener.deltaValue.vec2-tweener.startValue.vec2).magnitude/(tweener.endValue.vec2-tweener.startValue.vec2).magnitude;
			r = 1-r*0.6f;
			//r = r*0.6;
			copyView.scale = new Vector2(r,r);
		}).OnComplete(delegate() {
			copyView.TweenFade(0,0.2f).OnComplete(delegate() {
				_main.RemoveChild(copyView);

			});
		});
	}


	//public List<MonsterCardComponent> monsters = new List<MonsterCardComponent>();

	public void clearAllEnemy(){
		foreach (GObject o in monsterContianer.GetChildren()) {
			o.Dispose ();
		}
		monsterContianer.RemoveChildren ();
		PlayerData.getInstance ().chasingEnemies.Clear ();
	}

	public void chaseByEnemy(ChasingEnemyAbstract enemyInfo){
		PlayerData.getInstance ().addMonster (enemyInfo);
		addMonsterView (enemyInfo);
		StartCoroutine (lateCheckIfBattle());
	}

	public void reArrangeEnemies(){

		for (int i = 0; i < monsterContianer.numChildren-1; i++) {
			monsterContianer.GetChildAt(i).TweenMove (new Vector2 (-20 * (monsterContianer.numChildren-1-i) - 100, -20 * (monsterContianer.numChildren-1-i) - 100), 0.2f);
			//monsters [i].SetXY (-20*i -100,-20*i-100);
		}
	}

	public void addMonsterView(ChasingEnemyAbstract enemyInfo){
		MonsterCardComponent item = (MonsterCardComponent)UIPackage.CreateObject ("UIMain", "MonsterCard").asCom;
		monsterContianer.AddChild (item);
		item.SetXY (-100,-100);
		item.setName (enemyInfo.enemyId+"");
		//item.visible = false;
		showAddMonsterEffect (Vector2.zero,monsterContianer.LocalToGlobal(item.position));
		reArrangeEnemies ();
	}

	IEnumerator lateCheckIfBattle(){
		yield return new WaitForSecondsRealtime (0.5f);
		if (PlayerData.getInstance ().chasingEnemies.Count >= 3) {
			clearAllEnemy ();
			//initAndShow
			enterBattlePanel.initAndShow(delegate() {
				PlayerData.getInstance ().initBattle ();
				GameManager.getInstance ().enterBattle ();
			});
		}
	}

	public void showAddMonsterEffect(Vector2 originPosInGlobal,Vector2 toPosInGlobal){
		MonsterCardComponent copyView = (MonsterCardComponent)UIPackage.CreateObject ("UIMain", "MonsterCard").asCom;
		//GTextField copyView = new GTextField();

		//Vector2 f = _new_item_list.GetChildAt (idx).position;
		copyView.position = _main.GlobalToLocal(originPosInGlobal);
		copyView.sortingOrder = 100;

		_main.AddChild (copyView);

		copyView.TweenMove (_main.GlobalToLocal(toPosInGlobal),0.3f).OnUpdate(delegate(GTweener tweener) {
			copyView.InvalidateBatchingState();
			//float r = (tweener.deltaValue.vec2-tweener.startValue.vec2).magnitude/(tweener.endValue.vec2-tweener.startValue.vec2).magnitude;
			//r = 1-r*0.6f;
			//r = r*0.6;
			//copyView.scale = new Vector2(r,r);
		}).OnComplete(delegate() {
			
			//monsterContianer.GetChildAt(monsterContianer.numChildren-1).visible = true;

			copyView.TweenFade(0,0.2f).OnComplete(delegate() {
				_main.RemoveChild(copyView);
				copyView.Dispose();
			});
		});
	}

	public void enterShop(){
		shopPanel.Show ();
	}

	public void showDetailAmplifier(){
		itemDetailAmplifier.Show ();
	}
}
