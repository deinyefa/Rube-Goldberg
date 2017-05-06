using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    private AudioSource audioSource;

    public AudioClip level_1_To_4_Sound;
    public AudioClip startScreenSound;
    public AudioClip winScreenSound;
	
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        CheckLevel();
	}

    void CheckLevel ()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "StartScreen":
                audioSource.PlayOneShot(startScreenSound, 0.55f);
                break;
            case "Level1":
                audioSource.PlayOneShot(level_1_To_4_Sound, 0.55f);
                break;
            case "Level2":
                audioSource.PlayOneShot(level_1_To_4_Sound, 0.55f);
                break;
            case "Level3":
                audioSource.PlayOneShot(level_1_To_4_Sound, 0.55f);
                break;
            case "Level4":
                audioSource.PlayOneShot(level_1_To_4_Sound, 0.55f);
                break;
            case "WinScreen":
                audioSource.PlayOneShot(winScreenSound, 0.55f);
                break;
        }
    }
}
