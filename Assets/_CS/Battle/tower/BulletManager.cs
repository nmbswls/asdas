using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager
{

	public static int MAX_BULLET_IDLE_NUMBER = 50;
	static BulletManager _instance;

	public GameObject bulletPrefab;
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
		bulletPrefab = Resources.Load ("Prefabs/bullet01") as GameObject;
	}

	public void Emit(GameObject owner, GameObject target, bool isHoming, int vHight = 0, int height = 0)
	{
		GameObject b;
		if (_componentPool.Count > 0)
			b = _componentPool.Pop ();
		else
			b = GameObject.Instantiate (bulletPrefab,owner.transform.parent);
		BallisticBullet bullet = b.GetComponent<BallisticBullet> ();
		bullet.init(owner.GetComponent<Tower>(), target, isHoming, vHight, height);
	}

	public void ReturnComponent(GameObject com)
	{
		if (_componentPool.Count < MAX_BULLET_IDLE_NUMBER) {
			_componentPool.Push (com);
		}
	}
}

