using UnityEngine;
using System.Collections;

public class AtkCircleEffect : BaseEffect
{

	bool isBox = false;
	bool isSection = false;

	//int timeLeft = 0;

	SpriteRenderer image;

	protected override void Start(){
		base.Start ();
		image = GetComponentInChildren<SpriteRenderer> ();
	}

	protected override void effectAction(int timeInt){

		timeLeft -= timeInt;
		if (timeLeft <= 0) {
			Release ();
		} else {

		}
	}

	protected override void OnRelease(){
		image = null;
	}


	public void init(Vector3 pos, float range,int last = 900){
		transform.position = pos;
		transform.localScale = new Vector3 (range,range,99);
		this.timeLeft = last;
		initialized = true;
	}

//	IEnumerator kuosan(){
//		float time = 0.5f;
//		float t = 0;
//		while (t < time) {
//			t += Time.deltaTime;
//			//transform.localScale = new Vector3 (t*range*3,range*3,0);
//
//		}
//	}
}
