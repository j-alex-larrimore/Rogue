using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardController : MonoBehaviour {

	public int columns;
	public int rows;

	public GameObject[] floors;
	public GameObject[] outerWalls;
	private Transform gameBoard;
	private List<Vector3> obstaclesGrid;

	void Awake () {
		obstaclesGrid = new List<Vector3> ();
	}

	void Update () {
	
	}

	private void InitializeObstaclePositions(){
		obstaclesGrid.Clear();
		for (int x = 2; x < columns - 2; x++) {
			for (int y = 2; y < rows - 2; y++) {
				obstaclesGrid.Add(new Vector3(x, y, 0f));
			}
		}
	}

	private void SetupGameBoard(){
		gameBoard = new GameObject ("Game Board").transform;

		for (int x = 0; x < columns; x++) {
			for(int y = 0; y<rows; y++){
				GameObject selectedTile;
				if(x == 0 || y == 0 || x == columns - 1 || y == rows - 1){
					selectedTile = outerWalls[Random.Range (0, outerWalls.Length)];
				}else{
					selectedTile = floors[Random.Range(0, floors.Length)];
				}
				GameObject floorTile = (GameObject)Instantiate(selectedTile, new Vector3(x, y, 0f), Quaternion.identity);
				floorTile.transform.SetParent(gameBoard);
			}
		}
	}

	private void SetRandomObstaclesOnGrid(GameObject[] obstaclesArray, int minimum, int maximum){
		int obstacleCount = Random.Range (minimum, maximum + 1);

		if (obstacleCount > obstaclesGrid.Count) {
			obstacleCount = obstaclesGrid.Count;
		}

		for (int index = 0; index<obstacleCount; index++) {
			
		}
	}

	public void SetupLevel(){
		InitializeObstaclePositions ();
		SetupGameBoard();
	}
}
