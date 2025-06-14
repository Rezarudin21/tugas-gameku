using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
    public GameObject gameOverPanel;
    public Button tryAgainButton;

    void Start()
    {
        gameOverPanel.SetActive(false);
        tryAgainButton.onClick.AddListener(RestartLevel);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
