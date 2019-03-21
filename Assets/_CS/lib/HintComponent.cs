using UnityEngine;
using System.Collections;
using FairyGUI;

public class HintComponent : GComponent
{
	GTextField _hint;

	public override void ConstructFromXML(FairyGUI.Utils.XML cxml)
	{
		base.ConstructFromXML(cxml);
		_hint = this.GetChild ("hint").asTextField;
	}
	public void init(string hint){
		_hint.text = hint;
		move ();
	}
	public void move(){
		this.TweenMoveY(-100,2f).OnComplete(delegate() {
			Dispose();
		});
	}
//	GComponent hint;
//	string txt;
//	float v = 2;
//	// Use this for initialization
//	void Start ()
//	{
//		hint = GetComponent<UIPanel> ().ui;
//		hint.sortingOrder = 100;
//		hint.GetChild ("hint").asTextField.text=txt;
//		StartCoroutine (life(2f));
//
//	}
//	public void init(string txt){
//		this.txt = txt;
//	}
//
//	// Update is called once per frame
//	void Update ()
//	{
//	
//	}
//
//	IEnumerator life(float lifeTime){
//		float time = 0;
//		while (time < lifeTime*0.3) {
//			yield return null;
//			time += Time.deltaTime;
//			transform.position += -Vector3.down * Time.deltaTime * v * 3;
//		}
//		while (time < lifeTime*0.8) {
//			yield return null;
//			time += Time.deltaTime;
//			transform.position += -Vector3.down * Time.deltaTime * v;
//		}
//		while (time < lifeTime) {
//			yield return null;
//			time += Time.deltaTime;
//			transform.position += -Vector3.down * Time.deltaTime * v*5;
////			if ((text = GetComponent<Text> ()) != null) {
////				text.color = new Color (text.color.r,text.color.g,text.color.b,1 - time / lifeTime);
////			}
//		}
//		hint.Dispose ();
//		GameObject.Destroy (gameObject);
//	}
}

