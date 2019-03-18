using UnityEngine;
using System.Collections;
using FairyGUI;

public class AccesoryView : GComponent
{

	GLoader icon;
	int idx;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		icon = this.GetChild ("icon").asLoader;
		this.draggable = true;
		//this.onDragStart.Add(DragStart);
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
}

