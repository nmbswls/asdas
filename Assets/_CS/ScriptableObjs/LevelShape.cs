using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[CreateAssetMenu(fileName="LevelShape",menuName="cs526/LevelShape")]
[System.Serializable]
public class LevelShape : ScriptableObject
{

	[SerializeField]
	public List<Vector2Int> activePos = new List<Vector2Int>();

	[SerializeField]
	public Vector2Int spawnPos;

	[SerializeField]
	public Vector2Int endPos;
}

