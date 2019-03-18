using UnityEngine;
using System.Collections;
using FairyGUI;

public delegate void ConfirmCallback();
public class EnterBattlePanel : Window
{
	GButton _confirm;
	ConfirmCallback callback;
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject ("UIMain", "EnterBattleWindow").asCom;
		this.Center ();
		this.modal = true;

		_confirm = this.contentPane.GetChild ("confirm").asButton;
		_confirm.onTouchBegin.Add (delegate() {
			if(callback!=null){
				callback.Invoke();
			}
			Hide();
		});
	}

	public void initAndShow(ConfirmCallback callback){
		this.callback = callback;
		Show ();
	}


}

