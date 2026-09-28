using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;

    private Vector3 direction;
    private float timer;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Update()
    {
        // Moves bullet
        transform.position +=
            direction * speed * Time.deltaTime;

        timer += Time.deltaTime;

        // Delete after some time
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        CheckPlayer();
    }

    void CheckPlayer()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        // Hits the player
        if (distance < 0.5f)
        {
            GameManager.Instance.PlayerHit();
        }
    }
}