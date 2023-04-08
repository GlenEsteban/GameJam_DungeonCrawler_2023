using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] [Range(0,4)] float dotSpeedMultiplier = 1f;
    [SerializeField] float delayDotSpawn;
    [SerializeField] Image shieldIcon;
    [SerializeField] Image swordIcon; 
    [SerializeField] GameObject dot;
    [SerializeField] GameObject shieldDotSpawn;
    [SerializeField] GameObject swordDotSpawn;

    float time;
    float timeSinceShieldDot;
    float timeSinceSwordDot;
    float timeBetweenShieldDot;
    float timeBetweenSwordDot;

    public float GetDotMoveSpeed(){
        return dotSpeedMultiplier;
    }

    void Awake() {
        time = 0;
        timeSinceShieldDot = 0;
        timeBetweenShieldDot = 0;
        timeSinceSwordDot = 0;
        timeBetweenSwordDot = 0;
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time < delayDotSpawn) { return; }

        SpawnShieldDots();
        SpawnSwordDots();
    }

    void SpawnShieldDots() {
        timeSinceShieldDot += Time.deltaTime;
        if (timeSinceShieldDot > timeBetweenShieldDot) {
            timeSinceShieldDot = 0;
            timeBetweenShieldDot = Random.Range(.3f, 2f);
            Instantiate(dot, shieldDotSpawn.transform.position, Quaternion.identity, shieldDotSpawn.transform);
        }
    }

    void SpawnSwordDots() {
        timeSinceSwordDot += Time.deltaTime;
        if (timeSinceSwordDot > timeBetweenSwordDot) {
            timeSinceSwordDot = 0;
            timeBetweenSwordDot = Random.Range(.3f, 2f);
            Instantiate(dot, swordDotSpawn.transform.position, Quaternion.identity, swordDotSpawn.transform);
        }
    }
}
