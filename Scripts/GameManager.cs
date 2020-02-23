using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static int money = 500;
	
	public static Dictionary<string, string> dialogueVariables = new Dictionary<string, string> ();
	
	public static bool canMove = true;
    // Start is called before the first frame update
    void Awake()
    {
        dialogueVariables["Name"] = "Kelly";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
