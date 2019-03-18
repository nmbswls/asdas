using UnityEngine;
using System.Collections;

public class MapItem : MonoBehaviour
{
	public string type;
	public int value;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void init(string type, int value){
		this.type = type;
		this.value = value;
	}

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.tag != "Player") {
			return;
		}
		EffectManager.inst.Emit (transform);
		BattleManager.getInstance ().gainCoin (value);
		BattleManager.getInstance ().mapItemManager.removeItem (this);
		GameObject.Destroy (gameObject);
	}
}

