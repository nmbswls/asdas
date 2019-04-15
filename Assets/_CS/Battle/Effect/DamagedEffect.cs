using UnityEngine;
using System.Collections;

public class DamagedEffect : BaseEffect
{

	public Transform target;
	public SpriteRenderer sr;

	//int timeLeft = 0;

	protected override void Start(){
		base.Start ();
		sr = GetComponent<SpriteRenderer> ();
	}

	protected override void effectAction(int timeInt){
		
		transform.position = new Vector3(target.transform.position.x,target.transform.position.y,transform.position.z);
		timeLeft -= timeInt;
		if (timeLeft <= 0) {
			Release ();
		} else {

		}
	}

		

	public void init(Transform target,int timeInt){
		initialized = true;
		gameObject.SetActive (true);
		this.target = target;
		//sr.sprite = 
		this.timeLeft = timeInt;
	}

	protected override void OnRelease(){
		Destroy (gameObject);
	}
}

