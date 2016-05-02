﻿using UnityEngine;
using UnityEditor;
using System.IO;

namespace com.jbrettob.utils
{
	public static class ScriptableObjectUtils
	{
		/// <summary>
		///	This makes it easy to create, name and place unique new ScriptableObject asset files.
		/// </summary>
		public static void CreateAsset<T>() where T:ScriptableObject
		{
			T asset = ScriptableObject.CreateInstance<T> ();
			
			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if (path == "")
			{
				path = "Assets";
			}
			else if (Path.GetExtension (path) != "")
			{
				path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
			}
			
			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/_" + typeof(T).ToString() + ".asset");
			
			AssetDatabase.CreateAsset (asset, assetPathAndName);
			
			AssetDatabase.SaveAssets ();
			EditorUtility.FocusProjectWindow ();
			Selection.activeObject = asset;
		}
	}
}