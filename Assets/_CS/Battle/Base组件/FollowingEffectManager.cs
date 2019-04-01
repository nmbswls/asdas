using UnityEngine;
using System.Collections;

public class FollowingEffectManager : MonoBehaviour
{
	public GameObject burnedEffectPrefab;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void showEffect(){
		StartCoroutine (showEffectE (0.3f));
	}

	IEnumerator showEffectE(float time){
		GameObject effect = GameObject.Instantiate (burnedEffectPrefab,transform);
		effect.transform.localPosition += new Vector3 (0,0.3f,0);
		yield return new WaitForSecondsRealtime (time);
		GameObject.Destroy (effect);
	}
}

