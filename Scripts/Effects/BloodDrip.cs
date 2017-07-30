using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BloodDrip : MonoBehaviour {
	
	public Sprite[] frames;
	float framesPerSecond = 10f;

	void Update() {
		int index = (int)((Time.time * framesPerSecond) % (float)(frames.Length + 25));
		if (index > frames.Length - 1) {
			return;
		}
		this.GetComponent<Image>().sprite = frames[index];
	}
}
