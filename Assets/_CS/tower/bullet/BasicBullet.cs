using UnityEngine;
using System.Collections;

public class BasicBullet : MonoBehaviour,ILogicUpdate
{
	public int speed = 5;

	public int vOriginHeight = 0;
	protected Tower owner;
	protected GameLife target;

	public SpriteRenderer actualImage;

	public void UpdateLogic (int deltaTime)
	{
		doMovement (deltaTime);
	}
	protected virtual void doMovement(int deltaTime){
	}

	protected virtual void hitTarget(){
		Release ();
	}

	protected virtual void loseTarget(){
		Release ();
	}



	// Use this for initialization
	void Awake ()
	{
		actualImage = transform.Find ("sprite").GetComponent<SpriteRenderer>();
	}
	// Update is called once per frame
	protected virtual void Update ()
	{
		UpdateLogic ((int)(Time.deltaTime * 1000));
	}

	protected virtual void applyAtk(GameLife hit){
	}
		

	protected virtual void checkHit(){
	
	}

	void OnTriggerStay2D(Collider2D c){
		if (gameObject.activeSelf) {
			GameLife hit = c.GetComponentInParent<GameLife> ();
			if (hit.tag == "enemy" && validTarget(hit)) {
				applyAtk (hit);
			}
		}
	}

	protected virtual bool validTarget(GameLife gl){
		return true;
	}

	//预订提取为接口函数
	protected void Release()
	{
		gameObject.SetActive (false);
		owner = null;
		target = null;
		BulletManager.inst.ReturnComponent(gameObject);
	}
}

