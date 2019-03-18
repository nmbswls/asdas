using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class MemoPanel :  GComponent
{

	MemoInternalWindow memoWindow;
	GButton blendButton;


	GTextField _combDetail;

	MemoStoryWindow msw;

	bool isChangingView = false;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		base.ConstructFromXML (xml);
		memoWindow = (MemoInternalWindow)this.GetChild ("memo").asCom;
		blendButton = (GButton)this.GetChild ("blend").asButton;

		_combDetail = this.GetChild ("detail").asTextField;
		_combDetail.text = "";

		blendButton.onTouchEnd.Add (conbineAndGen);
//		foreach (string ss in PlayerData.getInstance ().ownedMemos) {
//			//addMemo
//			memoWindow.addMemo();
//
//		}
		msw = new MemoStoryWindow(this);


		//initMemoState
	}

	public void initMemoState(){
		memoWindow.RemoveChildren ();
		foreach (var kv in PlayerData.getInstance().memoState) {
			memoWindow.addMemo(kv.Key);
		}
//		for (int i = 0; i < 3; i++) {
//			memoWindow.addMemo("asdsad");
//		}
	}

	public void updateDetail(List<MemoItem> selected){
		string s = "";
		foreach (MemoItem item in selected) {
			string d = GameStaticData.getInstance().memoInfo[item.memoId].desp;
			s += d + "\n";
		}
		_combDetail.text = s;

	}

	public void conbineAndGen(){
		
		if (isChangingView)
			return;
		string res = GameStaticData.getInstance ().getPossibleCombination (memoWindow.selected);
		if (res == "") {
			Debug.Log ("invalid");
		} else {
			Debug.Log ("valud, res:"+res);
			memoWindow.fadeUsedMemo ();
			memoWindow.addMemo ("18",true);
			_combDetail.text = "";
			this.touchable = false;

		}
	}

	public void combineCallback(){
		msw.Show ();
		unlockUI ();
	}

	public void unlockUI(){
		isChangingView = false;
		this.touchable = true;
	}
}

