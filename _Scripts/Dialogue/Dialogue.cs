using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {

	public string filename = "oneLine";
	public GameObject controller;

	private bool listenKey = false;
	private bool inDialogue = false;

	public bool repeatLast = false;
	public bool randomDialogue = false;
	public bool iteratedDialogue = false;

	void Start(){
	}

	//When this function is triggered....
	void Update(){
		if (listenKey && Input.GetKeyDown ("space")) {
			ActionBtnClicked ();
		}
	}

	//When the player enter's the object's field, the action button clears out previous actions and sets up action button
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			listenKey = true;
		}
	}

	//When Player leaves obect's field, action panel disappears
	void OnTriggerExit2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			listenKey = false;
		}
	}



	//We hide the action panel, send the script file to the manager, then start the dialogue.
	void ActionBtnClicked(){
		if (!inDialogue) {
			inDialogue = true;
			controller.GetComponent<DialogueController> ().setDialogue (filename);
			if (randomDialogue) {
				controller.GetComponent<DialogueController> ().setType (1);
			}
			if (repeatLast) {
				controller.GetComponent<DialogueController> ().setType (2);
			}
			if (iteratedDialogue) {
				controller.GetComponent<DialogueController> ().setType (3);
			}
			//this.transform.parent.GetComponent<NPC_Movement> ().thisCanMove = false;
		}
		controller.GetComponent<DialogueController> ().advanceDialogue ();
	}


}
