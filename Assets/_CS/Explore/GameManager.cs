using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FairyGUI;




public delegate void PopupCallback();

public class GameManager : Singleton<GameManager> {

	bool inBattle = false;
	public Camera mainCamera;
	public Camera camera3D;
	public GridManager gridManager;

	public GetItemManager getItemManager;
	public EnterBattlePanel enterBattlePanel;

	public ExploreFinishWindow exploreFinishWindow;

	public ShopInterface shopPanel;

	//FGUI components
	GComponent _main;

	GTextField hpTextView;
	GTextField moneyTextView;
	GTextField sanTextView;
	GTextField levelTextView;
	public GGraph showdetailButton;

	GLoader _enterBattle;


	GComponent monsterContianer;

	public SelectionBranchManager sbManager;
	public ExploreMenu eMenu;

	public GComponent detailPanel;


	GComponent popupWindow;
	UnlockThingWindow unlockThingWindow;

	ItemDetailAmplifier itemDetailAmplifier;
	GGraph _mask;

	//game state

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (this);
		//DontDestroyOnLoad (mainCamera.gameObject);

		GameStaticData.getInstance ();
		initPlayer ();
		initUI ();
		resetStats ();

		lockUI ();

		gridManager.initGrid ();

		_mask.visible = true;
		_mask.alpha = 1;

		_mask.TweenFade(0,1.5f).OnComplete(delegate() {
			_mask.visible = false;
			if (PlayerData.getInstance ().guideStage == 0) {
				GuideManager.getInstance ().showGuideMoveMark (gridManager.getToturialGridPos ());
			}
			unlockUI();
			checkBattleRes ();
		});

		Time.timeScale = 1;
	}

//	
	void initPlayer(){
		string heroName = "hero"+((PlayerData.getInstance().heroIdx+1)+"").PadLeft (2,'0');
		Sprite s = Resources.Load<Sprite> ("image/"+heroName);
		gridManager.playerSymbol.GetComponentInChildren<SpriteRenderer> ().sprite = s;
	}
	// Update is called once per frame
	void Update () {
//		if (Input.GetKeyUp (KeyCode.B)) {
//			EnemyCombo ec = GameStaticData.getInstance ().getEnemyWithValue (5);
//			List<EnemyCombo> sss = new List<EnemyCombo> ();
//			sss.Add (ec);
//			sss.Add (ec);
//			sss.Add (ec);
//			chaseByEnemy (sss);
//		}
//
//		if (Input.GetKeyUp (KeyCode.C)) {
//			getNewScar (new List<Scar>());
//		}

	}

	void resetStats(){
		hpTextView.text = PlayerData.getInstance ().hp+"";
		moneyTextView.text = PlayerData.getInstance ().money+"";
		sanTextView.text = PlayerData.getInstance ().san+"";
		levelTextView.text = "Level."+PlayerData.getInstance ().nowLevelId;
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

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/PotionSmall", typeof(PotionSmall));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/ScarSmall", typeof(ScarSmallIcon));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerPropertyDetail", typeof(TowerPropertyAfter));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerDamageItem", typeof(TowerDamageItem));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/TowerSkillItem", typeof(TowerSkillItem));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/AccesoryView", typeof(AccesoryView));

		UIObjectFactory.SetPackageItemExtension("ui://UIMain/PropertyCompareLine", typeof(PropertyCompareLine));
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/PropertyModify", typeof(PropertyModify));




		UIObjectFactory.SetPackageItemExtension("ui://UIMain/Hint", typeof(HintComponent));

		//UIObjectFactory.SetPackageItemExtension("ui://UIMain/AccesoryView", typeof(AccesoryView));


		_main = GameObject.Find ("UIPanel").GetComponent<UIPanel> ().ui;

		initEncounterDetail ();



		monsterContianer = _main.GetChild ("monsters").asCom;

		hpTextView = _main.GetChild ("hp").asTextField;
		moneyTextView = _main.GetChild ("money").asTextField;
		sanTextView = _main.GetChild ("san").asTextField;
		levelTextView = _main.GetChild ("level").asTextField;

		showdetailButton = _main.GetChild ("detail_button").asGraph;
		//showdetailButton.url="detail";
		showdetailButton.onClick.Add (delegate(EventContext context) {
			if(!GRoot.inst.touchable){
				Debug.Log("不能点");
			}
			eMenu.Show();
			if (PlayerData.getInstance ().guideStage == 12) {
				GuideManager.getInstance ().showGuideTowerTab ();
				PlayerData.getInstance ().guideStage = 13;
			}
		});


		_enterBattle = _main.GetChild ("enterBattle").asLoader;
		_enterBattle.onTouchEnd.Add (delegate() {
			if(PlayerData.getInstance ().chasingEnemies.Count>0){
				clickEnterBattleButton();
			}
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

		exploreFinishWindow = new ExploreFinishWindow ();

		enterBattlePanel = new EnterBattlePanel ();
		getItemManager = new GetItemManager ();
		itemDetailAmplifier = new ItemDetailAmplifier ();

	}

	void initEncounterDetail(){
		Vector3 detailPanelPos = camera3D.ScreenToWorldPoint (new Vector3 (20f,camera3D.pixelHeight/2, 10));
		//detailPanelPos.z = 0;
		GameObject detailPanelGo = GameObject.Find ("EncounterDetailPanel");

		detailPanelGo.transform.position = detailPanelPos;
		detailPanel = detailPanelGo.GetComponent<UIPanel>().ui;
		detailPanel.visible = false;
		detailPanel.GetChild ("close").asLoader.onClick.Add (delegate() {
			detailPanel.visible = false;
		});
	}


	void checkBattleRes(){
		if (!PlayerData.getInstance ().isWaitingBattle||!PlayerData.getInstance().isFixedBattle) {
			return;
		}
		Debug.Log (PlayerData.getInstance ().beforeEid);
		EncounterInfo einfo = GameStaticData.getInstance ().getEncounterInfo (PlayerData.getInstance ().beforeEid);
		EncounterStage es = einfo.stages [PlayerData.getInstance ().beforeStage];
		int showIdx = -1;
		if (PlayerData.getInstance ().battleWin) {
			showIdx = es.converts [0].nextStageIdx;
		} else {
			showIdx = es.converts [1].nextStageIdx;
		}
		if (showIdx != -1) {
			sbManager.initEncounter (einfo.eId,showIdx);
			sbManager.RequestFocus();
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

		PlayerData.getInstance().enterNextLevel();
		if (PlayerData.getInstance ().nowLevelId == "end") {
			exploreFinishWindow.initAndShow();
			return;
		}
		//gridManager.initGrid();
		//resetStats();
		_mask.TweenFade (1, 0.5f).OnComplete (delegate() {
			gridManager.initGrid();
			resetStats();
			_mask.TweenFade(0,0.5f).OnComplete(delegate() {
				_mask.visible = false;
			});
		});
	}

	public void finishItemGet(){
		StartCoroutine(lateCheckIfBattle ());
	}


	public void initGetItemEffect(Vector2 originPosInGlobal,string itemId){
		NewItem copyView = (NewItem)UIPackage.CreateObject ("UIMain", "NewItem").asCom;
		copyView.init(itemId);
		showGetItemEffect (originPosInGlobal,copyView);
	}
	public void initGetItemEffect(List<string> itemIds){


		Vector2 center = new Vector2 (GRoot.inst.width/2,GRoot.inst.height/2);
		center.x -= (int)(itemIds.Count * 0.5f * 160);

		for(int i=0;i<itemIds.Count;i++){
			NewItem copyView = (NewItem)UIPackage.CreateObject ("UIMain", "NewItem").asCom;
			copyView.init(itemIds[i]);
			showGetItemEffect (center + new Vector2(160*i,i%2 * 10),copyView);
		}

	}

	void showGetItemEffect(Vector2 originPosInGlobal,NewItem itemCopy){
		GRoot.inst.AddChild (itemCopy);
		itemCopy.position = GRoot.inst.GlobalToLocal(new Vector2(originPosInGlobal.x - itemCopy.width/2, originPosInGlobal.y - itemCopy.height/2));
		itemCopy.sortingOrder = 100;
		itemCopy.touchable = false;
		itemCopy.alpha = 0.4f;
		//GRoot.inst.AddChild (copyView);


		itemCopy.TweenFade (1, 0.8f).OnComplete (delegate() {
			itemCopy.TweenMove (new Vector2 (0, GRoot.inst.height - itemCopy.height * 0.4f), 0.3f).OnUpdate (delegate(GTweener tweener) {
				itemCopy.InvalidateBatchingState ();
				float r = (tweener.deltaValue.vec2 - tweener.startValue.vec2).magnitude / (tweener.endValue.vec2 - tweener.startValue.vec2).magnitude;
				r = 1 - r * 0.6f;
				//r = r*0.6;
				itemCopy.scale = new Vector2 (r, r);
			}).OnComplete (delegate() {
				itemCopy.TweenFade (0, 0.2f).OnComplete (delegate() {
					GRoot.inst.RemoveChild (itemCopy);
					itemCopy.Dispose ();
				});
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

	public void chaseByEnemy(List<EnemyCombo> enemies){
		Vector2 center = new Vector2 (GRoot.inst.width/2,GRoot.inst.height/2);
		center.x -= (int)(enemies.Count * 0.5f * 140);
		for (int i = 0; i < enemies.Count; i++) {
			chaseByEnemy (enemies [i], center + new Vector2 (140 * i, i%2 * 20));
		}
	}

	public void chaseByEnemy(EnemyCombo enemy){
		Vector2 center = new Vector2 (GRoot.inst.width/2,GRoot.inst.height/2);
		chaseByEnemy (enemy, center);
	}

	public void chaseByEnemy(EnemyCombo enemyInfo,Vector2 originPos){
		PlayerData.getInstance ().addMonster (enemyInfo);
		addMonsterView (enemyInfo,originPos);
		//StartCoroutine (lateCheckIfBattle());
	}

	public void reArrangeEnemies(){

		for (int i = 0; i < monsterContianer.numChildren-1; i++) {
			monsterContianer.GetChildAt(i).TweenMove (new Vector2 (-20 * (monsterContianer.numChildren-1-i) - 100, -20 * (monsterContianer.numChildren-1-i) - 100), 0.2f);
			//monsters [i].SetXY (-20*i -100,-20*i-100);
		}
	}

	public void addMonsterView(EnemyCombo enemyInfo, Vector2 originPos){
		MonsterCardComponent item = (MonsterCardComponent)UIPackage.CreateObject ("UIMain", "MonsterCard").asCom;
		monsterContianer.AddChild (item);
		item.SetXY (-100,-100);
		//item.setName (enemyInfo.enemyId+"");
		item.setInfo (enemyInfo);
		item.visible = false;
		showAddMonsterEffect (originPos,monsterContianer.LocalToGlobal(item.position),item);
		reArrangeEnemies ();
	}

	IEnumerator lateCheckIfBattle(){
		lockUI ();
		//yield break;
		yield return new WaitForSecondsRealtime (1f);
		unlockUI ();
		if (PlayerData.getInstance ().chasingEnemies.Count > 3) {
			clickEnterBattleButton ();
		}
	}

	public void showAddMonsterEffect(Vector2 originPos,Vector2 toPosInGlobal,MonsterCardComponent item){
		lockUI ();
		MonsterCardComponent copyView = (MonsterCardComponent)UIPackage.CreateObject ("UIMain", "MonsterCard").asCom;
		//GTextField copyView = new GTextField();
		copyView.setInfo(item.info);
		//Vector2 f = _new_item_list.GetChildAt (idx).position;

		copyView.sortingOrder = 100;
		copyView.alpha = 0;
		GRoot.inst.AddChild (copyView);
		copyView.position = new Vector2(originPos.x - copyView.width/2,originPos.y - copyView.height/2);
		//_main.AddChild (copyView);
		copyView.TweenFade(1,0.4f).OnComplete(delegate() {
			copyView.TweenMove (_main.GlobalToLocal(toPosInGlobal),0.6f).OnUpdate(delegate(GTweener tweener) {
				copyView.InvalidateBatchingState();
				//float r = (tweener.deltaValue.vec2-tweener.startValue.vec2).magnitude/(tweener.endValue.vec2-tweener.startValue.vec2).magnitude;
				//r = 1-r*0.6f;
				//r = r*0.6;
				//copyView.scale = new Vector2(r,r);
			}).OnComplete(delegate() {

				//monsterContianer.GetChildAt(monsterContianer.numChildren-1).visible = true;
				item.visible = true;
				unlockUI();
				copyView.TweenFade(0,0.2f).OnComplete(delegate() {
					//_main.RemoveChild(copyView);
					GRoot.inst.RemoveChild (copyView);
					copyView.Dispose();
					_enterBattle.visible = true;

				});
			});
		});

	}

	public void enterShop(){
		shopPanel.initShop ();
		shopPanel.Show ();
	}


	public void showDetailAmplifier(string type,object content){
		itemDetailAmplifier.setInfo (type,content);
		itemDetailAmplifier.Show ();
	}

	void clickEnterBattleButton(){
		clearAllEnemy ();
		//initAndShow
		enterBattlePanel.initAndShow(delegate() {
			PlayerData.getInstance ().initBattle ();
			GameManager.getInstance ().enterBattle ();
		});
		_enterBattle.visible = false;
	}

	//  private AsyncOperation async = null;
	//	IEnumerator LoadBattle()
	//	{
	//		async = SceneManager.LoadSceneAsync("A1");
	//		yield return async;
	//		//_main.visible = false;
	//	}

	public void enterBattle(){
		if (!inBattle) {
			Load.SceneName = "Battle";//B场景的名字 
			SceneManager.LoadScene("Loading"); 

			//StartCoroutine (LoadBattle());
			inBattle = true;
			mainCamera.gameObject.SetActive (false);
		}
	}

	public void showHint(string txt="提示"){
		
		HintComponent hint = (HintComponent)UIPackage.CreateObject ("UIMain", "Hint").asCom;
		GRoot.inst.AddChild (hint);
		hint.Center ();
		hint.init (txt);
	}

	public void getNewScar(List<Scar> scars){
		PlayerData.getInstance ().scars.AddRange (scars);
		List<string> ss = new List<string> ();
		ss.Add ("s01");
		ss.Add ("s02");
		ss.Add ("s03");
		initGetItemEffect (ss);
	}


	int uiLockNum = 0;
	public void lockUI(){
		uiLockNum++;
		if (uiLockNum > 0) {
			GRoot.inst.touchable = false;
			_main.touchable = false;
		}
	}

	public void unlockUI(){
		uiLockNum--;
		if (uiLockNum == 0) {
			GRoot.inst.touchable = true;
			_main.touchable = true;
		}
	}



}
