using UnityEngine;
using System.Collections;
using com.jbrettob.core;
using com.jbrettob.data.enums;

public class StateManager:CoreObject {
	public enum Game { 
		Idle, 
		GenerateLevel,
		ChooseTeamStart, 
		ChooseTeamWait, 
		ChooseTeamEnded, 
		Start, 
		Playing, 
		Ended 
	}
	
	public enum Turn { 
		Start,
		ChooseTitle, 
		ChooseDirection, 
		Execute,
		Ended
	}

	public System.Action OnGameFinished;
	
	private Grid grid;
	private PlayerMoves playerMoves;
	private AudioPlayer audioPlayer;
	//
	private PlayerData[] playerDatas;
	private int currentGameState = 0;
	private int currentTurnState = 0;

	#region public methods
	public void Init(int playerCount) {
		grid = Object.FindObjectOfType<Grid>();
		
		playerMoves = Object.FindObjectOfType<PlayerMoves>();
		audioPlayer = Object.FindObjectOfType<AudioPlayer>();

		Camera.main.animation.Play("CameraBackgroundBasic");

		playerDatas = new PlayerData[playerCount];
		NewGame();
	}

	public PlayerData GetPlayerTurn() {
		if (playerDatas[1].turn) {
			return playerDatas[1];
		}

		return playerDatas[0];
	}

	public PlayerData[] GetAllPlayers() {
		return playerDatas;
	}

	public void SetPlayerTeam(int playerIndex) {
		if (playerDatas[0].team < 0) {
			playerDatas[0].team = playerIndex;
			SetPlayerColor(playerDatas[0]);
			
			SetGameState((int)StateManager.Game.ChooseTeamStart);
		} else {
			playerDatas[1].team = playerIndex;
			SetPlayerColor(playerDatas[1]);
		}
		
		// Ugly code
		if (playerDatas[0].team >= 0 && playerDatas[1].team >= 0) {
			SetGameState((int)Game.ChooseTeamEnded);
		}
	}

	public void GameStateHandler(int state) {
		switch (state) {
			case (int)Game.Idle:
				// Do nothing
				break;
			case (int)Game.GenerateLevel:
				// Start a new game (resets data)
				NewGame();
				// Generate level
				grid.StartLevel();
				break;
			case (int)Game.ChooseTeamStart:
				break;
			case (int)Game.ChooseTeamWait:
				// Waiting for players to choose a team
				break;
			case (int)Game.ChooseTeamEnded:
				// Players are done choosing a team
				SetGameState((int)Game.Start);
				break;
			case (int)Game.Start:
				// Signal for starting the game
			
				// PLAYER ONE STARTS
				playerDatas[0].turn = true;
				playerDatas[1].turn = false;
				// Temporarily (for testing)
				SetGameState((int)Game.Playing);
				break;
			case (int)Game.Playing:
				// Play out game logic (turn states)
				SetTurnState((int)Turn.Start);
				break;
			case (int)Game.Ended:
				// Show who won and lost
				Vector2 endScore = grid.getEndScores(playerDatas [0].teamColor, playerDatas [1].teamColor);
				if (endScore.x > endScore.y) {
					GameObject p1Head = GameObject.Find("Player1Head");
					p1Head.GetComponent<TweenScale>().ResetToBeginning();
					p1Head.GetComponent<UISprite>().color = Color.white;
					p1Head.GetComponent<TweenScale>().PlayForward();
				
					GameObject p2Head = GameObject.Find("Player2Head");
					p2Head.GetComponent<TweenScale>().ResetToBeginning();
					p2Head.GetComponent<UISprite>().color = Color.grey;
				} else {
					GameObject p2Head = GameObject.Find("Player2Head");
					p2Head.GetComponent<TweenScale>().ResetToBeginning();
					p2Head.GetComponent<UISprite>().color = Color.white;
					p2Head.GetComponent<TweenScale>().PlayForward();
				
					GameObject p1Head = GameObject.Find("Player1Head");
					p1Head.GetComponent<TweenScale>().ResetToBeginning();
					p1Head.GetComponent<UISprite>().color = Color.grey;

					// TODO: add callback when gameEnded
					if (OnGameFinished != null) {
						OnGameFinished();
					} else {
						logDebug("Game is finished but we have no callback!");
					}
				}
				break;
			default:
				logError("We received a state we can not handle. State: " + state);
				break;
		}
	}
	#endregion

	#region private methods
	private void NewGame() {
		playerDatas[0] = new PlayerData();
		playerDatas[1] = new PlayerData();
	}

	private void SwitchTurn() {
		if (playerDatas[0].turn) {
			playerDatas[0].turn = false;
			playerDatas[1].turn = true;
		} else {
			playerDatas[0].turn = true;
			playerDatas[1].turn = false;
		}
	}

	private void SendPlayerTeam(int playerIndex) {
		if (Project.IsOnlineMultiplayer.Equals(true)) {
			UnityNetworkManager.getInstance().Send(ServerEventNames.PLAYER_CHOOSE_TEAM, playerIndex);
		} else {
			SetPlayerTeam(playerIndex);
		}
	}

	private void SetPlayerColor(PlayerData player) {
		if (player.team == (int)PlayerNames.TEAM_COLOR.RED) {
			player.color = Color.red;
			player.teamColor = PlayerNames.TEAM_COLOR.RED;
		} else if (player.team == (int)PlayerNames.TEAM_COLOR.GREEN) {
			player.color = Color.green;
			player.teamColor = PlayerNames.TEAM_COLOR.GREEN;
		} else if (player.team == (int)PlayerNames.TEAM_COLOR.BLUE) {
			player.color = Color.blue;
			player.teamColor = PlayerNames.TEAM_COLOR.BLUE;
		}
	}

	private void GameReset() {
		SetGameState((int)Game.GenerateLevel);
		if (audioPlayer != null) {
			audioPlayer.play(AudioNames.SELECT_BUTTON);
		}
	}

	private void GameStart(GameObject gameObject) {
		logDebug("gameStart()");
	}

	private void SkipTurn(GameObject gameObject) {
		if ((playerDatas[0].turn || playerDatas[1].turn) && CurrentTurnState == (int)Turn.Start) {
			UnityNetworkManager.getInstance().Send(ServerEventNames.PLAYER_SKIP);
		}
	}

	private void SelectTeamRed(GameObject gameObject) {
		SendPlayerTeam((int)PlayerNames.TEAM_COLOR.RED);

		if (audioPlayer != null) {
			audioPlayer.play(AudioNames.SELECT_BUTTON);
		}
	}

	private void SelectTeamGreen(GameObject gameObject) {
		SendPlayerTeam((int)PlayerNames.TEAM_COLOR.GREEN);
		if (audioPlayer != null) {
			audioPlayer.play(AudioNames.SELECT_BUTTON);
		}
	}
	
	private void SelectTeamBlue(GameObject gameObject) {
		SendPlayerTeam((int)PlayerNames.TEAM_COLOR.BLUE);
		if (audioPlayer != null) {
			audioPlayer.play(AudioNames.SELECT_BUTTON);
		}
	}

	private void TurnStateHandler(int state) {
		PlayerData p = GetPlayerTurn();
		
		switch (state) {
			case (int)Turn.Start:
				if (audioPlayer != null) {
					audioPlayer.play(AudioNames.NEXT_TURN);
				}
				if (p.team == (int)PlayerNames.TEAM_COLOR.RED) {
					Camera.main.animation.Play("CameraBackgroundRed");
				}
				if (p.team == (int)PlayerNames.TEAM_COLOR.GREEN) {
					Camera.main.animation.Play("CameraBackgroundGreen");
				}
				if (p.team == (int)PlayerNames.TEAM_COLOR.BLUE) {
					Camera.main.animation.Play("CameraBackgroundBlue");
				}
				break;
			case (int)Turn.ChooseTitle:
				// Let the player choose a tile
				break;
			case (int)Turn.ChooseDirection:
				// Let the player choose a direction
				break;
			case (int)Turn.Execute:
				// Execute the decision
				break;
			case (int)Turn.Ended:
				// End Turn
				if (playerMoves != null) {
					playerMoves.clearSelectedGridPlace();
				}
				grid.scanLevelForSelectableTiles();
				if (grid.areEmptySpots()) {
					SwitchTurn();
					SetTurnState((int)Turn.Start);
				} else {
					SetGameState((int)Game.Ended);
				}
				break;
		}
	}
	#endregion

	#region getters & setters
	// Returns new current game state
	public int SetGameState(int state) {
		currentGameState = state;
		GameStateHandler(state);
		return CurrentGameState;
	}
	
	public int SetTurnState(int state) {
		currentTurnState = state;
		TurnStateHandler(state);
		return CurrentTurnState;
	}
	
	public int CurrentGameState {
		get { return currentGameState; }
	}
	public int CurrentTurnState {
		get { return currentTurnState; }
	}
	#endregion
}