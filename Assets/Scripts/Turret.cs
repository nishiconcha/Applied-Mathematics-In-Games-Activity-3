using UnityEngine;

public class Turret : MonoBehaviour
{
    // Three types of turrett
    public enum TurretType
    {
        Flame,
        Sniper,
        Shotgun
    }

    public TurretType turretType;

    public GameObject bulletPrefab;

    public float range = 8f;
    public float fireRate = 0.5f;

    public float coneAngle = 45f;

    private GameObject player;
    private float timer;
    private GameObject sniperTarget;
    private bool stopped;

    // Range visual
    private LineRenderer line;

    void Start()
    {
        // Find player to shoot at
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (stopped)
            return;

        FindTarget();

        // Update range visual
        DrawRange();

        if (player == null)
            return;

        // Aim at player
        AimAtPlayer();

        timer += Time.deltaTime;

        if (turretType == TurretType.Flame)
        {
            FlameAttack();
        }
        else if (turretType == TurretType.Sniper)
        {
            SniperAttack();
        }
        else if (turretType == TurretType.Shotgun)
        {
            ShotgunAttack();
        }
    }

    void FindTarget()
    {
        GameObject[] creatures =
            GameObject.FindGameObjectsWithTag("Creature");

        GameObject closest = null;
        float closestDistance = range;

        foreach (GameObject creature in creatures)
        {
            float distance = Vector3.Distance(
                transform.position,
                creature.transform.position
            );

            if (distance <= closestDistance)
            {
                closest = creature;
                closestDistance = distance;
            }
        }

        player = closest;
    }

    // player is the creature now
    void AimAtPlayer()
    {
        // Get direction to player
        Vector3 dir =
            player.transform.position -
            transform.position;

        dir.y = 0f;

        // Get the angle and the rotation off the turret
        float angle =
            Mathf.Atan2(dir.x, dir.z) *
            Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0f,
                angle,
                0f
            );
    }

    // Check if player is inside the range and cone
    bool PlayerInRange()
    {
        if (player == null)
            return false;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        return distance <= range;
    }

    bool PlayerInCone()
    {
        if (player == null)
            return false;

        Vector3 dir =
            player.transform.position -
            transform.position;

        dir.y = 0f;

        float angle = Vector3.Angle(
            transform.forward,
            dir
        );

        return PlayerInRange() &&
               angle <= coneAngle / 2f;
    }

    void FlameAttack()
    {
        // Fire continuously in cone
        if (PlayerInCone() && timer >= fireRate)
        {
            Vector3 direction =
                player.transform.position -
                transform.position;

            Shoot(direction);

            timer = 0f;
        }
    }

    void SniperAttack()
    {
        // Fire once at each new creature
        if (PlayerInRange() && player != sniperTarget)
        {
            Vector3 direction =
                player.transform.position -
                transform.position;

            Shoot(direction);

            sniperTarget = player;
        }

        // Reset after there are no creatures
        if (player == null)
        {
            sniperTarget = null;
        }
    }

    void ShotgunAttack()
    {
        // Fire spread 3 bullets
        if (PlayerInCone() && timer >= fireRate)
        {
            Vector3 direction =
                player.transform.position -
                transform.position;

            direction.y = 0f;

            Shoot(direction);

            Quaternion left =
                Quaternion.Euler(0f, -15f, 0f);

            Quaternion right =
                Quaternion.Euler(0f, 15f, 0f);

            Shoot(left * direction);
            Shoot(right * direction);

            timer = 0f;
        }
    }

    void Shoot(Vector3 direction)
    {
        // create bullet
        direction.y = 0f;
        direction.Normalize();

        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position + direction,
            Quaternion.identity
        );

        Bullet bulletScript =
            bullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }

    // turret detection area
    void DrawRange()
    {
        if (line == null)
            return;

        if (turretType == TurretType.Sniper)
        {
            DrawLine();
        }
        else
        {
            DrawCone();
        }
    }

    // Draw sniper line
    void DrawLine()
    {
        line.positionCount = 2;
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.forward * range);
    }

    // Draw flame and Shotgun cone
    void DrawCone()
    {
        int points = 20;

        line.positionCount = points + 2;

        line.SetPosition(0, Vector3.zero);

        for (int i = 0; i <= points; i++)
        {
            float angle =
                -coneAngle / 2f +
                (coneAngle / points) * i;

            Quaternion rotation =
                Quaternion.Euler(0f, angle, 0f);

            Vector3 point =
                rotation * Vector3.forward * range;

            line.SetPosition(i + 1, point);
        }

        line.SetPosition(
            points + 1,
            Vector3.zero
        );
    }

    public void StopTurret()
    {
        // Stop shooting
        stopped = true;
    }
}