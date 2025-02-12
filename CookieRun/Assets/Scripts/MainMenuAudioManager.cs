using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public MainMenuManager mainMenuManager;
    //public AudioSource bgmSource;
    private AudioSource sfxSource;
    public AudioClip ButtonClip;
    public AudioClip StartButtonClip;


    private float normalVolume = 1f;

    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        if (mainMenuManager == null)
        {
            mainMenuManager = FindObjectOfType<MainMenuManager>();
        }
    }

    public void PlayerButtonSound()
    {
        sfxSource.PlayOneShot(ButtonClip, normalVolume);
    }

    public void PlayerStartButtonSound()
    {
        sfxSource.PlayOneShot(StartButtonClip, normalVolume);
    }

    //public void PlayerStartButtonSound()
    //{
    //    sfxSource.PlayOneShot(StartButtonClip, normalVolume);
    //    Invoke(nameof(OnSoundEnd), StartButtonClip.length - 0.15f);
    //}
    //
    //private void OnSoundEnd()
    //{
    //    mainMenuManager.ChangeGameScene();
    //}
}
