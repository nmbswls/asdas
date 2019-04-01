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
	//BoxCollider2D bc;
	int bcRectSizeX;
	int bcRectSizeY;

	void Awake(){
		owner = GetComponent<GameLife> ();
		BoxCollider2D bc = GetComponent<BoxCollider2D> ();
		bcRectSizeX = (int)(bc.size.x * 1000);
		bcRectSizeY = (int)(bc.size.y * 1000);
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
		if (IsCollidingAABB(new Vector2Int(owner.posXInt,owner.posYInt))) {
			backToValidPos ();
		}
		if (doMove) {
			Vector2Int moveByInt = new Vector2Int ((int)(moveBy.x*1000),(int)(moveBy.y*1000));
			Vector2Int validMoveDest = DoStaticCollisions (moveByInt+new Vector2Int(owner.posXInt,owner.posYInt));
//			if (canSlide) {
//				if ((CollFlags & eCollFlags.DOWN) != 0 || (CollFlags & eCollFlags.UP) != 0) {
//					if (Vector2.Dot (moveBy.normalized, Vector2.up) > 0.9f || Vector2.Dot (moveBy.normalized, Vector2.down) > 0.9f) {
//						//validMoveDest = transform.position;
//					} else {
//						Vector2 newMoveBy = moveBy.magnitude * (moveBy.x > 0 ? Vector2.right : Vector2.left) * 0.8f;
//						validMoveDest = DoStaticCollisions (newMoveBy + new Vector2 (transform.position.x, transform.position.y));
//					}
//
//				} else if ((CollFlags & eCollFlags.LEFT) != 0 || (CollFlags & eCollFlags.RIGHT) != 0) {
//					if (Vector2.Dot (moveBy.normalized, Vector2.left) > 0.9f || Vector2.Dot (moveBy.normalized, Vector2.right) > 0.9f) {
//						//validMoveDest = transform.position;
//					} else {
//						Vector2 newMoveBy = moveBy.magnitude * (moveBy.y > 0 ? Vector2.up : Vector2.down) * 0.8f;
//						validMoveDest = DoStaticCollisions (newMoveBy + new Vector2 (transform.position.x, transform.position.y));
//					}
//
//				} else {
//					
//				}
//			}
			owner.posXInt = validMoveDest.x;
			owner.posYInt = validMoveDest.y;
			//Vector2 validMoveDest = DoStaticCollisions (moveToTarget);
			//transform.position = validMoveDest;
		}
		doMove = false;
	}

	bool IsCollidingAABB(Vector2Int vCheckPos){
		//BoxCollider2D bc = GetComponent<BoxCollider2D> ();

		Vector2Int posCenter = vCheckPos;
		//得到矩形
		Vector2Int lt = MapManager.getInstance().WorldToCell(posCenter+new Vector2Int(-bcRectSizeX/2,bcRectSizeY/2));
		//lt.y = -lt.y;
		Vector2Int rb = MapManager.getInstance().WorldToCell(posCenter+new Vector2Int(bcRectSizeX/2,-bcRectSizeY/2));
		//rb.y = -rb.y;
		for(int i=lt.y;i<=rb.y;i++){
			for (int j = lt.x; j <= rb.x; j++) {
				if (i<0||i>=MapManager.MAP_HEIGHT || j<0|| j>= MapManager.MAP_WIDTH || MapManager.getInstance ().staticBlocks [i] [j] == true) {
					return true;
				}
			}
		}
		return false;
	}

	Vector2Int DoStaticCollisions(Vector2Int toMove) 
	{


		Vector2Int vTempPos = toMove;
		Vector2Int vCheckedPos = toMove;

		CollFlags = eCollFlags.NONE;
		//2D AABB 专用碰撞算法
		if( IsCollidingAABB( vCheckedPos ))
		{
			//if(!canSlide)return vTempPos;
			//m_speed = 0f;
			vCheckedPos.y = owner.posYInt;
			if( !IsCollidingAABB( vCheckedPos ) )
			{
				vTempPos.y = owner.posYInt;
				CollFlags |= vTempPos.y > toMove.y? eCollFlags.DOWN : eCollFlags.UP;
			}
			else
			{
				vCheckedPos = toMove;
				vCheckedPos.x = owner.posXInt;
				if( !IsCollidingAABB( vCheckedPos ) )
				{
					vTempPos.x = owner.posXInt;
					CollFlags |= vTempPos.x > toMove.x? eCollFlags.LEFT : eCollFlags.RIGHT;
				}
				else
				{
					vTempPos.x = owner.posXInt;
					vTempPos.y = owner.posYInt;

					CollFlags |= vTempPos.y > toMove.y? eCollFlags.DOWN : eCollFlags.UP;
					CollFlags |= vTempPos.x > toMove.x? eCollFlags.LEFT : eCollFlags.RIGHT;
				}
			}

		}
		else
		{
			
		}

		return vTempPos;
	}

	public void backToValidPos(){
		
		if (!MapManager.getInstance ().isWorldPosObc (new Vector2Int(owner.posXInt,owner.posYInt))) {
			Vector2Int res = new Vector2Int (owner.posXInt, owner.posYInt);

			//Vector3 posWithTail = MapManager.getInstance ().WorldToCell (new Vector2Int(owner.posXInt,owner.posYInt));
			Vector2Int p = MapManager.getInstance ().WorldToCell (new Vector2Int(owner.posXInt,owner.posYInt));
			{
				Vector3Int toCheck = new Vector3Int (p.x-1,p.y,0);
				if (toCheck.x < 0 || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.x = p.x * MapManager.TILE_WIDTH *10  + bcRectSizeX / 2;
				}
			}
			{
				Vector3Int toCheck = new Vector3Int (p.x+1,p.y,0);
				if (toCheck.x >= MapManager.MAP_WIDTH || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.x = (p.x+1) * MapManager.TILE_WIDTH *10 - bcRectSizeX/ 2;
				}
			}
			{
				Vector3Int toCheck = new Vector3Int (p.x,p.y-1,0);
				if (toCheck.y < 0 || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.y = -p.y * MapManager.TILE_HEIGHT *10 - bcRectSizeY / 2;
				}
			}
			{
				Vector3Int toCheck = new Vector3Int (p.x,p.y+1,0);
				if (toCheck.y >= MapManager.MAP_HEIGHT || MapManager.getInstance().staticBlocks[toCheck.y][toCheck.x]) {
					res.y = -(p.y+1) * MapManager.TILE_HEIGHT *10 + bcRectSizeY / 2;
				}
			}
			owner.posXInt = res.x;
			owner.posYInt = res.y;
		}
	}

}

