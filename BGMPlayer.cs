using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BGMPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] BattlingBGM;
    [SerializeField] AudioClip ExploringBGM;
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = ExploringBGM;
        audioSource.Play();
    }
    public void PlayBattleBGM() {
        audioSource.Stop();
        audioSource.clip = BattlingBGM[Random.Range(0, BattlingBGM.Length)];
        audioSource.Play();
    }
}
