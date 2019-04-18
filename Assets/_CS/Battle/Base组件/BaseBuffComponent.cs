using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;

public class Buff{
	public int buffId;
	public int buffLevel;
	public int totalLastTime;
	public int leftTime;
	public Buff(int buffId,int buffLevel,int totalLastTime){
		this.buffId = buffId;
		this.buffLevel = buffLevel;
		this.totalLastTime = totalLastTime;
		this.leftTime = totalLastTime;
	}
	public Buff(){
	}
}

public class BaseBuffComponent : MonoBehaviour
{

	public bool withUI = true;

	protected List<Buff> buffs = new List<Buff>();


	GList _buffs_view;

	void Start ()
	{
		if (withUI) {
			Transform headBar = transform.Find ("HeadBar");
			if (headBar != null) {
				UIPanel panel = transform.Find("HeadBar").GetComponent<UIPanel>();
				_buffs_view = panel.ui.GetChild ("buffs").asList;
			}
		}
	}





	public void addBuff(Buff buff){
		buffs.Add (buff);
		if (withUI) {
			GComponent item = (GComponent)_buffs_view.AddItemFromPool ("ui://MHeadBar/BuffItem");
			GLoader loader = item.GetChild ("icon").asLoader;
			loader.url = "image/atk";
		}
	}

	public void Tick (int timeInt)
	{
		foreach (Buff buff in buffs) {
			buff.leftTime -= timeInt;
		}
		int i = 0;
		while (i < buffs.Count) {
			if (buffs [i].leftTime <= 0) {
				removeBuff (i);
				continue;
			}
			i++;
		}
		afterTick ();
	}

	protected virtual void afterTick(){
	}

	void removeBuff(int idx){
		buffs.RemoveAt (idx);
		if (withUI) {
			_buffs_view.RemoveChildToPoolAt (idx);
		}
	}

	public void reArrangeBuff(){
		
	}



	public void updateBuffedView(){
		
	}

	public void showBuffEffect(string buffName){
		//sprite jia effect
	}


}

