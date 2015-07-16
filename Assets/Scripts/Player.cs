using UnityEngine;
using System.Collections;

public class Player : MovingObject {

	private Animator animator;
	private int attackPower = 1;

	protected override void Start(){
		base.Start ();
		animator = GetComponent<Animator> ();
	}

	void Update () {
		if (!GameController.Instance.isPlayerTurn) {
			return;
		}

		int xAxis = 0;
		int yAxis = 0;

		xAxis = (int)Input.GetAxisRaw ("Horizontal");
		yAxis = (int)Input.GetAxisRaw ("Vertical");

		if (xAxis != 0) {
			yAxis = 0;
		}

		if (xAxis != 0 || yAxis != 0) {
			Move<Wall> (xAxis, yAxis);
			GameController.Instance.isPlayerTurn = false;
		}

	}

	protected override void HandleCollision<T>(T component){
		Wall wall = component as Wall;
		wall.DamageWall (attackPower);
		animator.SetTrigger ("playerAttack");
	}
}
