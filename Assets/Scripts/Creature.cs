using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header("Movement")]
    public float timeToReachTarget = 3f;

    [Header("Health")]
    public int health = 1;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private float totalTime;
    private bool isCubic;
    private bool dead;

    // Set quadratic path
    public void SetQuadraticPath(
        Vector3 start,
        Vector3 control,
        Vector3 end
    )
    {
        isCubic = false;

        p0 = start;
        p1 = control;
        p2 = end;

        totalTime = 0f;
        transform.position = start;
    }

    // Set cubic path
    public void SetCubicPath(
        Vector3 start,
        Vector3 control1,
        Vector3 control2,
        Vector3 end
    )
    {
        isCubic = true;

        p0 = start;
        p1 = control1;
        p2 = control2;
        p3 = end;

        totalTime = 0f;
        transform.position = start;
    }

    // Move along the path
    void Update()
    {
        if (dead)
            return;

        totalTime += Time.deltaTime;

        float t =
            Mathf.Clamp01(
                totalTime / timeToReachTarget
            );

        transform.position = GetPosition(t);

        if (t >= 1f)
        {
            ReachTarget();
        }
    }

    Vector3 GetPosition(float t)
    {
        if (!isCubic)
        {
            float u = 1f - t;

            return
                u * u * p0 +
                2f * u * t * p1 +
                t * t * p2;
        }

        float cubicU = 1f - t;

        return
            cubicU * cubicU * cubicU * p0 +
            3f * cubicU * cubicU * t * p1 +
            3f * cubicU * t * t * p2 +
            t * t * t * p3;
    }

    void ReachTarget()
    {
        dead = true;

        // Damage player
        if (GameManager.Instance != null)
        {
            GameManager.Instance.CreatureReachedTarget();
        }

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (dead)
            return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        dead = true;

        // Spawn coin
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.SpawnCoin(
                transform.position
            );
        }

        Destroy(gameObject);
    }
}