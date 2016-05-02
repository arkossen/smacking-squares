using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.jbrettob.data.enums;

public class Grid : MonoBehaviour {
    private const int  _STARTER_TILE_AMOUNT = 4;
    private const int _AMOUNT_OF_DIFFERENT_TILES = 3;
	private const int _ROW_WIDTH = 14;
	private const int _GRID_WIDTH = 14;
	private const int _GRID_HEIGHT = 11;
	
    public Transform gridBlockPrefab;
    public Transform[] upperGridTiles;
    
    public string tileDataString = string.Empty;

    private List<GridPiece> _gridPieceList = new List<GridPiece>();
    private AudioPlayer _audioPlayer;
    private bool _levelIsActive = false;

	#region unity
    void Start() {
        gridBlockPrefab.CreatePool();
        
        for (int i = 0; i < upperGridTiles.Length; i++) {
            upperGridTiles [i].CreatePool();
        }

        if (_audioPlayer == null) {
            _audioPlayer = Object.FindObjectOfType<AudioPlayer>();
        }
    }
	#endregion

	#region public
    public void StartLevel() {
        if (!_levelIsActive) {
			_levelIsActive = true;
			DestroyCurrentLevel();   
        }
    }

    public void scanLevelForSelectableTiles() {
        for (int i = _gridPieceList.Count -1; i >= 0; i--) {
            // check up
            if (_gridPieceList [i].gridPieceData.upNeighbour == -1 || _gridPieceList [_gridPieceList [i].gridPieceData.upNeighbour].gridPieceData.ownedByPlayer != (int)PlayerNames.TEAM_COLOR.BLANK) {
                // check right
                if (_gridPieceList [i].gridPieceData.rightNeighbour == -1 || _gridPieceList [_gridPieceList [i].gridPieceData.rightNeighbour].gridPieceData.ownedByPlayer != (int)PlayerNames.TEAM_COLOR.BLANK) {
                    // check down
                    if (_gridPieceList [i].gridPieceData.downNeighbour == -1 || _gridPieceList [_gridPieceList [i].gridPieceData.downNeighbour].gridPieceData.ownedByPlayer != (int)PlayerNames.TEAM_COLOR.BLANK) {
                        // check left
                        if (_gridPieceList [i].gridPieceData.leftNeighbour == -1 || _gridPieceList [_gridPieceList [i].gridPieceData.leftNeighbour].gridPieceData.ownedByPlayer != (int)PlayerNames.TEAM_COLOR.BLANK) {
                            _gridPieceList [i].setSelectable(false);
                        }
                    }
                }
            }
        }
    }

    public string SetPlayerTilesOnGameStartUp() {
        string tileData = string.Empty;
        
        int selectedTile = 0;
        int tileCount = TileCount();
        List<int> indexList = new List<int>();
        
        for (int i = 0; i < _STARTER_TILE_AMOUNT * _AMOUNT_OF_DIFFERENT_TILES; i++) {
        
            selectedTile = Random.Range(0, tileCount);
            if (!indexList.Contains(selectedTile)) {
                indexList.Add(selectedTile);
                
                tileData += selectedTile.ToString();
                if (i < _STARTER_TILE_AMOUNT * _AMOUNT_OF_DIFFERENT_TILES - 1) {
                    tileData += ";";
                }
            } else {
                i--;
            }
        }
        
        return tileData;
    }
    
    public void DevidePlayerTilesBasedOnStringData(string tileData) {
        string[] tileList = tileData.Split(';');
        
        for (int i = 0; i < tileList.Length; i++) {
            _gridPieceList [int.Parse(tileList [i])].SetToPlayer(Mathf.CeilToInt(i / _STARTER_TILE_AMOUNT) + 1);
        }
    }
	#endregion

	#region private
    private void DestroyCurrentLevel() {
        if (_gridPieceList.Count == 0) {
            onLevelDestroyed();
        } else {		
            for (int i = _gridPieceList.Count -1; i >= 0; i--) {
                _gridPieceList [i].Remove(_gridPieceList.Count - 1 == 0, onLevelDestroyed);
                _gridPieceList.RemoveAt(i);
            }
        }
    }
	
    private void onLevelInitalized() {
        _levelIsActive = false;
        //_stateManager.changeState((int)StateManager.GAME.CHOOSE_TEAM_START);
    }
	
    private void onLevelDestroyed() {
        InitLevel();
    }
	
    private void InitLevel() {
        if (_audioPlayer != null) {
            _audioPlayer.play(AudioNames.SPAWN_WORLD_2);
        }
        
        CreateGridTiles();
        if (tileDataString != string.Empty) {
            DevidePlayerTilesBasedOnStringData(tileDataString); //Multiplayer mode
        } else {
            DevidePlayerTilesBasedOnStringData(SetPlayerTilesOnGameStartUp()); //Singleplayer mode.
        }
        ShuffleGridTiles(_gridPieceList);
    }

    private void CreateGridTiles() {
        int x_StartPos = 0;
        int z_StartPos = 0;
        int x_Offset = 1;
        int z_Offset = 1;
		
        // TODO: check Possible FPS Drainer. 
        int tileCount = TileCount();
        GridPiece gridPiece = null;
		
        int up;
        int right;
        int down;
        int left;
        int rowLength = 0;
        Transform tile;
		
        for (int i = 0; i < tileCount; i++) {
            rowLength = Mathf.FloorToInt(i / _ROW_WIDTH);
            tile = gridBlockPrefab.Spawn(new Vector3(x_StartPos + (i * x_Offset) - (rowLength * _ROW_WIDTH), 0, z_StartPos + (z_Offset * rowLength)), Quaternion.identity) as Transform;
            tile.parent = this.transform;
            GridPieceData gridPieceData = new GridPieceData();
			
            gridPiece = tile.GetComponent<GridPiece>();
			
            gridPiece.gridPieceData = gridPieceData;
            gridPieceData.tileIndex = i;
            // basic
            up = gridPieceData.tileIndex - _GRID_WIDTH;
            right = gridPieceData.tileIndex + 1;
            down = gridPieceData.tileIndex + _GRID_WIDTH;
            left = gridPieceData.tileIndex - 1;
			
            //extra check
            if (up < 0) {
                up = -1;
            }
			
            if ((gridPieceData.tileIndex % _GRID_WIDTH) == (_GRID_WIDTH - 1)) {
                right = -1;
            }
			
            if (down >= TileCount()) {
                down = -1;
            }
			
            if ((gridPieceData.tileIndex % _GRID_WIDTH) == 0) {
                left = -1;
            }
			
            gridPiece.SaveNeighbours(up, right, down, left);
			
            _gridPieceList.Add(tile.GetComponent<GridPiece>());
        }

        if (_ROW_WIDTH > 12 ) {
            transform.position = new Vector3(transform.position.x - (_ROW_WIDTH - 12.5f), transform.position.y, transform.position.z);
        }
    }
    
    private void ShuffleGridTiles(List<GridPiece> list) {
        List<GridPiece> tempShuffleList = new List<GridPiece>();
		
        // test
        for (int j = 0; j < list.Count; j++) {
            tempShuffleList.Add(list [j]);
        }
		
        // TODO: check with allocation?!
        //tempShuffleList = list;
		
        ShuffleList(tempShuffleList);
		
        for (int i = 0; i < tempShuffleList.Count; i++) {
            tempShuffleList [i].gridPieceData.shuffeledIndex = i;
            tempShuffleList [i].Init(i == tempShuffleList.Count - 1, onLevelInitalized);
        }
    }
	
    private List<GridPiece> ShuffleList(List<GridPiece> list) {
        int listCount = list.Count;
		
        for (int i = 0; i < list.Count; i++) {
            int indexToSwap = Random.Range(i, listCount);
            GridPiece oldValue = list [i];
            list [i] = list [indexToSwap];
            list [indexToSwap] = oldValue;
        }
		
        return list;
    }
	#endregion

	#region getters & setters
    public int TileCount() {
        int tileCount = _GRID_WIDTH * _GRID_HEIGHT;
        return tileCount;
    }

    public bool areEmptySpots() {
        return (getCountByColor(PlayerNames.TEAM_COLOR.BLANK) > 0);
    }

    public Vector2 getEndScores(PlayerNames.TEAM_COLOR playerOne, PlayerNames.TEAM_COLOR playerTwo) {
        Vector2 result = new Vector2();

        result.x = getCountByColor(playerOne);
        result.y = getCountByColor(playerTwo);

        return result;
    }

    public List<GridPiece> gridPieceList {
        get { return _gridPieceList; }
    }
	
    private int getCountByColor(PlayerNames.TEAM_COLOR teamColor) {
        int result = 0;
		
        for (int i = 0; i < _gridPieceList.Count; i++) {
            if (_gridPieceList [i].gridPieceData.tileColor == (int)teamColor) {
                result++;
            }
        }

        return result;
    }
	#endregion
}
