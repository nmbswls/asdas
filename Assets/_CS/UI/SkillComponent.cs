using UnityEngine;
using System.Collections;
using FairyGUI;

public class SkillComponent : GComponent
{
	GComponent _skills;
	GComponent _s1;
	GComponent _s2;
	GComponent _s3;
	GComponent _s4;

	public SkillComponent (GComponent mainView)
	{
		_skills = mainView.GetChild ("skills").asCom;
		_s1 = _skills.GetChild ("s1").asCom;
		_s2 = _skills.GetChild ("s2").asCom;
		_s3 = _skills.GetChild ("s3").asCom;
		_s4 = _skills.GetChild ("s4").asCom;

		_s1.onClick.Add (click);

	}

	public void click(){
		Debug.Log ("click s1");
	}
	public void Rotate(){
		_s4.visible = true;
		GTween.To(0f, -45f, 0.4f).SetTarget(this)
			.OnUpdate((GTweener tweener) => { this.UpdateRotation(tweener.value.x); }).OnComplete(this.OnCompleted);
	}

	public void UpdateRotation(float angle){
		_skills.rotation = (float)angle;

	}
	public void UpdatePosition(Vector2 v){
	}
	public void OnCompleted(){
		_s4.visible = false;
		_skills.rotation = 0f;
	}
}

