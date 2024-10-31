using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;
    [SerializeField] private AudioSource echo;

    [SerializeField] private AudioClip homeMusicClip;
    [SerializeField] private AudioClip battleMusicClip;
    [SerializeField] private AudioClip lazerSFXChip;
    [SerializeField] private AudioClip plasmaSFXChip;
    [SerializeField] private AudioClip hitSFXClip;
    [SerializeField] private AudioClip explosionSFXClip;

    public void PlayHomeMusic()
    {
        if (music.clip == homeMusicClip)
            return;
        music.loop = true;
        music.clip = homeMusicClip;
        music.Play();
    }

    public void PlayBattleMusic()
    {
        if (music.clip == battleMusicClip)
            return;
        music.loop = true;
        music.clip = battleMusicClip;
        music.Play();
    }

    public void PlayLaserSFX()
    {
        sfx.pitch = Random.Range(1f, 2f);
        sfx.PlayOneShot(lazerSFXChip);
    }
    public void PlayPlasmaSFX()
    {
        sfx.pitch = Random.Range(1f, 2f);
        sfx.PlayOneShot(plasmaSFXChip);
    }
    public void PlayHitSFX()
    {
        sfx.pitch = Random.Range(1f, 2f);
        sfx.PlayOneShot(hitSFXClip);
    }
    public void PlayExplosionSFX()
    {
        echo.pitch = Random.Range(1f, 2f);
        echo.PlayOneShot(explosionSFXClip);
    }
    

}
