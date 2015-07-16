using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static GameController Instance;
	public bool isPlayerTurn;
	public bool areEnemiesMoving;

	private BoardController boardController;

	private List<Enemy> enemies;

	void Awake () {
		if (Instance != null && Instance != this) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad (gameObject);
		boardController = GetComponent<BoardController> ();
		enemies = new List<Enemy> ();

	}

	void Start(){
		boardController.SetupLevel();
		isPlayerTurn = true;
		areEnemiesMoving = false;
	}

	void Update () {

		if(isPlayerTurn || areEnemiesMoving){
			return;
		}
			StartCoroutine (MoveEnemies ());


	}

	private IEnumerator MoveEnemies(){
		areEnemiesMoving = true;

		yield return new WaitForSeconds(0.2f);

		foreach (Enemy enemy in enemies) {
			enemy.MoveEnemy();
			yield return new WaitForSeconds(enemy.moveTime);
		}

		areEnemiesMoving = false;
		isPlayerTurn = true;
	}

	public void AddEnemyToList(Enemy enemy){
		enemies.Add (enemy);

	}
}
