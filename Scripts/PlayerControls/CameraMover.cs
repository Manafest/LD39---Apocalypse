using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {

	public float moveSpeed;
	public float resetSpeed;
	public float zoomSpeed;

	private float minZoom = 10f;
	private float maxZoom = 60f;

	public void Horizontal (float input) 
	{
		StopCoroutine("reset");
		Vector3 move = new Vector3 (input * (moveSpeed) * Time.deltaTime, 0 , 0);
		this.transform.Translate (move);
	}

	public void Vertical (float input) 
	{
		StopCoroutine("reset");
		Vector3 move = new Vector3 (0, input * moveSpeed * Time.deltaTime, 0);
		this.transform.Translate (move);
	}

	public void ResetCameraTO (GameObject ob) 
	{
		StopCoroutine("reset");
		StartCoroutine("reset", ob);
	}

	IEnumerator reset (GameObject ob)
	{
		Vector3 position = ob.transform.position;
		position.z = -10;
		while(Vector3.Distance(position, this.transform.position) > 0.05f)
		{
			this.transform.position = Vector3.Lerp (this.transform.position, position, Time.deltaTime * resetSpeed);
			position = ob.transform.position;
			position.z = -10;
			yield return null;
		}
	}

	public void Zoom (float input) 
	{
		float zoom = Mathf.Lerp (Camera.main.orthographicSize, Camera.main.orthographicSize - (input * zoomSpeed), Time.deltaTime);
		if (zoom > minZoom && zoom < maxZoom) {
			Camera.main.orthographicSize = zoom;
		}
	}
}
