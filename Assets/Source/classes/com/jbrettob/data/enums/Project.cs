/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

namespace com.jbrettob.data.enums
{
	/// <summary>
	/// Project, where you can set the Project name, version and is a live build.
	/// </summary>
	public class Project
	{
		public const string Name = "Smacking Squares";
		public const string Version = "0.00.002";
		public const bool IsLive = false;
		public static bool IsOnlineMultiplayer = true;

		public const string MasterServerIp = "jbrettob.no-ip.org";
		public const int MasterServerPort = 25002;
		public const int GAME_PORT = 25002;
		public const string GameServerName = Project.Name + Project.Version;
	}
}