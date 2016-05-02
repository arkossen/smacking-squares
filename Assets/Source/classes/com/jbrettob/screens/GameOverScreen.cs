using UnityEngine;
using com.jbrettob.core;

public class GameOverScreen:CoreScreen
{
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.A)) {
						Navigate ("SelectTeamScreen");
				}
		}
}