using UnityEngine;
using com.jbrettob.core;

public class UIScreenManager:CoreMonoBehaviour
{
	public void Navigate(string screenName)
	{
		Transform go = transform.Find(screenName);

		if (go.Equals(null)) {
			logError("Navigate() screenName: " + screenName + " does not exist! Please check your code + gameObject.name");
			return;
		}
		CoreScreen currentScreen;

		foreach (Transform to in transform)
		{
			currentScreen = to.GetComponent<CoreScreen>();

			if (currentScreen != null)
			{
				currentScreen.onDeactivated();
			}

			NGUITools.SetActive(to.gameObject, false);
		}

		currentScreen = go.GetComponent<CoreScreen>();

		if (currentScreen != null)
		{
			currentScreen.onActivated();
		}

		NGUITools.SetActive(go.gameObject, true);
	}
}