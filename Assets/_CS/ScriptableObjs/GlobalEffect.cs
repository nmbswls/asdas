using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName="Global Effect",menuName="cs526/global effect")]
[System.Serializable]
public class GlobalEffect : MonoBehaviour
{

	[SerializeField]
	public string globalEffectId;

	[SerializeField]
	public string globalEffectName;

	[SerializeField]
	[TextArea(3,5)]
	public string globalEffectDesp;
}

