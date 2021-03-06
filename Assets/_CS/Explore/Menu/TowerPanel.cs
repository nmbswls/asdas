﻿using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;




public class TowerPanel : GComponent
{

	GList towers;
	//GList tComponents;
	ComponentView[] components = new ComponentView[5];

	//GLoader movingIcon;
	TowerPropertyAfter _property;

	GButton _switch_detail;
	bool isDetailShown = false;

	int towerIdx = -1;

	int nowDragIdx = -1;
	Vector2 beforeDragPos = Vector2.zero;
	bool isDropOnSlot = false;

	TowerComponentChooseWindow towerComponentWindow;

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML(xml);
		components[0] = (ComponentView)this.GetChild ("c0").asCom;
		components[1] = (ComponentView)this.GetChild ("c1").asCom;
		components[2] = (ComponentView)this.GetChild ("c2").asCom;
		components[3] = (ComponentView)this.GetChild ("c3").asCom;

		for (int i = 0; i < 4; i++) {
			components [i].idx = i;
			components [i].towerPanel = this;
			components [i].visible = false;
			int idx = i;
			components [i].onTouchEnd.Add (delegate() {
				clickComponent(idx);
			});
		}

		towers = this.GetChild ("towers").asList;

		_switch_detail = this.GetChild ("switch").asButton;
		_switch_detail.selected = true;
		_switch_detail.onClick.Add (delegate() {
			isDetailShown = !isDetailShown;
			if (isDetailShown) {
				for(int i=0;i<4;i++){
					components[i].showDetail();
				}
			} else {
				for(int i=0;i<4;i++){
					components[i].hideDetail();
				}
			}
		});
		//tComponents = this.GetChild ("components").asList;

		//movingIcon = this.GetChild ("moving_icon").asLoader;
		//movingIcon.sortingOrder = int.MaxValue;

		towers.onClickItem.Add(clickTower);
		towers.itemRenderer = RenderTowers;
		towers.numItems = PlayerData.getInstance ().ownedTowers.Count;

		//tComponents.foldInvisibleItems = true;
		//tComponents.itemRenderer = RenderComponents;
		//tComponents.numItems = PlayerData.getInstance ().bagComponents.Count;
		//tComponents.EnsureBoundsCorrect();

//		DragDropManager.inst.dragAgent.onDragEnd.Add(delegate(EventContext context2) {
//			if(!isDropOnSlot)
//				tComponents.GetChildAt(nowDragIdx).visible = true;
//			tComponents.numItems = PlayerData.getInstance ().bagComponents.Count;
//			nowDragIdx = -1;
//		});

		_property = (TowerPropertyAfter)this.GetChild ("property").asCom;
		_property.visible = false;

		towerComponentWindow = new TowerComponentChooseWindow (this);

	}

	void RenderTowers(int index, GObject obj){
		GButton item = obj.asButton;
		string name = PlayerData.getInstance ().ownedTowers [index].tbase.tname;
		item.GetChild("name").asTextField.text = name;
		//Be carefull, RenderListItem is calling repeatedly, dont call 'Add' here!
//		item.onClick.Set(delegate(EventContext context) {
//			
//		});
	}
//	void RenderComponents(int index, GObject obj){
//		GComponent item = obj.asCom;
//		item.visible = true;
//		GLoader icon = item.GetChild ("icon").asLoader;
//
//		TowerComponent tc = PlayerData.getInstance ().bagComponents [index];
//		if (tc == null) {
//			icon.url = "Equips/empty";
//		} else {
//			icon.url = "Equips/" + tc.cid;
//		}
//		item.onTouchEnd.Add (delegate() {
//			GameManager.getInstance().showDetailAmplifier(tc.cdesp);
//		});
//		item.draggable = true;
//
//		item.onDragStart.Set(delegate(EventContext context) {
//			context.PreventDefault();
//			DragDropManager.inst.StartDrag(item, "Equips/"+tc.cid, (object)(index), (int)context.data);
//			startDragBagItem(index);
//		});
//
//		//string name = PlayerData.getInstance ().bagComponents [index].cname;
//	}
		
//	void startDragBagItem(int idx){
//		beforeDragPos = tComponents.LocalToGlobal(tComponents.GetChildAt(idx).position);
//		isDropOnSlot = false;
//		//tComponents.GetChildAt(index).visible = false;
//		for (int i = idx+1; i < tComponents.numItems; i++) {
//			Vector2 startPos = tComponents.GetChildAt (i).position;
//			Vector2 endPos = tComponents.GetChildAt (i-1).position;
//			tComponents.GetChildAt (i).TweenMove (endPos,0.3f);
//		}
//		tComponents.RemoveChildAt(idx);
//		nowDragIdx = idx;
//		tComponents.EnsureBoundsCorrect ();
//	}


	void clickTower(EventContext context){
		GComponent item = (GComponent)context.data;
		int idx = towers.GetChildIndex (item);
		towerIdx = idx;
		TowerTemplate tt = PlayerData.getInstance ().ownedTowers[idx];
		for (int i = 0; i < 4; i++) {
			components [i].visible = true;
			if (tt.components [i] == null) {
				components[i].setInfo (null);
			} else {
				components[i].setInfo (tt.components[i]);
			}
		}
		_property.visible = true;
		updateCalcedProperty (tt);

		if (PlayerData.getInstance ().guideStage == 14) {
			GuideManager.getInstance ().showGuideCloseMenu ();
			PlayerData.getInstance ().guideStage = 15;
		}
	}

	void clickComponent(int idx){
		TowerTemplate tt = PlayerData.getInstance ().ownedTowers [towerIdx];
		towerComponentWindow.SetAndShow (idx,tt.components [idx]);
	}

//	public void changeComponent (int componentIdx, int bagSlot){
//		isDropOnSlot = true;
//		if (towerIdx < 0)
//			return;
//		TowerTemplate tt = PlayerData.getInstance ().ownedTowers [towerIdx];
//		TowerComponent toEquip = PlayerData.getInstance ().bagComponents [bagSlot];
//		TowerComponent equipped = tt.components [componentIdx];
//
//		tt.components [componentIdx] = toEquip;
//		components [componentIdx].setInfo (tt.components[componentIdx]);
//
//		if (equipped != null) {
//			
//			movingIcon.url = "Equips/" + equipped.cid;
//			movingIcon.xy = components [componentIdx].xy;
//			movingIcon.visible = true;
//
//
//			PlayerData.getInstance ().bagComponents [bagSlot] = equipped;
//			tComponents.numItems = PlayerData.getInstance ().bagComponents.Count;
//			tComponents.GetChildAt (bagSlot).visible = false;
//			tComponents.EnsureBoundsCorrect ();
//			for (int i = tComponents.numItems-1; i > bagSlot; i--) {
//				Vector2 startPos = tComponents.GetChildAt (i - 1).position;
//				Vector2 endPos = tComponents.GetChildAt (i).position;
//				tComponents.GetChildAt (i).xy = startPos;
//				tComponents.GetChildAt (i).TweenMove (endPos,0.3f);
//			}
//
//			movingIcon.TweenMove(this.GlobalToLocal(beforeDragPos),0.3f).OnStart(delegate(GTweener tweener) {
//				
//			}).OnUpdate(delegate(GTweener tweener) {
//				movingIcon.InvalidateBatchingState(); 
//			}).OnComplete(delegate(GTweener tweener) {
//				movingIcon.visible = false;
//				//PlayerData.getInstance ().bagComponents [bagSlot] = equipped;
//				tComponents.GetChildAt (bagSlot).visible = true;
//			});
//		} else {
//			PlayerData.getInstance ().bagComponents.RemoveAt (bagSlot);
//			tComponents.numItems = PlayerData.getInstance ().bagComponents.Count;
//		}
//		updateCalcedProperty (tt);
//	}

	public void changeComponent (int componentIdx, int bagSlot){
		isDropOnSlot = true;
		if (towerIdx < 0)
			return;
		TowerTemplate tt = PlayerData.getInstance ().ownedTowers [towerIdx];
		TowerComponent toEquip = PlayerData.getInstance ().bagComponents [bagSlot];
		TowerComponent equipped = tt.components [componentIdx];

		tt.components [componentIdx] = toEquip;
		components [componentIdx].setInfo (tt.components[componentIdx]);

		if (equipped != null) {
			PlayerData.getInstance ().bagComponents [bagSlot] = equipped;

		} else {
			PlayerData.getInstance ().bagComponents.RemoveAt (bagSlot);
		}
		updateCalcedProperty (tt);
	}


//	IEnumerator moveIcon(Vector2 moveTo){
//		while (true) {
//			movingIcon.xy = Vector2.Lerp (movingIcon.xy, moveTo, 0.3f);
//			yield return null;
//			if ((movingIcon.xy - moveTo).magnitude < 1f) {
//				yield break;
//			}
//		}
//	}
//	public void unequip (int componentIdx){
//		if (componentIdx < 0||componentIdx>=4)
//			return;
//		TowerTemplate tt = PlayerData.getInstance ().ownedTowers [towerIdx];
//		TowerComponent equipped = tt.components [componentIdx];
//		if (equipped == null)
//			return;
//		PlayerData.getInstance ().bagComponents.Add (equipped);
//		tComponents.numItems = PlayerData.getInstance ().bagComponents.Count;
//		components [componentIdx].setInfo (null);
//		tt.components [componentIdx] = null;
//
//
//		movingIcon.url = "Equips/" + equipped.cid;
//		movingIcon.xy = components [componentIdx].xy;
//		movingIcon.visible = true;
//		tComponents.GetChildAt (tComponents.numItems-1).visible = false;
//		tComponents.EnsureBoundsCorrect ();
//		movingIcon.TweenMove(this.GlobalToLocal(tComponents.LocalToGlobal(tComponents.GetChildAt(tComponents.numItems-1).xy)),0.3f).OnStart(delegate(GTweener tweener) {
//
//		}).OnUpdate(delegate(GTweener tweener) {
//			movingIcon.InvalidateBatchingState(); 
//		}).OnComplete(delegate(GTweener tweener) {
//			movingIcon.visible = false;
//			//PlayerData.getInstance ().bagComponents [bagSlot] = equipped;
//			tComponents.GetChildAt (tComponents.numItems-1).visible = true;
//		});
//	
//		updateCalcedProperty (tt);
//
//	}

	public void unequip (int componentIdx){
		if (componentIdx < 0||componentIdx>=4)
			return;
		TowerTemplate tt = PlayerData.getInstance ().ownedTowers [towerIdx];
		TowerComponent equipped = tt.components [componentIdx];
		if (equipped == null)
			return;
		PlayerData.getInstance ().bagComponents.Add (equipped);
		components [componentIdx].setInfo (null);
		tt.components [componentIdx] = null;
		updateCalcedProperty (tt);
	}

	public void moveCopyIcon(){
	}
		
	public void updateCalcedProperty(TowerTemplate tt){
		TowerBase tb = tt.tbase;
		TowerBattleProperty property = TowerBattleProperty.genTowerBattleProperty (tt);

		_property.setName (tb.tname);

		if (tb.atkType == eAtkType.MELLE_AOE || tb.atkType == eAtkType.MELLE_POINT) {
			string s = "Melee";
			_property.setAtkRange(s);
		} else {
			string s = string.Format("{0:f1}", property.atkRange * 0.001f);
			_property.setAtkRange(s);
		}

		int atkInteval = tb.towerModel.atkInterval;
		int actualInteval = (int)(atkInteval / (1.0f + property.atkSpd * 0.01f));

		string ss = string.Format("{0:f1}", actualInteval * 0.001f);

		_property.setAtkSpd(ss,property.atkSpd+"");

		_property.setHit(tb.mingzhong);
		//string s =  string s =string.Format("{0:f2}", x);

		_property.setDamage (property.mainAtk,property.extraAtk);
		_property.setSkill (property.originSkills);
	}


	public GObject getFirstTower(){
		return towers.GetChildAt (0);
	}
}

