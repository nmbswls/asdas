﻿using UnityEngine;
using System.Collections;
using FairyGUI;

public class ItemDetailAmplifier : Window
{
	GLoader close;
	GTextField _desp;
	GLoader _pic;
	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("UIMain", "ItemDetail").asCom;
		this.Center();
		this.modal = true;

		_desp = this.contentPane.GetChild("desp").asTextField;
		_pic = this.contentPane.GetChild ("pic").asLoader;

		close = this.contentPane.GetChild("close").asLoader;
		close.onClick.Add (delegate(EventContext context) {
			this.Hide();
		});
	}
	string info;
	public void setInfo(string info){
		this.info = info;

	}


	protected override void OnShown(){
		_desp.text = info;
		//_pic.url = "";
	}
}

