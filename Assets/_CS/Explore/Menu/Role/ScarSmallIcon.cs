using UnityEngine;
using System.Collections;
using FairyGUI;

public class ScarSmallIcon : GComponent
{

	string desp;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		this.onTouchBegin.Add (delegate() {
			GameManager.getInstance().showDetailAmplifier("减少生命上限");
		});
	}

	public void setDesp(string desp){
		this.desp = desp;
	}
}

