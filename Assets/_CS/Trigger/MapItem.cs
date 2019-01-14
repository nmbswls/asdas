using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour
{
	public string type;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag != "Player") {
			return;
		}
		EffectManager.inst.Emit (transform);
		GameManager.getInstance ().mapItemManager.removeItem (this);
		GameObject.Destroy (gameObject);


	}
}

