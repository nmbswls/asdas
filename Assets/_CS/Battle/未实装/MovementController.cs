using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour {


	public PhysicsComponent physicsComponent;

	public List<Vector3> path;
	public bool hasPath;

	void Awake(){
		physicsComponent = GetComponent<PhysicsComponent> ();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		Dir.z = 0f;
//		Vector3 toMove = transform.position + Dir * MOVE_SPEED * Time.deltaTime;
//		if (addedForce != null) {
//			Vector3 dist = addedForce.dir * addedForce.v * Time.deltaTime;
//			toMove += dist;
//		}
//		physicsComponent.distPos = 

	}
}
