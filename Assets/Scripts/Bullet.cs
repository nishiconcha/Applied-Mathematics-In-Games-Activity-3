using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float hitDistance = 1f;

    private Vector3 direction;
    private float timer;

    // Set bullet direction
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    // The bullet moves
    void Update()
    {
        transform.position +=
            direction * speed * Time.deltaTime;

        timer += Time.deltaTime;

        // destroys it after some time
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        CheckCreatureHit();
    }

    void CheckCreatureHit()
    {
        // To find the creature or the enemy
        GameObject[] creatures =
            GameObject.FindGameObjectsWithTag("Creature");

        foreach (GameObject creatureObject in creatures)
        {
            float distance = Vector3.Distance(
                transform.position,
                creatureObject.transform.position
            );

            if (distance <= hitDistance)
            {
                Creature creature =
                    creatureObject.GetComponent<Creature>();

                if (creature != null)
                {
                    // Damage is taken
                    creature.TakeDamage(1);
                }

                Destroy(gameObject);

                return;
            }
        }
    }
}