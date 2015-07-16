using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public static SoundController Instance;
	public AudioSource soundEffect;
	public AudioSource music;

	void Awake() {
		if (Instance != null && Instance != this) {
			DestroyImmediate(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingle(params AudioClip[] clips){
		RandomizeSoundEffect (clips);
		soundEffect.Play ();
	}

	private void RandomizeSoundEffect(AudioClip[] clips){
		int randomSoundIndex = Random.Range (0, clips.Length);
		soundEffect.clip = clips [randomSoundIndex];

	}
}
