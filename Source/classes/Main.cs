using com.jbrettob.core;
using com.jbrettob.data.enums;
using UnityEngine;
using com.jbrettob.debug;
using com.jbrettob.utils;

namespace com.jbrettob.pages
{
	[AddComponentMenu("")]
	public class Main:CorePage
	{
		public override void Awake()
		{
			// show debug logs and such
			DebugLoggerManager.USE_LOG = !Debug.isDebugBuild;
			DebugLoggerManager.SHOW_LOG = !Debug.isDebugBuild;

			base.Awake();
			DontDestroyOnLoad(gameObject);
		}
	
		public override void init()
		{
			Application.LoadLevel(WorldNames.HOME_WORLD);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Q))
			{
				ScreenshotUtils.takeScreenshot(Project.Name);
			}
			if (Input.GetKeyDown(KeyCode.M))
			{
				AudioListener.volume = (AudioListener.volume == 0) ? 1 : 0;
				if (AudioListener.volume == 0)
				{
					PlayerPrefs.SetInt("AudioMuted", 0);
				}
				else
				{
					PlayerPrefs.SetInt("AudioMuted", 1);
				}
				PlayerPrefs.Save();
			}
		}
	
		private void OnLevelWasLoaded(int index)
		{
			if (debug) logDebug("OnLevelWasLoaded() index: " + index + " name: " + Application.loadedLevelName);
			
			GameObject loadedLevel = GameObject.Find(Application.loadedLevelName);
			if (loadedLevel != null) 
			{
				CorePage loadedLevelComponent = loadedLevel.GetComponent<CorePage>();
				if (loadedLevelComponent != null)
				{
					if (debug) logDebug("OnLevelWasLoaded() loadedLevelComponent: " + loadedLevelComponent);
					loadedLevelComponent.init();
					loadedLevelComponent = null;
				}
				else
				{
					if (debug) logDebug("OnLevelWasLoaded() loadedLevelComponent: " + loadedLevelComponent);
				}
				loadedLevel = null;
			}
			else
			{
				if (debug) logDebug("OnLevelWasLoaded() loadedLevel: " + loadedLevel);
			}
		}
	}
}