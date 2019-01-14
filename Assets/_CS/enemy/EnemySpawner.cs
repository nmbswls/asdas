using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public int currentEnemyIdx = 0;
	public int MAX_ENEMY = 5;
	public int nowEnemy = 0;

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
				GameLife enemy = MonsterFactory.createEnemy ("ZOMBIE",transform.position,transform);
				nowEnemy++;
				localEnemies.Add (enemy);
				enemy.OnDieCallback += delegate() {
					GameManager.getInstance().enemy.Remove(enemy);
					this.localEnemies.Remove(enemy);
					this.nowEnemy --;
				};
				spawnCoolDown = 5000;
			}
		}
	}
}
