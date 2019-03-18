using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FairyGUI;



public class PlayerController : IUnitController {




	public Rigidbody2D rBody;


	public Camera2DController camera2D;


	TargetFollower cameraFollowBehaviour;

	protected virtual void Start()
	{
		rBody = GetComponent<Rigidbody2D> ();

		if( camera2D == null )
		{
			camera2D = GameObject.FindObjectOfType<Camera2DController>();
		}

		cameraFollowBehaviour = camera2D.transform.GetComponent<TargetFollower>();
		cameraFollowBehaviour.Target = transform;

		moveDir = new float[]{0,0};
		faceDir = new float[]{0,-1};


	}

	public string keyUpString = "w";
	public string keyDownStrig = "s";
	public string keyLeftStrig = "a";
	public string keyRightStrig = "d";

	public static float offsetMin = 500f;
//	public string keyRunStrign = "left shift";
//	public string keyJumpString = "space";
//	public string keyRollString = "v";
//	public string keyJabString = "b";
//	public string keyAttackString = "j";
//	public string keyAttackRString = "k";



	//控制器
	private string[] keys = new string[4];


	private int[,] dirVec = new int[4,2]{{0,1}, {-1,0}, {0,-1}, {1,0}};


	private List<int> moveOptStack = new List<int> ();
	public float characterSizeY;



	public static float OPT_INPUT_INTERVAL =  0.13f;
	float optInputCD = 0f;

	void Awake(){
		keys = new string[]{keyUpString, keyLeftStrig, keyDownStrig, keyRightStrig};
	}


	void checkActionInput(){
		if (optInputCD > 0) {
			return;
		} else {
			if(Input.GetKeyDown(KeyCode.J)){
				//gameLife.attack ();
			}
		}
	}

	Vector2 SquareToCircle(Vector2 input){
		Vector2 output = Vector2.zero;
		output.x = input.x * Mathf.Sqrt (1 - (input.y * input.y)/2.0f);
		output.y = input.y * Mathf.Sqrt (1 - (input.x * input.x)/2.0f);
		return output;
	}

	int mapIdax= 0;

	// Update is called once per frame
	void Update () {

		checkMoveInput ();

		if (!inputEnabled) {
			DTargetV = 0;
			DTargetH = 0;
		}

//		DH = Mathf.SmoothDamp (DH,DTargetH,ref velocityDForward,0.1f);
//		DV = Mathf.SmoothDamp (DV,DTargetV,ref velocityDLateral,0.1f);

		DH = DTargetH;
		DV = DTargetV;

		Vector2 circleVector = SquareToCircle (new Vector2(DH,DV));
		float DHReal = circleVector.x;
		float DVReal = circleVector.y;
		if (Mathf.Abs (DHReal) < Mathf.Abs (DVReal)) {
			DHReal = 0;
		} else {
			DVReal = 0;
		}

		Dmag = Mathf.Sqrt (DHReal * DHReal + DVReal*DVReal);
		Dvec = new Vector2 (DHReal,DVReal);

		if (Input.GetKeyDown ("k")) {
			GetComponent<PlayerModule> ().SpawnTower (0);
		}
//		if (Input.GetKeyDown ("l")) {
//			GetComponent<PlayerModule> ().SpawnTower (1);
//		}
//		if (Input.GetKeyDown ("p")) {
//			GetComponent<PlayerModule> ().SpawnTower (2);
//		}
		if (Input.GetKeyDown ("y")) {
			GetComponent<GameLife> ().buffManager.addBuff (new Buff(1,50,5000));
//			foreach (GameLife gl in BattleManager.getInstance().enemies) {
//				gl.buffManager.addBuff (new Buff(1,50,5000));
//			}
		}

		if (Input.GetKeyDown ("m")) {
			MapManager.getInstance ().LoadMap ((mapIdax++)%2 + 1);
		}


		//转向;

	}

	void checkMoveInput(){
//		float degree = BattleManager.getInstance ().mainUIManager.degree;
//		float offset = BattleManager.getInstance ().mainUIManager.offset;
//
//		if (offset < offsetMin) {
//			DTargetH = 0;
//			DTargetV = 0;
//			return;
//		}
//
//		if (degree < 45 && degree >= -45) {
//			DTargetH = 1;
//			DTargetV = 0;
//		} else if (degree < 135 && degree >= 45) {
//			DTargetH = 0;
//			DTargetV = -1;
//		} else if (degree < -45 && degree >= -135) {
//			DTargetH = 0;
//			DTargetV = 1;
//		} else {
//			DTargetH = -1;
//			DTargetV = 0;
//		}
//		return;
		bool optUpdate = false;
		for (int i = 0; i < 4; i++) {
			if (Input.GetKeyDown (keys [i])) {
				if (!moveOptStack.Contains (i)) {
					moveOptStack.Add (i);
					optUpdate = true;
				}
			}
		}
		for (int i = 0; i < 4; i++) {
			if (Input.GetKeyUp (keys [i])) {
				moveOptStack.Remove (i);
				optUpdate = true;
			}
		}
		if (!optUpdate) {
			return;
		}
		moveDir [0] = 0;
		moveDir [1] = 0;
		if (moveOptStack.Count > 4) {
			return;
		}
		if (moveOptStack.Count == 2) {
			if (((moveOptStack [0] % 2) ^ (moveOptStack [1] % 2)) != 0) {
				moveDir [0] = dirVec [moveOptStack [1], 0];
				moveDir [1] = dirVec [moveOptStack [1], 1];
			}
		} else {
			for (int i = 0; i < moveOptStack.Count; i++) {
				moveDir [0] += dirVec [moveOptStack [i], 0];
				moveDir [1] += dirVec [moveOptStack [i], 1];
			}
		}
		DTargetH = moveDir [0];
		DTargetV = moveDir [1];
	}
}
