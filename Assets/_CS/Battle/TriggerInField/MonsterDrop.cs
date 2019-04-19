using UnityEngine;
using System.Collections;

public class MonsterDrop : MonoBehaviour
{
	public MonsterDropManager dropManager;
	public bool triggered = false;
	public bool destroyed = false;
	MapObject target = null;

	public Vector2 targetPos;

	public bool isMovingOrigin = false;

	public float unpickableTime = 0;
	// Use this for initialization

	
	// Update is called once per frame
	protected void Update ()
	{
		if (destroyed)
			return;
		if (unpickableTime > 0) {
			unpickableTime -= Time.deltaTime;
		}
		if (!triggered) {
			if (isMovingOrigin) {
				Vector2 d = (targetPos - new Vector2(transform.position.x,transform.position.y));
				if (d.magnitude < 0.1f) {
					transform.position = new Vector3 (targetPos.x, targetPos.y, -1f);
					isMovingOrigin = false;
				} else {
					Vector2 dest = new Vector2(transform.position.x,transform.position.y) + d.normalized * Time.deltaTime * 3f;
					transform.position = new Vector3 (dest.x,dest.y,-1f);
				}
			}
			return;
		}
			

		//Vector2Int d = new Vector2Int (target.posXInt - posXInt, target.posYInt - posYInt);
		//float
		{
			Vector3 d = (target.transform.position - transform.position);
			d.z = 0;
			if (d.magnitude < Time.deltaTime * 6f) {
				dropManager.removeDrop (this);
				destroyed = true;
				GameObject.Destroy (gameObject);
			} else {
				Vector2 dest = transform.position + d.normalized * Time.deltaTime * 6f;
				transform.position = new Vector3 (dest.x,dest.y,0);
			}
		}
	}
	public void trigger(){
		if (unpickableTime > 0)
			return;
		if (triggered)
			return;
		triggered = true;
		target = BattleManager.getInstance ().player.gl;
	}

	public void moveToRandomNearby(){
		Vector2 r = Random.insideUnitCircle;
		r *= 0.5f;
		targetPos = new Vector2(transform.position.x,transform.position.y) + r;
		isMovingOrigin = true;
	}
}	

