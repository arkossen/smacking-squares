using UnityEngine;

public class StartGameScreen:CoreScreen {
	private Grid _grid;

	public void OnGenerateLevel() {
		if (debug) {
			logDebug("OnGenerateLevel");
		}

		_grid = Object.FindObjectOfType<Grid>();
		_grid.StartLevel();

		Navigate("MainMenu");
	}
}