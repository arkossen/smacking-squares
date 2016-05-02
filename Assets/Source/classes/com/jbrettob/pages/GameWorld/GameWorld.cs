using com.jbrettob.core;
using com.jbrettob.data;
using com.jbrettob.data.enums;
using UnityEngine;

namespace com.jbrettob.pages
{
    [AddComponentMenu("")]
    public class GameWorld:CorePage
	{
        public override void init()
		{
            base.init();
        
            if (isStandAlone())
			{
                // Do standalone stuff, like insert DataManager data
                DataManager.GetInstance().api.init();
            }

			UnityNetworkManager.getInstance().init(Project.Name, Project.Version, Project.GAME_PORT, false, Project.MasterServerIp, Project.MasterServerPort);
        }
		
        void Update()
		{
            if (Input.GetKeyDown(KeyCode.Space))
			{
                Application.LoadLevel(WorldNames.HOME_WORLD);
            }
        }
    }
}