using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonFX : MonoBehaviour
{

    public AudioSource SFX;
    public AudioClip HighlightFx;

    public void HighlightSound()
    {
        SFX.PlayOneShot(HighlightFx);
    }

}