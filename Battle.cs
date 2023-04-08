using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Battle : MonoBehaviour
{
    [SerializeField] bool isShield;
    [SerializeField] bool isSword;
    [SerializeField] List<Dot> dotList = new List<Dot>();
    [SerializeField] float iconScaleMultiplier = 2f;
    [SerializeField] AudioClip defendSFX;
    [SerializeField] AudioClip attackSFX;
    SFXPlayer sfxPlayer;
    RectTransform rectTransform;
    Vector3 startingScale;
 
    public bool isDefending;
    public bool isAttacking;
    public void SetIsDefending(bool state) {
        isDefending = state;
    }
    public void SetIsAttacking(bool state) {
        isAttacking = state;
    }

    void Start(){
        rectTransform = GetComponent<RectTransform>();
        startingScale = transform.localScale;
        sfxPlayer = FindObjectOfType<SFXPlayer>();
    }

    public void Defend() {
        if (isShield) {
            sfxPlayer.PlayClip(defendSFX);
            StartCoroutine(ScaleIcon());
            if (dotList.Count == 0) { return; }
            dotList[0].DestroyDot();
            Debug.Log("Defend!");
        }
    }
    public void Attack() {
        if (isSword) {
            sfxPlayer.PlayClip(defendSFX);
            StartCoroutine(ScaleIcon());
            if (dotList.Count == 0) { return; }
            dotList[0].DestroyDot();
            Debug.Log("Attack!");
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // add collider to list if dot
        Dot dot = other.GetComponent<Dot>();
        if (dot == null) { return; }
        dotList.Add(dot);
    }
    void OnTriggerExit2D(Collider2D other) {
        // remove collider from list if is in list
        Dot dot = other.GetComponent<Dot>();
        if (dot == null) { return; }
        if (dotList.Contains(dot)) { 
            dotList.Remove(dot);
        }
    }

    IEnumerator ScaleIcon(){
        rectTransform.localScale = new Vector3 (rectTransform.localScale.x * iconScaleMultiplier, rectTransform.localScale.y * iconScaleMultiplier, 0);
        yield return new WaitForSeconds(.1f);
        rectTransform.localScale = startingScale;
    }
}
