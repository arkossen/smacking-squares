using System.Collections;
using UnityEngine;

namespace com.jbrettob.utils
{
	public static class GameObjectExtentions
	{
		public static T GetSafeComponent<T>(this GameObject obj) where T : Component
		{
			T component = obj.GetComponent<T>();
			
			if(component == null)
			{
				component = obj.AddComponent<T>();
			}
			
			return component;
		}
	}
}

