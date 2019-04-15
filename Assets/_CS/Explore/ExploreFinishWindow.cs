using UnityEngine;
using System.Collections;
using FairyGUI;

public class ExploreFinishWindow : Window
{

	GButton _confirm;
	//ConfirmCallback callback;
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject ("UIMain", "ExploreFinishPanel").asCom;
		this.Center ();
		this.modal = true;

		_confirm = this.contentPane.GetChild ("confirm").asButton;
		_confirm.onTouchBegin.Add (delegate() {
			Hide();
		});
	}

	public void initAndShow(){
		//this.callback = callback;
		Show ();
	}
}

