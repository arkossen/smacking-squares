using com.jbrettob.core;
using UnityEngine;

namespace com.jbrettob.data {
    public abstract class SingletonManager<T>:CoreMonoBehaviour where T:CoreMonoBehaviour {
        protected static T _INSTANCE;
		
        public static T getInstance() {
            if (_INSTANCE == null) {
                _INSTANCE = new GameObject(typeof(T).Name).AddComponent<T>();
            }
			
            return _INSTANCE;
        }
		
        void OnApplicationQuit() {
            _INSTANCE = null;
        }
    }
}