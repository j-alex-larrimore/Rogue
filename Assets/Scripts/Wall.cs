using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public int hitPoints = 2;

	void Start () {
	
	}

	public void DamageWall(int damageReceived){
		hitPoints -= damageReceived;
		if(hitPoints <= 0){
			gameObject.SetActive(false);
		}
	}
}
