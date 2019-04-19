using UnityEngine;
using System.Collections;
using FairyGUI;

public class ComponentView : GComponent
{
	GLoader cIcon;

	GGraph _detail_bg;
	GTextField _detail;

	public int idx;
	public TowerPanel towerPanel;

	public TowerComponent tc;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		//this.onClick.Add (unequip);
		cIcon = this.GetChild ("icon").asLoader;
		//this.onDrop.Add (replaceComponent);
		_detail = this.GetChild("detail").asTextField;
		_detail_bg = this.GetChild ("detail_bg").asGraph;

	}

	public void setInfo(TowerComponent tc){
		this.tc = tc;
		if (tc == null) {
			cIcon.url = GameConstant.TOWER_COMPONENT_ICON_PATH+"empty";
			_detail.text = "";
		} else {
			cIcon.url = GameConstant.TOWER_COMPONENT_ICON_PATH + tc.cid;
			_detail.text = tc.cname + "\n" + tc.getEffects();
		}

	}
	public void showDetail(){
		if (tc != null) {
			_detail.visible = true;
			_detail_bg.visible = true;
		}

	}
	public void hideDetail(){
		_detail.visible = false;
		_detail_bg.visible = false;
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

