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
	BoxCollider2D bc;

	void Awake(){
		owner = GetComponent<GameLife> ();
		bc = GetComponent<BoxCollider2D> ();
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	public bool doMove;
	//public Vector2 moveToTarget;
	public Vector2 moveBy;
	public Vector2 velocity;
	public bool canSlide = false;
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (IsCollidingAABB(new Vector2(transform.position.x,transform.position.y))) {
			backToValidPos ();
		}
		if (doMove) {
			Vector2 validMoveDest = DoStaticCollisions (moveBy+new Vector2(transform.position.x,transform.position.y));
			if (canSlide) {
				if ((CollFlags & eCollFlags.DOWN) != 0 || (CollFlags & eCollFlags.UP) != 0) {
					if (Vector2.Dot (moveBy.normalized, Vector2.up) > 0.8f || Vector2.Dot (moveBy.normalized, Vector2.down) > 0.8f) {

					} else {
						Vector2 newMoveBy = moveBy.magnitude * (moveBy.x>0?Vector2.right:Vector2.left)*0.4f;
						validMoveDest = DoStaticCollisions (newMoveBy+new Vector2(transform.position.x,transform.position.y));
					}

				}else if ((CollFlags & eCollFlags.LEFT) != 0 || (CollFlags & eCollFlags.RIGHT) != 0) {
					if (Vector2.Dot (moveBy.normalized, Vector2.left) > 0.8f || Vector2.Dot (moveBy.normalized, Vector2.right) > 0.8f) {
						
					} else {
						Vector2 newMoveBy = moveBy.magnitude * (moveBy.y>0?Vector2.up:Vector2.down)*0.4f;
						validMoveDest = DoStaticCollisions (newMoveBy+new Vector2(transform.position.x,transform.position.y));
					}

				}
			}
			owner.posXInt = (int)(validMoveDest.x * 1000f);
			owner.posYInt = (int)(validMoveDest.y * 1000f);
			//Vector2 validMoveDest = DoStaticCollisions (moveToTarget);
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
		//List<Vector2> collideDirs = new List<Vector2>();
		CollFlags = eCollFlags.NONE;
		//2D AABB 专用碰撞算法
		if( IsCollidingAABB( vCheckedPos ))
		{
			if(!canSlide)return  transform.position;
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

	public void backToValidPos(){
		if (!MapManager.getInstance ().isWorldPosObc (transform.position)) {
			Vector3 res = transform.position;

			Vector3Int p = MapManager.getInstance ().worldPosToCellPos (transform.position);

			{
				Vector3Int toCheck = new Vector3Int (p.x-1,p.y,0);
				if (toCheck.x < 0 || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.x = p.x * MapManager.TILE_WIDTH *0.01f  + bc.size.x / 2;
				}
			}
			{
				Vector3Int toCheck = new Vector3Int (p.x+1,p.y,0);
				if (toCheck.x >= MapManager.MAP_WIDTH || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.x = (p.x+1) * MapManager.TILE_WIDTH *0.01f + - bc.size.x / 2;
				}
			}
			{
				Vector3Int toCheck = new Vector3Int (p.x,p.y-1,0);
				if (toCheck.y < 0 || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.y = p.y * MapManager.TILE_HEIGHT *0.01f + bc.size.y / 2;
					res.y = -res.y;
				}
			}
			{
				Vector3Int toCheck = new Vector3Int (p.x,p.y+1,0);
				if (toCheck.y >= MapManager.MAP_HEIGHT || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.y = (p.y+1) * MapManager.TILE_HEIGHT *0.01f - bc.size.y / 2;
					res.y = -res.y;
				}
			}
			transform.position = res;
			//if(new Vector3(p.x-1,p.y))
		}
	}

}

