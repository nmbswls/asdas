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

public class BuffComponent : MonoBehaviour
{

	public GameLife bindTarget;
	public bool withUI = true;

	public List<Buff> buffs = new List<Buff>();
	//public List<BuffItem> buffIcons = new List<BuffItem>();

	// Use this for initialization

	GList _buffs_view;
	public GComponent hpBar;
	void Start ()
	{
		if (withUI) {
			Transform headBar = transform.Find ("HeadBar");
			if (headBar!=null) {
				UIPanel panel = transform.Find("HeadBar").GetComponent<UIPanel>();
				_buffs_view = panel.ui.GetChild ("buffs").asList;
				hpBar = panel.ui.GetChild ("n0").asCom;
				_buffs_view.itemRenderer = RenderBuffList;

				//			for (int i = 0; i < 2; i++)
				//			{
				//				GComponent item = (GComponent)_list.AddItemFromPool("ui://MHeadBar/BuffItem");
				//				GLoader loader = item.GetChild ("icon").asLoader;
				//				loader.url = "image/atk";
				//			}
			}
		}
	}

	public float getSpeedRate(){
		float rate = 1f;
		foreach (Buff buff in buffs) {
			if (buff.buffId == 1) {
				int level = buff.buffLevel;
				rate *= (1-level / 100f);
			}
		}
		return rate;
	}

	public int getAtkChange(){
		int change = 0;
		foreach (Buff buff in buffs) {
			if (buff.buffId == 10) {
				int level = buff.buffLevel;
				change += level;
			}else if (buff.buffId == 11) {
				int level = buff.buffLevel;
				change -= level;
			}

		}
		return change;
	}

	public int getDefChange(){
		int change = 0;
		foreach (Buff buff in buffs) {
			if (buff.buffId == 20) {
				int level = buff.buffLevel;
				change += level;
			}else if (buff.buffId == 21) {
				int level = buff.buffLevel;
				change -= level;
			}

		}
		return change;
	}


	public void OnEnable(){

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
	}

	void removeBuff(int idx){
		buffs.RemoveAt (idx);
		if (withUI) {
			_buffs_view.RemoveChildToPoolAt (idx);
		}
	}

	public void reArrangeBuff(){
		
	}
	void OnDisable(){
		bindTarget = null;
	}




	void RenderBuffList(int index, GObject obj)
	{
		//GComponent bi = (GComponent)obj;
		//GLoader loader = bi.GetChild ("n0").asLoader;

		//bi.position - _list.width;
		//		button.icon = "i" + UnityEngine.Random.Range(0, 10);

	}

	public void updateBuffedView(){
		
	}
}

