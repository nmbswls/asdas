using UnityEngine;
using System.Collections;
using FairyGUI;

public class TowerDamageItem : GComponent
{
	GLoader _icon;
	GTextField _num;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		_icon = this.GetChild ("type").asLoader;
		_num = this.GetChild ("v").asTextField;
	}

}

