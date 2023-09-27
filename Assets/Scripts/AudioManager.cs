using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;

    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = mixerGroup;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void StopPlaying (string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
		Debug.LogWarning("Sound: " + name + " not found!");
		return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Stop ();
 	}

	public void PitchUpTwo(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		
		if(s.source.pitch < 2f){
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f) + 0.5f);
		}
    }

	public void AudioSuperSpeed(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.pitch = 1.5f;
    }

	public void AudioSpeed(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.pitch = 1.25f;
    }

	public void AudioNormal(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.pitch = 1;
    }

	public void lowVolume (string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) - 0.020f;
	}

	public void normalVolume (string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f)) + 0.020f;
	}

    public void StopFadeOut (string sound, float FadeTime)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
		Debug.LogWarning("Sound: " + name + " not found!");
		return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
	
		StartCoroutine (FadeOut(s,FadeTime));


		}
	

	IEnumerator FadeOut (Sound fs ,float FadeTime) {
			float startVolume = fs.source.volume;
	
			while (fs.source.volume > 0.001) {
				fs.source.volume -= startVolume * Time.deltaTime / FadeTime;
	
			yield return null;
        }
		fs.source.Stop ();
	}

	public void PlayFadeIn (string sound, float FadeTime)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
		Debug.LogWarning("Sound: " + name + " not found!");
		return;
		}
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
	
		StartCoroutine (FadeIn(s, FadeTime));


	}

	IEnumerator FadeIn (Sound fs ,float FadeTime) {
		float target = fs.source.volume;
		fs.source.volume = 0f;
		fs.source.Play ();
			while (fs.source.volume < target) {
				fs.source.volume += Time.deltaTime * FadeTime;
	
			yield return null;
        }
		
	}

    public void Pause(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Pause();
	}

    public void pauseAll()
	{
		foreach (Sound s in sounds)
		{	if(s.name!="OnClickMenu" && s.name!="OnQuitGame" && s.name!="OnBackMenu" && s.source.isPlaying){
				s.source.volume = (s.source.volume/100)*30;
			}
		}
	}

	public void stopPauseAll()
	{
		foreach(Sound s in sounds){
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
			s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
		}
	}
}
