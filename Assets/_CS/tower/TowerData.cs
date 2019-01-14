using UnityEngine;
using System.Collections;

[SerializeField]
public class TowerData
{
	public eAtkType atkType = eAtkType.MELLE;
	public int damage = 10000;
	public int atkInteval = 2000;
	public int atkRange = 3000;
	public int atkPreanimTime = 300;
}
[SerializeField]
public enum eAtkType
{
	MELLE = 0,
	RANGE = 1,
	MELLE_SECTOR = 2,
	UNHOMING = 3,
	RANGE_CIRCLE = 4,
}
