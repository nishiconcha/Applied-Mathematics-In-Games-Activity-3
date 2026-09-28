using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;

    void Awake()
    {
        Instance = this;

        // hide win panel
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void PlayerHit()
    {
        // Restart
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void WinGame()
    {
        // win panel
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }
}