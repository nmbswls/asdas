using UnityEngine;
using System.Collections;
using FairyGUI;

public class MonsterCardComponent : GComponent
{
	GTextField numberTF;
	GList starTF;
	GLoader picture;

	string sname = "xx";
	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		numberTF = this.GetChild("num").asTextField;
		picture = this.GetChild("loader").asLoader;
	}

	public void setName(string name){
		this.sname = name;
		numberTF.text = sname;
	}

	public void setCardLook(ChasingEnemyAbstract enemies){
		int totalNum = 0;
		for (int i = 0; i < enemies.enemyId.Count; i++) {
			totalNum += enemies.enemyNum [i];
		}
	}

}

