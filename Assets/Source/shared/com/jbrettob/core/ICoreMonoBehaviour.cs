namespace com.jbrettob.core {
	public interface ICoreMonoBehaviour:IDestruct {
		void Awake();
		void Invoke(System.Action callback, float time);
		void InvokeRepeating(System.Action callback, float time, float repeatRate);
		I GetInterfaceComponent<I>() where I:class;
	}
}