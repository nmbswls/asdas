using UnityEngine;
using System.Collections;
using FairyGUI;

public class ItemWithDetail : GComponent
{


	int type;
	int c;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		this.onTouchBegin.Add (delegate() {
			Debug.Log("click");
			GameManager.getInstance().showDetailAmplifier("sassass");
		});
	}
}

