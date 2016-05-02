using UnityEditor;
using UnityEngine;

public class ReplaceSelectionWindow:ScriptableWizard
{
	static GameObject replacement = null;
	static bool keep = false;
	
	public GameObject ReplacementObject = null;
	public bool KeepOriginals = false;
	
	[MenuItem("GameObject/Replace Selection")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Replace Selection", typeof(ReplaceSelectionWindow), "Replace");
	}
	
	public ReplaceSelectionWindow()
	{
		ReplacementObject = replacement;
		KeepOriginals = keep;
	}
	
	void OnWizardUpdate()
	{
		replacement = ReplacementObject;
		keep = KeepOriginals;
	}
	
	void OnWizardCreate()
	{
		if (replacement == null)
			return;
		
		Undo.RegisterCreatedObjectUndo(this, "Replace Selection");
		
		Transform[] transforms = Selection.GetTransforms(
			SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		
		foreach (Transform t in transforms)
		{
			GameObject g;
			PrefabType pref = PrefabUtility.GetPrefabType(replacement);
			
			if (pref == PrefabType.Prefab || pref == PrefabType.ModelPrefab)
			{
				g = (GameObject)PrefabUtility.InstantiatePrefab(replacement);
			}
			else
			{
				g = (GameObject)Editor.Instantiate(replacement);
			}
			g.transform.parent = t.parent;
			g.name = replacement.name;
			g.transform.localPosition = t.localPosition;
			g.transform.localScale = t.localScale;
			g.transform.localRotation = t.localRotation;
		}
		
		if (!keep)
		{
			foreach (GameObject g in Selection.gameObjects)
			{
				GameObject.DestroyImmediate(g);
			}
		}
	}
}