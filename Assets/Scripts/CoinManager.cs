using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("Coin")]
    public GameObject coinPrefab;
    public RectTransform coinTarget;
    public Canvas canvas;

    [Header("Bank")]
    public int coins = 100;
    public TMP_Text coinText;
    public CoinUI coinUI;

    [Header("Coin Value")]
    public int coinValue = 10;

    void Awake()
    {
        Instance = this;

        UpdateCoinText();
    }

    public void SpawnCoin(Vector3 worldPosition)
    {
        if (coinPrefab == null)
            return;

        if (canvas == null)
            return;
        
        // Convert world position to UI position
        Vector2 screenPosition =
            Camera.main.WorldToScreenPoint(
                worldPosition
            );

        RectTransform canvasRect =
            canvas.GetComponent<RectTransform>();

        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            null,
            out localPosition
        );

        // creates the coin
        GameObject coin =
            Instantiate(
                coinPrefab,
                canvasRect
            );

        Coin coinScript =
            coin.GetComponent<Coin>();

        if (coinScript != null)
        {
            coinScript.Setup(
                coinTarget,
                coinValue,
                localPosition
            );
        }
    }

    // this animates and add the coin
    public void AddCoins(int amount)
    {
        coins += amount;

        UpdateCoinText();

        if (coinUI != null)
        {
            coinUI.Punch();
        }
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text =
                coins.ToString();
        }
    }
}