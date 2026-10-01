using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum PathType
    {
        Quadratic,
        Cubic
    }

    [Header("Spawn Settings")]
    public PathType pathType;
    public GameObject creaturePrefab;
    public Transform spawnPoint;
    public Transform target;
    public float spawnInterval = 3f;

    [Header("Quadratic Control")]
    public Transform quadraticControl;

    [Header("Cubic Controls")]
    public Transform cubicControl1;
    public Transform cubicControl2;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnCreature();
        }
    }

    // spawning
    void SpawnCreature()
    {
        GameObject obj = Instantiate(
            creaturePrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Creature creature =
            obj.GetComponent<Creature>();

        if (creature == null)
            return;

        // Set quadratic path
        if (pathType == PathType.Quadratic)
        {
            creature.SetQuadraticPath(
                spawnPoint.position,
                quadraticControl.position,
                target.position
            );
        }
        // Set cubic path
        else if (pathType == PathType.Cubic)
        {
            creature.SetCubicPath(
                spawnPoint.position,
                cubicControl1.position,
                cubicControl2.position,
                target.position
            );
        }
    }

    // To see the path visually
    void OnDrawGizmos()
    {
        if (spawnPoint == null || target == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 previousPoint = spawnPoint.position;

        for (int i = 1; i <= 30; i++)
        {
            float t = i / 30f;

            Vector3 currentPoint;

            if (pathType == PathType.Quadratic)
            {
                float u = 1f - t;

                currentPoint =
                    u * u * spawnPoint.position +
                    2f * u * t * quadraticControl.position +
                    t * t * target.position;
            }
            else
            {
                if (cubicControl1 == null || cubicControl2 == null)
                    return;

                float u = 1f - t;

                currentPoint =
                    u * u * u * spawnPoint.position +
                    3f * u * u * t * cubicControl1.position +
                    3f * u * t * t * cubicControl2.position +
                    t * t * t * target.position;
            }

            Gizmos.DrawLine(previousPoint, currentPoint);

            previousPoint = currentPoint;
        }
    }
}