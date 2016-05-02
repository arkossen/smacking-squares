using System.Collections.Generic;
using UnityEngine;
using com.jbrettob.data;
using com.jbrettob.data.enums;
using com.jbrettob.gamenetwork;

public class LobbyScreen:CoreScreen {
	#region properties
	private ClientLogic _clientLogic;
	private ServerLogic _serverLogic;
	private int _tries;
	#endregion

	#region Public methods
	public override void onActivated() {
		// TODO: add unitynetworking at this side
		// Get all rooms, check if a room is available and try to connect to a random room
		// Try this for 5 times,
		// if 5th time doesnt work show that there are no opponents available and try to host your own room?
		// Else go back to MainMenu
		_tries = 0;

		UnityNetworkManager.getInstance().getAllRooms("RandomGameName", onGetRooms);
		_clientLogic = DataManager.GetInstance().ClientLogic = new ClientLogic();
		_serverLogic = DataManager.GetInstance().ServerLogic = new ServerLogic();

		ClientLogic.SetCallback(onClientLogicStateChange);
	}

	public override void onDeactivated ()
	{
		ClientLogic.SetCallback(null);
	}

	public void onClickBackButton() {
		UnityNetworkManager.getInstance().leaveRoom();
		Navigate("MainMenu");
	}
	#endregion

	#region Private methods
	private void onClientLogicStateChange(UnityRPCData data) {
		logDebug("onClientLogicStateChange() data: " + data.eventName);
	}

	private void onGetRooms(UnityRoom[] rooms) {
		_tries++;
		if(_tries > 5) {
			logWarning("onGetRooms() we have tried more then 5 times now! Do we want to do something else now?");
			UnityNetworkManager.getInstance().createRoom("Room " + Random.Range(1000, 9999), string.Empty, "RandomGameName", 1, onRoomJoined, onRoomJoinedError, _serverLogic.onPlayerConnected, _serverLogic.onPlayerDisconnected, _serverLogic.onRoomLeft);

			return;
		}

		if(rooms.Length > 0) {
			// try to connect with a random room
			rooms = filterRooms(rooms, true, true);
			UnityRoom room = rooms [Random.Range(0, (rooms.Length - 1))];
			if(room != null) {
				logDebug("onGetRooms() Connecting with a room");
				UnityNetworkManager.getInstance().joinRoom(room, onRoomJoined, onRoomJoinedError, _serverLogic.onRoomLeft);
			} else {
				logDebug("onGetRooms() We have no rooms! lets try again");
				UnityNetworkManager.getInstance().getAllRooms("RandomGameName", onGetRooms);
			}
		} else {
			logDebug("onGetRooms() We have no rooms! Lets try again");
			UnityNetworkManager.getInstance().getAllRooms("RandomGameName", onGetRooms);
		}
	}

	private UnityRoom[] filterRooms(UnityRoom[] rooms, bool password, bool full) {
		UnityRoom[] result = new UnityRoom[rooms.Length];
		
		for(int i = 0; i < rooms.Length; i++) {
			if(password && rooms [i].passwordProtected)
				continue;
			if(full && (rooms [i].connectedPlayers == rooms [i].playerLimit))
				continue;
			result [i] = rooms [i];
		}

		return result;
	}

	private void onRoomJoined() {
		logDebug("onRoomJoined()");

		_clientLogic.GameStarted();
		if (Network.isServer) {
			_serverLogic.GameStarted(_clientLogic);
		}
	}

	private void onRoomJoinedError(NetworkConnectionError error) {
		logError("onRoomJoinedError() error: " + error);
		onClickBackButton();
	}
	#endregion
}