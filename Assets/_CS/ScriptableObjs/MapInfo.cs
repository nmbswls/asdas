using UnityEngine;
using System.Collections;



[CreateAssetMenu(fileName="info",menuName="cs526/map")]
[System.Serializable]
public class MapInfo : ScriptableObject
{

	[SerializeField]
	public string mapName = "map01";

	[SerializeField]
	public int width;

	[SerializeField]
	public int height;
}

