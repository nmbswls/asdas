using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(TowerSkill))]  
public class TowerSkillEditor : Editor
{

	private SerializedObject obj;  
	private TowerSkill towerSkill;  
    private SerializedProperty skillType;  




	private SerializedProperty targetType;  
	private SerializedProperty checkPoint;  
	private SerializedProperty bulletStyle; 
	private SerializedProperty debuff;





    void OnEnable()  
    {  
	    obj = new SerializedObject(target); 

		skillType = obj.FindProperty ("tsType");

		checkPoint = obj.FindProperty("checkPoint");
		targetType = obj.FindProperty("targetType");  
		bulletStyle = obj.FindProperty("bulletStyle");  

		debuff = obj.FindProperty("debuff");  

	}  
    public override void OnInspectorGUI()  
    {  
		base.OnInspectorGUI();
		obj.Update();

//		EditorGUILayout.PropertyField(skillId);  
//		EditorGUILayout.PropertyField(skillName);  
//		EditorGUILayout.PropertyField(skillDesp);  
//		EditorGUILayout.PropertyField(maxLv);
//
//
//		EditorGUILayout.PropertyField(cooldown,true);
//		EditorGUILayout.PropertyField(x,true);
//		EditorGUILayout.PropertyField(y,true);
//		EditorGUILayout.PropertyField(z,true);

		towerSkill = (TowerSkill)target;  
		towerSkill.tsType = (eTowerSkillType)EditorGUILayout.EnumPopup("Type-", towerSkill.tsType);  
        

		if (towerSkill.tsType == eTowerSkillType.ACTIVE) {
			EditorGUILayout.PropertyField(targetType); 

			EditorGUILayout.PropertyField(bulletStyle.FindPropertyRelative ("bulletName")); 
		}else if(towerSkill.tsType == eTowerSkillType.PASSIVE){
			EditorGUILayout.PropertyField(checkPoint);
		}else if(towerSkill.tsType == eTowerSkillType.AURA){
			EditorGUILayout.PropertyField(debuff);
		}

		obj.ApplyModifiedProperties ();
	} 


}

