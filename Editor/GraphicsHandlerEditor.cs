using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GraphicsHandler))]
public class GraphicsHandlerEditor : Editor {

	public override void OnInspectorGUI()
	{
		GraphicsHandler myScript = (GraphicsHandler)target;
		DrawDefaultInspector ();

		if (GUILayout.Button ("Initalise")) 
		{
			myScript.CreateLevel();
		}

	}
}
