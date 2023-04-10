using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Battle : MonoBehaviour
{
    [SerializeField] float iconScaleMultiplier = 2f;
    [SerializeField] bool isShield;
    [SerializeField] bool isSword;
    [SerializeField] List<Dot> dotList = new List<Dot>();
    [SerializeField] AudioClip defendSFX;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip recoilSFX;
    [SerializeField] Animator animator;
    GameObject player;
    GameObject enemy;
    Health playerHealth;
    Health enemyHealth;
    BattleStats playerBattleStats;
    BattleStats enemyBattleStats;
    int recoilDamage;
    int damageDealt;
    int blocksTillRecovery;
    int recoveryAmount;
    public int comboBlocks;
    public int comboMisses;
    BattleDotSpawner battleDotSpawner;
    SFXPlayer sfxPlayer;
    RectTransform rectTransform;
    HealthUI healthUI;
    Vector3 startingScale;
    bool isBattling;
    bool isLerping;

    void Start() {

        rectTransform = GetComponent<RectTransform>();
        startingScale = transform.localScale;
        sfxPlayer = FindObjectOfType<SFXPlayer>();
        battleDotSpawner = FindObjectOfType<BattleDotSpawner>();
        healthUI = FindObjectOfType<HealthUI>();    

        // Cache player battle info
        player = FindObjectOfType<PlayerController>().gameObject;
        playerHealth = player.GetComponent<Health>();
        playerBattleStats = player.GetComponent<BattleStats>();
        blocksTillRecovery = playerBattleStats.GetBlocksTillRecovery();
        recoveryAmount = playerBattleStats.GetRecoveryAmount();
        recoilDamage = playerBattleStats.GetRecoilDamage();
        damageDealt = playerBattleStats.GetDamageDealt();

        // Cache enemy battle info
        // enemy = player.GetComponent<PlayerController>().GetEnemy().gameObject;
        // enemyHealth = enemy.GetComponent<Health>();
        // enemyBattleStats = enemy.GetComponent<BattleStats>();

        // blocksTillRecovery = playerBattleStats.GetBlocksTillRecovery();
        // recoveryAmount = playerBattleStats.GetRecoveryAmount();
        // recoilDamage = playerBattleStats.GetRecoilDamage();
        // damageDealt = playerBattleStats.GetDamageDealt();
    
    }

    public void StartBattle() {
        if (animator == null) { return; }
        animator.SetBool("isBattling", true);
        battleDotSpawner.SetIsBattling(true);
  
        // reset combos at start of battle
        comboBlocks = 0;
        comboMisses = 0;
    }

    public void EndBattle() {
        animator.SetBool("isBattling", false);
        battleDotSpawner.SetIsBattling(false);
    }

    public void Defend() {
        if (isShield) {
            StartCoroutine(ScaleIcon());
            if (dotList.Count == 0) {
                // Play recoil SFX
                sfxPlayer.PlayClip(recoilSFX);

                // Deal recoil damage and UpdateUI
                playerHealth.TakeDamage(recoilDamage);
                healthUI.ScaleHeart("player");
            }
            else {
                // Play defend SFX
                sfxPlayer.PlayClip(defendSFX);

                dotList[0].DestroyDot();

                // Increment combo hits
                comboBlocks ++;
            }
        }
    }
    public void Attack() {
        if (isSword) {
            StartCoroutine(ScaleIcon());
            if (dotList.Count == 0) {
                // Play recoil SFX
                sfxPlayer.PlayClip(recoilSFX);

                // Deal recoil damage and Update UI
                playerHealth.TakeDamage(recoilDamage);
                healthUI.ScaleHeart("player");

                // Increment combo misses
                comboMisses ++;
                if (comboMisses > blocksTillRecovery) {

                    //regen health enemy
                    comboMisses = 0;
                }
            }
            else {
                // Play attack SFX
                sfxPlayer.PlayClip(attackSFX);

                dotList[0].DestroyDot();

                // Deal damage to enemy
                healthUI.ScaleHeart("enemy");
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

    IEnumerator ScaleIcon() {
        if (!isLerping) {
            isLerping = true;
            rectTransform.localScale = new Vector3 (rectTransform.localScale.x * iconScaleMultiplier, rectTransform.localScale.y * iconScaleMultiplier, 0);
            yield return new WaitForSeconds(.1f);
            rectTransform.localScale = startingScale;
            isLerping = false;
        }
        else {
            yield return null;
        }
    }
}
