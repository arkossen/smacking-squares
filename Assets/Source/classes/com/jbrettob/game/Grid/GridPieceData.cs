using UnityEngine;
using System;

[Serializable]
public class GridPieceData {
	public int tileIndex;
	public int shuffeledIndex;
	public int ownedByPlayer;
	public int tileColor = 0;
	public bool starterTileInit = false;

	public int upNeighbour;
	public int rightNeighbour;
	public int downNeighbour;
	public int leftNeighbour;
}