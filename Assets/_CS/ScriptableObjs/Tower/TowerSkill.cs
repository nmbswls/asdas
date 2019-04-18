using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public enum eTowerSkillType{
	NONE = -1,
	ACTIVE = 0,
	SELF_TARGET = 1,
	PASSIVE = 2,
	AURA = 3,
}

[System.Serializable]
public enum ePassiveCheckPoint{
	ATK = 0,
	DANAGED = 1,
}


[CreateAssetMenu(fileName="New Tower Skill",menuName="cs526/tower/tower skill")]
[System.Serializable]
public class TowerSkill : ScriptableObject
{
	[SerializeField]
	public string skillId;

	[SerializeField]
	public string skillName;

	[SerializeField]
	[TextArea(3,5)]
	public string skillDesp;



	[SerializeField]
	public int maxLv;

	[SerializeField]
	public List<int> cooldown;



	[SerializeField]
	public List<int> x;

	[SerializeField]
	public List<int> y;

	[SerializeField]
	public List<int> z;


	[HideInInspector]
	[SerializeField]
	public eTowerSkillType tsType = eTowerSkillType.NONE;

	//主动
	[HideInInspector]
	[SerializeField]
	public eAtkType targetType = eAtkType.MELLE_POINT;

	[HideInInspector]
	//[SerializeField]
	public BulletStyle bulletStyle = new BulletStyle(); 

	//被动
	[HideInInspector]
	[SerializeField]
	public ePassiveCheckPoint checkPoint = ePassiveCheckPoint.ATK;


	//光环
	[HideInInspector]
	[SerializeField]
	public bool debuff = false;


	public TowerSkill(){
	}
	public TowerSkill(string skillId){
		this.skillId = skillId;
	}
}

