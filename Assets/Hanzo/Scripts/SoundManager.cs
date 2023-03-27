using UnityEngine.Audio;
using UnityEngine;
using System;
using UnityEngine; 
using UnityEngine.SceneManagement;

// [RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	public Sound[] sounds;

	public static SoundManager instance;


	// Use this for initialization
	void Awake () {

		if (instance == null)
			instance = this;
		else {
			Destroy (gameObject);
			return;
		}

		DontDestroyOnLoad (gameObject);

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;

		}

	}

	void Start()
	{
		Play ("Theme");
	}

	// Update is called once per frame
	public void Play ( string name) {

		Sound s =	Array.Find(sounds, sound => sound.name == name);

		s.source.Play();
	}
	void Update()
	{
		if(SceneManager.GetActiveScene().name == "menu")
		{
			Destroy(gameObject);
		}
	}
}
