using UnityEngine;
using System.Collections;
using FairyGUI;

public class ShowClickMask : GComponent
{
	GMovieClip effect;
	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		//effect = this.GetChild("effect").asMovieClip;

		//this.onClick.Add(OnClick);

	}


}

