using UnityEngine;
using System.Collections;

public class Player : MovingObject {

	void Update () {
		int xAxis = 0;
		int yAxis = 0;

		xAxis = (int)Input.GetAxisRaw ("Horizontal");
		yAxis = (int)Input.GetAxisRaw ("Vertical");

		CanObjectMove (xAxis, yAxis);
	}
}
