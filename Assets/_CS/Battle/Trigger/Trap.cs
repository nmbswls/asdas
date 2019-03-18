using UnityEngine;
using System.Collections;

public class Trap : MapObject
{
	public string type;
	public int value;

	public PhysicsComponent pc;
	public Animator anim;

	bool used = false;
	// Use this for initialization
	void Start ()
	{
		anim = GetComponentInChildren<Animator> ();
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
		if (collider.tag == "Player") {
			return;
		}
		if (used)
			return;
		GameLife enemy = collider.GetComponent<GameLife> ();
		anim.SetTrigger ("active");
		used = true;
		enemy.buffManager.addBuff (new Buff(1,99,1000));
		//EffectManager.inst.Emit (transform);
		GameObject.Destroy (gameObject,1.0f);
	}
}

