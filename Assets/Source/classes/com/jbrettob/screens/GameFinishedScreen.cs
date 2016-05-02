using UnityEngine;
using System.Collections;
using com.jbrettob.data;

public class GameFinishedScreen:CoreScreen
{
	public override void onActivated()
	{

	}

	public void onRestartClicked()
	{
		logDebug("onRestartClicked()");
		DataManager.GetInstance().StateManager.Init(2);
		Navigate("SelectTeam");
	}
}