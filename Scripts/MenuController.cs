using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void Play ()
	{
		SceneManager.UnloadSceneAsync (0);
		SceneManager.LoadScene (1);
	}
	public void Return ()
	{
		SceneManager.UnloadSceneAsync (1);
		SceneManager.LoadScene (0);
	}
}
