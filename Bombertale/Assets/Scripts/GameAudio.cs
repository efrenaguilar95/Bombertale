using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (SceneManager.GetActiveScene().name == "Main")
        {
            float rand = Random.Range(0, 1000);
            int index = Random.Range(0, music.Length);
            if (rand <= 1)
            {
                musicSource.clip = dogSong;
            }
            else
            {
                musicSource.clip = music[index];
            }
            musicSource.Play();
        }
    }

    public void SelectMusic(int musicIndex)
    {
        if(musicIndex == 9)
        {
            musicSource.clip = dogSong;
        }
        else
        {
            musicSource.clip = music[musicIndex];
        }
        musicSource.Play();
    }
}
