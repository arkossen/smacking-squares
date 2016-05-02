using System;
using System.IO;
using com.jbrettob.core;
using UnityEngine;

namespace com.jbrettob.utils
{
	public class ScreenshotUtils:CoreObject
	{
		private static string _SCREENSHOT_FOLDER = "/screenshots/";
		private static string _PATH = Application.dataPath + _SCREENSHOT_FOLDER;
		
		public static void setScreenshotFolder(string path)
		{
			_SCREENSHOT_FOLDER = path;
			Debug.Log("setScreenshotFolder() set path to: " + _SCREENSHOT_FOLDER);
		}
		
		public static void takeScreenshot(string filename)
		{
			// create directory if not exist, else skips this part automaticly
			Directory.CreateDirectory(_PATH);
			string screenShotName = filename + "_" + DateTime.Now.ToString("yyyy.MM.dd-HH.mm.ss.ffff") + ".png";
			Application.CaptureScreenshot(_PATH + screenShotName);
			
			Debug.Log("takeScreenshot() saved at: " + _PATH + screenShotName);
		}
	}
}