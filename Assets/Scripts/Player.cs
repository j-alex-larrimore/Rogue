using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject {

	public Text healthText;

	public AudioClip movementSound1;
	public AudioClip movementSound2;
	public AudioClip chopSound1;
	public AudioClip chopSound2;
	public AudioClip fruitSound1;
	public AudioClip fruitSound2;
	public AudioClip sodaSound1;
	public AudioClip sodaSound2;

	private Animator animator;
	private int attackPower = 1;
	private int playerHealth; 
	private int healthPerFruit = 5;
	private int healthPerSoda = 10;
	private int secondsUntilNextLevel = 1;

	protected override void Start(){
		base.Start ();
		animator = GetComponent<Animator> ();
		playerHealth = GameController.Instance.playerCurrentHealth;
		healthText.text = "Health: " + playerHealth;
	}

	private void OnDisable(){
		GameController.Instance.playerCurrentHealth = playerHealth;
	}

	void Update () {
		CheckIfGameOver ();
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
			playerHealth--;
			healthText.text = "Health: " + playerHealth;
			SoundController.Instance.PlaySingle(movementSound1, movementSound2);
			Move<Wall> (xAxis, yAxis);
			GameController.Instance.isPlayerTurn = false;
		}

	}

	private void OnTriggerEnter2D(Collider2D objectPlayerCollidedWith){
		if (objectPlayerCollidedWith.tag == "Exit") {
			Invoke ("LoadNewLevel", secondsUntilNextLevel);
			enabled = false;	
		} else if (objectPlayerCollidedWith.tag == "Fruit") {
			playerHealth += healthPerFruit;
			healthText.text = "+" + healthPerFruit + " Health\n" + "Health: " + playerHealth;
			objectPlayerCollidedWith.gameObject.SetActive (false);
			SoundController.Instance.PlaySingle(fruitSound1, fruitSound2);
		} else if (objectPlayerCollidedWith.tag == "Soda") {
			playerHealth += healthPerSoda;
			healthText.text = "+" + healthPerSoda + " Health\n" + "Health: " + playerHealth;
			objectPlayerCollidedWith.gameObject.SetActive (false);
			SoundController.Instance.PlaySingle(sodaSound1, sodaSound2);
		}
	}

	private void LoadNewLevel(){
		Application.LoadLevel (Application.loadedLevel);
	}

	protected override void HandleCollision<T>(T component){
		Wall wall = component as Wall;
		wall.DamageWall (attackPower);
		SoundController.Instance.PlaySingle (chopSound1, chopSound2);
		animator.SetTrigger ("playerAttack");
	}

	public void TakeDamage(int damageReceived){
		playerHealth -= damageReceived;
		healthText.text = "-" + damageReceived + " Health\n" + "Health: " + playerHealth;
		animator.SetTrigger ("playerHurt");
	}

	private void CheckIfGameOver(){
		if (playerHealth <= 0) {
			GameController.Instance.GameOver();
		}
	}
}
