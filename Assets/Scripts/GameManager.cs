using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player HP")]
    public int maxHP = 20;
    public int currentHP = 20;

    [Header("HP UI")]
    public Image realHPBar;
    public Image ghostHPBar;

    public TMP_Text hpText;

    private float ghostDelay = 0.5f;
    private float ghostTimer;

    private bool gameOver;

    void Update()
    {
        UpdateRealHP();
        UpdateGhostHP();
    }

    public void CreatureReachedTarget()
    {
        if (gameOver)
            return;

        currentHP--;

        if (currentHP <= 0)
        {
            currentHP = 0;
        }

        ghostTimer = 0f;
    }

    void UpdateRealHP()
    {
        if (realHPBar != null)
        {
            realHPBar.fillAmount =
                (float)currentHP / maxHP;
        }

        if (hpText != null)
        {
            hpText.text =
                "HP: " +
                currentHP +
                " / " +
                maxHP;
        }
    }

    void UpdateGhostHP()
    {
        if (ghostHPBar == null)
            return;

        float target =
            (float)currentHP / maxHP;

        if (ghostHPBar.fillAmount <= target)
        {
            ghostHPBar.fillAmount = target;
            return;
        }

        ghostTimer += Time.deltaTime;

        if (ghostTimer < ghostDelay)
            return;

        float current =
            ghostHPBar.fillAmount;

        float newValue =
            Mathf.Lerp(
                current,
                target,
                5f * Time.deltaTime
            );

        ghostHPBar.fillAmount =
            Mathf.Clamp01(newValue);
    }
}