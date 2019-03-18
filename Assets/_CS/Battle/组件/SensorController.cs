using UnityEngine;
using System.Collections;

public class SensorController : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	void OnTriggerEnter2D(Collider2D collider){
		if (this.tag == "enemy") {
			if (collider.tag == "Player") {
//				GameLife player = collider.GetComponentInParent<GameLife> ();
//				if (player != null) {
//					player.knock (collider.transform.position - this.transform.position,0.2f,6f);
//				}
			}
		}
	}
}

