using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource bgmSource;
    private AudioSource sfxSource;
    public AudioClip jumpClip;
    public AudioClip slideClip;
    public AudioClip scoreBoardClip;
    public AudioClip jellyEatClip;
    public AudioClip potionEatClip;
    public AudioClip itemEatClip;
    public AudioClip ButtonSoundClip;
    public AudioClip CoinClip;
    public AudioClip hitClip;

    private float normalVolume = 1f;

    private Dictionary<AudioClip, float> lastPlayTimes = new Dictionary<AudioClip, float>();

    private void Start()
    {
        sfxSource = GetComponent<AudioSource>();
        if (bgmSource != null)
        {
            bgmSource.loop = true;  // BGM 무한 반복
            bgmSource.Play();
        }
    }

    public void PlayerCoinSound()
    {
        PlaySoundWithCooldown(CoinClip, 0.8f, 0.4f);
    }

    public void PlayerhitSound()
    {
        PlaySoundWithCooldown(hitClip, 0.8f, 0.5f);
    }

    public void PlayerJumpSound()
    {
        sfxSource.PlayOneShot(jumpClip, normalVolume);
    }

    public void PlayerSlideSound()
    {
        PlaySoundWithCooldown(slideClip, 0.8f, 1f);
    }

    public void PlayerScoreBoardSound()
    {
        sfxSource.PlayOneShot(scoreBoardClip, normalVolume);
    }

    public void PlayerJellySound()
    {
        PlaySoundWithCooldown(jellyEatClip, 0.8f, 0.4f); // 40% 지나면 다시 재생 가능
    }

    public void PlayerPotionSound()
    {
        sfxSource.PlayOneShot(potionEatClip, normalVolume);
    }

    public void PlayerItemClipSound()
    {
        sfxSource.PlayOneShot(itemEatClip, normalVolume);
    }

    public void PlayerButtonClipSound()
    {
        sfxSource.PlayOneShot(ButtonSoundClip, normalVolume);
    }

    private void PlaySoundWithCooldown(AudioClip clip, float volume, float percent = 1f)
    {
        if (clip == null) return;

        float clipLength = clip.length * percent; // 특정 비율(예: 0.67) 만큼만 기다림

        if (!lastPlayTimes.ContainsKey(clip) || Time.time - lastPlayTimes[clip] >= clipLength)
        {
            sfxSource.PlayOneShot(clip, volume);
            lastPlayTimes[clip] = Time.time;  // 해당 사운드의 마지막 재생 시간 업데이트
        }
    }

}
