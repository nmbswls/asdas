using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapItemManager : MonoBehaviour
{

	public static int MAX_AMOUNT = 10;

	private int nowItemAmount = 0;
	public GameObject itemPrefab;
	public Transform mapPanel;

	public List<MapItem> items = new List<MapItem>(); 


	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (nowItemAmount >= MAX_AMOUNT) {
			return;
		}
		int toSpawn = MAX_AMOUNT - nowItemAmount;
		for (int i = 0; i < toSpawn; i++) {
			GameObject o = GameObject.Instantiate (itemPrefab,mapPanel);

			int x = -1;
			int y = -1;
			while (true) {
				x = Random.Range (0,MapManager.MAP_WIDTH-1);
				y = Random.Range (0,MapManager.MAP_HEIGHT-1);
				if (!MapManager.getInstance ().isCellObc (new Vector3Int (x, y, 0))) {
					break;
				} else {
					//Debug.Log (x+","+y);
				}
			}

			o.transform.position = MapManager.getInstance().tilemap.CellToWorld(new Vector3Int (x,-y,0));
			//o.transform.position = new Vector3(x*0.5f,-y*0.5f);
			o.transform.position = new Vector3 (o.transform.position.x + MapManager.TILE_WIDTH * 0.5f * 0.01f,o.transform.position.y + MapManager.TILE_HEIGHT * 0.5f * 0.01f,0);
			o.GetComponent<MapItem> ().init ("money",10);
			items.Add (o.GetComponent<MapItem>());
			nowItemAmount++;
		}
	}

	public void removeItem(MapItem item){
		items.Remove (item);
		nowItemAmount--;
	}
}

