using UnityEngine;
using System.Collections;
using System.Collections.Generic;



//不知道取什么名 表示各encounter的出现条件和被选中的概率
[System.Serializable]
public class EncounterEntry{
	public string eid;
	public int chance;
	public string condition;
}

[CreateAssetMenu(fileName="level",menuName="cs526/level")]
[System.Serializable]
public class LevelEntry : ScriptableObject
{

	[SerializeField]
	private string levelId;
	public string LevelId {
		get {
			return this.levelId;
		}
	}

	[SerializeField]
	private int numOfGrid;
	public int NumOfGrid {
		get {
			return this.numOfGrid;
		}
	}

	[SerializeField]
	private int numOfEncounter;
	public int NumOfEncounter {
		get {
			return this.numOfEncounter;
		}
	}

	[SerializeField]
	private List<EncounterEntry> possibleEncounters = new List<EncounterEntry>();

	public List<EncounterEntry> PossibleEncounters {
		get {
			return this.possibleEncounters;
		}
	}

	[SerializeField]
	private string nextLevel;
	public string NextLevel {
		get {
			return this.nextLevel;
		}
	}

//	public List<string> getRandomEncounters(int num){
//		List<int> gailv = new List<int> ();
//		gailv.Add (possibleEncounters[0].chance);
//		for (int i = 1; i < PossibleEncounters.Count; i++) {
//			gailv.Add (gailv[i-1]+possibleEncounters[i].chance);
//		}
//		return null;
//	}
	public List<string> getRandomEncounters(int num){
		List<string> ee = new List<string> ();
		for (int i = 0; i < possibleEncounters.Count; i++) {
			ee.Add (possibleEncounters[i].eid);
		}

		for (int i = ee.Count-1; i > 0; i--) {
			int pos = Random.Range (0,i);
			var x = ee[i];
			ee[i] = ee[pos];
			ee[pos] = x;
		}
		return ee.GetRange (0,num);
	}

}

