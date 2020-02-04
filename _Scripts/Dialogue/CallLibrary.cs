using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CallLibrary : MonoBehaviour {

	public static string thisParameter = "";

	private static bool dictionaryLoaded = false;

	public static Dictionary<string, System.Action> globalCalls = new Dictionary<string, System.Action> ();
	// Use this for initialization
	void Awake () {
		if (!dictionaryLoaded) {
			globalCalls.Add ("testOut", testOut);
			globalCalls.Add ("testParm", testParm);
			globalCalls.Add ("testNumeric", testNumeric);
			globalCalls.Add ("addGold", addGold);
			dictionaryLoaded = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void testOut(){
		Debug.Log ("Test Out");
	}

	public static void testParm(){
		Debug.Log (thisParameter);
	}

	public static void testNumeric(){
		int val = int.Parse (thisParameter);
		Debug.Log(10 * val);
	}

	public static void addGold(){
		int val = int.Parse (thisParameter);
		GameManager.money += val;
	}
}
