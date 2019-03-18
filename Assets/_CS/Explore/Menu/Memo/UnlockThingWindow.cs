using UnityEngine;
using System.Collections;
using FairyGUI;

public class UnlockThingWindow : Window
{

	GObject _lockObj;
	GButton _confirm;
	Transition shakeLock;
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject ("UIMain", "UnlockEncounterWindow").asCom;
		this.Center ();
		this.modal = true;
		_lockObj = this.contentPane.GetChild ("n0");
		shakeLock = this.contentPane.GetTransition ("t0");
		_confirm = this.contentPane.GetChild ("n2").asButton;

		_confirm.onTouchBegin.Add (delegate() {
			Hide ();
		});

	}

	protected override void OnShown(){
		_lockObj.visible = true;
		this.contentPane.GetController("c1").SetSelectedIndex(0);
		shakeLock.Play (delegate() {
			_lockObj.visible = false;
			this.contentPane.GetController("c1").SetSelectedIndex(1);
		});
	}
}

