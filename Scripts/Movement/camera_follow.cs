using UnityEngine;
using System.Collections;

public class camera_follow : MonoBehaviour {
	
	public Transform target;
	Camera mycam;

	// Use this for initialization
	void Start () {
		mycam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		mycam.orthographicSize = (Screen.height/100f)/2f;
		
		if(target){
			transform.position = Vector3.Lerp(transform.position, target.position, 0.1f) + new Vector3(0,0,-.5f);
		}
	}
}
