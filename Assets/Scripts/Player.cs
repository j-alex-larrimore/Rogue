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

	private int minScreenWidth;
	private int maxScreenWidth;
	private int tileSize;

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
		if (!GameController.Instance.isPlayerTurn) {
			return;
		}
		
		CheckIfGameOver ();

		Vector2 touchPosition;
		Vector2 currentPosition = this.getPosition();

		int xAxis = 0;
		int yAxis = 0;
	
		//Debug.Log ("updating");
		if (Input.touchCount > 0) {
			touchPosition = Input.GetTouch (0).position;
			xAxis = (int)touchPosition.x;
			yAxis = (int)touchPosition.y;
			int screenWidth = Screen.currentResolution.width;
			int screenHeight = Screen.currentResolution.height;
			minScreenWidth = (screenWidth - screenHeight) / 2;
			maxScreenWidth = minScreenWidth + screenHeight;
			tileSize = screenHeight/10;

			if(xAxis >= minScreenWidth && xAxis <= maxScreenWidth){
				xAxis -= minScreenWidth; //Ignores the extra space from the left of the screen
				xAxis /= tileSize;
				yAxis /= tileSize;

				if(Mathf.Abs(xAxis - (int)currentPosition.x) > Mathf.Abs(yAxis - (int)currentPosition.y)){
					yAxis = 0;
					if(xAxis - (int)currentPosition.x > 0){
						xAxis = 1;
					}
					else if(xAxis - (int)currentPosition.x < 0){
						xAxis = -1;
					}else{
						xAxis = 0;
					}
				}else {
					xAxis = 0;
					if(yAxis - (int)currentPosition.y > 0){
						yAxis = 1;
					}
					else if(yAxis - (int)currentPosition.y < 0){
						yAxis = -1;
					}else{
						yAxis = 0;
					}
				}
			}else{
				xAxis = 0;
				yAxis = 0;
			}
		} else {
			xAxis = (int)Input.GetAxisRaw ("Horizontal");
			yAxis = (int)Input.GetAxisRaw ("Vertical");
			if (xAxis != 0) {
				yAxis = 0;
			}
		}

		if (xAxis != 0 || yAxis != 0) {
			playerHealth--;
			healthText.text = "Health: " + playerHealth;
			SoundController.Instance.PlaySingle(movementSound1, movementSound2);
			Move<Wall> ((int)xAxis, (int)yAxis);
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
