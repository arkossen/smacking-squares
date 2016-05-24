using com.jbrettob.core;
using System.Collections.Generic;

public class AutoFillBoard:CoreObject
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