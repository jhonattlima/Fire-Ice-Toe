using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioClip chosen_clip = null;

    public static MusicPlayer instance;
    public AudioClip board;
    public AudioClip draw;
    public AudioClip mainMenu;
    public AudioClip lose;
    public AudioClip win;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(string clip)
    {
        switch (clip)
        {
            case "musicBoard":
                chosen_clip = board;
                break;
            case "musicMainMenu":
                chosen_clip = mainMenu;
                break;
            case "musicDraw":
                chosen_clip = draw;
                break;
            case "musicLoser":
                chosen_clip = lose;
                break;
            case "musicWinner":
                chosen_clip = win;
                break;
        }
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(chosen_clip);
    }
}
