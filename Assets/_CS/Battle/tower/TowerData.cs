using UnityEngine;
using System.Collections;

//[SerializeField]
//public class TowerData
//{
//	public eAtkType atkType = eAtkType.MELLE_POINT;
//	public int damage = 10000;
//	public int atkInteval = 2000;
//	public int atkRange = 3000;
//	public int atkPreanimTime = 300;
//	public eProperty property = eProperty.NONE;
//}

[SerializeField]
public enum eAtkType
{
	NONE = -1,
	MELLE_POINT = 0,
	MELLE_AOE = 1,
	RANGED_HOMING = 2,
	RANGED_INSTANT = 3,
	RANGED_UNHOMING = 4,
	RANGED_MULTI = 5,

}
