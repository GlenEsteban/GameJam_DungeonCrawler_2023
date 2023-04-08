using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] float yPosOffset = -100f;
    RectTransform rectTransform;
    BattleDotSpawner dotSpawner;
    Rigidbody2D rb;
    float dotMoveSpeed;

    private void OnDisable() {
        Destroy(gameObject);
    }

    void Start() {
        dotSpawner = FindObjectOfType<BattleDotSpawner>();
        dotMoveSpeed = 1 / dotSpawner.GetDotMoveSpeed();
        rectTransform = GetComponent<RectTransform>();
        rb = GetComponent<Rigidbody2D>();
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + yPosOffset, transform.position.z);
        StartCoroutine(LerpPosition(targetPosition, 5f * dotMoveSpeed));
    }
    
    public void DestroyDot(){
        //Play sfx

        rb.isKinematic = false;
        rb.velocity = new Vector2 (Random.Range(1,10), Random.Range(1,10));
        
        Destroy(gameObject);
    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration) {
        float time = 0;
        Vector3 startPosition = rectTransform.position;
        while (time < duration)
        {
            rectTransform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
