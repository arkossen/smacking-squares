using System.Collections.Generic;
using UnityEngine;
using com.jbrettob.core;
using com.jbrettob.data;

namespace com.jbrettob.gamenetwork {
	public class ClientLogic:CoreObject {
		#region Static properties
		private static System.Action<UnityRPCData> _CALLBACK;
		
		public static void SetCallback(System.Action<UnityRPCData> callback) {
			_CALLBACK = callback;
		}
		#endregion

		#region properties
		private UnityNetworkManager unityNetworkManager;
		private PlayerMoves playerMoves;
		private Grid grid;
		#endregion

		#region public methods
		public void GameStarted() {
			playerMoves = Object.FindObjectOfType(typeof(PlayerMoves)) as PlayerMoves;
			grid = Object.FindObjectOfType(typeof(Grid)) as Grid;

			unityNetworkManager = UnityNetworkManager.getInstance();
			unityNetworkManager.SetOnGetMessage(OnGetMessage);
		}
	
		public void OnGetMessage(UnityRPCData data) {
			switch(data.eventName) {
				case ServerEventNames.PLAYER_JOINED:
					PlayerJoined(data);
					break;
				case ServerEventNames.GENERATE_LEVEL:
					GenerateLevel(data);
					break;
				case ServerEventNames.PLAYER_CHOOSE_TEAM:
					PlayerChooseTeam(data);
					break;
				case ServerEventNames.PLAYER_TURN:
					PlayerSetTurn(data);
					break;
				case ServerEventNames.PLAYER_SKIP:
					PlayerSkipTurn(data);
					break;
				default:
					logDebug("OnGetMessage() " + data.eventName + ": " + data);
					break;
			}
		}
		#endregion

		#region private methods
		private void PlayerJoined(UnityRPCData data) {
			if (debug) logDebug("PlayerJoined()");
		}
    
		private void GenerateLevel(UnityRPCData data) {
			if (debug) logDebug("GenerateLevel()");
			grid.tileDataString = data.getString(0);
			DataManager.GetInstance().StateManager.SetGameState((int)StateManager.Game.GenerateLevel);

			UIScreenManager uiScreenManager = Object.FindObjectOfType<UIScreenManager>() as UIScreenManager;
			uiScreenManager.Navigate("SelectTeam");
		}
    
		private void PlayerChooseTeam(UnityRPCData data) {
			if (debug) logDebug("PlayerChooseTeam()");
			DataManager.GetInstance().StateManager.SetPlayerTeam(data.getInt(0));

			if (_CALLBACK != null) {
				_CALLBACK(data);
			}
		}

		private void PlayerSetTurn(UnityRPCData data) {
			if (debug) logDebug("PlayerSetTurn()");
			playerMoves.updateSelectedGridPiece(data.getInt(0), data.getFloat(1), data.getFloat(2), data.getFloat(3), data.getFloat(4));
		}
    
		private void PlayerSkipTurn(UnityRPCData data) {
			if (debug) logDebug("PlayerSkipTurn()");
			DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Ended);
		}
		#endregion
	}
}