using UnityEngine;
using System.Collections;
using FairyGUI;


public enum eGuideStage{
	START = 0,
	MOVE_MARK = 1,
	MOVE_PLAYER = 2,
	CHOOSE_BRANCH = 3,
	CHOOSE_REWARD = 4,
	CHOOSE_REWARD_CONFIRM = 5,
	DETAIL_MENU = 6,
	TOWER_PANEL = 7,
	CHOOSE_ONE_TOWER = 8,
	CLOSE_DETAIL_MANU = 9,
}

public class GuideManager : Singleton<GuideManager>
{

	//GComponent _mainView;
	public GuideLayer _guideLayer;

	GuidePanel guidePanel;

	GameManager gm;
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (this);
		gm = GameManager.getInstance ();

	}
	bool init = false;
	void Init(){
		UIObjectFactory.SetPackageItemExtension("ui://UIMain/GuideLayer", typeof(GuideLayer));
		_guideLayer = (GuideLayer)UIPackage.CreateObject("UIMain", "GuideLayer").asCom;
		_guideLayer.SetSize(GRoot.inst.width, GRoot.inst.height);
		_guideLayer.AddRelation(GRoot.inst, RelationType.Size);
		init = true;

		guidePanel = new GuidePanel ();
	}

	public void finishGuide(){
		_guideLayer.RemoveFromParent();
	}

	public void showGuide(GObject target, string hint = "select one of branches", string arrowPos = "rt"){
		if (!init) {
			Init ();
		}
		_guideLayer.setWindowUntouchable ();
		GRoot.inst.AddChild(_guideLayer); //!!Before using TransformRect(or GlobalToLocal), the object must be added first
		//Rect rect = target.TransformRect(new Rect(0, 0, target.width, target.height), _guideLayer);
		Rect rect = target.TransformRect(new Rect(0, 0, target.width, target.height), GRoot.inst);
		if (arrowPos == "rt") {
			_guideLayer.setStyleRt ();
		} else {
			_guideLayer.setStyleLb ();
		}

		_guideLayer.setWindow (rect);
		_guideLayer.setHint (hint);
	}

	public void showGuide(Rect rectGlobal,string hint = "write hint here",string arrowPos = "rt"){
		if (!init) {
			Init ();
		}
		_guideLayer.setWindowUntouchable ();
		GRoot.inst.AddChild(_guideLayer); //!!Before using TransformRect(or GlobalToLocal), the object must be added first
		//Rect rect = target.TransformRect(new Rect(0, 0, target.width, target.height), _guideLayer);

		float width = rectGlobal.size.x/2;
		float height = rectGlobal.size.y/2;
		Vector2 center = _guideLayer.GlobalToLocal (rectGlobal.center);

		if (arrowPos == "rt") {
			_guideLayer.setStyleRt ();
		} else {
			_guideLayer.setStyleLb ();
		}

		_guideLayer.setWindow (new Rect(center.x - width,center.y - height,2*width,2*height));
		_guideLayer.setHint (hint);
	}



	public void showGuide(Vector2 center, int width,string hint = "write hint here",string arrowPos = "rt"){
		if (!init) {
			Init ();
		}
		_guideLayer.setWindowUntouchable ();
		GRoot.inst.AddChild(_guideLayer); //!!Before using TransformRect(or GlobalToLocal), the object must be added first
		//Rect rect = target.TransformRect(new Rect(0, 0, target.width, target.height), _guideLayer);

		_guideLayer.GetChild ("mark").SetXY (center.x,center.y);;

		if (arrowPos == "rt") {
			_guideLayer.setStyleRt ();
		} else {
			_guideLayer.setStyleLb ();
		}



		_guideLayer.setWindow (new Rect(center.x - width,center.y - width,2*width,2*width));
		_guideLayer.setHint (hint);
	}




	public void showGuideTouchable(Vector2 center, int width,EventCallback0 callback,string hint = "write hint here",string arrowPos = "rt"){
		if (!init) {
			Init ();
		}
		_guideLayer.setWindowTouchable (callback);

		GRoot.inst.AddChild(_guideLayer); //!!Before using TransformRect(or GlobalToLocal), the object must be added first
		//Rect rect = target.TransformRect(new Rect(0, 0, target.width, target.height), _guideLayer);

		_guideLayer.GetChild ("mark").SetXY (center.x,center.y);;

		if (arrowPos == "rt") {
			_guideLayer.setStyleRt ();
		} else {
			_guideLayer.setStyleLb ();
		}



		_guideLayer.setWindow (new Rect(center.x - width,center.y - width,2*width,2*width));
		_guideLayer.setHint (hint);
	}


	//juti gongneng
	public void showGuideDetail(){
		GuideManager.getInstance ().showGuide (gm.showdetailButton,"Check What you have Here","rt");
	}

	public void hideGuide(){
		GuideManager.getInstance ().finishGuide ();
	}
	public void showGuideMovePlayer(Vector2 screenPos){
		screenPos.y = Screen.height - screenPos.y;
		Vector2 pos = GRoot.inst.GlobalToLocal(screenPos);
		GuideManager.getInstance ().showGuide (pos,80," Touch Again \n Move To Reveal ");
	}

	public void showGuideMoveMark(Vector2 screenPos){
		screenPos.y = Screen.height - screenPos.y;
		Vector2 pos = GRoot.inst.GlobalToLocal(screenPos);
		GuideManager.getInstance ().showGuide (pos,80," Touch \n Check One Block ");
	}

	public void showGuideChooseBranch(){
		GuideManager.getInstance ().showGuide (gm.sbManager.getFirstBranch(),"Select Either Branches");
	}

	public void showGuideChooseItem(){
		GuideManager.getInstance ().showGuide (gm.getItemManager.getFirstChoice(),"Select One ");
	}
	public void showGuideConfirmChooseItem(){
		GuideManager.getInstance ().showGuide (gm.getItemManager.getConfirmButton(),"Select Either Branches");
	}

	public void showGuideTowerTab(){
		GuideManager.getInstance ().showGuide (gm.eMenu.getTowerTab(),"Enter Tower Panel\nSee What You Have","lb");
	}

	public void showGuideFirstTower(){
		GuideManager.getInstance ().showGuide (gm.eMenu.getFirstTower(),"Touch One Your Tower\nSee The Detail");
	}

	public void showGuideCloseMenu(){
		GuideManager.getInstance ().showGuide (gm.eMenu.getCloseButton(),"Close Panel \n Continue Explore","lb");
	}





	public void showGuidePanel(){
		if (!init) {
			Init ();
		}
		guidePanel.Show ();
	}
}

