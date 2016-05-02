using System;
using System.Collections;
using UnityEngine;
using com.jbrettob.data;
using com.jbrettob.data.enums;

public class UnityNetworkManager:SingletonManager<UnityNetworkManager> {
	private string _gameName;
	private string _gameVersion;
	private string _gameType;
	private int _port = 25002;
	private string _roomName;
	//
	private string _username = string.Empty;
	private bool _loggedIn;
	//
	private string _testMessage = string.Empty;
	private bool _doneTesting;
	private bool _useNat;
	private ConnectionTesterStatus _connectionTestResult;
	private bool _probingPublicIP;
	private float _timer;
	//
	private Action _onRoomJoined;
	private Action<NetworkConnectionError> _onRoomJoinedError;
	private Action<NetworkPlayer> _onPlayerConnected;
	private Action<NetworkPlayer> _onPlayerDisconnected;
	private Action<UnityRPCData> _onGetMessage;
	private Action<NetworkDisconnection> _onRoomLeft;
	private Action<UnityRoom[]> _onGetRoom;
	
	public override void Awake() {
		base.Awake();
		
		name = "_UnityNetworkManager";
		gameObject.AddComponent<NetworkView>();
		networkView.stateSynchronization = NetworkStateSynchronization.Off;
		networkView.observed = null;
	}
	
	#region IUnityNetworking API
	public void init(string gameName, string gameVersion, int port) {
		init(gameName, gameVersion, port, false);
	}
	
	public void init(string gameName, string gameVersion, int port, bool debug) {
		this.debug = debug;
		_gameName = gameName;
		_gameVersion = gameVersion;
		_port = port;
		_timer = 0;
		if (!_doneTesting) testConection();
	}

	public void init(string gameName, string gameVersion, int port, bool debug, string masterServerIP, int masterServerPort) {
		this.debug = debug;
		_gameName = gameName;
		_gameVersion = gameVersion;
		_port = port;
		_timer = 0;

		MasterServer.ipAddress = masterServerIP;
		MasterServer.port = masterServerPort;

		if (!_doneTesting) {
			testConection();
		}
	}
	
	private void testConection() {
		_connectionTestResult = Network.TestConnection();
		switch (_connectionTestResult) {
			case ConnectionTesterStatus.Error: 
				_testMessage = "Problem determining NAT capabilities";
				_doneTesting = true;
				break;
				
			case ConnectionTesterStatus.Undetermined: 
				_testMessage = "Undetermined NAT capabilities";
				_doneTesting = false;
				break;
							
			case ConnectionTesterStatus.PublicIPIsConnectable:
				_testMessage = "Directly connectable public IP address.";
				_useNat = false;
				_doneTesting = true;
				break;
				
			case ConnectionTesterStatus.PublicIPPortBlocked:
				_testMessage = "Non-connectble public IP address (port " + _port + " blocked), running a server is impossible.";
				_useNat = false;
				if (!_probingPublicIP) {
					if (debug) logDebug("Testing if firewall can be circumvented");
					_connectionTestResult = Network.TestConnectionNAT();
					_probingPublicIP = true;
					_timer = Time.time + 10;
				} else if (Time.time > _timer) {
					_probingPublicIP = false;
					_useNat = true;
					_doneTesting = true;
				}
				break;
			case ConnectionTesterStatus.PublicIPNoServerStarted:
				_testMessage = "Public IP address but server not initialized, it must be started to check server accessibility. Restart connection test when ready.";
				break;
				
			case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
				if (debug) logDebug("LimitedNATPunchthroughPortRestricted");
				_testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers.";
				_useNat = true;
				_doneTesting = true;
				break;
						
			case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
				if (debug) logDebug("LimitedNATPunchthroughSymmetric");
				_testMessage = "Limited NAT punchthrough capabilities. Cannot connect to all types of NAT servers. Running a server is ill adviced as not everyone can connect.";
				_useNat = true;
				_doneTesting = true;
				break;
			
			case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
			case ConnectionTesterStatus.NATpunchthroughFullCone:
				if (debug) logDebug("NATpunchthroughAddressRestrictedCone || NATpunchthroughFullCone");
				_testMessage = "NAT punchthrough capable. Can connect to all servers and receive connections from all clients. Enabling NAT punchthrough functionality.";
				_useNat = true;
				_doneTesting = true;
				break;
			
			default:
				_testMessage = "Error in test routine, got " + _connectionTestResult;
				break;
		}
		
		if (debug) logDebug("testConnection() result: " + _testMessage);
	}
	
	public void connect(string username) {
		_username = username;
		_loggedIn = true;
	}
	
	public bool isLoggedIn() {
		return _loggedIn;
	}
	
	public void logOut() {
		OnApplicationQuit();
	}
	
	public void createRoom(string roomName, string password, string gameType, int maxConnetions, Action onRoomJoined, Action<NetworkConnectionError> onRoomJoinedError, Action<NetworkPlayer> onPlayerConnected, Action<NetworkPlayer> onPlayerDisconnected, Action<NetworkDisconnection> onRoomLeft) {
		_onRoomJoined = onRoomJoined;
		_onRoomJoinedError = onRoomJoinedError;
		_onPlayerConnected = onPlayerConnected;
		_onPlayerDisconnected = onPlayerDisconnected;
		_onRoomLeft = onRoomLeft;
		_roomName = roomName;
		_gameType = gameType;
		Network.incomingPassword = password;
		Network.InitializeServer(maxConnetions, _port, _useNat);
	}
	
	public void joinRoom(UnityRoom room, Action onRoomJoined, Action<NetworkConnectionError> onRoomJoinedError, Action<NetworkDisconnection> onRoomLeft) {
		if (debug) logDebug("joinRoom() room: " + room);
		_onRoomJoined = onRoomJoined;
		_onRoomJoinedError = onRoomJoinedError;
		_onRoomLeft = onRoomLeft;
		Network.Connect(room.ip, room.port);
	}
	
	public void leaveRoom() {
		if (Network.isServer)
			MasterServer.UnregisterHost();
		Network.Disconnect();
	}
	
	public void disconnect() {
		Network.Disconnect();
		_loggedIn = false;
		_username = string.Empty;
	}
	
	public bool isInRoom() {
		return (Network.peerType == NetworkPeerType.Client || Network.peerType == NetworkPeerType.Server);
	}
	
	public void getAllRooms(string roomType, Action<UnityRoom[]> onGetRoom) {
		getAllRooms(roomType, onGetRoom, 1f);
	}

	public void getAllRooms(string roomType, Action<UnityRoom[]> onGetRoom, float timeDelay) {
		_onGetRoom = onGetRoom;
		MasterServer.ClearHostList();
		MasterServer.RequestHostList("RandomGameName");
		
		Invoke("onGetRooms", timeDelay);
	}

	private void onGetRooms() {
		HostData[] hosts = MasterServer.PollHostList();
		UnityRoom[] rooms = new UnityRoom[hosts.Length];
		
		for (int i = 0; i < hosts.Length; i++) {
			rooms [i] = new UnityRoom(hosts [i]);
		}
		
		_onGetRoom(rooms);
	}

	public void Send(string eventName, params object[] data) {
		if (Network.isServer) {
			OnGetMessage(eventName, parseToString(data));
		} else if (Network.isClient) {
			networkView.RPC("onGetMessage", RPCMode.Server, eventName, parseToString(data));
		}
	}
	
	public void Send(string eventName, bool others, params object[] data) {
		if (Network.isServer) networkView.RPC("OnGetMessage", RPCMode.Others, eventName, parseToString(data));
	}
	
	private string parseToString(params object[] data) {
		string result = "";
		
		for (int i = 0; i < data.Length; i++) {
			result += data [i].ToString();
			if (i != data.Length - 1) {
				result += ",";
			}
		}
		
		if(debug) logDebug("parseToString() result: " + result);
		
		return result;
	}
	
	public void SetOnGetMessage(Action<UnityRPCData> onGetMessage) {
		_onGetMessage = onGetMessage;
	}

	void OnApplicationQuit() {
		if (debug) logDebug("OnApplicationQuit");
		if (Network.isServer) MasterServer.UnregisterHost();
		Network.Disconnect();
	}
	
	[RPC]
	public void OnGetMessage(string eventName, string data) {
		UnityRPCData newData = new UnityRPCData();
		newData.parseData(eventName, data);
		
		if (_onGetMessage != null) {
			_onGetMessage(newData);
		}
	}
	
	public string gameName {
		get { return _gameName; }
	}
	
	public string gameVersion {
		get { return _gameVersion; }
	}
	
	public string username {
		get { return _username; }
	}
	
	public int port {
		get { return _port; }
	}
	
	public string ip {
		get { return (Network.player.externalIP != null) ? Network.player.externalIP : Network.player.ipAddress; }
	}
	
	public NetworkPlayer[] getConnectedPlayers() {
		return Network.connections;
	}
	#endregion
	
	#region Unity in-build network API
	private void OnFailedToConnectToMasterServer(NetworkConnectionError info) {
		if (debug) {
			logDebug("Could not connect to master server: " + info);
		}
	}
	
	private void OnMasterServerEvent(MasterServerEvent msEvent) {
		if (msEvent == MasterServerEvent.RegistrationSucceeded) {
			if (debug) {
				logDebug("OnMasterServerEvent() Server registered");
			}
		}
	}
	
	private void OnServerInitialized() {
		if (debug) logDebug("OnServerInitialized()");
		MasterServer.RegisterHost(_gameType, gameName, _roomName);
		
		if (_onRoomJoined != null) {
			_onRoomJoined();
		} else {
			logError("OnServerInitialized() has no callback but is connected to a room!");
		}
	}
	
	private void OnConnectedToServer() {
		if (debug) logDebug("OnConnectedToServer()");
		
		if (_onRoomJoined != null) {
			_onRoomJoined();
		} else {
			logError("OnConnectedToServer() has no callback but is connected to a room!");
		}
	}
	
	private void OnFailedToConnect(NetworkConnectionError error) {
		if(_onRoomJoinedError != null) {
			// TODO: insert a message based on the error enums
			_onRoomJoinedError(error);
		} else {
			logError("OnFailedToConnect() has no callback but contains an error: " + error);
		}
	}
	
	private void OnPlayerConnected(NetworkPlayer networkPlayer) {
		if (debug) logDebug("OnPlayerConnected()");

		if (_onPlayerConnected != null) {
			_onPlayerConnected(networkPlayer);
		} else {
			logError("OnPlayerConnected() has no callback but a networkPlayer has connected: " + networkPlayer);
		}
	}
	
	private void OnPlayerDisconnected(NetworkPlayer networkPlayer) {
		if (debug) logDebug("OnPlayerDisconnected()");

		if(_onPlayerDisconnected != null) {
			_onPlayerDisconnected(networkPlayer);
		} else {
			logError("OnPlayerDisconnected() has no callback but a networkPlayer has connected: " + networkPlayer);
		}
	}
	
	private void OnDisconnectedFromServer(NetworkDisconnection message) {
		if (Network.isServer) {
			if (debug) logDebug("Local server connection disconnected");
			if (_onRoomLeft != null) _onRoomLeft(message);
		} else {
			if (message == NetworkDisconnection.LostConnection) {
				if (debug) logDebug("Lost connection to the server");
				if (_onRoomLeft != null) _onRoomLeft(message);
			} else {
				if (debug) logDebug("Disconnected from the server");
				if (_onRoomLeft != null) _onRoomLeft(message);
			}
		}
	}
	#endregion
}