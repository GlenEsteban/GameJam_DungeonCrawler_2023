using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour
{
    AudioSource audioSource;

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayClip(AudioClip clip){
        if (clip == null) { return;}
        audioSource.PlayOneShot(clip);
    }
    
}
