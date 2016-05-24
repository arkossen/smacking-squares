using UnityEditor;
using UnityEngine;

namespace com.jbrettob.editor
{
	public class CoreEditorWindow:EditorWindow
	{
		public bool debug;
		
		public void logDebug(string message)
		{
			Debug.Log("[" + this + "] " + message);
		}
		
		public void logWarning(string message)
		{
			Debug.LogWarning("[" + this + "] " + message);
		}
		
		public void logError(string message)
		{
			Debug.LogError("[" + this + "] " + message);
		}
	}
}

