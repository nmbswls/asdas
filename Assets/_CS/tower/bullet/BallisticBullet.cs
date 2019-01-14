using UnityEngine;
using System.Collections;

public class BallisticBullet : BasicBullet
{

	public bool arriveGround = false;
	public static float Gg = 9.8f;
	public float OFFSET = 0.15f;
	private int height;
	private int vHeight;

	// Use this for initialization
	void Start ()
	{
	
	}
	


	public virtual void init(Tower owner, GameObject target, int vHeight = 0,int height = 0){
		this.owner = owner;
		this.target = target.GetComponent<GameLife>();
		this.vHeight = vHeight;
		this.height = height;
		vOriginHeight = vHeight;
		transform.position = owner.transform.position;
		gameObject.SetActive (true);
	}

	protected override void doMovement(int deltaTime){
		if (target== null || !target.IsAlive) {
			loseTarget ();
			return;
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
			Vector3 dir = (target.transform.position - this.transform.position);
			dir.z = 0;
			int vValid = (int)((dir.x * 1000f / leftTime) * 1000); 
			Vector2 bulletFace = new Vector2 (vValid / 1000f, vHeight / 1000f).normalized;
			actualImage.transform.right = new Vector3 (bulletFace.x, bulletFace.y, 0);

			if (dir.magnitude < 0.1f) {
				transform.position = target.transform.position;  
			} else {
				transform.position += dir * 1000 / (leftTime) * deltaTime * 0.001f; 
			}
		} else {
			Vector3 dir = (target.transform.position - this.transform.position);
			dir.z = 0;
			if (dir.magnitude < speed * deltaTime * 0.001f) {
				transform.position = target.transform.position;  
			} else {
				dir.Normalize ();
				transform.position += dir * speed * deltaTime * 0.001f; 
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
		hit.DoDamage (owner.damage);
		EffectManager.inst.EmitAtkCircleEffect (hit.transform,3);
		//hit.findclosesr ();
		Release ();
	}
}

