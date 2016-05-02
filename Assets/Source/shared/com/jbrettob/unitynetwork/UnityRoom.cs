using com.jbrettob.core;
using UnityEngine;

public class UnityRoom:CoreObject
{
	private string _comment;
	private int _connectedPlayers;
	private string _gameName;
	private string _gameType;
	private string _guid;
	private string[] _ip;
	private bool _passwordProtected;
	private int _playerLimit;
	private int _port;
	private bool _useNat;
	
	public UnityRoom(HostData data)
	{
		_comment = data.comment;
		_connectedPlayers = data.connectedPlayers;
		_gameName = data.gameName;
		_gameType = data.gameType;
		_guid = data.guid;
		_ip = data.ip;
		_passwordProtected = data.passwordProtected;
		_playerLimit = data.playerLimit;
		_port = data.port;
		_useNat = data.useNat;
	}
	
	public string comment
	{
		get { return _comment; }
	}
	
	public int connectedPlayers
	{
		get { return _connectedPlayers; }
	}
	
	public string gameName
	{
		get { return _gameName; }
	}
	
	public string gameType
	{
		get { return _gameType; }
	}
	
	public string guid
	{
		get { return _guid; }
	}
	
	public string[] ip
	{
		get { return _ip; }
	}
	
	public bool passwordProtected
	{
		get { return _passwordProtected; }
	}
	
	public int playerLimit
	{
		get { return _playerLimit; }
	}
	
	public int port
	{
		get { return _port; }
	}
	
	public bool useNat
	{
		get { return _useNat; }
	}
	
	public override string ToString()
	{
		return string.Format("[UnityRoom: comment={0}, connectedPlayers={1}, gameName={2}, gameType={3}, guid={4}, ip={5}, passwordProtected={6}, playerLimit={7}, port={8}, useNat={9}]", comment, connectedPlayers, gameName, gameType, guid, ip, passwordProtected, playerLimit, port, useNat);
	}
}