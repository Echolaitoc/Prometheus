﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrometheusRenamer : EditorWindow
{
	private string renameTarget;
	private string replaceSearch;
	private string replaceTarget;
	private bool renameNumbered = true;

	[MenuItem("Window/Prometheus Rename Tool")]
	private static void Init()
	{
		PrometheusRenamer window = (PrometheusRenamer)EditorWindow.GetWindow(typeof(PrometheusRenamer));
		window.Show();
	}

	private void OnGUI()
	{
		GUILayout.BeginHorizontal ();
		EditorGUILayout.BeginVertical ();
		GUILayout.Label ("");
		if (GUILayout.Button ("Rename"))
		{
			Rename ();
		}
		EditorGUILayout.Separator ();
		GUILayout.Label ("");
		if (GUILayout.Button ("Replace"))
		{
			Replace ();
		}
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		GUILayout.Label ("New Name");
		renameTarget = EditorGUILayout.TextField (renameTarget);
		EditorGUILayout.Separator ();
		GUILayout.Label ("Search");
		replaceSearch = EditorGUILayout.TextField (replaceSearch);
		EditorGUILayout.EndVertical ();

		EditorGUILayout.BeginVertical ();
		GUILayout.Label ("Numbered");
		renameNumbered = EditorGUILayout.Toggle (renameNumbered);
		EditorGUILayout.Separator ();
		GUILayout.Label ("Target");
		replaceTarget = EditorGUILayout.TextField (replaceTarget);
		EditorGUILayout.EndVertical ();
	}

	private void Rename()
	{
		var gameObjects = Selection.gameObjects;
		System.Array.Sort (gameObjects, new UnityTransformSort ());
		int counter = 0;
		Undo.RecordObjects (gameObjects, "Renaming to " + renameTarget);
		foreach (var gameObject in gameObjects)
		{
			gameObject.name = renameTarget + (renameNumbered ? ("_" + counter.ToString()) : "");
			counter++;
		}
	}

	private void Replace()
	{
		if (replaceSearch == null || replaceSearch.Length <= 0)
		{
			return;
		}
		var gameObjects = Selection.gameObjects;
		Undo.RecordObjects (gameObjects, "Replacing " + replaceSearch + "->" + replaceTarget);
		foreach (var gameObject in gameObjects)
		{
			gameObject.name = gameObject.name.Replace (replaceSearch, replaceTarget);
		}
	}

	public class UnityTransformSort : System.Collections.Generic.IComparer<GameObject>
	{
		public int Compare(GameObject lhs, GameObject rhs)
		{
			if (lhs == rhs) return 0;
			if (lhs == null) return -1;
			if (rhs == null) return 1;
			return (lhs.transform.GetSiblingIndex() > rhs.transform.GetSiblingIndex()) ? 1 : -1;
		}
	}
}
