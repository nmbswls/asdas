using UnityEngine;
using System.Collections;
using FairyGUI;

public class ItemWithDetail : GComponent
{


	string desp;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		this.onTouchBegin.Add (delegate() {
			Debug.Log("click");

			GameManager.getInstance().showDetailAmplifier(desp);
		});
	}

	public void setDesp(string desp){
		this.desp = desp;
	}
}

