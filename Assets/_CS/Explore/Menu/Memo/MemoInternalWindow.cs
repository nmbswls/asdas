using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class MemoInternalWindow : GComponent
{
	public static int MAX_COMBINATION_COUNT = 4;

	public List<MemoItem> allShownMemo = new List<MemoItem>();
	public List<MemoItem> selected = new List<MemoItem>();

	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		
	}

	public void addMemo(string memoIdx,bool withEffect = false){
		MemoItem button = (MemoItem)UIPackage.CreateObject("UIMain", "MemoItem").asCom;

		this.AddChild (button);
		MemoInfo info = null;
		GameStaticData.getInstance ().memoInfo.TryGetValue(memoIdx,out info);
		if (info == null) {
			info = GameStaticData.getInstance ().memoInfo["default"];
		}
		button.memoId = info.memoId;
		allShownMemo.Add (button);
		button.SetXY (info.fixedX,info.fixedY);
		button.setDesp (info.desp);
		button.onChanged.Add (delegate() {
			if(button.selected){
				if(selected.Count>MAX_COMBINATION_COUNT){
					return;
				}
				selected.Add(button);
			}else{
				selected.Remove(button);
			}
			((MemoPanel)(this.parent.asCom)).updateDetail(selected);

		});
		if (withEffect) {
			button.alpha = 0;
			button.TweenFade (1, 1f).OnComplete(delegate() {
				((MemoPanel)(this.parent.asCom)).combineCallback();
			});
		}
	}

	public void fadeUsedMemo(){
		foreach (MemoItem memoItem in selected) {
			PlayerData.getInstance ().memoState [memoItem.memoId] = 0;
			memoItem.TweenFade (0, 0.3f).OnComplete (delegate() {
				memoItem.Dispose ();
			});
		}
		selected.Clear();
	}



}

