/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

using System;
using System.Collections;
using com.jbrettob.core;
using com.jbrettob.data;
using com.jbrettob.data.api;
using Svelto.Tasks;
using UnityEngine;

namespace com.jbrettob.core
{
	public class StartUp:CoreObject
	{
		public void Start(CorePage go, Action onStartupComplete)
		{
			if (debug) logDebug("Start()");
			
			SerialTasks startupSequence = new SerialTasks();
			startupSequence.Add(initApplication());
			startupSequence.Add(initAPI());
			startupSequence.Add(initApplication());
			startupSequence.Add(this.onStartupComplete(onStartupComplete));
			go.StartCoroutine(startupSequence.GetEnumerator());
		}
		
		private IEnumerator initApplication()
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = 30;
			Application.runInBackground = true;

			// Note: screen rotation, works only on mobile and tablet devices
			Screen.autorotateToPortrait = false;
			Screen.autorotateToPortraitUpsideDown = false;
			Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
			Screen.orientation = ScreenOrientation.AutoRotation;
			
			yield return true;
		}
	
		private IEnumerator initAPI()
		{
			if (debug) logDebug("initAPI's()");
			bool useDummy = true;
	
			if (useDummy)
			{
				DataManager.GetInstance().api = new DummyAPI();
			}
			else
			{
				DataManager.GetInstance().api = new API();
			}
			yield return true;
		}
		
		private IEnumerator onStartupComplete(Action onStartupComplete)
		{
			if (debug) logDebug("onStartupComplete() onStartupComplete: " + onStartupComplete.Method.Name);
			onStartupComplete();
			yield return true;
		}
	}
}