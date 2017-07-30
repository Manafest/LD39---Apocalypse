using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	public float life = 1.0f;
	public float frequancy = 0.2f;

	// Update is called once per frame
	void Update () {
		life -= Time.deltaTime;
		frequancy -= Time.deltaTime;
		if (frequancy < 0f) {
			this.GetComponentInChildren<SpriteRenderer> ().flipX = !this.GetComponentInChildren<SpriteRenderer> ().flipX;
			frequancy = 0.2f;
		}
		if (life < 0f) {
			Destroy (this.gameObject);
		}
	}
}
