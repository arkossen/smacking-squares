/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */
using com.jbrettob.data.api;
using com.jbrettob.gamenetwork;

namespace com.jbrettob.data {
	/// <summary>
	/// DataManager as a singleton.
	/// </summary>
	public class DataManager {
		#region properties
		private static DataManager _INSTANCE;
		public IAPI api;
		public StateManager StateManager;
		public ClientLogic ClientLogic;
		public ServerLogic ServerLogic;
		#endregion
		
		/// <summary>
		/// Gets the instance of <see cref="DataManager"/> class.
		/// </summary>
		/// <returns>
		/// The instance.
		/// </returns>
		public static DataManager GetInstance() {
			if(DataManager._INSTANCE == null) {
				DataManager._INSTANCE = new DataManager();
				DataManager._INSTANCE.StateManager = new StateManager();
			}
			
			return DataManager._INSTANCE;
		}
	}
}