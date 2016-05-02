using UnityEngine;
using com.jbrettob.core;

public class CoreScreen:CoreMonoBehaviour
{
	private UIScreenManager _manager;

	public override void Awake()
	{
		base.Awake();
		init();
	}

	private void init()
	{
		if (_manager != null)
		{
			return;
		}

		_manager = transform.parent.GetComponent<UIScreenManager>();
	}

	public virtual void onActivated()
	{

	}

	public virtual void onDeactivated()
	{

	}

	public void Navigate(string screenName)
	{
		_manager.Navigate(screenName);
	}
}