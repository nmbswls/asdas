using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletManager
{

	public static int MAX_BULLET_IDLE_NUMBER = 50;
	static BulletManager _instance;

	public GameObject bulletPrefab;
	public List<Sprite> bulletSprites = new List<Sprite>();
	public Dictionary<string,Sprite> bulletSpritesDic = new Dictionary<string,Sprite>();
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
//		{
//			GameObject a = Resources.Load ("Prefabs/bullet/bullet01") as GameObject;
//			bulletSprites.Add (a.GetComponent<SpriteRenderer> ().sprite);
//		}
//		{
//			GameObject a = Resources.Load ("Prefabs/bullet/bullet02") as GameObject;
//			bulletSprites.Add (a.GetComponent<SpriteRenderer> ().sprite);
//		}
	}

	public void Emit(string style, GameObject owner, GameObject target, bool isHoming, int vHight = 0, int height = 0)
	{
		GameObject b;
		if (_componentPool.Count > 0)
			b = _componentPool.Pop ();
		else
			b = GameObject.Instantiate (bulletPrefab,owner.transform.parent);

		if (!bulletSpritesDic.ContainsKey (style)) {
			GameObject a = Resources.Load ("Prefabs/bullet/"+style) as GameObject;
			if (a != null) {
				bulletSpritesDic [style] = a.GetComponent<SpriteRenderer> ().sprite;
			} else {
				bulletSpritesDic [style] = new Sprite();
			}
		}
		Sprite sprite = bulletSpritesDic[style];

		BallisticBullet bullet = b.GetComponent<BallisticBullet> ();
		bullet.GetComponentInChildren<SpriteRenderer> ().sprite = sprite;
		bullet.init(owner.GetComponent<Tower>(), target, isHoming, vHight, height);
	}

	public void ReturnComponent(GameObject com)
	{
		if (_componentPool.Count < MAX_BULLET_IDLE_NUMBER) {
			_componentPool.Push (com);
		}
	}
}

