using UnityEngine;
using System.Collections;
using FairyGUI;

public class PropertyModify : GComponent
{

	Controller _state;

	GImage _up;
	GImage _down;

	GTextField _v;
	// Use this for initialization
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		_state = this.GetController ("change");
		_up = this.GetChild ("up").asImage;
		_down = this.GetChild ("down").asImage;
		_v = this.GetChild ("v").asTextField;
	}

	public void setValue(string change){
		_up.visible = false;
		_down.visible = false;
		this.visible = true;
		if (change == "0" || change == "0.0") {
			this.visible = false;
			return;
		}

		if (change.StartsWith ("-")) {
			
			_down.visible = true;
		} else {
			_up.visible = true;
		}
		_v.text = change;
	}
}

