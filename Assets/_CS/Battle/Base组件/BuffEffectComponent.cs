using UnityEngine;
using System.Collections;

public class BuffEffectComponent : MonoBehaviour
{

	public MapObject owner;
	//public GameObject burnedEffectPrefab;


	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void showEffect(string buffId){

		//Resources.Load ("buffId") as ;

		StartCoroutine (showEffectE (0.3f));
	}

	IEnumerator showEffectE(float time){
//		GameObject effect = GameObject.Instantiate (burnedEffectPrefab,transform);
//		effect.transform.localPosition += new Vector3 (0,0.3f,0);
		yield return new WaitForSecondsRealtime (time);
//		GameObject.Destroy (effect);
	}
}

