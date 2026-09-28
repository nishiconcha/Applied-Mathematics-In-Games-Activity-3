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
    private bool sniperFired;
    private bool stopped;

    // Range visual
    private LineRenderer line;

    void Start()
    {
        // Find player to shoot at
        player = GameObject.FindGameObjectWithTag("Player");
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (stopped || player == null)
            return;

        // Aim at player
        AimAtPlayer();

        // Update range visual
        DrawRange();

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

    void AimAtPlayer()
    {
        // Get direction to player
        Vector3 dir = player.transform.position - transform.position;

        // Get the angle and the rotation off the turret
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    // Check if player is inside the range and cone
    bool PlayerInRange()
    {
        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        return distance <= range;
    }

    bool PlayerInCone()
    {
        Vector3 dir = player.transform.position - transform.position;
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
            Shoot(transform.forward);
            timer = 0f;
        }
    }

    void SniperAttack()
    {
        // Fire once when player enters range
        if (PlayerInRange() && !sniperFired)
        {
            Shoot(transform.forward);
            sniperFired = true;
        }

        // Reset after player leaves
        if (!PlayerInRange())
        {
            sniperFired = false;
        }
    }

    void ShotgunAttack()
    {
        // Fire spread 3 bullets
        if (PlayerInCone() && timer >= fireRate)
        {
            Shoot(transform.forward);

            Quaternion left =
                Quaternion.Euler(0f, -15f, 0f);

            Quaternion right =
                Quaternion.Euler(0f, 15f, 0f);

            Shoot(left * transform.forward);
            Shoot(right * transform.forward);

            timer = 0f;
        }
    }

    void Shoot(Vector3 direction)
    {
        // create bullet
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

        line.SetPosition(points + 1, Vector3.zero);
    }

    public void StopTurret()
    {
        // Stop shootingg
        stopped = true;
    }
}