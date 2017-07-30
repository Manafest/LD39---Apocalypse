using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyText : MonoBehaviour {

	/// <summary>
	/// Initalise the specified text, life and type. type 0 is damage, type 1 is healing, 2 is spell.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="life">Life.</param>
	/// <param name="type">Type.</param>
	public void Initalise (string text, float life, int type) {
		Vector3 pos = this.transform.position;
		pos.z = -1;
		this.transform.position = pos;
		this.GetComponent<TextMesh> ().text = text;
		duration = life;
		waitTime = Random.Range (0f, 0.8f);
		angle = Random.Range (-0.6f, 0.6f);
		if (type == 1) {
			this.GetComponent<TextMesh> ().color = Color.green;
		}
		if (type == 2) {
			this.GetComponent<TextMesh> ().color = Color.blue;
		}
	}

	float angle;
	float waitTime;
	float smoothing = 15f;
	float duration = 2f;

	// Update is called once per frame
	void Update () {
		if (waitTime > 0) {
			waitTime -= Time.deltaTime;
			this.transform.Translate (angle * Time.deltaTime, Time.deltaTime, 0f);
			return;
		}
		duration -= Time.deltaTime;
		this.transform.Translate (angle * Time.deltaTime * smoothing, Time.deltaTime * smoothing, 0f);
		if (duration < 0f) {
			Destroy (this.gameObject);
		}
	}
}
