using UnityEngine;
using System.Collections;
using FairyGUI;

public class ScarSmallIcon : GComponent
{

	Scar scar;

	GLoader _icon;
	GTextField _num;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		_icon = this.GetChild ("icon").asLoader;
		_num = this.GetChild ("num").asTextField;

		this.onTouchBegin.Add (delegate() {
			GameManager.getInstance().showDetailAmplifier("scar",scar);
		});
	}

	public void setInfo(Scar scar){
		this.scar = scar;
		ScarStaticInfo sinfo = GameStaticData.getInstance ().getScarInfo (scar.scarId);
		_num.text = scar.value+"";
		_icon.url = "image/Scar/"+sinfo.scarId;
	}
}

