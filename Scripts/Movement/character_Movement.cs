using UnityEngine;
using System.Collections;

public class character_Movement : MonoBehaviour {
	
	Rigidbody2D rbody;
	//Animator anim;
	public int speed = 1;

	public float minX = 0.0f;
	public float maxX = 10.0f;

	void Start () {
		rbody = GetComponent<Rigidbody2D>();
		//anim = GetComponent<Animator>();

	}
	
	void Update () {

		if (GameManager.canMove) {
			Vector2 movement_vector = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		
			if (movement_vector != Vector2.zero) {
				//anim.SetBool ("isWalking", true);
				//anim.SetFloat ("Input_x", movement_vector.x);
				//anim.SetFloat ("Input_y", movement_vector.y);
			} else {
				//anim.SetBool ("isWalking", false);
			}

			//Mathf.Clamp (rbody.transform.position.x, minX, maxX);

		
			rbody.MovePosition (rbody.position + movement_vector * Time.deltaTime * speed);
		}
	}
}
