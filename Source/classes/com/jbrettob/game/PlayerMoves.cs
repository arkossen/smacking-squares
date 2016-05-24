using UnityEngine;
using System.Collections;
using com.jbrettob.core;
using System.Collections.Generic;
using com.jbrettob.data.enums;
using com.jbrettob.data;

public class PlayerMoves:CoreMonoBehaviour {
    private UnityNetworkManager _unityNetworkManager;
    private Grid _grid;
    
    private GridPiece _selectedGridPiece;
    private Vector2 _oldMousePosition;
    
    private Camera _mainCamera;
    private AudioPlayer _audioPlayer;

	#region unity
	public override void Awake() {
		base.Awake();
		_unityNetworkManager = UnityNetworkManager.getInstance();
		_grid = FindObjectOfType(typeof(Grid)) as Grid;
	}

    void Start() {
        _mainCamera = Camera.main;
        _audioPlayer = Object.FindObjectOfType<AudioPlayer>();
    }

    void Update() {
        if (
			DataManager.GetInstance().StateManager.CurrentGameState != (int)StateManager.Game.Playing ||
			DataManager.GetInstance().StateManager.CurrentTurnState != (int)StateManager.Turn.Start
			) {
             //it's not our turn yet to interact
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            locateSelectedGridPiece();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            clearSelectedGridPlace();
        }

        if (_selectedGridPiece != null) {
            if (Input.GetKeyDown(KeyCode.W)) {
                if (_selectedGridPiece.SetNextNeighbour(GridPiece.DIRECTION.DOWN, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            } else if (Input.GetKeyDown(KeyCode.D)) {
                if (_selectedGridPiece.SetNextNeighbour(GridPiece.DIRECTION.RIGHT, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            } else if (Input.GetKeyDown(KeyCode.S)) {
                if (_selectedGridPiece.SetNextNeighbour(GridPiece.DIRECTION.UP, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            } else if (Input.GetKeyDown(KeyCode.A)) {
                if (_selectedGridPiece.SetNextNeighbour(GridPiece.DIRECTION.LEFT, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            }
        }
    }
	#endregion

	#region public
    public void clearSelectedGridPlace() {
        if (_selectedGridPiece != null && _selectedGridPiece.shaderSwitcher != null) {
            _selectedGridPiece.shaderSwitcher.SwitchShader();
        }
        _selectedGridPiece = null;
    }
    
    public void updateSelectedGridPiece(int _gridPieceIdentifierIndex, float _oldMousePositionX, float _oldMousePositionY, float _newMousePositionX, float _newMousePositionY) {
        float diffX = 0;
        float diffY = 0;
        
        GridPiece _gridPiece = null;
        
        for (int i = 0; i < _grid.gridPieceList.Count; i++) {
            if (_grid.gridPieceList [i].gridPieceData.tileIndex == _gridPieceIdentifierIndex) {
                _gridPiece = _grid.gridPieceList [i];
            }
        }
        
        diffX = _oldMousePositionX - _newMousePositionX;
        diffY = _oldMousePositionY - _newMousePositionY;
        
        if (Mathf.Abs(diffX) > Mathf.Abs(diffY)) {
            if (diffX > 0) {
                if (_gridPiece.SetNextNeighbour(GridPiece.DIRECTION.LEFT, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            } else {
                if (_gridPiece.SetNextNeighbour(GridPiece.DIRECTION.RIGHT, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            }
        } else {
            if (diffY > 0) {
                if (_gridPiece.SetNextNeighbour(GridPiece.DIRECTION.UP, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            } else {
                if (_gridPiece.SetNextNeighbour(GridPiece.DIRECTION.DOWN, onTurnComplete)) {
					DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Execute);
                }
            }
        }
    }
	#endregion

	#region private
    private void locateSelectedGridPiece() {
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit)) {
            GridPiece gridPiece = hit.collider.gameObject.GetComponent<GridPiece>();
			if (gridPiece != null && gridPiece.gridPieceData.ownedByPlayer != (int)PlayerNames.TEAM_COLOR.BLANK && gridPiece.gridPieceData.ownedByPlayer != DataManager.GetInstance().StateManager.GetPlayerTurn().team) {
                _audioPlayer.play(AudioNames.SELECT_WRONG);
            }
            
			if (gridPiece != null && gridPiece.gridPieceData.ownedByPlayer == DataManager.GetInstance().StateManager.GetPlayerTurn().team && gridPiece.isSelectable) {
                if (_selectedGridPiece != null) {
                    if (_selectedGridPiece.shaderSwitcher != null) {
                        _selectedGridPiece.shaderSwitcher.SwitchShader();
                    }
                }
                _selectedGridPiece = gridPiece;
                if (_selectedGridPiece.shaderSwitcher != null) {
                    _selectedGridPiece.shaderSwitcher.SwitchShader();
                }
                if (_audioPlayer != null) {
                    _audioPlayer.play(AudioNames.SELECT);
                }
                _oldMousePosition = Input.mousePosition;
            } else {
                if (_selectedGridPiece != null) {
                    Vector2 _newMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    
					if (Project.IsOnlineMultiplayer.Equals(true)) {
                        _unityNetworkManager.Send(ServerEventNames.PLAYER_TURN, _selectedGridPiece.gridPieceData.tileIndex, _oldMousePosition.x, _oldMousePosition.y, _newMousePosition.x, _newMousePosition.y);
                    } else {
                        updateSelectedGridPiece(_selectedGridPiece.gridPieceData.tileIndex, _oldMousePosition.x, _oldMousePosition.y, _newMousePosition.x, _newMousePosition.y);
                    }
                    
                    if (_selectedGridPiece.shaderSwitcher != null) {
                        _selectedGridPiece.shaderSwitcher.SwitchShader();
                    }
                    _selectedGridPiece = null;
                }
            }
        }
    }
   
    private void onTurnComplete() {
        if (debug) logDebug("onTurnComplete()");
		DataManager.GetInstance().StateManager.SetTurnState((int)StateManager.Turn.Ended);
    }
	#endregion

}