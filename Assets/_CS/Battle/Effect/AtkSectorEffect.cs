using UnityEngine;
using System.Collections;

public class AtkSectorEffect : BaseEffect
{

	bool isBox = false;
	bool isSection = false;

	//int timeLeft = 0;

	SpriteRenderer image;

	void Start(){
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
		Destroy (gameObject);
	}


	public void init(Vector3 pos, Vector3 face,int last = 300){
		transform.position = pos;
		transform.right = face;
		this.timeLeft = last;
		initialized = true;
	}
}

