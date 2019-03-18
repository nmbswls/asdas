using UnityEngine;
using System.Collections;

public class BaseEffect : MonoBehaviour
{

	protected bool initialized=false;
	protected int timeLeft = 0;
	protected virtual void effectAction(int timeInt){
		timeLeft -= timeInt;
		if (timeLeft <= 0) {
			Release ();
		} else {

		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (initialized == false) {
			return;
		}
		effectAction ((int)(Time.deltaTime*1000));
	}


	public void Release()
	{
		OnRelease ();
		gameObject.SetActive (false);
		initialized = false;
	}

	protected virtual void OnRelease(){
		
	}

	public virtual void init(int timeInt){
		this.timeLeft = timeInt;
		initialized = true;
	}
}

