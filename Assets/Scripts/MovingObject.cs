using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rigidBody;
	private LayerMask collisionLayer;
	private float inverseMoveTime;
	private Vector2 endPosition;
	private Vector2 startPosition;

	protected virtual void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		rigidBody = GetComponent<Rigidbody2D> ();
		collisionLayer = LayerMask.GetMask ("Collision Layer");
		inverseMoveTime = 1.0f / moveTime;
	}

	protected virtual void Move<T>(int xDirection, int yDirection){
		RaycastHit2D hit;
		bool canMove = CanObjectMove (xDirection, yDirection, out hit);

		if (canMove) {
			return;
		}

		T hitComponent = hit.transform.GetComponent<T>();

		if (hitComponent != null) {
			HandleCollision(hitComponent);
		}
	}

	protected bool CanObjectMove(int xDirection, int yDirection, out RaycastHit2D hit){
		startPosition = rigidBody.position;
		endPosition = startPosition + new Vector2 (xDirection, yDirection);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (startPosition, endPosition, collisionLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			StartCoroutine (SmoothMovementRoutine (endPosition));
			return true;
		}

		return false;
	}

	protected Vector2 getPosition(){
		return rigidBody.position;
	}

	protected IEnumerator SmoothMovementRoutine(Vector2 endPosition){
		float remainingDistanceToEndPosition;

		do{
			remainingDistanceToEndPosition = (rigidBody.position - endPosition).sqrMagnitude;
			Vector2 updatedPosition = Vector2.MoveTowards(rigidBody.position, endPosition, inverseMoveTime * Time.deltaTime);
			rigidBody.MovePosition(updatedPosition);
			yield return null;
		}while(remainingDistanceToEndPosition > float.Epsilon);
	}

	protected abstract void HandleCollision<T> (T component);

}
