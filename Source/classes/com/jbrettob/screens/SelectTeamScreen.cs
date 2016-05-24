using UnityEngine;
using com.jbrettob.core;
using com.jbrettob.data;
using com.jbrettob.gamenetwork;
using com.jbrettob.data.enums;

public class SelectTeamScreen:CoreScreen
{
	public UIButton btnRedTeam;
	public UIButton btnGreenTeam;
	public UIButton btnBlueTeam;

	[SerializeField]
	private int _team = -1;

	public override void onActivated()
	{
		ClientLogic.SetCallback(onStateChange);
		reset();
	}

	private void onStateChange(UnityRPCData data)
	{
		logDebug("onStateChange() data: " + data.eventName);
		if (data.eventName != ServerEventNames.PLAYER_CHOOSE_TEAM) return;

		PlayerData[] players = DataManager.GetInstance().StateManager.GetAllPlayers();
		for (int i = 0; i < players.Length; i++)
		{
			if (players[i].team.Equals((int)PlayerNames.TEAM_COLOR.RED))
			{
				btnRedTeam.gameObject.SetActive(false);
			}
			else if (players[i].team.Equals((int)PlayerNames.TEAM_COLOR.GREEN))
			{
				btnGreenTeam.gameObject.SetActive(false);
			}
			else if (players[i].team.Equals((int)PlayerNames.TEAM_COLOR.BLUE))
			{
				btnBlueTeam.gameObject.SetActive(false);
			}
		}

		next();
	}

	public void onSelectedTeam()
	{
		logDebug("onSelectedTeam()");

		if (UICamera.lastHit.collider.gameObject.name == btnRedTeam.name) {
			btnRedTeam.gameObject.SetActive(false);
			logDebug("selected Red");
			if (Project.IsOnlineMultiplayer)
			{
				UnityNetworkManager.getInstance().Send(ServerEventNames.PLAYER_CHOOSE_TEAM, (int)PlayerNames.TEAM_COLOR.RED);
			}
			else
			{
				DataManager.GetInstance().StateManager.SetPlayerTeam((int)PlayerNames.TEAM_COLOR.RED);
				next();
			}
		}
		else if (UICamera.lastHit.collider.gameObject.name == btnGreenTeam.name) {
			logDebug("selected Green");
			btnGreenTeam.gameObject.SetActive(false);
			if (Project.IsOnlineMultiplayer)
			{
				UnityNetworkManager.getInstance().Send(ServerEventNames.PLAYER_CHOOSE_TEAM, (int)PlayerNames.TEAM_COLOR.GREEN);
			}
			else
			{
				DataManager.GetInstance().StateManager.SetPlayerTeam((int)PlayerNames.TEAM_COLOR.GREEN);
				next();
			}
		}
		else if (UICamera.lastHit.collider.gameObject.name == btnBlueTeam.name) {
			logDebug("selected Blue");
			btnBlueTeam.gameObject.SetActive(false);
			if (Project.IsOnlineMultiplayer)
			{
				UnityNetworkManager.getInstance().Send(ServerEventNames.PLAYER_CHOOSE_TEAM, (int)PlayerNames.TEAM_COLOR.BLUE);
			}
			else
			{
				DataManager.GetInstance().StateManager.SetPlayerTeam((int)PlayerNames.TEAM_COLOR.BLUE);
				next();
			}
		}
	}

	private void reset()
	{
		_team = -1;

		btnRedTeam.gameObject.SetActive(true);
		btnGreenTeam.gameObject.SetActive(true);
		btnBlueTeam.gameObject.SetActive(true);

		next();
	}

	private void next()
	{
		btnRedTeam.gameObject.GetComponent<TweenScale>().ResetToBeginning();
		btnGreenTeam.gameObject.GetComponent<TweenScale>().ResetToBeginning();
		btnBlueTeam.gameObject.GetComponent<TweenScale>().ResetToBeginning();

		if (_team > 0)
		{
			// we got all teams lets continue to the game
			Navigate("Game");
		}
		else
		{
			btnRedTeam.gameObject.GetComponent<TweenScale>().PlayForward();
			btnGreenTeam.gameObject.GetComponent<TweenScale>().PlayForward();
			btnBlueTeam.gameObject.GetComponent<TweenScale>().PlayForward();

			// we need to rest the position of the three objects
			btnRedTeam.GetComponent<UIAnchor>().relativeOffset = new Vector2((_team < 0) ? -0.4f : 0.4f, 0.2f);
			btnGreenTeam.GetComponent<UIAnchor>().relativeOffset = new Vector2((_team < 0) ? -0.4f : 0.4f, 0);
			btnBlueTeam.GetComponent<UIAnchor>().relativeOffset = new Vector2((_team < 0) ? -0.4f : 0.4f, -0.2f);
			_team++;
		}
	}
}