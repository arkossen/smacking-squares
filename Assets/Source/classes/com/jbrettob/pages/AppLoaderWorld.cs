using UnityEngine;

namespace com.jbrettob.pages
{
	[AddComponentMenu("")]
	/// <summary>
	/// This class, should be set in level 0.
	/// All resources get loaded in scene 0, and once that is complete, it will continue to scene 1.
	/// This reduces the application load at start!
	/// </summary>
	public class AppLoaderWorld:MonoBehaviour
	{
		void Start()
		{
			Application.LoadLevel(1);
		}
	}
}