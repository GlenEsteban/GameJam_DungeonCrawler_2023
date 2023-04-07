using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] float delayDotSpawn;
    [SerializeField] Image shieldIcon;
    [SerializeField] Image swordIcon; 
    [SerializeField] GameObject dot;
    [SerializeField] GameObject shieldDotSpawn;
    [SerializeField] GameObject swordDotSpawn;
    [SerializeField] AudioClip battleStartSFX;
    float time;
    float timeSinceLastDot;
    float timeBetweenDotSpawn;

    void Awake() {
        time = 0;
        timeSinceLastDot = 0;
        timeBetweenDotSpawn = 0;
    }
    void Update() {
        time += Time.deltaTime;
        if (time < delayDotSpawn) {return;}

        timeSinceLastDot += Time.deltaTime;
        if (timeSinceLastDot > timeBetweenDotSpawn) {
            StartCoroutine(SpawnDots(shieldDotSpawn));
            StartCoroutine(SpawnDots(swordDotSpawn));
            timeSinceLastDot = 0;
        }
    }

    IEnumerator SpawnDots(GameObject spawnPoint){
        yield return new WaitForSeconds(timeBetweenDotSpawn);
        timeBetweenDotSpawn = Random.Range(.1f, 3f);
        Instantiate(dot, spawnPoint.transform.position, Quaternion.identity, spawnPoint.transform);
    }
}
