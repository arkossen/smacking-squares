/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

using com.jbrettob.debug;
using UnityEngine;

namespace com.jbrettob.core
{
	/// <summary>
	/// Core object, which also implements <see cref="IDebuggable"/> class for debug functionalities.
	/// </summary>
	public abstract class CoreObject:IDebuggable, IDestruct
	{
		private bool _debug;
		
		#region IDebuggable implementation
		public void logDebug(string message)
		{
			if (DebugLoggerManager.USE_LOG)
			{
				Debug.Log(DebugLoggerManager.getInstance.logDebug("[" + this + "] " + message));
			}
			else
			{
				Debug.Log("[" + this + "] " + message);
			}
		}
		
		public void logWarning(string message)
		{
			if (DebugLoggerManager.USE_LOG)
			{
				Debug.LogWarning(DebugLoggerManager.getInstance.logWarning("[" + this + "] " + message));
			}
			else
			{
				Debug.LogWarning("[" + this + "] " + message);
			}
		}
		
		public void logError(string message)
		{
			if (DebugLoggerManager.USE_LOG)
			{
				Debug.LogError(DebugLoggerManager.getInstance.logError("[" + this + "] " + message));
			}
			else
			{
				Debug.LogError("[" + this + "] " + message);
			}
		}
	
		public bool debug
		{
			get
			{
				return _debug;
			}
			set
			{
				_debug = value;
			}
		}
		#endregion

		#region IDestruct implementation
		public virtual void destruct()
		{
			// override method
		}
		#endregion
	}
}