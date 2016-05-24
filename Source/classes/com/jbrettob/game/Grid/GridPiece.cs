using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.jbrettob.core;

public class GridPiece:CoreMonoBehaviour, ISelectable {
    public enum DIRECTION {
        UP,
        RIGHT,
        DOWN,
        LEFT
    }

    public static float _CURRENT_TIME = 0.25f;
    public static float _AMOUNT_PASSED = 0;
    private static Grid _grid;
    private static AudioPlayer _audioPlayer;
    private static CameraShake _cameraShake;
    private static Camera _mainCamera;

    private GridPieceData _gridPieceData;
    private ShaderSwitcher _shaderSwitcher;
    private Transform _gridPieceUpperPart;

    private System.Action _onInitCallback;
    private System.Action _onDestroyCallback;
    private System.Action _onTurnCallback;

    private float _baseTime = 0.25f;
    private float _incrementShakeStrength = 0.03f;
    private bool _isSelectable;

	#region Unity methods
    public override void Awake() {
        base.Awake();
        if (_grid == null) {
            _grid = FindObjectOfType(typeof(Grid)) as Grid;
        }
    }
	
    void Start() {
        this.CreatePool();

        // TODO: check with scaling
        this.transform.localScale = new Vector3(0, 0, 0);

        if (_cameraShake == null) {
            _cameraShake = UnityEngine.Object.FindObjectOfType<CameraShake>() as CameraShake;
        }
        if (_mainCamera == null) {
            _mainCamera = Camera.main;
        }
        if (_audioPlayer == null) {
            _audioPlayer = UnityEngine.Object.FindObjectOfType<AudioPlayer>();
        }
    }
	#endregion

	#region public
    public bool SetNextNeighbour(DIRECTION dir, System.Action callback) {
        _onTurnCallback = callback;

        if (dir == DIRECTION.UP) {
            if (gridPieceData.upNeighbour == -1 || (_grid.gridPieceList [gridPieceData.upNeighbour].gridPieceData.tileColor != (int)PlayerNames.TEAM_COLOR.BLANK)) {
                if (gridPieceData.upNeighbour != -1) {
                    _grid.gridPieceList [gridPieceData.upNeighbour].Init(true);
                }

                if (_AMOUNT_PASSED > 0 && _onTurnCallback != null) {
                    _onTurnCallback();
                    _CURRENT_TIME = _baseTime;
                    if (_cameraShake != null) {
                        _cameraShake.ShakeCamera(_mainCamera, 0.3f + _incrementShakeStrength * _AMOUNT_PASSED, _incrementShakeStrength * _AMOUNT_PASSED);
                    }
                    if (_audioPlayer != null) {
                        _audioPlayer.play(AudioNames.HIT_4);
                    }
                    _AMOUNT_PASSED = 0;
                }

                if (_audioPlayer != null) {
                    _audioPlayer.play(AudioNames.SELECT_WRONG);
                }

                return false;
            }

            StopCoroutine("SetNextNeighbourAnimation");
            StartCoroutine("SetNextNeighbourAnimation", dir);
        } else if (dir == DIRECTION.RIGHT) {
            if (gridPieceData.rightNeighbour == -1 || (_grid.gridPieceList [gridPieceData.rightNeighbour].gridPieceData.tileColor != (int)PlayerNames.TEAM_COLOR.BLANK)) {
                if (gridPieceData.rightNeighbour != -1) {
                    _grid.gridPieceList [gridPieceData.rightNeighbour].Init(true);
                }

                if (_AMOUNT_PASSED > 0 && _onTurnCallback != null) {
                    _onTurnCallback();
                    _CURRENT_TIME = _baseTime;
                    if (_cameraShake != null) {
                        _cameraShake.ShakeCamera(_mainCamera, 0.3f + _incrementShakeStrength * _AMOUNT_PASSED, _incrementShakeStrength * _AMOUNT_PASSED);
                    }
                    if (_audioPlayer != null) {
                        _audioPlayer.play(AudioNames.HIT_4);
                    }
                    _AMOUNT_PASSED = 0;
                }

                if (_audioPlayer != null) {
                    _audioPlayer.play(AudioNames.SELECT_WRONG);
                }
				
                return false;
            }

            StopCoroutine("SetNextNeighbourAnimation");
            StartCoroutine("SetNextNeighbourAnimation", dir);
        } else if (dir == DIRECTION.DOWN) {
            if (gridPieceData.downNeighbour == -1 || (_grid.gridPieceList [gridPieceData.downNeighbour].gridPieceData.tileColor != (int)PlayerNames.TEAM_COLOR.BLANK)) {
                if (gridPieceData.downNeighbour != -1) {
                    _grid.gridPieceList [gridPieceData.downNeighbour].Init(true);
                }

                if (_AMOUNT_PASSED > 0 && _onTurnCallback != null) {
                    _onTurnCallback();
                    _CURRENT_TIME = _baseTime;
                    if (_cameraShake != null) {
                        _cameraShake.ShakeCamera(_mainCamera, 0.3f + _incrementShakeStrength * _AMOUNT_PASSED, _incrementShakeStrength * _AMOUNT_PASSED);
                    }
                    if (_audioPlayer != null) {
                        _audioPlayer.play(AudioNames.HIT_4);
                    }
                    _AMOUNT_PASSED = 0;
                }

                if (_audioPlayer != null) {
                    _audioPlayer.play(AudioNames.SELECT_WRONG);
                }
				
                return false;
            }

            StopCoroutine("SetNextNeighbourAnimation");
            StartCoroutine("SetNextNeighbourAnimation", dir);
        } else if (dir == DIRECTION.LEFT) {
            if (gridPieceData.leftNeighbour == -1 || (_grid.gridPieceList [gridPieceData.leftNeighbour].gridPieceData.tileColor != (int)PlayerNames.TEAM_COLOR.BLANK)) {
                if (gridPieceData.leftNeighbour != -1) {
                    _grid.gridPieceList [gridPieceData.leftNeighbour].Init(true);
                }

                if (_AMOUNT_PASSED > 0 && _onTurnCallback != null) {
                    _onTurnCallback();
                    _CURRENT_TIME = _baseTime;
                    if (_cameraShake != null) {
                        _cameraShake.ShakeCamera(_mainCamera, 0.3f + _incrementShakeStrength * _AMOUNT_PASSED, _incrementShakeStrength * _AMOUNT_PASSED);
                    }
                    if (_audioPlayer != null) {
                        _audioPlayer.play(AudioNames.HIT_4);
                    }
                    _AMOUNT_PASSED = 0;
                }

                if (_audioPlayer != null) {
                    _audioPlayer.play(AudioNames.SELECT_WRONG);
                }
				
                return false;
            }

            StopCoroutine("SetNextNeighbourAnimation");
            StartCoroutine("SetNextNeighbourAnimation", dir);
        }

        return true;
    }

    public void SetToPlayer(int playerColor) {
        gridPieceData.ownedByPlayer = playerColor;
        _gridPieceData.tileColor = playerColor;
        //gridPieceData.starterTileInit = true;

        setSelectable(true);

        if (playerColor != (int)PlayerNames.TEAM_COLOR.BLANK) {
            CreateUpperTile(playerColor);
        }
    }

    public void SetTileFilled(bool filled) {
        gridPieceData.starterTileInit = filled;
    }

    public void SaveNeighbours(int up, int right, int down, int left) {
        gridPieceData.upNeighbour = up;
        gridPieceData.rightNeighbour = right;
        gridPieceData.downNeighbour = down;
        gridPieceData.leftNeighbour = left;
    }

    public void Init(bool immediate = false) {
        Init(false, _onInitCallback, immediate);
    }

    public void Init(bool isLastOne, System.Action onInitCallback, bool immediate = false) {
        _onInitCallback = onInitCallback;

        if (immediate) {
            animation.Play("GridBlockSpawn");
        } else {
            StartCoroutine("WaitForSecondsToPlayAnimation", isLastOne);
        }
    }

    public void Remove() {
        Remove(false, _onDestroyCallback);
    }

    public void Remove(bool isLastOne, System.Action onDestroyCallback) {
        _onDestroyCallback = onDestroyCallback;
        StartCoroutine("WaitForRemoveAnimationAndDestroyTile", isLastOne);
    }
	#endregion

	#region private
    private IEnumerator SetNextNeighbourAnimation(DIRECTION dir) {
        // animation timer
        _CURRENT_TIME -= 0.03f;
        _AMOUNT_PASSED += 1;
        if (_audioPlayer != null) {
            _audioPlayer.play(AudioNames.BLIP);
        }
        yield return new WaitForSeconds(Mathf.Max(_CURRENT_TIME, 0.05f));
		
        if (dir == DIRECTION.UP) {
            _grid.gridPieceList [_gridPieceData.upNeighbour].SetToPlayer(_gridPieceData.ownedByPlayer);
            _grid.gridPieceList [_gridPieceData.upNeighbour].SetNextNeighbour(dir, _onTurnCallback);
        } else if (dir == DIRECTION.RIGHT) {
			
            _grid.gridPieceList [_gridPieceData.rightNeighbour].SetToPlayer(_gridPieceData.ownedByPlayer);
            _grid.gridPieceList [_gridPieceData.rightNeighbour].SetNextNeighbour(dir, _onTurnCallback);
        } else if (dir == DIRECTION.DOWN) {
			
            _grid.gridPieceList [_gridPieceData.downNeighbour].SetToPlayer(_gridPieceData.ownedByPlayer);
            _grid.gridPieceList [_gridPieceData.downNeighbour].SetNextNeighbour(dir, _onTurnCallback);
        } else if (dir == DIRECTION.LEFT) {
            _grid.gridPieceList [_gridPieceData.leftNeighbour].SetToPlayer(_gridPieceData.ownedByPlayer);
            _grid.gridPieceList [_gridPieceData.leftNeighbour].SetNextNeighbour(dir, _onTurnCallback);
        }
    }

    private void CreateUpperTile(int _player) {
        int playerRandomUpperTile = 0;
		
        if (_player == (int)PlayerNames.TEAM_COLOR.RED) {
            playerRandomUpperTile = UnityEngine.Random.Range(0, 4);
        } else if (_player == (int)PlayerNames.TEAM_COLOR.GREEN) {
            playerRandomUpperTile = UnityEngine.Random.Range(4, 9);
        } else if (_player == (int)PlayerNames.TEAM_COLOR.BLUE) {
            playerRandomUpperTile = UnityEngine.Random.Range(9, 13);
        }

        // TODO: Object pool this in a good way, since it doesnt work completly with ObjectPool. Scale is getting to 0
        //_grid.UpperGridTiles[playerRandomUpperTile].CreatePool();
        //_gridPieceUpperPart = _grid.UpperGridTiles[playerRandomUpperTile].Spawn(transform.position, Quaternion.identity) as Transform;
        _gridPieceUpperPart = Instantiate(_grid.upperGridTiles [playerRandomUpperTile], transform.position, Quaternion.identity) as Transform;
        _shaderSwitcher = _gridPieceUpperPart.GetComponent<ShaderSwitcher>();
        _gridPieceUpperPart.parent = transform;
        // TODO: optimize this, GetComponentInChildren takes to much ms
        _gridPieceUpperPart.GetComponentInChildren<Animation>().Play("GridBlockJump");
    }

    IEnumerator WaitForSecondsToPlayAnimation(bool isLastOne) {
        yield return new WaitForSeconds(0.01f * gridPieceData.shuffeledIndex);
        animation.Play("GridBlockSpawn");

        while (animation.isPlaying) {
            yield return new WaitForSeconds(1f);
        }

        if (isLastOne && _onInitCallback != null) {
            _onInitCallback();
        }
    }

    IEnumerator WaitForRemoveAnimationAndDestroyTile(bool isLastOne) {
        yield return new WaitForSeconds(0.01f * gridPieceData.shuffeledIndex);
        animation.Play("GridBlockDestroy");

        while (animation.isPlaying) {
            yield return new WaitForSeconds(1f);
        }

        this.Recycle();

        if (isLastOne && _onDestroyCallback != null) {
            _onDestroyCallback();
        }
    }
	#endregion

	#region getters & setters
    public GridPieceData gridPieceData {
        get { return _gridPieceData;}
        set { _gridPieceData = value; }
    }
	
    public ShaderSwitcher shaderSwitcher {
        get { return _shaderSwitcher;}
    }
	
    public int tileIndex {
        get { return _gridPieceData.tileIndex;}
    }
	
    public int shuffledIndex {
        get { return _gridPieceData.shuffeledIndex;}
    }
	
    public int tileColor {
        get { return _gridPieceData.tileColor; }
    }
	
    public bool TileFilled {
        get{ return _gridPieceData.starterTileInit; }
    }
	public int ownedByPlayer {
		get { return _gridPieceData.ownedByPlayer; }
	}
	#endregion

	#region ISelectable implementation
    public bool isSelectable {
        get { return _isSelectable; }
    }

    public void setSelectable(bool isSelectable) {
        _isSelectable = isSelectable;
    }
	#endregion
}