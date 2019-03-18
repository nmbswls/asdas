using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public string enemyName = "enemy01";
	public int currentEnemyIdx = 0;
	public int MAX_ENEMY = 5;
	public int nowEnemy = 0;

	public int spawnInteval = 50000;
	int spawnCoolDown = 0;
	public List<GameLife> localEnemies = new List<GameLife>(); 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (spawnCoolDown > 0) {
			spawnCoolDown -= /*从服务器取server frame time*/(int)(Time.deltaTime * 1000f);
		}

		if (spawnCoolDown <= 0) {
			if (nowEnemy < MAX_ENEMY) {
				GameLife enemy = MonsterFactory.createEnemy (enemyName,transform.position,transform);
				nowEnemy++;
				localEnemies.Add (enemy);
				enemy.OnDieCallback += delegate() {
					BattleManager.getInstance().enemyDie(enemy);
					this.localEnemies.Remove(enemy);
					this.nowEnemy --;
				};
				spawnCoolDown = spawnInteval;
			}
		}
	}
}
