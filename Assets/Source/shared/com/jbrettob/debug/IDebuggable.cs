/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

namespace com.jbrettob.debug
{
	/// <summary>
	/// Interface debuggable.
	/// </summary>
	public interface IDebuggable
	{
		bool debug { get; set; }
		
		void logDebug(string message);
		
		void logWarning(string message);
		
		void logError(string message);
	}
}