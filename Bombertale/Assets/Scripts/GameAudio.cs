using UnityEngine;
using System.Collections;

public class GameAudio : MonoBehaviour
{
    public AudioClip[] music;
    AudioSource[] sounds;
    public AudioSource musicSource;
    public AudioSource bombSound;
    public AudioSource deathSound;
    public AudioSource pickupSound; 
    public AudioClip dogSong;
    void Start()
    {
        float rand = Random.Range(0,1000);
        int index = Random.Range(0, music.Length);
        if(rand <= 1)
        {
            musicSource.clip = dogSong;
        }
        else {
            musicSource.clip = music[index];
        }
        musicSource.Play();
    }

}
