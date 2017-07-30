using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	// Use this for initialization
	public void initalise (Vector3 target) {
		StopCoroutine("moveToPos");
		StartCoroutine("moveToPos", target);
	}

	IEnumerator moveToPos (Vector3 target)
	{
		while(Vector3.Distance(target, this.transform.position) > 2f)
		{
			this.transform.position = Vector3.Lerp (this.transform.position, target, 
				(Time.deltaTime * MovementRules.moveSpeed * 10) / Vector3.Distance(target, this.transform.position));
			yield return null;
		}
		Destroy (this.gameObject);
	}
}
