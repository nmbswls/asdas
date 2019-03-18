using UnityEngine;
using System.Collections;
using System;
using CosTomUtil;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PathSeekBehaviour : MonoBehaviour
{

	public SearchMapNode[][] map;

	public EnemyController enemyCtrl;
	public GameLife target;



	public List<Vector3> pathPos = new List<Vector3>();

	PriorityQueue<SearchMapNode> openList = new PriorityQueue<SearchMapNode> (new NodeComparer());
	HashSet<SearchMapNode> closedList = new HashSet<SearchMapNode>();


	public void stopFollow(){
		target.followers.Remove (this);
	}

	public void resumeFollow(){
		target.followers.Add (this);
		reSearchPath ();
	}

	public void reSearchPath(){

		searchFixedPath (target.transform.position);
//		if (!findObcOnPath ()) {
//			enemyCtrl.path = new List<Vector3>();
//			enemyCtrl.path.Add(target.transform.position);
//			enemyCtrl.hasPath = true;
//			enemyCtrl.pathIdx = 0;
//			return;
//		}
//
//		Vector3Int startPos = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
//		Vector3Int endPos = MapManager.getInstance ().tilemap.WorldToCell (target.transform.position);
//		SearchMapNode startNode = map [-startPos.y] [startPos.x];
//		SearchMapNode endNode = map [-endPos.y] [endPos.x];
//		if (search (startNode, endNode)) {
//			enemyCtrl.path = pathPos;
//			enemyCtrl.hasPath = true;
//			enemyCtrl.pathIdx = 0;
//		} else {
//			enemyCtrl.path = new List<Vector3>();
//			enemyCtrl.hasPath = false;
//			enemyCtrl.pathIdx = 0;
//		}
	}

	public void searchFixedPath(Vector3 targetPos){
		if (!findObcOnPath (targetPos)) {
			//Debug.Log ("无障碍");
			enemyCtrl.path = new List<Vector3>();
			enemyCtrl.path.Add(target.transform.position);
			enemyCtrl.hasPath = true;
			enemyCtrl.pathIdx = 0;
			return;
		}

		Vector3Int startPos = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
		Vector3Int endPos = MapManager.getInstance ().tilemap.WorldToCell (targetPos);
		SearchMapNode startNode = map [-startPos.y] [startPos.x];
		SearchMapNode endNode = map [-endPos.y] [endPos.x];
		if (search (startNode, endNode)) {
			enemyCtrl.path = pathPos;
			enemyCtrl.hasPath = true;
			enemyCtrl.pathIdx = 0;
		} else {
			enemyCtrl.path = new List<Vector3>();
			enemyCtrl.hasPath = false;
			enemyCtrl.pathIdx = 0;
		}
	}

	bool isLeader;
	public PathSeekBehaviour leader = null;


	void Awake(){
		enemyCtrl = GetComponent<EnemyController> ();
	}

	public void changeIsLeader(bool isLeader){
		if (isLeader) {
			target.followers.Add (this);
			isLeader = true;
		} else {
			target.followers.Add (this);
			isLeader = true;
		}
	}

	public void selectLeader(){
		
//		PathSeekBehaviour closestOne = MapManager.getInstance ().getClosestEnemy ();
//		if ((closestOne.transform.position - transform.position).magnitude > 2f) {
//			changeIsLeader (true);
//		} else if (!closestOne.isLeader && closestOne.leader == null) {
//			changeIsLeader (true);
//		}

	}



	void setTarget(){
		target = BattleManager.getInstance ().player.gl;
		target.followers.Add (this);
	}


	void Start ()
	{
		map = new SearchMapNode[MapManager.MAP_HEIGHT][];
		for (int i = 0; i < MapManager.MAP_HEIGHT; i++) {
			map [i] = new SearchMapNode[MapManager.MAP_WIDTH];
			for (int j = 0; j < MapManager.MAP_WIDTH; j++) {
				map [i] [j] = new SearchMapNode (i,j,!MapManager.getInstance().staticBlocks[i][j]);
			}
		}
		setTarget ();
		//reSearchPath ();
	}
	


	public float getH(SearchMapNode node,SearchMapNode endNode){
		return Mathf.Abs (node.row - endNode.row) + Mathf.Abs (node.col - endNode.col);
	}




	public bool search(SearchMapNode startNode,SearchMapNode endNode){
		openList.Clear ();
		closedList.Clear ();

		SearchMapNode node = startNode;
		node.parent = null;
		int tryCount = 0;


		float time = System.Environment.TickCount;

		while (!node.Equals(endNode)) {
			tryCount++;
			int startY = node.row - 1 > 0 ? node.row - 1  : 0;
			int startX = node.col - 1 > 0 ? node.col - 1  : 0;
			int endX = node.col + 1 <= MapManager.MAP_WIDTH-1 ? node.col + 1 : MapManager.MAP_WIDTH-1;
			int endY = node.row + 1 <= MapManager.MAP_HEIGHT-1 ? node.row + 1 : MapManager.MAP_HEIGHT-1;
			for (int i = startY; i <= endY; i++) {
				for (int j = startX; j <= endX; j++) {
					//
					SearchMapNode test = map[i][j];

					if (test.Equals(node)) {
						continue;
					}
					float costF = 1f;
					if (!test.walkable || !map[node.row][test.col].walkable || !map[test.row][node.col].walkable) {
						continue;
					}

//					if (MapManager.getInstance ().dynamicBlocks [test.row] [test.col]) {
//						costF = 5f;
//					}
					float cost = 1f;
					if (!(node.row == test.row || node.col == test.col)) {
						cost = 1.4f;
					}
					float g = node.g + cost * costF;
					float h = getH(test,endNode);
					float f = g + h;
					if (openList.Contains (test)) {
						if (test.f > f) {
							test.f = f;
							test.g = g;
							test.h = h;
							test.parent = node;
						}
					} else if(!closedList.Contains(test)){
						test.f = f;
						test.g = g;
						test.h = h;
						test.parent = node;
						openList.Push (test);
					}

				}
			}
			closedList.Add (node);
			if (openList.Count == 0) {
				return false;
			}
			node = openList.Pop ();
		}
		SearchMapNode p = endNode;
		pathPos.Clear ();
		while (p.parent != null) {
			Vector3 pos = MapManager.getInstance().tilemap.CellToWorld(new Vector3Int(p.col,-p.row,0));
			pos.x += MapManager.TILE_WIDTH * 0.5f * 0.01f;
			pos.y += MapManager.TILE_HEIGHT * 0.5f * 0.01f;
			pos.z = 0;
			pathPos.Insert(0,pos);
			p = p.parent;
		}
		//Debug.Log ("Spend Times :" + (System.Environment.TickCount - time));
		return true;
	}

	public bool findObcOnPath(Vector3 fixedTarget){
		Vector3 startInWorld = transform.position;
		Vector3 endInWorld = fixedTarget;



		Vector3 start = MapManager.getInstance ().tilemap.WorldToCell (startInWorld);
		start.y = -start.y;
		Vector3 end = MapManager.getInstance ().tilemap.WorldToCell (endInWorld);
		end.y = -end.y;




		start += new Vector3 (0.5f,0.5f);
		end += new Vector3 (0.5f,0.5f);
		if (start.x == end.x) {
			if (checkY ((int)start.y, (int)end.y, (int)start.x)) {
				return true;
			}
			return false;
		}
		if (start.x > end.x) {
			Vector2 tmp = start;
			start = end;
			end = tmp;
		}
		int startIntX = (int)Mathf.Ceil (start.x);
		float k = (start.y - end.y) / (start.x - end.x);
		float nowY = (startIntX - start.x) * k + start.y;
		float lastY = nowY;

		int xInt = startIntX;


		for (int i = (int)start.y; i <= (int)nowY; i++) {
			if (checkY (start.y, nowY, xInt - 1)) {
				return true;
			}
		}

		while(xInt < (int)end.x){
			xInt++;
			lastY = nowY;
			nowY += 1 * k;
			bool res = checkY (lastY,nowY,xInt-1);
			if (res) {
				return true;
			}
		}
		for (int i = (int)lastY; i <= (int)end.y; i++) {
			if (checkY (lastY, end.y, xInt - 1)) {
				return true;
			}
		}
		return false;
	}
	public bool findObcOnPath(){
		return findObcOnPath (target.transform.position);

	}

	public bool checkY(float yf, float yt, int x){
		if (yf > yt) {
			float tmp = yf;
			yf = yt;
			yt = tmp;
		}
		for (int i = (int)yf; i <= (int)yt; i++) {
			if (MapManager.getInstance ().staticBlocks [i] [x]) {
				return true;
			}
		}
		return false;
	}

}

public class SearchMapNode{
	public int row;
	public int col;
	public bool walkable;

	public float g;
	public float h;
	public float f;

	public SearchMapNode parent = null;

	public SearchMapNode(float f){
		this.f = f;
	}
	public SearchMapNode(){
	}
	public SearchMapNode(int row, int col, bool walkable = true){
		this.row = row;
		this.col = col;
		this.walkable = walkable;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if ((obj.GetType().Equals(this.GetType())) == false)
		{
			return false;
		}
		SearchMapNode temp = null;
		temp = (SearchMapNode)obj;

		return this.row.Equals(temp.row) && this.col.Equals(temp.col);

	}
	public override int GetHashCode()
	{
		return this.row.GetHashCode() + this.col.GetHashCode();
	}
}

public class NodeComparer : IComparer<SearchMapNode>{
	public int Compare (SearchMapNode x, SearchMapNode y)
	{
		if (x.f == y.f)
			return 0;
		else if (x.f > y.f)
			return -1;
		else
			return 1;
	}


}