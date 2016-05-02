namespace com.jbrettob.core
{
	public interface ICorePage
	{
		bool isStandAlone();
		void destructMainCamera();
		void init();
	}
}