using UnityEngine;
using System.Collections;




[CreateAssetMenu(fileName="New Tower Model",menuName="cs526/tower/tower model")]
[System.Serializable]
public class TowerModel : ScriptableObject
{

	[SerializeField]
	public string modelName = "warrior";

	[SerializeField]
	public int atkInterval = 2000;

	[SerializeField]
	public int atkRange = 3000;

	[SerializeField]
	public int atkPreTime = 300;

	[SerializeField]
	public int atkPostTime = 700;

	[SerializeField]
	public int castPreTime = 800;

	[SerializeField]
	public int castPostTime = 700;
}

