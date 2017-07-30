using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningText : MonoBehaviour {

	// Use this for initialization
	public void Initalise () {
		duration = 1f;
		fadeDuration = 1f;
		Color color = this.GetComponent<Text> ().color;
		color.a = fadeDuration;
		this.GetComponent<Text> ().color = color;

	}

	public float duration = 1f;
	public float fadeDuration = 1f;
	// Update is called once per frame
	void Update () {
		if (duration > 0f) {
			duration -= Time.deltaTime;
		} else {
			if (fadeDuration > 0f) {
				Color color = this.GetComponent<Text> ().color;
				color.a = fadeDuration;
				this.GetComponent<Text> ().color = color;
				fadeDuration -= Time.deltaTime;
			} else {
				duration = 1f;
				fadeDuration = 1f;
				this.gameObject.SetActive (false);
			}
		}
	}
}
