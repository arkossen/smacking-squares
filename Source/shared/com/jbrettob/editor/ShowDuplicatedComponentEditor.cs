using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ShowDuplicatedComponentEditor : EditorWindow {
	#region Fields
	private Vector2 _scrollPos;
	private GameObject[] _gameObjects;
	private List<List<MonoBehaviour>> _monoBehaviours = new List<List<MonoBehaviour>>();
	#endregion

	[MenuItem ("Window/Utils/Show Duplicated Component(s)")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		ShowDuplicatedComponentEditor _WINDOW = (ShowDuplicatedComponentEditor)EditorWindow.GetWindow(typeof(ShowDuplicatedComponentEditor));
		_WINDOW.title = "Show Duplicated Component(s)";
	}

	#region Unity methods
	void OnGUI() {
		if (GUILayout.Button((_gameObjects == null || _gameObjects.Length == 0) ? "Load GameObjects" : "Reload GameObjects")) {
			//_gameObjects = Object.FindObjectsOfType<GameObject>();
			_gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

			List<MonoBehaviour> list = null;

			_monoBehaviours.Clear();

			for (int i = 0; i < _gameObjects.Length; i++) {
				if (_gameObjects[i].transform.parent == null)
				{
					list = GameObjectHasDuplicatedComponent(_gameObjects[i]);
					if (list != null) {
						_monoBehaviours.Add(list);
					}
				}
			}
		}

		_scrollPos = GUILayout.BeginScrollView(_scrollPos, false, true);
		DrawList();
		GUILayout.EndScrollView();
	}
	#endregion

	#region Private methods
	private void DrawList() {
		// check before we continue
		if (_gameObjects == null || _gameObjects.Length <= 0) return;

		for (int i = 0; i < _monoBehaviours.Count; i++) {
			if (_monoBehaviours[i].Count > 0)
			{
				GUILayout.Label(_monoBehaviours[i][0].gameObject.name);
				for (int j = 0; j < _monoBehaviours[i].Count; j++) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.ObjectField("\t[" + j + "] " + _monoBehaviours[i][j].GetType().ToString(), _monoBehaviours[i][j], typeof(MonoBehaviour), true);
					EditorGUILayout.EndHorizontal();
				}
			}
		}
	}

	private List<MonoBehaviour> GameObjectHasDuplicatedComponent(GameObject go) {
		MonoBehaviour[] monoBehaviours = go.GetComponents<MonoBehaviour>();

		List<MonoBehaviour> result = null;

		if (monoBehaviours.Length > 0) {
			result = new List<MonoBehaviour>();
			for (int i = 0; i < monoBehaviours.Length ; i++) {
				if (CompareArrayAtIndex(monoBehaviours, i).Equals(true))
				{
					result.Add(monoBehaviours[i]);
				}
			}
		}

		return result;
	}

	private bool CompareArrayAtIndex(MonoBehaviour[] list, int index) {

		for (int i = 0; i < list.Length; i++) {
			// skip our own index
			if (i == index) continue;

			if (list[i].GetType() == list[index].GetType()) {
				return true;
			}
		}

		return false;
	}
	#endregion
}