﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
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

[System.Serializable]
public enum eProperty{
	NONE= 0,
	RED = 1,
	BLUE = 2,
	GREEN = 3,
	PURPLE = 4,
	YELLOW = 5,
	BLACK = 6,
}

[System.Serializable]
public class BulletStyle{

	[SerializeField]
	public string bulletName = "default";
	[SerializeField]
	public bool isBall;
	[SerializeField]
	public int flySpeed;
}

[CreateAssetMenu(fileName="New Tower Model",menuName="cs526/tower/tower base")]
[System.Serializable]
public class TowerBase : ScriptableObject
{
	[SerializeField]
	public string tid;

	[SerializeField]
	public string tname;

	[SerializeField]
	public TowerModel towerModel;

	[SerializeField]
	[TextArea(3,10)]
	public string tdesp;

	[SerializeField]
	public BulletStyle bulletStyle = new BulletStyle();

	[SerializeField]
	public int[] cost = new int[3];

	[SerializeField]
	public eAtkType atkType = eAtkType.NONE;

	[SerializeField]
	public AtkInfo mainAtk = new AtkInfo();

	[SerializeField]
	public List<AtkInfo> extraAtk = new List<AtkInfo>();

	[SerializeField]
	public int mingzhong;

	[SerializeField]
	public int atkSpd;

	[SerializeField]
	public List<SkillState> skills = new List<SkillState>();
}
	