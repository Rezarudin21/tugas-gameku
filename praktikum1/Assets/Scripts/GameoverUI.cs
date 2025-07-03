using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text scoreText; // Drag ke Text "Score" di Inspector
    public TMP_Text highScoreText; // (Opsional) Jika masih ingin menampilkan High Score

    void Start()
    {
        // Ambil skor terakhir dari PlayerPrefs
        int finalScore = PlayerPrefs.GetInt("LastScore", 0);
        if (scoreText != null)
        {
            scoreText.text = "Score: " + finalScore.ToString();
        }

        // (Opsional) Ambil dan tampilkan High Score juga jika kamu mau
        if (highScoreText != null)
        {
            int highScore = PlayerPrefs.GetInt("HighScore", 0);
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }

    // Fungsi ketika tombol "YES" diklik
    public void OnClickYes()
    {
        SceneManager.LoadScene("GameplayScene"); // Ganti dengan nama scene gameplay-mu
    }

    // Fungsi ketika tombol "NO" diklik
    public void OnClickNo()
    {
        SceneManager.LoadScene("MainMenuScene"); // Ganti dengan nama scene menu utamamu
    }
}