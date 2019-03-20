using UnityEngine;
using System.Collections;
using FairyGUI;

public class ExploreMenu : Window
{

	RolePanel rolePanel;
	TowerPanel towerPanel;
	MemoPanel memoPanel;

	GLoader close;
	protected override void OnInit()
	{


		this.contentPane = UIPackage.CreateObject("UIMain", "ExploreMenu").asCom;
		this.Center();
		this.modal = true;

		rolePanel = (RolePanel)this.contentPane.GetChild("p1").asCom;
		towerPanel = (TowerPanel)this.contentPane.GetChild ("p2").asCom;
		memoPanel = (MemoPanel)this.contentPane.GetChild ("p3").asCom;
		//this.MakeFullScreen();
		//this.width = GRoot.inst.width;
		this.SetXY (GRoot.inst.width/2 - this.width/2, 0);
		//this.width = 1136;
		//this.height = 640;
		//this.width = GRoot.inst.width;
		//this.height = GRoot.inst.height;
		//GRoot.inst.SetContentScaleFactor(1136,640,FairyGUI.UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
		close = this.contentPane.GetChild("close").asLoader;
		close.url="detail";
		close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});

	}

	protected override void OnShown(){
		memoPanel.initMemoState ();
		rolePanel.updateView ();
	}
}

