using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsComponent : MonoBehaviour
{
	[System.Flags]
	public enum eCollFlags
	{
		NONE = 0,
		DOWN = (1 << 0),
		LEFT = (1 << 1),
		RIGHT = (1 << 2),
		UP = (1 << 3)
	}
	public eCollFlags CollFlags = eCollFlags.NONE;


	public GameLife owner;

	void Awake(){
		owner = GetComponent<GameLife> ();
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	public bool doMove;
	public Vector2 moveToTarget;
	public Vector2 velocity;
	// Update is called once per frame
	void Update ()
	{
		if (doMove) {
			Vector2 validMoveDest = DoStaticCollisions (moveToTarget);
			transform.position = validMoveDest;
		}
		doMove = false;
	}

	bool IsCollidingAABB(Vector2 vCheckPos){
		BoxCollider2D bc = GetComponent<BoxCollider2D> ();

		Vector2 posCenter = bc.offset + vCheckPos;
		//得到矩形
		Vector3Int lt = MapManager.getInstance().tilemap.WorldToCell(posCenter+new Vector2(-bc.size.x/2,bc.size.y/2));
		lt.y = -lt.y;
		Vector3Int rb = MapManager.getInstance().tilemap.WorldToCell(posCenter+new Vector2(bc.size.x/2,-bc.size.y/2));
		rb.y = -rb.y;
		for(int i=lt.y;i<=rb.y;i++){
			for (int j = lt.x; j <= rb.x; j++) {
				if (i<0||i>=MapManager.MAP_HEIGHT || j<0|| j>= MapManager.MAP_WIDTH || MapManager.getInstance ().staticBlocks [i] [j] == true) {
					return true;
				}
			}
		}
		return false;
	}

	Vector2 DoStaticCollisions(Vector2 toMove) 
	{
		if (false) {
			return toMove;
		}
		Vector3 vTempPos = toMove;
		Vector3 vCheckedPos = toMove;
		List<Vector2> collideDirs = new List<Vector2>();
		CollFlags = eCollFlags.NONE;
		//2D AABB 专用碰撞算法
		if( IsCollidingAABB( vCheckedPos ))
		{
			//m_speed = 0f;
			vCheckedPos.y = transform.position.y;
			if( !IsCollidingAABB( vCheckedPos ) )
			{
				vTempPos.y = transform.position.y;
				CollFlags |= transform.position.y > transform.position.y? eCollFlags.DOWN : eCollFlags.UP;
			}
			else
			{
				vCheckedPos = toMove;
				vCheckedPos.x = transform.position.x;
				if( !IsCollidingAABB( vCheckedPos ) )
				{
					vTempPos.x = transform.position.x;
					CollFlags |= transform.position.x > transform.position.x? eCollFlags.LEFT : eCollFlags.RIGHT;
				}
				else
				{
					vTempPos = transform.position;
					CollFlags |= transform.position.y > transform.position.y? eCollFlags.DOWN : eCollFlags.UP;
					CollFlags |= transform.position.x > transform.position.x? eCollFlags.LEFT : eCollFlags.RIGHT;
				}
			}

		}
		else
		{
			
		}

		return vTempPos;
	}


}

