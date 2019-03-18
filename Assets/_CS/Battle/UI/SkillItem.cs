using UnityEngine;
using System.Collections;
using FairyGUI;

public class SkillItem : GComponent
{

	GLoader _icon;
	GTextField _cdText;
	GImage _mask;

	//string iconUrl = "";
	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);

		_icon = this.GetChild ("icon").asLoader;
		_cdText = this.GetChild ("cd").asTextField;
		_mask = this.GetChild ("mask").asImage;

	}

	public void updateCd(int time){
		if (time <= 0) {
			_mask.visible = false;
			_cdText.visible = false;
		} else {
			_mask.visible = true;
			_mask.fillAmount = time / 5000f;
			_cdText.visible = true;
			_cdText.text = time/1000 + "s";
		}
	}


	public void setIcon(string iconName){
		_icon.url = "image/"+iconName;
	}


}

