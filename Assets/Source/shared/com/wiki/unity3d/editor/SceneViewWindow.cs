using com.jbrettob.editor;
using UnityEngine;
using UnityEditor;

public class SceneViewWindow:CoreEditorWindow
{
	/// <summary>
	/// Tracks scroll position.
	/// </summary>
	private Vector2 _scrollPos;
	
	/// <summary>
	/// Initialize window state.
	/// </summary>
	[MenuItem("Window/Scene View")]
	internal static void Init()
	{
		SceneViewWindow window = (SceneViewWindow)GetWindow(typeof(SceneViewWindow), false, "Scene View");
		window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 100f, 100f);
	}
	
	/// <summary>
	/// Called on GUI events.
	/// </summary>
	internal void OnGUI()
	{
		EditorGUILayout.BeginVertical();
		_scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, false, false);
		
		GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
		{
			EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
			if (scene.enabled)
			{
				string sceneName = System.IO.Path.GetFileNameWithoutExtension(scene.path);
				bool pressed = GUILayout.Button(i + ": " + sceneName, new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleLeft });
				if (pressed)
				{
					if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
					{
						EditorApplication.OpenScene(scene.path);
					}
				}
			}
		}
		
		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}
	
	void OnInspectorUpdate()
	{
		Repaint();
	}
}