using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    private AudioSource _audioSource;
    private AudioClip chosen_clip = null;

    public static SFXPlayer instance;
    public AudioClip burn;
    public AudioClip freeze;
    public AudioClip buttonConfirmation;
    public AudioClip buttonHighlight;

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
            case "burn":
                chosen_clip = burn;
                break;
            case "freeze":
                chosen_clip = freeze;
                break;
            case "buttonConfirmation":
                chosen_clip = buttonConfirmation;
                break;
            case "buttonHighlight":
                chosen_clip = buttonHighlight;
                break;
        }
        _audioSource.pitch = Random.Range(0.9f, 1.1f);
        _audioSource.PlayOneShot(chosen_clip);
    }
}
