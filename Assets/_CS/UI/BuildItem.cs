using UnityEngine;
using System.Collections;
using FairyGUI;

public class BuildItem : GButton
{

	string name;

	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);


	}

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}
}

