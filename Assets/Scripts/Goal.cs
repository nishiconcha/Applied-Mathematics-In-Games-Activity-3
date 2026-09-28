using UnityEngine;

public class Goal : MonoBehaviour
{
    public float goalRange = 1f;

    void Update()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position
        );

        // Player reached goal
        if (distance <= goalRange)
        {
            GameManager.Instance.WinGame();
        }
    }
}