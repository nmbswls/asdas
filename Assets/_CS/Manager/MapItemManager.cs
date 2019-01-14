using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapItemManager : MonoBehaviour
{

	public int MAX_AMOUNT = 10;

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
		for (int i = 0; i < MAX_AMOUNT - nowItemAmount; i++) {
			GameObject o = GameObject.Instantiate (itemPrefab,mapPanel);
			int x = Random.Range (0,MapManager.MAP_WIDTH);
			int y = Random.Range (0,MapManager.MAP_HEIGHT);

			o.transform.position = MapManager.getInstance().tilemap.CellToWorld(new Vector3Int (x,-y,0));
			o.transform.position = new Vector3 (o.transform.position.x,o.transform.position.y,0);
			items.Add (o.GetComponent<MapItem>());
			nowItemAmount++;
		}
	}

	public void removeItem(MapItem item){
		items.Remove (item);
		nowItemAmount--;
	}
}

