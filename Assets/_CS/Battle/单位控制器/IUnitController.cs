using UnityEngine;
using System.Collections;

public class IUnitController : MonoBehaviour
{
	public bool inputEnabled = true;


	public float DV;
	public float DH;
	public float DTargetV;
	public float DTargetH;
	public float Dmag;
	public Vector3 Dvec;

	public float velocityDForward;
	public float velocityDLateral;

	public Vector2 offsetV;

	public float[] moveDir = new float[]{0,0};
	public float[] faceDir = new float[]{0,-1};

}

