/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

using System.Collections.Generic;
using UnityEngine;

namespace com.jbrettob.debug
{
	// hides the component from the menu scripts
	[AddComponentMenu("")]
	/// <summary>
	/// DebugLoggerManager to log message and set a timestamp on it.
	/// </summary>
	public class DebugLoggerManager:MonoBehaviour
	{
		/// <summary>
		/// Show the log by default on screen.
		/// </summary>
		public static bool SHOW_LOG = false;
		public static bool USE_LOG = true;
		//
		private static DebugLoggerManager _INSTANCE;
		//
		private List<LogItem> _logs;
		private float _timeStamp;
		private int _maxLines;
		private int _currentLine;
		private Rect _position;
		private Color _guiColor;
		private GUIStyle _guiStyle;
		
		/// <summary>
		/// Gets the instance of <see cref="DebugLoggerManager"/> class.
		/// </summary>
		/// <value>
		/// The instance of <see cref="DebugLoggerManager"/> class.
		/// </value>
		public static DebugLoggerManager getInstance
		{
			get
			{
				if (_INSTANCE == null)
				{
					GameObject go = GameObject.Find("_DebugLoggerManager");
					if (go != null)
					{
						Destroy(go);
					}
					_INSTANCE = new GameObject("_DebugLoggerManager").AddComponent<DebugLoggerManager>();
				}
	
				return _INSTANCE;
			}
		}
		
		public void Awake()
		{
			_logs = new List<LogItem>();
			_currentLine = 0;
			_timeStamp = Time.time;
			_maxLines = 40;
			_position = new Rect(10, Screen.height - (10 * (_maxLines + 2)) - 10, 400, 10 * (_maxLines + 2));
			_guiColor = Color.cyan;
			
			DontDestroyOnLoad(gameObject);
		}
		
		void Update()
		{
			_timeStamp = Time.time;
	
			if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				DebugLoggerManager.SHOW_LOG = (DebugLoggerManager.SHOW_LOG) ? false : true;
			}
		}
		
		/// <summary>
		/// Logs the debug.
		/// </summary>
		/// <returns>
		/// The debug.
		/// </returns>
		/// <param name='message'>
		/// Message.
		/// </param>
		public string logDebug(string message)
		{
			LogItem logItem = new LogItem(0, "[" + getTimeStamp().ToString("0.000") + "]: " + message);
			_logs.Add(logItem);
			_currentLine ++;
	
			return logItem.message;
		}
		
		/// <summary>
		/// Logs the warning.
		/// </summary>
		/// <returns>
		/// The warning.
		/// </returns>
		/// <param name='message'>
		/// Message.
		/// </param>
		public string logWarning(string message)
		{
			LogItem logItem = new LogItem(1, "[" + getTimeStamp().ToString("0.000") + "]: " + message);
			_logs.Add(logItem);
			_currentLine ++;
	
			return logItem.message;
		}
		
		/// <summary>
		/// Logs the error.
		/// </summary>
		/// <returns>
		/// The error.
		/// </returns>
		/// <param name='message'>
		/// Message.
		/// </param>
		public string logError(string message)
		{
			LogItem logItem = new LogItem(2, "[" + getTimeStamp().ToString("0.000") + "]: " + message);
			_logs.Add(logItem);
			_currentLine ++;
	
			return logItem.message;
		}
		
		void OnGUI()
		{
			if (!SHOW_LOG) return;
			GUI.depth = 0;
			_position = new Rect(10, Screen.height - (10 * (_maxLines + 2)) - 10, 400, 10 * (_maxLines + 2));
			
			if (_guiStyle == null)
			{
				_guiStyle = new GUIStyle();
				_guiStyle.fontSize = 9;
				_guiStyle.fixedWidth = _position.width;
				_guiStyle.fixedHeight = _position.height;
				_guiStyle.clipping = TextClipping.Clip;
				_guiStyle.wordWrap = true;
				_guiStyle.normal.textColor = _guiColor;
			}
			GUI.Label(_position, getLogsAsString(), _guiStyle);
		}
	
		void OnApplicationQuit()
		{	
			_INSTANCE = null;
		}
		
		/// <summary>
		/// Gets the logs as string.
		/// </summary>
		/// <returns>
		/// The logs as string.
		/// </returns>
		private string getLogsAsString()
		{
			string result = "Debug logger: " + " unity (" + Application.unityVersion + ")" + "\n";
			int startIndex = ((_logs.Count - _maxLines) <= 0) ? 0 : (_logs.Count - _maxLines);
			
			/*
			// top to bottom
			for (int i = startIndex; i < _logs.Count; i++)
			{
				result += _logs[i].message + "\n";
			}
			*/
	
			// bottom to top
			for (int i = _logs.Count-1; i > startIndex; i--)
			{
				result += _logs[i].message + "\n";
			}
			
			return result;
		}
		
		/// <summary>
		/// Gets the time stamp.
		/// </summary>
		/// <returns>
		/// The time stamp.
		/// </returns>
		private float getTimeStamp()
		{
			_timeStamp += Time.deltaTime;
			return _timeStamp;
		}
		
		/// <summary>
		/// Gets or sets the max lines.
		/// </summary>
		/// <value>
		/// The max lines.
		/// </value>
		public int maxLines
		{
			get
			{
				return _maxLines;
			}
			
			set
			{
				_maxLines = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>
		/// The position.
		/// </value>
		public Rect position
		{
			get
			{
				return _position;
			}
			
			set
			{
				_position = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the color of the GUI.
		/// </summary>
		/// <value>
		/// The color of the GUI.
		/// </value>
		public Color guiColor
		{
			get
			{
				return _guiColor;
			}
	
			set
			{
				_guiColor = value;
			}
		}
	}
	
	/// <summary>
	/// LogItem.
	/// </summary>
	internal class LogItem
	{
		private int _level;
		private string _message;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="LogItem"/> class.
		/// </summary>
		/// <param name='logLevel'>
		/// Log level.
		/// </param>
		/// <param name='logMessage'>
		/// Log message.
		/// </param>
		public LogItem(int logLevel, string logMessage)
		{
			_level = logLevel;
			_message = logMessage;
		}
		
		/// <summary>
		/// Gets the level of the log.
		/// </summary>
		/// <value>
		/// The level.
		/// </value>
		public int level
		{
			get
			{
				return _level;
			}
		}
		
		/// <summary>
		/// Gets the message of the log.
		/// </summary>
		/// <value>
		/// The message.
		/// </value>
		public string message
		{
			get
			{
				return _message;
			}
		}
	}
}