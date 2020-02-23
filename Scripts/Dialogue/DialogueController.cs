using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Linq;

//This is the meat of the dialogue. I put all the messy parts over here.
public class DialogueController : MonoBehaviour {

	//We get Game Objects
	public GameObject textPanel;
	public Text textItem;
	public GameObject optionPanel;
	public GameObject btnPrefab;

	//We put all the lines of dialogue into an array and initialize the counter.
	private string[] lines;
	private int dialogueCounter = 0;
	private string thisLine;
	public string label = "";

	//This is used to stop the label from resetting the dialogue counter on every iteration of the dialogue button (We get stuck in a loop)
	private bool labelMet = false;

	//Button controls
	private bool isButton = false;
	private bool isWaiting = false;

	private int dialogueType = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//This moves the dialogue script into an array.
	public void setDialogue(string filename){
		lines = File.ReadAllLines ("Assets/_scripts/Dialogue2/Scripts/test/"+filename+".txt");
	}

	public void setType(int type){
		dialogueType = type;
	}

	//This checks for speacial considerations (labels) then advances the dialogue.
	public void advanceDialogue(){

		GameManager.canMove = false;

		if (!isWaiting) {

			switch (dialogueType) {
			case 1:
				//Random
				int max = getMaxLabels ("rdm");
				int rdmInt = Random.Range (1, max);
				label = "rdm" + rdmInt;
				break;
			case 2:
				//Repeat
				break;
			case 3:
				//Iterated
				if (label == "") {
					label = "dia1";
				}
				break;
			default:
				break;
			}
			if (label.Length > 0 && !labelMet) {
				dialogueCounter = goToLabel (label);
			}
			//We load the next line, advnace the counter (This oder is important!!) Show the panel, clean the text for any special inline conditions, and display the text.
			if (dialogueCounter < lines.Length) {
				thisLine = lines [dialogueCounter];
				dialogueCounter++;
				textPanel.SetActive (true);
				cleanLine ();
				//If this line is a button, we don't want to display it.
				if (!isButton) {
					textItem.text = thisLine;
				}
				//Else, there are no more lines, we just end the dialogue.
			} else {
				EndDialogue ();
			}

			if (dialogueType == 3) {
				int max = getMaxLabels ("dia");
				int i = int.Parse (label.Substring (3, 4));
				if (i > max) {
					i = max - 1;
				}
				label = "dia" + (i + 1);
			}
		}
	}

	//Every time the next line is loaded, this function looks for special considerations and "cleans" the line before displaying it.
	void cleanLine(){
		if ((isButton && thisLine.Length < 1 ) || (isButton && thisLine.Substring (0, 1) != "{"))  {
			isWaiting = true;
			return;
		}
		//Comments and blank lines are skipped.
		if (thisLine.Length < 1 || thisLine.Substring(0,1) == "#") {
			advanceDialogue ();
			return;
		}
		//if the next line is a label marker, starts with - we end the dialogue.
		if (thisLine.Substring (0, 1) == "-") {
			//If this is a button line, we don't want to end dialogue when we reach label, we want to wait for user input.
			if (!isButton) {
				EndDialogue ();
				return;
			} 
		}
		//I put variables in square brackets [] I like having a different ending character. This grabs the variable and looks it up in a dictionary on Game Manager.
		//Unfortunately you can't use [] in your dialogue. You can totally change this if you want.
		if (thisLine.IndexOf ("[") >= 0) {
			int tmpStart = thisLine.IndexOf ("[") + 1;
			int tmpEnd = thisLine.IndexOf ("]") - tmpStart;
			string output = thisLine.Substring (tmpStart, tmpEnd);
			thisLine = thisLine.Replace ("[" + output + "]", GameManager.dialogueVariables[output]);
		}
		//If the first characters on the line are GOTO, change the label and go there next.
		if (thisLine.Substring(0,4) == "GOTO") {
			execGoTo (thisLine);
			return;
		}

		//If the first characters on the line are CALL, get call function and parameters.
		if (thisLine.Substring(0,4) == "CALL") {
			//I set this as another function so it can be reused on buttons.
			execCall (thisLine);
			return;
		}
		//If the first character on the line is { we create a button.
		if (thisLine.Substring(0,1) == "{") {
			createButton ();
		}
	}

	//This resets variables and closes the text panel GUI
	public void EndDialogue(){
		dialogueCounter = 0;
		labelMet = false;
		label = "";
		textPanel.SetActive (false);
		GameManager.canMove = true;
	}


	//Changes the dialogue counter to find the label.
	public int goToLabel(string label){
		int counter = 0;
		for (int i = 0; i < lines.Length; i++) {
			if (lines [i] == "-" + label) {
				counter = i + 1;
				labelMet = true;
			}
		}
		return counter;
	}

	//Changes label to whatever comes after the word GOTO
	//NOTE: execGoTo and execCall take a string paramter so that buttons work.
	void execGoTo(string newLine){
		//destroys all buttons before moving on.
		foreach (Transform child in optionPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		isButton = false;
		//This allows multiple labels per convo
		labelMet = false;
		optionPanel.SetActive (false);
		isWaiting = false;
		int tmpStart = newLine.IndexOf ("GOTO") + 5;
		string output = newLine.Substring (tmpStart, (newLine.Length-tmpStart));
		label = output;
		advanceDialogue ();
	}

	void execCall(string newLine){
		//destroys all buttons before moving on.
		foreach (Transform child in optionPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		isButton = false;
		optionPanel.SetActive (false);
		isWaiting = false;
		//This gets a little tricky because there could be a paramter but there might not be.

		//So first, let's declare our variables.
		string output = "";

		//Isolate call and parameters
		string sliceNDice = newLine.Substring (newLine.IndexOf ("CALL") + 5);

		//If there is a space, there is a parameter
		if (sliceNDice.IndexOf (" ") > 0) {
			//Get everything from beginning to first space, the method name.
			output = sliceNDice.Substring (0, sliceNDice.IndexOf (" "));
			//Everything else is set as the parameter
			CallLibrary.thisParameter = sliceNDice.Substring (sliceNDice.IndexOf (" ") + 1);
			//If there is no parameter me just call whatever is there.
		} else {
			output = sliceNDice;
		}

		//****Now here's the tricky part. How do we call the function and add a parm that could be ANY data type???
		CallLibrary.globalCalls [output]();
		advanceDialogue ();
	}

	//On create a button we need to do a lot of things, let's steo through
	void createButton(){
		//Specify this is a button. This is important to wait for user input later.
		isButton = true;
		//Gets the label, the text that appears on the button
		string btnLabel = thisLine.Substring (1, thisLine.IndexOf ("}") - 1);
		string btnAction = "";
		//Show option panel
		optionPanel.SetActive(true);
		//Add Button Prefabs
		var tmpLine = thisLine;
		var newOptionBtn = Instantiate(btnPrefab, optionPanel.transform) as GameObject;
		Button b = newOptionBtn.GetComponent<Button> ();
		GameObject newName = newOptionBtn.transform.Find ("Text").gameObject;
		newName.GetComponent<Text> ().text = btnLabel;
		newOptionBtn.SetActive (true);
		//Buttons will either change label or call a function. This figures out which.
		if (thisLine.IndexOf ("GOTO") >= 0) {
			b.onClick.AddListener (() => execGoTo(tmpLine));;
		}
		if (thisLine.IndexOf ("CALL") >= 0) {
			b.onClick.AddListener (() => execCall(tmpLine));
		}

		//Focus first button
		//Advance Dialogue
		advanceDialogue ();

	}

	public int getMaxLabels(string label){
		int counter = 0;
		for (var i = 0; i < lines.Length; i++) {
			if (lines [i].Length > 4 && lines [i].Substring(0,4) == "-" + label) {
				counter++;
			}
		}
		return counter;
	}
}
