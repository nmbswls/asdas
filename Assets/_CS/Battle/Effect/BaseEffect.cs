using UnityEngine;
using System.Collections;

public class BaseEffect : MapObject
{

	public MapObject owner;

	protected bool initialized=false;
	protected int timeLeft = 0;
	protected virtual void effectAction(int timeInt){
		if (owner != null) {
			this.posXInt = owner.posXInt;
			this.posYInt = owner.posYInt;
		}
		timeLeft -= timeInt;
		if (timeLeft <= 0) {
			Release ();
		} else {

		}

	}

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
		if (initialized == false) {
			return;
		}
		effectAction ((int)(Time.deltaTime*1000));
	}


	public void Release()
	{
		
		owner = null;
		timeLeft = 0;
		//transform.Find ("effect").localRotation = Quaternion.identity;
		//EffectManager.inst.ReturnComponent (gameObject);
		initialized = false;
		OnRelease ();
		gameObject.SetActive (false);

	}

	protected virtual void OnRelease(){
		EffectManager.inst.ReturnComponent (gameObject);
	}

	public virtual void init(int timeInt,  MapObject owner = null){
		this.timeLeft = timeInt;
		this.posXInt = owner.posXInt;
		this.posYInt = owner.posYInt;
		this.owner = owner;
		initialized = true;
		gameObject.SetActive (true);
		OnInit ();
	}
	public virtual void init(int timeInt,  Vector2Int pos){
		this.timeLeft = timeInt;
		this.posXInt = pos.x;
		this.posYInt = pos.y;
		this.owner = null;
		initialized = true;
		gameObject.SetActive (true);
		OnInit ();
	}

	public virtual void OnInit(){

	}
}

