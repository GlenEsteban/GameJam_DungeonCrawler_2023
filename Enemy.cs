using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Enemy : MonoBehaviour
{

    [SerializeField] float radiusLookTrigger = 5f;
    [SerializeField] float encounterOffset = 5f;
    [SerializeField] float encounterDuration = 1f;
    [SerializeField] GameObject enemyMesh;
    Grid grid;
    GameObject enemy;
    GameObject player;
    Vector3 startingPosition;
    bool isHidden;
    bool isLerping;
    bool runOnce;

    void Awake() {
        startingPosition = this.transform.position;
        player = FindObjectOfType<PlayerController>().gameObject;
        enemy = this.transform.GetChild(0).gameObject;

        Instantiate(enemyMesh, transform.position, Quaternion.identity, transform.GetChild(0));
        CheckIfHidden();
    }

    void Update() {

        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        float distanceToPlayer = (player.transform.position - this.transform.position).magnitude;
        if (distanceToPlayer < radiusLookTrigger)
        {
            transform.LookAt(player.transform);
        }
    }

    void CheckIfHidden(){
        float random = Random.Range(0,100);
        isHidden = random < 50;
        if (isHidden) {
            enemy.SetActive(false);
        }
        else {
            enemy.SetActive(true);
        }
    }

    public void EncounterEnemy(){
        if (isHidden) {
            this.transform.position = new Vector3(transform.position.x, transform.position.y + encounterOffset, transform.position.z);
            this.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(LerpPosition(startingPosition, encounterDuration));
        }

        Debug.Log("BATTLE!!");
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        isLerping = true;
        float time = 0;
        Vector3 startPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isLerping = false;
    }
}
