using UnityEngine;
using System.Collections;

public class CoinUI : MonoBehaviour
{
    private Vector3 originalScale;

    void Start()
    {
        originalScale =
            transform.localScale;
    }

    public void Punch()
    {
        StopAllCoroutines();

        StartCoroutine(PunchAnimation());
    }

    // the animation effect
    IEnumerator PunchAnimation()
    {
        float duration = 0.2f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t =
                timer / duration;

            float scale =
                1f +
                Mathf.Sin(t * Mathf.PI) *
                0.25f;

            transform.localScale =
                originalScale * scale;

            yield return null;
        }

        // Reset coin UI size
        transform.localScale =
            originalScale;
    }
}