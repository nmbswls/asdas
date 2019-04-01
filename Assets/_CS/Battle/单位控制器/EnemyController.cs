using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum eBehavePattern{
	IDLE = 0,
	WANDER = 1,
	CHASE = 2,
	AVOID = 3,
}



public class EnemyController : IUnitController
{
	public List<Vector3> path = new List<Vector3>();
	public bool hasPath;
	public int pathIdx = 0;


	PhysicsComponent physicsComponent;
	PathSeekBehaviour pathSeeker;

	eBehavePattern behavePatter = eBehavePattern.IDLE;

	public GameLife gl;

	public int chaseTimeInt = 0;
	public static int maxChaseTime = 5000;
	public static float maxMissDis = 3;

	public int idleTimeInt = 0;
	public static int perIdleTime = 1500;

	public int wanderTimeInt = 0;
	public Vector2 wanderTarget;

	public List<EnemySkill> skills = new List<EnemySkill>();
	// Use this for initialization
	void Awake ()
	{
		physicsComponent = GetComponent<PhysicsComponent> ();
		pathSeeker = GetComponent<PathSeekBehaviour> ();

		gl = GetComponent<GameLife> ();
	}

	void Start(){
		if (!BattleManager.getInstance ().enemies.Contains (GetComponent<GameLife> ())) {
			BattleManager.getInstance ().enemies.Add (GetComponent<GameLife> ());
		}
	}



	public void flocking(){
		Vector3 offset = Vector3.zero;
		foreach (GameLife gl in BattleManager.getInstance ().enemies) {
			Vector3 diff = (gl.transform.position - transform.position);
			if (diff.magnitude < 0.4f) {
				offset += -diff.normalized * (0.4f-diff.magnitude)*4;
			}
		}
		offsetV = offset;
	}

	void logicUpdate(){
		for (int i = 0; i < skills.Count; i++) {
			
		}
	}

	void Update ()
	{
		int timeInt = (int)(Time.deltaTime * 1000f);
		if (behavePatter == eBehavePattern.CHASE) {
			chaseTimeInt += timeInt;
			if (chaseTimeInt > maxChaseTime) {
				behavePatter = eBehavePattern.IDLE;
				chaseTimeInt = 0;
			}

		} else if (behavePatter == eBehavePattern.WANDER) {
			wanderTimeInt += timeInt;
			Vector2 gl2D = transform.position;
			Vector2 target2D = BattleManager.getInstance ().player.transform.position;
			if ((target2D - gl2D).magnitude < maxMissDis) {
				gl.showEmoji (0);
				behavePatter = eBehavePattern.CHASE;
				pathSeeker.resumeFollow ();
				pathIdx = 0;
				chaseTimeInt = 0;
				physicsComponent.canSlide = true;
				return;
			}
			if(wanderTimeInt > 3000){
				wanderTarget = Random.onUnitSphere;
				wanderTimeInt = 0;

			}else if(wanderTimeInt>1000){
				wanderTarget = Vector2.zero;
			}
		}else if(behavePatter == eBehavePattern.IDLE){
			idleTimeInt += timeInt;
			if (idleTimeInt > perIdleTime) {
				behavePatter = eBehavePattern.CHASE;
				idleTimeInt = 0;

				Vector2 target2D = transform.position;
				Vector2 gl2D = BattleManager.getInstance ().player.transform.position;
				if ((target2D - gl2D).magnitude > maxMissDis) {

					behavePatter = eBehavePattern.WANDER;
					pathSeeker.stopFollow();
					wanderTimeInt = 0;
					Vector2Int togo = MapManager.getInstance ().getRandomPosToGo (new Vector2Int(gl.posXInt,gl.posYInt));
					pathSeeker.searchFixedPath (togo);
					pathIdx = 0;
					physicsComponent.canSlide = false;
				}
			}
		}
		avoidCol ();

	}

	void FixedUpdate(){

		if (behavePatter == eBehavePattern.WANDER) {
			flocking ();
			physicsComponent.doMove = true;
			physicsComponent.moveBy = wanderTarget * gl.WalkSpeed *0.4f * Time.fixedDeltaTime + offsetV *  Time.fixedDeltaTime;
			//Debug.Log (wanderTarget * gl.WalkSpeed *0.4f * Time.fixedDeltaTime + offsetV *  Time.fixedDeltaTime);

		}else if(behavePatter == eBehavePattern.CHASE) {
			physicsComponent.doMove = false;
			Vector2 dd = Vector2.zero;
			if (!hasPath) {

			} else if (pathIdx >= path.Count) {
				hasPath = false;

			} else {
				Vector3 targetPos = path [pathIdx];

				dd = (targetPos - this.transform.position);

				if (dd.magnitude < 0.2f) {
					pathIdx++;
				}
			}
			flocking ();

			physicsComponent.doMove = true;
			physicsComponent.moveBy = /*new Vector2(this.transform.position.x,this.transform.position.y) + */dd.normalized * gl.WalkSpeed * Time.fixedDeltaTime + offsetV *  Time.fixedDeltaTime;
		}else if(behavePatter == eBehavePattern.IDLE){
			
		}
	}


	void avoidCol(){
		RaycastHit2D hit = Physics2D.Raycast (transform.position, Dvec,0.5f,1<<LayerMask.NameToLayer("Obstacle"));

		if (hit.collider != null) {
			Debug.Log ("hit");
			offsetV += hit.normal * 1f;
		}
	}
}

