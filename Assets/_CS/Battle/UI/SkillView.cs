using UnityEngine;
using System.Collections;
using FairyGUI;
using System.Collections.Generic;

public class SkillView : GComponent
{
	GComponent _skills;
	SkillItem[] _ss = new SkillItem[4];

	public SkillView (GComponent mainView)
	{
		_skills = mainView.GetChild ("skills").asCom;
		_ss[0] = (SkillItem)_skills.GetChild ("s1").asCom;
		_ss[1] = (SkillItem)_skills.GetChild ("s2").asCom;
		_ss[2] = (SkillItem)_skills.GetChild ("s3").asCom;
		_ss[3] = (SkillItem)_skills.GetChild ("s4").asCom;

		_ss [0].onTouchBegin.Add (delegate() {
			BattleManager.getInstance().playerSkillManager.useSkill(0);
		});
		_ss [1].onTouchBegin.Add (delegate() {
			BattleManager.getInstance().playerSkillManager.useSkill(1);
		});
		_ss [2].onTouchBegin.Add (delegate() {
			BattleManager.getInstance().playerSkillManager.useSkill(2);
		});

		_ss [0].setIcon ("atk");
		//_s1.onClick.Add (click);

	}

	public void click(){
		Debug.Log ("click s1");
	}
	public void Rotate(){
		_ss[3].visible = true;
		GTween.To(0f, -45f, 0.4f).SetTarget(this)
			.OnUpdate((GTweener tweener) => { this.UpdateRotation(tweener.value.x); }).OnComplete(this.OnCompleted);
	}

	public void UpdateRotation(float angle){
		_skills.rotation = (float)angle;

	}
	public void UpdatePosition(Vector2 v){
		
	}

	public void OnCompleted(){
		_ss[3].visible = false;
		_skills.rotation = 0f;
	}

	public void updateSkillView(){
		List<int> cdInt = BattleManager.getInstance ().playerSkillManager.pcSkillCd;
		for (int i = 0; i < cdInt.Count; i++) {
			_ss [i].updateCd (cdInt[i]);
		}
	}
}

