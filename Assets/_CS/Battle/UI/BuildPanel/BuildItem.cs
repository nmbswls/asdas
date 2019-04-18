using UnityEngine;
using System.Collections;
using FairyGUI;

public class BuildItem : GButton
{

	TowerTemplate info;
	GComponent _pic_container;
	GLoader _pic;
	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		_pic_container = this.GetChild ("pic").asCom;
		_pic = _pic_container.GetChild ("icon").asLoader;
	}


	public void setTowerInfo(TowerTemplate info){
		this.info = info;
		_pic.url = "image/TowerBigPicture/"+info.tbase.tid;
	}

	public TowerTemplate getTTInfo(){
		return info;
	}

}

