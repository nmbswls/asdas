using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;

public class HeadBarManager : Singleton<HeadBarManager>
{
	GComponent _mainView;

	GComponent highLightEnemy = null;
	void Start()
	{
		Application.targetFrameRate = 60;
		foreach(GameLife enemy in BattleManager.getInstance ().enemies){
			UIPanel panel = enemy.transform.Find("HeadBar").GetComponent<UIPanel>();
			panel.ui.GetChild("n0").asProgress.value = enemy.hp;
			panel.ui.GetChild ("n0").asProgress.max = enemy.maxHP;
		}

	}

	void Update(){
		foreach(GameLife enemy in BattleManager.getInstance ().enemies){
			UIPanel panel = enemy.transform.Find("HeadBar").GetComponent<UIPanel>();
			panel.ui.GetChild("n0").asProgress.value = enemy.hp;
			panel.ui.GetChild ("n0").asProgress.max = enemy.maxHP;



		}

	}

	void updateHp(){
		foreach(GameLife enemy in BattleManager.getInstance ().enemies){
			UIPanel panel = enemy.transform.Find("HeadBar").GetComponent<UIPanel>();
			panel.ui.GetChild("n0").asProgress.value = enemy.hp;
			panel.ui.GetChild ("n0").asProgress.max = enemy.maxHP;
		}
	}





}