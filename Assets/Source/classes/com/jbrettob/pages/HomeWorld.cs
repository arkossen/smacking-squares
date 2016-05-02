using com.jbrettob.core;
using com.jbrettob.data;
using com.jbrettob.data.enums;
using UnityEngine;

namespace com.jbrettob.data.pages
{
	[AddComponentMenu("")]
	public class HomeWorld:CorePage
	{
		public Texture splashScreen;
		private Rect _screenRect;

		public override void init()
		{
			_screenRect = new Rect(0,0, Screen.width, Screen.height);

			if (Project.IsLive.Equals(false))
			{
				endSplashScreen();
			}
			else
			{
				Invoke("endSplashScreen", 2);
			}
		}

		void OnGUI()
		{
			GUI.DrawTexture(_screenRect, splashScreen);
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				endSplashScreen();
			}
		}

		private void endSplashScreen()
		{
			CancelInvoke("endSplashScreen");
			Application.LoadLevel(WorldNames.GAME_WORLD);
		}
	}
}