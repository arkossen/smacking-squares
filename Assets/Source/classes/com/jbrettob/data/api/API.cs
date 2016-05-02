/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */
using com.jbrettob.core;

namespace com.jbrettob.data.api
{
	/// <summary>
	/// Dummy API.
	/// </summary>
	public class API:CoreObject, IAPI
	{
		public void init()
		{
			logDebug("API.init()");
		}
		
		public override void destruct()
		{
			logDebug("API.destruct()");
		}
	}
}