using com.jbrettob.core;
using UnityEngine;

namespace com.jbrettob.data
{
	public class StreamingAssets:CoreObject
	{
		private static string _RESULT;
		
		/// <summary>
		/// Gets the file path.
		/// </summary>
		/// <returns>
		/// The file path.
		/// </returns>
		/// <param name='value'>
		/// Value.
		/// </param>
		public static string GetFilePath(string value)
		{
			_RESULT = null;
			
			if (
				Application.platform == RuntimePlatform.WindowsEditor ||
				Application.platform == RuntimePlatform.WindowsPlayer
				)
				
			{
				_RESULT = "file://" + value;
			}
			else if(
				Application.platform == RuntimePlatform.OSXEditor ||
				Application.platform == RuntimePlatform.OSXPlayer
				)
			{
				_RESULT = "/Users/" + value;
			}
			else if (
				Application.platform == RuntimePlatform.WindowsWebPlayer ||
				Application.platform == RuntimePlatform.OSXWebPlayer
			)
			{
				_RESULT = "StreamingAssets/" + value;
			}
			else
			{
				_RESULT = value;
			}
			
			return _RESULT;
		}
	}
}