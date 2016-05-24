using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour {
	[SerializeField]
	private AudioClip[] audioClips;
	private List<AudioSource> _audioSources = new List<AudioSource>();

	#region unity
	void Start () {
		AudioSource audioSource;
		foreach(AudioClip audioClip in audioClips) {
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.clip = audioClip;
			_audioSources.Add(audioSource);
		}

		if (PlayerPrefs.HasKey("AudioMuted"))
		{
			if (PlayerPrefs.GetInt("AudioMuted", 1).Equals(1))
			{
				unmute();
			}
			else
			{
				mute();
			}
		}
		else
		{
			PlayerPrefs.SetInt("AudioMuted", 1);
			PlayerPrefs.Save();
			unmute();
		}
	}
	#endregion

	#region public
	public void play(string name) {
		foreach (AudioSource audioSource in _audioSources) {
			if (audioSource.clip.name == name) {
				audioSource.Play();
			}
		}
	}

	public void stop(string name) {
		foreach (AudioSource audioSource in _audioSources) {
			if (audioSource.clip.name == name) {
				audioSource.Stop();
			}
		}
	}

	public void stopAll()
	{
		foreach (AudioSource audioSource in _audioSources) {
			audioSource.Stop();
		}
	}

	/// <summary>
	/// Mute all audio sources, which can be controled by AudioListener.
	/// </summary>
	public void mute()
	{
		AudioListener.volume = 0;
		PlayerPrefs.SetInt("AudioMuted", 0);
		PlayerPrefs.Save();
	}

	/// <summary>
	/// Unmute all audio sources, which can be controled by AudioListener.
	/// </summary>
	public void unmute()
	{
		AudioListener.volume = 1;
		PlayerPrefs.SetInt("AudioMuted", 1);
		PlayerPrefs.Save();
	}
	#endregion

	#region getters & setters
	/// <summary>
	/// Gets a value indicating whether AudioListener is muted.
	/// </summary>
	/// <value><c>true</c> if is muted; otherwise, <c>false</c>.</value>
	public bool isMuted
	{
		get { return (AudioListener.volume == 0); }
	}
	#endregion
}