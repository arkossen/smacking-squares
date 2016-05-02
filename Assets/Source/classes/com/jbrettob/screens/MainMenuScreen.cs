using UnityEngine;
using System.Collections;
using com.jbrettob.data.enums;
using com.jbrettob.debug;
using com.jbrettob.data;

public class MainMenuScreen:CoreScreen
{
		public override void Awake ()
		{
				base.Awake ();
				DebugLoggerManager.SHOW_LOG = true;
				DebugLoggerManager.USE_LOG = true;
		}

		public void onSelectedLocalMultiplayer ()
		{
				logDebug("onSelectedLocalMultiplayer()");
				DataManager.GetInstance().StateManager.Init(2);
				Project.IsOnlineMultiplayer = false;
				Navigate ("SelectTeam");
		}

		public void onSelectedOnlineMultiplayer ()
		{
				logDebug("onSelectedOnlineMultiplayer()");
				DataManager.GetInstance().StateManager.Init(2);
				Project.IsOnlineMultiplayer = true;
				Navigate("LobbyScreen");
		}
}