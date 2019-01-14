using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance;

	public GameObject UILayer;


	public List<GameLife> enemy = new List<GameLife>();
	private List<Tower> towers = new List<Tower>();
	public GameLife player;

	public MainUIManager mainUIManager;
	public MapItemManager mapItemManager;
	public static GameManager getInstance(){
		return instance;
	}

	void Awake(){
		checkGameObject ();
		checkSingle ();
	}




	public void addTower(Tower t){
		towers.Add (t);
		MapManager.getInstance ().updateBlock ();
	}

	public void removeTower(Tower t){
		towers.Remove (t);
		MapManager.getInstance ().updateBlock ();
	}

	public List<Tower> getAllTower(){
		return towers;
	}

	void Start(){
		mapItemManager = GetComponent<MapItemManager> ();
	}

	public GameLife getPlayer(){
		return player;
	}

	public void lockAllEnemies(){
		
	}

	public void unlockAllEnemies(){
		
	}



	void Update(){
//		if (Input.GetKeyDown (KeyCode.Escape)) {
//			if (!UILayer.activeSelf) {
//				UILayer.SetActive (true);
//				Time.timeScale = 0.0f;
//				playerAC.lockActor ();
//				lockAllEnemies ();
//			} else {
//				UILayer.SetActive (false);
//				Time.timeScale = 1.0f;
//				playerAC.unlockActor ();
//				unlockAllEnemies ();
//			}
//
//		}
	}
	private void checkGameObject(){
		if (tag == "GM") {
			return;
		}
		Destroy (this);
	}

	private void checkSingle(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
			return;
		}
		Destroy (this);
	}
}
