using com.jbrettob.core;

public class Player:CoreObject {
	#region properties
	public UnityEngine.Transform transform;
	public int id;
	public string username;
	public int x;
	public int y;
	public int z;
	#endregion

	#region Public method
	public Player() {
		id = -1;
		username = "guest";
		x = 0;
		y = 0;
		z = 0;
	}
	#endregion
}