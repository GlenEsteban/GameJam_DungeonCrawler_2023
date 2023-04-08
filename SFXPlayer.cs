using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;

    public void PlayClip(AudioClip clip){
        audioSource.PlayOneShot(clip);
    }
    
}
