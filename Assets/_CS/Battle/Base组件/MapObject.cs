using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour
{
	//[HideInInspector]
	public int posXInt;
	//[HideInInspector]
	public int posYInt;
//	[HideInInspector]
//	public int height;

	[HideInInspector]
	public bool isHighLight = false;

	public GameObject shadow;

	public GameObject actualObj;
	public static float maxZ = 100f;
	public static float minZ = 0f;

	float rate;
	float mapHeight;
	void Awake(){
		
	}
	// Use this for initialization
	protected virtual void Start ()
	{
		mapHeight = MapManager.getInstance().MAP_HEIGHT;
		rate = (maxZ - minZ) / mapHeight;
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{
		//transform.position = new Vector3 (transform.position.x, transform.position.y, (float)(mapHeight+(transform.position.y)) * rate + minZ - (isHighLight?5f:0f));
		transform.position = new Vector3 (posXInt*0.001f, posYInt*0.001f, (float)(mapHeight+(transform.position.y)) * rate + minZ - (isHighLight?5f:0f));
	}

	public void updatePosImmediately(){
		transform.position = new Vector3 (posXInt*0.001f, posYInt*0.001f, (float)(mapHeight+(transform.position.y)) * rate + minZ - (isHighLight?5f:0f));
	}


}

