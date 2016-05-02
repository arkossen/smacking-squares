using com.jbrettob.debug.fps;
/*
 * Copyright (c) 2013 Jayce Rettob <www.jbrettob.com>
 * 
 * This work is licensed under the Creative Commons Attribution-NonCommercial-NoDerivs 3.0 Unported License.
 * To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-nd/3.0/.
 */

namespace com.jbrettob.core {
    public abstract class CorePage:CoreMonoBehaviour, ICorePage {
        // our default camera
        public UnityEngine.Transform mainCamera;
        //
        private static bool _INITIALIZED;
        private HUDFPS _hudFPS;
        //
        private bool _isStandAlone;
        private StartUp _startUp;
        //
        protected bool _init;
	
        void Start() {
            if (!_INITIALIZED) {
                if (UnityEngine.Application.isEditor) {
                    logDebug("Start() Running inside Unity Editor!");
                }
				
                _INITIALIZED = _isStandAlone = true;
				
                if (debug) {
                    _hudFPS = gameObject.AddComponent<HUDFPS>();
                }
				
                _startUp = new StartUp();
                _startUp.Start(this, onStartUpComplete);
            }
        }
		
        private void onStartUpComplete() {
            init();
        }
	
		#region IAbstractPage implementation
        public virtual void init() {
            // override method
            _init = true;
        }
		
        // This has nothing to do with unity Application.isStandAlone
        public bool isStandAlone() {
            return _isStandAlone;
        }

        public void destructMainCamera() {
            if (mainCamera != null) {
                UnityEngine.GameObject.Destroy(mainCamera.gameObject);
            }
        }
		#endregion
	
        public override void destruct() {
            if (_hudFPS != null) {
                Destroy(_hudFPS);
            }
			
            destructMainCamera();
			
            base.destruct();
        }
		
        void OnDestroy() {
            destruct();
        }
    }
}