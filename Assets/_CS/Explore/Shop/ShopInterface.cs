using UnityEngine;
using System.Collections;
using FairyGUI;

public class ShopInterface : Window
{

	GLoader close;
	protected override void OnInit()
	{


		this.contentPane = UIPackage.CreateObject ("UIMain", "ShopInterface").asCom;
		this.Center ();
		this.modal = true;


		close = this.contentPane.GetChild("close").asLoader;
		close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});
	}
}

