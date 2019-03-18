using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

[System.Serializable]
public class MemoInfo{
	
	public string memoId;
	public int fixedX;
	public int fixedY;
	public string desp;
	public int memoType = 0;
}

[System.Serializable]
public class MemoCombinationRule{
	public List<string> toCombine = new List<string> ();
	public string resId;
}

public class MemoItem : GButton
{
	public string memoId;
	GTextField _desp;
	public override void ConstructFromXML(FairyGUI.Utils.XML xml)
	{
		_desp = this.GetChild ("n3").asTextField;
	}

	public void setDesp(string desp){
		_desp.text = desp;
		adjustSize ();
	}
	public void adjustSize(){
		if (_desp.text.Length > 10) {
			this.scale = new Vector2 (1.5f, 1.5f);
			_desp.textFormat.size = 30;
		} else {
			this.scale = new Vector2 (1f, 1f);
			_desp.textFormat.size = 36;
		}
	}
}

