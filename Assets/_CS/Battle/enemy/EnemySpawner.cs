using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public int difficultyLevel = 1;

	public string enemyName = "10000";
	public int currentEnemyIdx = 0;
	public int MAX_ENEMY = 5;
	public int nowEnemy = 0;

	public int spawnInteval = 10000;
	int spawnCoolDown = 0;
	public List<GameLife> localEnemies = new List<GameLife>(); 

	public void Tick (int timeDelta, int timeTotal) {

		if (difficultyLevel <= 5) {
			if (timeTotal > 2000 * difficultyLevel) {
				difficultyLevel += 1;
				Debug.Log ("diffi 升级");
			}
		}

		if (spawnCoolDown > 0) {
			spawnCoolDown -= /*从服务器取server frame time*/timeDelta;
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
