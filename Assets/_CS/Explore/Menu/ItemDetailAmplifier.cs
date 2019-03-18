using UnityEngine;
using System.Collections;
using FairyGUI;

public class ItemDetailAmplifier : Window
{
	GLoader close;
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "ItemDetail").asCom;
		this.Center();
		this.modal = true;
		close = this.contentPane.GetChild("close").asLoader;
		close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});
	}
	string info;
	public void setINfo(string info){
		this.info = info;
	}
	protected override void OnShown(){
	
	}
}

