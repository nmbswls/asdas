using UnityEngine;
using System.Collections;

public class MapObject : MonoBehaviour
{

	public int posXInt;
	public int posYInt;
	public int height;

	public GameObject shadow;
	public GameObject actualObj;
	public static float maxZ = 100f;
	public static float minZ = 0f;
	void Awake(){
		
	}
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		//shadow.transform.position = new Vector3 (posXInt * 0.001f, posYInt * 0.001f, (float)(mapHeight+(transform.position.y - characterSizeY*0.5f)) * rate + minZ);
	}


}

