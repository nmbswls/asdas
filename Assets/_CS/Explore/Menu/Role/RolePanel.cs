using UnityEngine;
using System.Collections;
using FairyGUI;

public class RolePanel : GComponent
{
	GList l0;
	GList l1;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		l0 = this.GetChild ("n21").asList;
		l1 = this.GetChild ("n24").asList;


	}
}

