using UnityEngine;
using System.Collections;

public class BaseEffect : MonoBehaviour
{

	protected bool initialized=false;

	protected virtual void effectAction(int timeInt){
		
	}

	// Update is called once per frame
	void Update ()
	{
		if (initialized = false) {
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
}

