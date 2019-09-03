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
            case "board":
                chosen_clip = board;
                break;
            case "draw":
                chosen_clip = draw;
                break;
            case "mainMenu":
                chosen_clip = mainMenu;
                break;
            case "lose":
                chosen_clip = lose;
                break;
            case "win":
                chosen_clip = win;
                break;
        }
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(chosen_clip);
    }
}
