using UnityEngine;
using System.Collections;
using FairyGUI;

public class NewItem : GButton
{
	GTextField _txt;
	GLoader _icon;

	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		_txt = this.GetChild ("text").asTextField;
		_icon = this.GetChild ("pic").asLoader;
	}

	public void init(string itemId){
		if (itemId.StartsWith ("t")) {
			TowerBase tb = GameStaticData.getInstance ().getTowerBase (itemId);
			_icon.url = "";
			_txt.text = tb.tname + '\n' + tb.tdesp;
		}else if(itemId.StartsWith ("c")){
			TowerComponent tc = GameStaticData.getInstance ().getComponentInfo (itemId);
			_icon.url = "";
			_txt.text = tc.cname + '\n' + tc.cdesp;
		}else{
			Debug.Log (itemId);
			_txt.text = "要水货伤痕p";
		}

	}
}

