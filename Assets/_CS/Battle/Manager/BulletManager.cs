using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager
{

	public static int MAX_BULLET_IDLE_NUMBER = 50;
	static BulletManager _instance;

	public GameObject bulletPrefab;
	//public List<Sprite> bulletSprites = new List<Sprite>();
	public Dictionary<string,AnimatorOverrideController> bulletDic = new Dictionary<string,AnimatorOverrideController>();
	public static BulletManager inst
	{
		get
		{
			if (_instance == null)
				_instance = new BulletManager();
			return _instance;
		}
	}



	private readonly Stack<GameObject> _componentPool = new Stack<GameObject>();

	public BulletManager()
	{
		bulletPrefab = Resources.Load ("Prefabs/bullet_base") as GameObject;
//		{
//			GameObject a = Resources.Load ("Prefabs/bullet/bullet01") as GameObject;
//			bulletSprites.Add (a.GetComponent<SpriteRenderer> ().sprite);
//		}
//		{
//			GameObject a = Resources.Load ("Prefabs/bullet/bullet02") as GameObject;
//			bulletSprites.Add (a.GetComponent<SpriteRenderer> ().sprite);
//		}
	}

	public void EmitBullet(string style, Tower owner, GameLife target, bool isHoming, int vHight = 0, int height = 0)
	{
		GameObject b;
		if (_componentPool.Count > 0)
			b = _componentPool.Pop ();
		else
			b = GameObject.Instantiate (bulletPrefab,owner.transform.parent);

		if (!bulletDic.ContainsKey (style)) {

			AnimatorOverrideController tAnimCtrl =  Resources.Load ("OverrideAnimCtrl/bullet/"+style) as AnimatorOverrideController;
			if (tAnimCtrl != null) {
				bulletDic [style] = tAnimCtrl;
			} else {
				bulletDic [style] = Resources.Load ("OverrideAnimCtrl/bullet/Default") as AnimatorOverrideController;
			}
			//GameObject a = Resources.Load ("Prefabs/bullet/"+style) as GameObject;
//			if (a != null) {
//				bulletSpritesDic [style] = a.GetComponent<SpriteRenderer> ().sprite;
//			} else {
//				bulletSpritesDic [style] = new Sprite();
//			}
		}
		AnimatorOverrideController animCtrl = bulletDic[style];

		BallisticBullet bullet = b.GetComponent<BallisticBullet> ();
		bullet.GetComponentInChildren<Animator>().runtimeAnimatorController = animCtrl;
		bullet.init(owner, target, isHoming, vHight, height);
	}

	public void ReturnComponent(GameObject com)
	{
		if (_componentPool.Count < MAX_BULLET_IDLE_NUMBER) {
			_componentPool.Push (com);
		}
	}
}

