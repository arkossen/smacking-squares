using UnityEngine;
using System.Collections;

public class ServerEventNames {
    public const string START_GAME = "0";
	public const string GENERATE_LEVEL = "1";

	// Player events
    public const string PLAYER_JOINED = "2";
    public const string PLAYER_LEFT = "3";
    public const string PLAYER_CHOOSE_TEAM = "4";
    public const string PLAYER_TURN = "5";
    public const string PLAYER_SKIP = "6";
	public const string PLAYER_SET_TURN = "7";
}