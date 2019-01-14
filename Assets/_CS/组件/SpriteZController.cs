using UnityEngine;
using System.Collections;

public class SpriteZController : MonoBehaviour
{

	public SpriteRenderer m_spriteRender;

	public static float maxZ = 100f;
	public static float minZ = 0f;

	public float mapHeight = 200 * 0.32f;

	public float characterSizeY;

	// Use this for initialization
	void Start () 
	{
		if (m_spriteRender == null)
		{
			m_spriteRender = GetComponentInChildren<SpriteRenderer>();
		}

		characterSizeY = m_spriteRender.bounds.size.y;
		//m_OverlayLayerZ = AutoTileMap.Instance.FindFirstLayer(AutoTileMap.eLayerType.Overlay).Depth;
		//m_GroundOverlayLayerZ = AutoTileMap.Instance.FindLastLayer(AutoTileMap.eLayerType.Ground).Depth;
	}

	// Update is called once per frame
	void Update () 
	{
		float rate = (maxZ - minZ) / mapHeight;
		//调整z
		transform.position = new Vector3 (transform.position.x, transform.position.y, (float)(mapHeight+(transform.position.y - characterSizeY*0.5f)) * rate + minZ);
	}
}

