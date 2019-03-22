using UnityEngine;
using System.Collections;
using FairyGUI;

public class AccesoryView : GButton
{

	GLoader _icon;
	GTextField _name;
	public GGraph _bg;
	int idx;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		_icon = this.GetChild ("icon").asLoader;
		_name = this.GetChild ("name").asTextField;
		_bg = this.GetChild ("bg").asGraph;
	}

	public void updateView(TowerComponent tc){

		if (tc == null) {
			_icon.url = "Equips/empty";
			_name.text = "无";

		} else {
			_icon.url = "Equips/" + tc.cid;
			_name.text = tc.cname;
		}


	}

	public void setUneuip(TowerComponent tc){
		_icon.url = "image/atk";
		_name.text = "卸下";
	}





//	public void setInfo(TowerComponent tt,int idx){
//		if (tt == null) {
//			icon.url = "Equips/empty";
//		} else {
//			icon.url = "Equips/" + tt.cid;
//		}
//	}
//	void DragStart(EventContext context)
//	{
//		GComponent item = (GComponent)context.data;
//		//取消掉源拖动
//		context.PreventDefault();
//		//icon是这个对象的替身图片，userData可以是任意数据，底层不作解析。context.data是手指的id。
//		DragDropManager.inst.StartDrag(null, "Equips/staff", (object)"", (int)context.data);
//	}
	public void resetClick(EventCallback0 callback){
		this.onClick.Clear ();
		this.onClick.Add (__click);
		this.onClick.Add (callback);
	}
}

