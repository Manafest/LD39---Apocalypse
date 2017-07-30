using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManger : MonoBehaviour {

	public CameraMover cameraMover;
	public MouseManager mouseManager;
	public PlayerManager Player;
	
	// Update is called once per frame
	void Update () {
		/*if(Input.GetButton("Submit")) {
			cameraMover.ResetCameraTO (Player);
		}*/

		if (Input.GetButtonDown("Horizontal")) {
			Player.Horizontal (Input.GetAxis ("Horizontal"));
		}
		if (Input.GetButtonDown("Vertical")) {
			Player.Vertical (Input.GetAxis ("Vertical"));
		}

		cameraMover.Zoom (Input.GetAxis ("Mouse ScrollWheel"));

		if (Input.GetButton("Space")) {
			// Space is for next turn
		}
	}
}
