using UnityEngine;
using System.Collections;
using FairyGUI;

public class ComponentView : GComponent
{
	GLoader cIcon;

	public int idx;
	public TowerPanel towerPanel;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		//this.onClick.Add (unequip);
		cIcon = this.GetChild ("icon").asLoader;
		//this.onDrop.Add (replaceComponent);

	}

	public void setInfo(TowerComponent tt){
		if (tt == null) {
			cIcon.url = "Equips/empty";
		} else {
			cIcon.url = "Equips/" + tt.cid;
		}
	}

//	public void replaceComponent(EventContext context){
//		context.StopPropagation ();
//		int idxInBag = (int)context.data;
//		//towerPanel.changeComponent (idx, idxInBag);
//	}

//	public void unequip(EventContext context){
//		towerPanel.unequip (idx);
//	}
		
}

