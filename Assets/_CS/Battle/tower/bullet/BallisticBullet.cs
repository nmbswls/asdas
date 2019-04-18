using UnityEngine;
using System.Collections;

public class BallisticBullet : BasicBullet
{

	public bool isHoming;
	public Vector3 fixedTargetPos;

	public bool arriveGround = false;
	public static float Gg = 9.8f;
	public float OFFSET = 0.15f;
	private int height;
	private int vHeight;
	private bool loseTar;
	// Use this for initialization
	void Start ()
	{
	
	}

	protected override void loseTarget ()
	{
		base.loseTarget ();
		fixedTargetPos = target.transform.position;
		loseTar = true;
	}


	public virtual void init(Tower owner, GameLife target, bool isHoming, int vHeight = 0,int height = 0){
		this.owner = owner;
		this.target = target;
		this.vHeight = vHeight;
		this.height = height;
		this.isHoming = isHoming;
		this.fixedTargetPos = target.transform.position;
		this.loseTar = false;
		if (vHeight == 0)
			arriveGround = true;
		else 
			arriveGround = false;
		vOriginHeight = vHeight;

		transform.position = owner.transform.position;

		gameObject.SetActive (true);

	}

	protected override void doMovement(int deltaTime){
		if (loseTar) {
			
		}else if (isHoming && (target == null || !target.IsAlive)) {
			loseTarget ();
		} else if(isHoming){
			fixedTargetPos = target.transform.position;
		}

		if (!arriveGround) {
			if (height >= 0) {
				vHeight -= (int)Gg * deltaTime;
				height += (int)(vHeight * 0.001f) * deltaTime;
			}
			if (height < 0) {
				arriveGround = true;
			}
		}

		if (!arriveGround) {
			actualImage.transform.localPosition = new Vector3 (0, height * 0.001f + OFFSET, 0);

			int leftTime = (int)((vOriginHeight + vHeight) / Gg);
			Vector3 dir = (fixedTargetPos - this.transform.position);
			dir.z = 0;
			int vValid = (int)((dir.x * 1000f / leftTime) * 1000); 
			Vector2 bulletFace = new Vector2 (vValid / 1000f, vHeight / 1000f).normalized;
			actualImage.transform.right = new Vector3 (bulletFace.x, bulletFace.y, 0);

			if (dir.magnitude < 0.03f) {
				transform.position = fixedTargetPos;  
			} else {
				transform.position += dir * 1000 / (leftTime) * deltaTime * 0.001f; 
			}
		} else {
			Vector3 dir = (fixedTargetPos - this.transform.position);
			dir.z = 0;
			if (dir.magnitude < speed * deltaTime * 0.001f) {
				transform.position = fixedTargetPos;  
			} else {
				dir.Normalize ();
				transform.position += dir * speed * deltaTime * 0.001f; 
			}

			Vector2 bulletFace = new Vector2 (dir.x / 1000f, dir.y / 1000f).normalized;
			actualImage.transform.right = new Vector3 (bulletFace.x, bulletFace.y, 0);


		}
		transform.position = new Vector3 (transform.position.x,transform.position.y,0);
		if (loseTar) {
			Vector3 dir = (fixedTargetPos - this.transform.position);
			dir.z = 0;
			if (dir.magnitude < 0.1f) {
				Release ();
			}
		}

	}

	protected override bool validTarget(GameLife gl){
		if (height < 100) {
			return true;
		}
		return false;
	}

	protected override void applyAtk(GameLife hit){
		if (owner != null && owner.isActiveAndEnabled) {
			owner.applyAtk (hit);
			//hit.DoDamage (owner.damage,owner.property);
			EffectManager.inst.EmitAtkCircleEffect (hit.transform,3);
		}
		//hit.findclosesr ();
		Release ();
	}
}

