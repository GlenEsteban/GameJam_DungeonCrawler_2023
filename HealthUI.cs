using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] Image playerHeart;
    [SerializeField] Image enemyHeart;
    [SerializeField] Image playerHealthBar;
    [SerializeField] Image enemyHealthBar;
    [SerializeField] float iconScaleMultiplier = 1.1f;
    [SerializeField] float healthBarFillSpeed = .1f;
    RectTransform rectTransform;
    Vector3 startingScale;
    void Start() {
        startingScale = playerHeart.rectTransform.localScale;
    }
    public void ScaleHeart(string heart) { // the bool name sucks but who cares
        StartCoroutine(ScaleIcon(heart));
    }
    IEnumerator ScaleIcon(string heart){
        if (playerHeart.enabled == true && heart.Equals("player")) {
            playerHeart.rectTransform.localScale = new Vector3 (playerHeart.rectTransform.localScale.x * iconScaleMultiplier, playerHeart.rectTransform.localScale.y * iconScaleMultiplier, 0);
            yield return new WaitForSeconds(.1f);
            playerHeart.rectTransform.localScale = startingScale;
        }
        else if (enemyHeart.enabled == true && heart.Equals("enemy")){ // if !isPlayer then enemy is assumed
            enemyHeart.rectTransform.localScale = new Vector3 (enemyHeart.rectTransform.localScale.x * iconScaleMultiplier, enemyHeart.rectTransform.localScale.y * iconScaleMultiplier, 0);
            yield return new WaitForSeconds(.1f);
            enemyHeart.rectTransform.localScale = startingScale;
        }
    }

    public void UpdateHealthBar(string healthbar, float fillTarget) {
        StartCoroutine(LerpHealthbarFill(healthbar, fillTarget));
    }

    IEnumerator LerpHealthbarFill(string healthbar, float fillTarget) {
        float time = 0;
        float playerCurrentFill = playerHealthBar.fillAmount;
        float enemyCurrentFill = enemyHealthBar.fillAmount;
        while (time < healthBarFillSpeed) {
            if (healthbar.Equals("player")){
                playerHealthBar.fillAmount = Mathf.Lerp(playerCurrentFill, fillTarget, time / healthBarFillSpeed);
            }
            else if (healthbar.Equals("player"))  {
                enemyHealthBar.fillAmount = Mathf.Lerp(enemyCurrentFill, fillTarget, time / healthBarFillSpeed);
            }
            time += Time.deltaTime;
            yield return null;
        }
            if (healthbar.Equals("player")){
                playerHealthBar.fillAmount = fillTarget;
            }
            else if (healthbar.Equals("enemy")) {
                enemyHealthBar.fillAmount = fillTarget;
            }
    }
}
