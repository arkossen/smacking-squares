using UnityEngine;
using System.Collections;
using com.jbrettob.core;
using System;

public class CountdownTimer:CoreMonoBehaviour {
	public UISprite TimerSprite;
	//
	private AudioPlayer audioPlayer;
	private Action onFinished;
	//
	private int time = 10;
	private const int BASE_TIME = 10;

	public override void Awake() {
		base.Awake();
		audioPlayer = UnityEngine.Object.FindObjectOfType<AudioPlayer>();

		StartTimer();
	}

	#region public methods
	public void StartTimer() {
		StartTimer(null);
	}

	public void StartTimer(Action callback) {
		StartCoroutine(UpdateTimer());

		onFinished = callback;
	}

	public void stopTimer() {
		StopCoroutine("UpdateTimer");
	}
	#endregion

	#region private methods
	private IEnumerator UpdateTimer() {
		time = BASE_TIME;
		TimerSprite.gameObject.SetActive(true);
		
		while (true) {
			TimerSprite.spriteName = "timer-" + time;
			if (audioPlayer != null) {
				audioPlayer.play(AudioNames.CLOCK);
			}
			
			yield return new WaitForSeconds(1);
			time--;
			
			if (time < 0) {
				StopCoroutine("updateTimer");
				if (onFinished != null) {
					onFinished();
				}
			}
		}
	}
	#endregion
}
