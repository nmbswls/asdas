using UnityEngine;
using System.Collections;
using FairyGUI;

public class ExploreMenu : Window
{

	RolePanel rolePanel;
	TowerPanel towerPanel;
	MemoPanel memoPanel;

	GComponent roleTab;
	GComponent towerTab;
	GComponent memoTab;

	GLoader _close;
	protected override void OnInit()
	{


		this.contentPane = UIPackage.CreateObject("UIMain", "ExploreMenu").asCom;
		this.Center();
		this.modal = true;

		rolePanel = (RolePanel)this.contentPane.GetChild("p1").asCom;
		towerPanel = (TowerPanel)this.contentPane.GetChild ("p2").asCom;
		memoPanel = (MemoPanel)this.contentPane.GetChild ("p3").asCom;

		roleTab = this.contentPane.GetChild ("role_tab").asCom;
		towerTab = this.contentPane.GetChild ("tower_tab").asCom;
		memoTab = this.contentPane.GetChild ("memo_tab").asCom;

		towerTab.onClick.Add (delegate() {
			if (PlayerData.getInstance ().guideStage == 13) {
				GuideManager.getInstance ().showGuideFirstTower ();
				PlayerData.getInstance ().guideStage = 14;
			}
		});

		//this.MakeFullScreen();
		//this.width = GRoot.inst.width;
		this.SetXY (GRoot.inst.width/2 - this.width/2, 0);
		//this.width = 1136;
		//this.height = 640;
		//this.width = GRoot.inst.width;
		//this.height = GRoot.inst.height;
		//GRoot.inst.SetContentScaleFactor(1136,640,FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
		_close = this.contentPane.GetChild("close").asLoader;
		//_close.url="detail";
		_close.onClick.Add (delegate(EventContext context) {
			this.Hide();
			if (PlayerData.getInstance ().guideStage == 15) {
				GuideManager.getInstance ().hideGuide ();
				PlayerData.getInstance ().guideStage = -1;
				PlayerPrefs.SetInt ("isFirstGame",0);
			}
		});

	}

	protected override void OnShown(){
		memoPanel.initMemoState ();
		rolePanel.updateView ();
	}

	public GObject getTowerTab(){
		return towerTab;
	}

	public GObject getCloseButton(){
		return _close;
	}

	public GObject getFirstTower(){
		return towerPanel.getFirstTower ();
	}
}

