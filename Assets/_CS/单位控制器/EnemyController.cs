using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : IUnitController
{
	public List<Vector3> path = new List<Vector3>();
	public bool hasPath;
	public int pathIdx = 0;


	PhysicsComponent physicsComponent;
	PathSeekBehaviour pathSeeker;


	// Use this for initialization
	void Awake ()
	{
		physicsComponent = GetComponent<PhysicsComponent> ();
		pathSeeker = GetComponent<PathSeekBehaviour> ();
	}

	void Start(){
		if (!GameManager.getInstance ().enemy.Contains (GetComponent<GameLife> ())) {
			GameManager.getInstance ().enemy.Add (GetComponent<GameLife> ());
		}
	}

	public void flocking(){
		Vector3 offset = Vector3.zero;
		foreach (GameLife gl in GameManager.getInstance ().enemy) {
			if ((gl.transform.position - transform.position).magnitude < 0.4f) {
				offset += (transform.position - gl.transform.position).normalized * 0.4f;
			}
		}
		offsetV = offset;
	}

	void Update ()
	{
		avoidCol ();
	}

	void FixedUpdate(){
		flocking ();
		physicsComponent.doMove = false;
		if (!hasPath) {
			return;
		}
		if (pathIdx >= path.Count) {
			hasPath = false;
			return;
		}

		Vector3 targetPos = path [pathIdx];

		Vector2 dd = (targetPos - this.transform.position);



		if (dd.magnitude < 0.1f) {
			pathIdx++;
		}

//		rBody.velocity += pCtrl.offsetV;
		//physicsComponent.moveToTarget ();
		physicsComponent.doMove = true;
		physicsComponent.moveToTarget = new Vector2(this.transform.position.x,this.transform.position.y) + dd.normalized * 3 * Time.fixedDeltaTime + offsetV *  Time.fixedDeltaTime;
	}


	void avoidCol(){
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Dvec,0.5f,1<<LayerMask.NameToLayer("Obstacle"));

		if (hit.collider != null) {
			Debug.Log ("hit");
			offsetV += hit.normal * 1f;
		}
	}
}

