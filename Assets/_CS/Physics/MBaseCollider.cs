using UnityEngine;
using System.Collections;

public class MBaseCollider : MonoBehaviour
{

	public bool dirty;
	public GameObject owner;

//	public abstract int AvgCollisionRadius
//	{
//		get;
//	}

	public void OnEnable()
	{
		this.dirty = true;
		//this.owner.Release();
	}

	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

