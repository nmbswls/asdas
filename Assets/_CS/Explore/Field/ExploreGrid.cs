using UnityEngine;
using System.Collections;

public class ExploreGrid : MonoBehaviour
{
	public string eid = "";
	public int i = 0;
	public int j = 0;
	public bool isFinish;
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void reveal(){
		transform.GetChild (1).gameObject.SetActive (false);
	}

}

