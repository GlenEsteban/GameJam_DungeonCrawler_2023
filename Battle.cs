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
    [SerializeField] AudioClip recoilSFX;
    SFXPlayer sfxPlayer;
    RectTransform rectTransform;
    Vector3 startingScale;
    public bool isBattling;
    public bool isDefending;
    public bool isAttacking;
    public void SetIsBattling(bool state) {
        isBattling = state;
    }

    void Start(){
        rectTransform = GetComponent<RectTransform>();
        startingScale = transform.localScale;
        sfxPlayer = FindObjectOfType<SFXPlayer>();
    }

    public void Defend() {
        if (isShield) {
            StartCoroutine(ScaleIcon());
            if (dotList.Count == 0) {
                sfxPlayer.PlayClip(recoilSFX);
            }
            else {
                sfxPlayer.PlayClip(defendSFX);
                dotList[0].DestroyDot();
            }
        }
    }
    public void Attack() {
        if (isSword) {
            StartCoroutine(ScaleIcon());
            if (dotList.Count == 0) {
                sfxPlayer.PlayClip(recoilSFX);
            }
            else {
                sfxPlayer.PlayClip(attackSFX);
                dotList[0].DestroyDot();
            }
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
