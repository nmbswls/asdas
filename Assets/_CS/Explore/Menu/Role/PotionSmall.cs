using UnityEngine;
using System.Collections;
using FairyGUI;

public class PotionSmall : GComponent
{

	Potion potion;


	GLoader _icon;


	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);

		_icon = this.GetChild ("icon").asLoader;

		this.onTouchBegin.Add (delegate() {
			Debug.Log("click");
			GameManager.getInstance().showDetailAmplifier("potion",potion);
		});
	}


	public void setInfo(Potion potion){
		this.potion = potion;
		_icon.url = "image/Potion/"+potion.pid;
	}
}

