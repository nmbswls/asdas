using UnityEngine;
using System.Collections;
using FairyGUI;

public class SelectionBranch:GButton
{
	int branchIdx = 0;
	GTextField _content;

	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		_content = this.GetChild ("title").asTextField;
	}

	public void init(int branchIdx, string content){
		this.branchIdx = branchIdx;
		_content.text = content;
	}

	public int getIndex(){
		return branchIdx;
	}
}

