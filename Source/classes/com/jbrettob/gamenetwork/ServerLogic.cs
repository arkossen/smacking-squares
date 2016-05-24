using System.Collections.Generic;
using com.jbrettob.core;
using UnityEngine;

namespace com.jbrettob.gamenetwork {
	public class ServerLogic:CoreObject {
		private UnityNetworkManager _unityNetworkManager;
		private Grid _grid;
		private List<Player> _players = new List<Player>();
		private ClientLogic _clientLogic;

		#region public methods
		public void GameStarted(ClientLogic clientLogic) {
			if (debug) logDebug("GameStarted()");
			_clientLogic = clientLogic;
			_grid = Object.FindObjectOfType(typeof(Grid)) as Grid;
        
			_unityNetworkManager = UnityNetworkManager.getInstance();
			_unityNetworkManager.SetOnGetMessage(OnGetMessage);
		
			if (Network.isServer) {
				_unityNetworkManager.Send(ServerEventNames.PLAYER_JOINED, 0);
			}
		}
	
		public void OnGetMessage(UnityRPCData data) {
			switch (data.eventName) {
				case ServerEventNames.PLAYER_JOINED:
						playerJoined(data);
						break;
				case ServerEventNames.PLAYER_LEFT:
						playerLeft(data);
						break;
				case ServerEventNames.GENERATE_LEVEL:
						generateLevel(data);
						break;
				case ServerEventNames.PLAYER_CHOOSE_TEAM:
						playerChooseTeam(data);
						break;
				case ServerEventNames.PLAYER_TURN:
						playerSetTurn(data);
						break;
				case ServerEventNames.PLAYER_SKIP:
						playerSkipTurn(data);
						break;
				default:
						logDebug("onGetMessage() " + data.eventName);
						break;
			}
		
			_clientLogic.OnGetMessage(data);
		}

		public void onPlayerConnected(NetworkPlayer networkPlayer) {
			if (debug) logDebug("onPlayerConnected() networkPlayer: " + networkPlayer);
			_unityNetworkManager.Send(ServerEventNames.PLAYER_JOINED, networkPlayer.ToString());
		
			if (Network.isServer) {
				if (debug) logDebug("_unityNetworkManager.getConnectedPlayers().Length: " + _unityNetworkManager.getConnectedPlayers().Length);
				if (debug) logDebug("Network.maxConnections: " + Network.maxConnections);

				//-1 because the server is hosted by 1 player.
				if (_unityNetworkManager.getConnectedPlayers().Length == Network.maxConnections) {
					if (debug) logDebug("Maximum amount of players reached");
					_unityNetworkManager.Send(ServerEventNames.GENERATE_LEVEL, _grid.SetPlayerTilesOnGameStartUp());
				}
			}
		}
	
		public void onPlayerDisconnected(NetworkPlayer networkPlayer) {
			if (debug) logDebug("onPlayerDisconnected() networkPlayer: " + networkPlayer);
			_unityNetworkManager.Send(ServerEventNames.PLAYER_LEFT, networkPlayer.ToString());
		}
	
		public void onRoomLeft(NetworkDisconnection message) {
			if (debug) logDebug("onRoomLeft() info: " + message);
			for (int i = 0; i < _players.Count; i++) {
				if (_players[i].transform != null) {
					GameObject.Destroy(_players [i].transform.gameObject);
				}
			}
			_players.Clear();
		}
		#endregion

		#region private methods
		private void playerJoined(UnityRPCData data) {
			if (Network.isServer) {
				if (debug) logDebug("playerJoined() " + data.Length);
			}
		}
	
		private void playerLeft(UnityRPCData data) {
			if (Network.isServer) {
				if (debug) logDebug("playerLeft()");
			}
		}
	
		private void generateLevel(UnityRPCData data) {
			if (Network.isServer) {
				if (debug) logDebug("generateLevel() create level for all players!");
				_unityNetworkManager.Send(data.eventName, true, data.getString(0));
			}
		}
    
		private void playerChooseTeam(UnityRPCData data) {
			if (Network.isServer) {
				if (debug) logDebug("playerChooseTeam() sending selected team to all players!");
				_unityNetworkManager.Send(data.eventName, true, data.getInt(0));
			}
		}

		private void playerSetTurn(UnityRPCData data) {
			if (Network.isServer) {
				if (debug) logDebug("playerSetTurn() send turn to all players!");
				_unityNetworkManager.Send(data.eventName, true, data.getInt(0), data.getFloat(1), data.getFloat(2), data.getFloat(3), data.getFloat(4));
			}
		}
    
		private void playerSkipTurn(UnityRPCData data) {
			if (Network.isServer) {
				if (debug) logDebug("playerSkipTurn() send skip turn to all players!");
				_unityNetworkManager.Send(data.eventName, true);
			}
		}
    
		private Player getPlayerById(int id) {
			Player player = null;
		
			for (int i = 0; i < _players.Count; i++) {
				if (_players [i].id == id) {
					player = _players[i];
				}
			}

			return player;
		}
		#endregion
	}
}