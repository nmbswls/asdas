using UnityEngine;
using System.Collections;
using System;
using CosTomUtil;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PathTest : MonoBehaviour
{

	public SearchMapNode[][] map;
	//public Image[][] images;

	public MovementController movementCtrl;


	PriorityQueue<SearchMapNode> openList = new PriorityQueue<SearchMapNode> (new NodeComparer());
	HashSet<SearchMapNode> closedList = new HashSet<SearchMapNode>();

	public void reSearchPath(){
		Vector3Int startPos = MapManager.getInstance ().tilemap.WorldToCell (transform.position);
		Vector3Int endPos = MapManager.getInstance ().tilemap.WorldToCell (target.transform.position);
		SearchMapNode startNode = map [-startPos.y] [startPos.x];
		SearchMapNode endNode = map [-endPos.y] [endPos.x];
		if (search (startNode, endNode)) {
			movementCtrl.path = pathPos;
			movementCtrl.hasPath = true;
		} else {
//			movementCtrl.path = new Vector3();
			movementCtrl.hasPath = false;
		}
	}
	public GameLife target;

	bool isLeader;
	public PathSeekBehaviour leader = null;
	public void changeIsLeader(bool isLeader){
		if (isLeader) {
//			target.followers.Add (this);
			isLeader = true;
		} else {
//			target.followers.Add (this);
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

	int maxI = 30;
	int maxJ = 30;

	//public GameObject image;
	//public GameObject dot;
	//public Transform dotLayer;
	//public Transform UILayer;

	//public Button btn;


	public List<Vector3> pathPos = new List<Vector3>();
	public int nowIndex = 0;

	void setTarget(){
		target = GameManager.getInstance ().player;
//		target.followers.Add (this);
	}
	void OnEnable(){

	}

	// Use this for initialization
	void Start ()
	{



		map = new SearchMapNode[maxI][];
		//images = new Image[maxI][];
		for (int i = 0; i < maxI; i++) {
			map [i] = new SearchMapNode[maxJ];
			//images [i] = new Image[maxJ];
			for (int j = 0; j < maxJ; j++) {
				map [i] [j] = new SearchMapNode (i,j,true);
				//				GameObject o = GameObject.Instantiate (image,UILayer);
				//				o.transform.localPosition = new Vector3 (10*j,-10*i,0);
				//				images [i] [j] = o.GetComponent<Image> ();
				//				UIEventListener mListener = o.AddComponent<UIEventListener> ();
				//				int finalI = i;
				//				int finalJ = j;
				//				mListener.ClickEvent += delegate(GameObject gb,PointerEventData eventData) {
				//					
				//					map[finalI][finalJ].walkable = !map[finalI][finalJ].walkable;
				//					o.GetComponent<Image>().color = map[finalI][finalJ].walkable ? Color.white : Color.red;
				//				};
			}
		}
		//		UIEventListener m2Listener = btn.gameObject.AddComponent<UIEventListener> ();
		//
		//		m2Listener.ClickEvent += delegate(GameObject gb,PointerEventData eventData) {
		//			foreach(Transform child in dotLayer){
		//				GameObject.Destroy(child.gameObject);
		//			}
		//			search();
		//		};
		//Debug.DrawLine(new Vector3(0,0,0), new Vector3(100,100,100),Color.yellow,100000);


		//findObcOnPath (new Vector2(0.4f,2.6f),new Vector2(4.7f,0.4f));
		setTarget ();
		reSearchPath ();
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
			int endX = node.col + 1 <= maxJ-1 ? node.col + 1 : maxJ-1;
			int endY = node.row + 1 <= maxI-1 ? node.row + 1 : maxI-1;
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
			//GameObject o = GameObject.Instantiate (dot,dotLayer);
			//o.transform.position = images [p.row] [p.col].transform.position;
			pathPos.Insert(0,MapManager.getInstance().tilemap.CellToWorld(new Vector3Int(p.col,-p.row,0)));
			p = p.parent;
		}
		nowIndex = 0;

		//Debug.Log ("Spend Times :" + (System.Environment.TickCount - time));
		return true;
	}

	public static void findObcOnPath(Vector2 start,Vector2 end){
		if (start.x > end.x) {
			Vector2 tmp = start;
			start = end;
			end = tmp;
		}
		int startIntX = (int)Mathf.Ceil (start.x);
		float k = (start.y - end.y) / (start.x - end.x);
		float nowY = (startIntX - start.x) * k + start.y;
		float lastY = -1;

		int xInt = startIntX;



		for (int i = (int)start.y; i <= (int)nowY; i++) {
			checkY (start.y,nowY,xInt-1);
		}

		while(xInt < end.x){
			xInt++;
			lastY = nowY;
			nowY += 1 * k;
			checkY (lastY,nowY,xInt-1);

		}
		for (int i = (int)lastY; i <= (int)end.y; i++) {
			checkY (lastY,end.y,xInt-1);
		}
		//y = kx + b;
	}

	public static void checkY(float yf, float yt, int x){
		if (yf > yt) {
			float tmp = yf;
			yf = yt;
			yt = tmp;
		}
		for (int i = (int)yf; i <= (int)yt; i++) {
			//			Debug.Log ((x) + "," + i + "占有");
		}
	}

}

//public class SearchMapNode{
//	public int row;
//	public int col;
//	public bool walkable;
//
//	public float g;
//	public float h;
//	public float f;
//
//	public SearchMapNode parent = null;
//
//	public SearchMapNode(float f){
//		this.f = f;
//	}
//	public SearchMapNode(){
//	}
//	public SearchMapNode(int row, int col, bool walkable = true){
//		this.row = row;
//		this.col = col;
//		this.walkable = walkable;
//	}
//
//	public override bool Equals(object obj)
//	{
//		if (obj == null)
//		{
//			return false;
//		}
//		if ((obj.GetType().Equals(this.GetType())) == false)
//		{
//			return false;
//		}
//		SearchMapNode temp = null;
//		temp = (SearchMapNode)obj;
//
//		return this.row.Equals(temp.row) && this.col.Equals(temp.col);
//
//	}
//	public override int GetHashCode()
//	{
//		return this.row.GetHashCode() + this.col.GetHashCode();
//	}
//}
//
//public class NodeComparer : IComparer<SearchMapNode>{
//	public int Compare (SearchMapNode x, SearchMapNode y)
//	{
//		if (x.f == y.f)
//			return 0;
//		else if (x.f > y.f)
//			return -1;
//		else
//			return 1;
//	}
//
//
//}