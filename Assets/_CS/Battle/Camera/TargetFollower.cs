using UnityEngine;
using System.Collections;

public class TargetFollower : MonoBehaviour
{

	public float DampTime = 0.5f;
	public Transform Target;

	private Vector3 velocity = Vector3.zero;
	private Camera m_camera;

	float rightBound = 0;
	void Start()
	{
		m_camera = GetComponent<Camera>();
		rightBound = MapManager.TILE_WIDTH * MapManager.getInstance().MAP_WIDTH / 100f;

	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		if (Target)
		{
			Vector3 point = Target.position;
			if (point.x < 3) {
				point.x = 3;
			}
			if (point.x > rightBound - 3f) {
				point.x = rightBound - 3f;
			}
			//右边界计算
			Vector3 delta = /*Target.position*/point - m_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f)); //(new Vector3(0.5, 0.5, point.z));
			delta.z=0;
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, DampTime);

		}		
	}
}

