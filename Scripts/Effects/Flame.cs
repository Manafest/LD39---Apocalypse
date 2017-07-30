using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {

	public float life = 2.0f;
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
			float lerp = Mathf.Lerp (1, 0, Time.deltaTime * -life * 100);
			this.transform.localScale = new Vector3 (lerp, lerp, 1);
			if (lerp < 0.1) {
				Destroy (this.gameObject);
			}
		}
	}
}
