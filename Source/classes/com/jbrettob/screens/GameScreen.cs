using System.Collections;
using com.jbrettob.data;
using com.jbrettob.gamenetwork;
using com.jbrettob.data.enums;

public class GameScreen:CoreScreen {
	public override void Awake() {
		base.Awake();
		DataManager.GetInstance().StateManager.OnGameFinished += onGameFinished;
	}

	public override void onActivated() {
		ClientLogic.SetCallback(onChangeState);
	}

	private void onChangeState(UnityRPCData data) {
		if (data.eventName == ServerEventNames.PLAYER_LEFT) {
			onExitButtonClicked();
		}
	}

	public void onSkipTurn() {
		if (Project.IsOnlineMultiplayer) {
			UnityNetworkManager.getInstance().Send(ServerEventNames.PLAYER_SKIP);
		} else {
			DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Ended);
		}
	}

	public void onExitButtonClicked() {
		Navigate("MainMenu");
	}

	private void onGameFinished() {
		logDebug("onGameFinished()");

		Navigate("GameFinished");
	}
}