using com.jbrettob.core;
using System.Collections.Generic;

public class AutoFillBoardCopy:CoreObject
{
	private const int _maxIterationPreventionIndex = 100;
	//
	private List<GridPiece> _savedGridPieces;
	private List<GridPiece> _gridPieces;
	private int _index = 0;

	#region public

	/// <summary>
	/// Checks the auto fill board.
	/// </summary>
	/// <param name="gridPieces">Grid pieces.</param>
	/// <param name="playerData">Player data.</param>
	public void checkAutoFillBoard(List<GridPiece> gridPieces, PlayerData playerData)
	{
		_gridPieces = gridPieces;

		// but also check for tiles we can fill
		_savedGridPieces = new List<GridPiece>();
		
		// 1) start somewhere, pref at the end or the beginning.
		GridPiece startGridPiece = _gridPieces[_gridPieces.Count - 1];
		
		// 2) go 'one direction', to the left
		_index = 0;
		checkHorizontalRow(startGridPiece, startGridPiece, playerData.teamColor);
		_index = 0;
		checkVerticalRow(startGridPiece, startGridPiece, playerData.teamColor);
		
		// 3) check results
		setFillAndClearList(playerData.teamColor);
	}
	#endregion

	#region private

	/// <summary>
	/// Checks the horizontal row. From Left to Right (except our index is in reverse).
	/// </summary>
	/// <param name="initGridPiece">Init grid piece.</param>
	/// <param name="startGridPiece">Start grid piece.</param>
	/// <param name="teamColor">Team color.</param>
	private void checkHorizontalRow(GridPiece initGridPiece, GridPiece startGridPiece, PlayerNames.TEAM_COLOR teamColor)
	{
		_index++;
		
		if (_index > _maxIterationPreventionIndex)
		{
			logWarning("we've just used to much iterations! Exit endless loop to prevent crashing");
			return;
		}
		
		// check if initGridPiece is same as our teamColor
		if (initGridPiece.ownedByPlayer == (int) teamColor)
		{
			if (initGridPiece != startGridPiece)
			{
				logDebug("==");
				logDebug("checkRow() initGridPiece: " + initGridPiece.tileIndex + " startGridPiece: " + startGridPiece.tileIndex + " color: " + teamColor);
				if (startGridPiece.ownedByPlayer == (int) teamColor)
				{
					logDebug("startGridPiece " + startGridPiece.tileIndex + " is the owned by our player : " + teamColor);

					setFillAndClearList(teamColor);

					if (startGridPiece.tileIndex > 0)
					{
						// check if startGridPiece is not on the outerboard
						GridPiece nextGridPiece = _gridPieces[(startGridPiece.tileIndex-1)];
						checkHorizontalRow(initGridPiece, nextGridPiece, teamColor); 
					}
					else
					{
						logDebug("checkRow() We have reached the end of the tileIndex!");
					}
				}
				else if (startGridPiece.ownedByPlayer == (int) PlayerNames.TEAM_COLOR.BLANK)
				{
					// is an empty color, add this to list and compare later.
					logDebug("startGridPiece " + startGridPiece.tileIndex + " is empty and added to the list.");
					
					_savedGridPieces.Add(startGridPiece);
					
					if (startGridPiece.tileIndex > 0)
					{
						GridPiece nextGridPiece = _gridPieces[(startGridPiece.tileIndex-1)];
						checkHorizontalRow(initGridPiece, nextGridPiece, teamColor);
					}
					else
					{
						logDebug("checkRow() We have reached the end of the tileIndex!");
					}
				}
				else
				{
					// is taken by a other color
					// reset list and start over from next tile.
					logDebug("startGridPiece " + startGridPiece.tileIndex + " is owned by another player! Restart sequence from next tile");
				}
			}
			else
			{
				GridPiece nextGridPiece = _gridPieces[(initGridPiece.tileIndex-1)];
				checkHorizontalRow(initGridPiece, nextGridPiece, teamColor); 
			}
		}
		else
		{
			// find next initGridPiece based on given initGridPiece
			if (initGridPiece.tileIndex > 0)
			{
				GridPiece nextGridPiece = _gridPieces[(initGridPiece.tileIndex-1)];
				checkHorizontalRow(nextGridPiece, nextGridPiece, teamColor); 
			}
			else
			{
				logDebug("checkRow() We have reached the end of the tileIndex!");
			}
		}
	}

	/// <summary>
	/// Checks the vertical row. From Up to Down.
	/// </summary>
	/// <param name="initGridPiece">Init grid piece.</param>
	/// <param name="startGridPiece">Start grid piece.</param>
	/// <param name="teamColor">Team color.</param>
	private void checkVerticalRow(GridPiece initGridPiece, GridPiece startGridPiece, PlayerNames.TEAM_COLOR teamColor)
	{
		_index++;
		
		if (_index > _maxIterationPreventionIndex)
		{
			logWarning("we've just used to much iterations! Exit endless loop to prevent crashing");
			return;
		}

		// check if initGridPiece is same as our teamColor
		if (initGridPiece.ownedByPlayer == (int) teamColor)
		{
			if (initGridPiece != startGridPiece)
			{
				logDebug("==");
				logDebug("checkRow() initGridPiece: " + initGridPiece.tileIndex + " startGridPiece: " + startGridPiece.tileIndex + " color: " + teamColor);
			}
		}
	}

	private void setFillAndClearList(PlayerNames.TEAM_COLOR teamColor)
	{
		if (_savedGridPieces == null || _savedGridPieces.Count <= 0) return;

		logDebug("setFill() _savedGridPieces.Count: " + _savedGridPieces.Count);
		for (int i = 0; i < _savedGridPieces.Count; i++)
		{
			_savedGridPieces[i].SetToPlayer((int) teamColor);
		}
		_savedGridPieces.Clear();
	}

	#endregion
}