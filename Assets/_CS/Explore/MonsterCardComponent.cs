using UnityEngine;
using System.Collections;
using FairyGUI;

public class MonsterCardComponent : GComponent
{
	GTextField[] numberTF = new GTextField[3];
	GTextField[] nameTF = new GTextField[3];
	GLoader picture;

	public EnemyCombo info;
	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		numberTF[0] = this.GetChild("num1").asTextField;
		numberTF[1] = this.GetChild("num2").asTextField;
		numberTF[2] = this.GetChild("num3").asTextField;
		nameTF[0] = this.GetChild("name1").asTextField;
		nameTF[1] = this.GetChild("name1").asTextField;
		nameTF[2] = this.GetChild("name1").asTextField;
		//picture = this.GetChild("loader").asLoader;
	}



	public void setCardLook(EnemyCombo enemies){
		int totalNum = 0;
		for (int i = 0; i < enemies.enemyId.Count; i++) {
			totalNum += enemies.enemyNum [i];
		}
	}

	public void setInfo(EnemyCombo info){
		this.info = info;
		for (int i = 0; i < 3; i++) {
			if (i >= info.enemyId.Count)
				break;
			string eid = info.enemyId [i];
			int num = info.enemyNum [i];
			string ename = GameStaticData.getInstance ().getEnemyInfo (eid).enemyName;
			numberTF [i].text = "x" + num;
			nameTF [i].text = ename;
			numberTF [i].visible = true;
			nameTF [i].visible = true;
		}
		//this.GetController ("c0").;
	}

}

