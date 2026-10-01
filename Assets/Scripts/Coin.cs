using UnityEngine;

public class Coin : MonoBehaviour
{
    public float speed = 8f;

    private RectTransform rectTransform;
    private RectTransform target;
    private int value;

    private bool moving;

    public void Setup(
        RectTransform newTarget,
        int newValue,
        Vector2 startPosition
    )
    {
        rectTransform = GetComponent<RectTransform>();

        target = newTarget;
        value = newValue;

        // Set coin starting position
        rectTransform.anchoredPosition = startPosition;

        moving = true;
    }

    void Update()
    {
        if (!moving || target == null)
            return;

        // Move coin to UIs
        rectTransform.anchoredPosition =
            Vector2.Lerp(
                rectTransform.anchoredPosition,
                target.anchoredPosition,
                speed * Time.deltaTime
            );

        float distance =
            Vector2.Distance(
                rectTransform.anchoredPosition,
                target.anchoredPosition
            );

        if (distance <= 5f)
        {
            Collect();
        }
    }

    // Add coins to bank
    void Collect()
    {
        moving = false;

        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(value);
        }

        Destroy(gameObject);
    }
}