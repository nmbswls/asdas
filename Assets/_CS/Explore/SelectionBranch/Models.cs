using System;
using System.Collections.Generic;


[System.Serializable]
public class EncounterInfo{
	public string eId;
	public int type;
	public string desp = "An block";
	public Dictionary<int,EncounterStage> stages = new Dictionary<int,EncounterStage>();
}

[System.Serializable]
public class EncounterStage{
	public int sId;
	public string desp = "...............";
	//public bool isFinishStage = false;
	//public bool isBattleStage = false;
	public eStageType stageType = eStageType.NORMAL;
	public string extra;
	public EncounterBattleInfo battleInfo = new EncounterBattleInfo();
	public EncounterRes res = null;
	//public int randomValue = 0;
	//public int battleRes = 0;
	public List<EncounterConvert> converts = new List<EncounterConvert>();
}

[System.Serializable]
public class EncounterBattleInfo{
	public int difficulty = 1;
	public int liveTime = 300;
	public int killEnemy = 10;
}

[System.Serializable]
public class EncounterRes{
	public eFinishType type = eFinishType.NORMAL;
	public string finishEffect = "none";
	public List<EncounterReward> rewords = new List<EncounterReward> ();
}

[System.Serializable]
public class EncounterReward{
	public string rewardName = "hp";
	public int rewardAmount = 30;
	public int chance = 100;
}

[System.Serializable]
public class EncounterConvert{
	public int nextStageIdx;
	public string choiceDesp = "...";
	public List<EncounterConvertCheck> checks = new List<EncounterConvertCheck>();
}

[System.Serializable]
public class EncounterConvertCheck{
	public string key;
	public eCheckOpt fuhao = eCheckOpt.eq;
	public string value;
}

[System.Serializable]
public enum eCheckOpt{
	eq = 1,
	gt = 2,
	lt = 3,
	elt = 4,
	egt = 5,
	has = 6,
}


[System.Serializable]
public enum eStageType{
	NORMAL = 0,
	BATTLE = 1,
	CHECK = 2,
	FINISH = 3,
	RANDOM = 4,
}

[System.Serializable]
public enum eFinishType{
	NORMAL = 0,
	ANOTHER_ENCOUNTER = 1,
	NEXT_LEVEK = 2,
}



