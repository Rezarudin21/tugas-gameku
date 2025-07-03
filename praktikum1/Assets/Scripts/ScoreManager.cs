using UnityEngine;

public class ScoreManager : MonoBehaviour
{
public TMPro.TMP_Text scoreText; // Untuk menampilkan skor saat ini
public TMPro.TMP_Text highScoreText; // Untuk menampilkan high score
private float score = 0f;
private bool isGameOver = false;
private int highScore;

void Start()
{
    // Inisialisasi High Score default ke 100 jika belum ada
    if (!PlayerPrefs.HasKey("HighScore"))
    {
        PlayerPrefs.SetInt("HighScore", 100);
        PlayerPrefs.Save();
        Debug.Log("High Score default diset ke 100");
    }

    highScore = PlayerPrefs.GetInt("HighScore");
    UpdateHighScoreUI();
}

void Update()
{
    if (isGameOver) return;

    score += Time.deltaTime * 5;
    int intScore = Mathf.FloorToInt(score);

    if (scoreText != null)
        scoreText.text = "Score: " + intScore.ToString();

    // Selalu update high score jika score baru lebih tinggi
    if (intScore > highScore)
    {
        highScore = intScore;
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();

        UpdateHighScoreUI();
        Debug.Log("High Score baru otomatis disimpan: " + highScore);
    }
}

public float GetScore()
{
    return score;
}

public void ResetHighScore()
{
    PlayerPrefs.DeleteKey("HighScore");
    PlayerPrefs.Save();

    highScore = 0;
    UpdateHighScoreUI();
    Debug.Log("High Score telah di-reset.");
}

private void UpdateHighScoreUI()
{
    if (highScoreText != null)
        highScoreText.text = "High Score: " + highScore.ToString();
}

public void StopGame()
{
    isGameOver = true;

    // Simpan skor terakhir yang diperoleh player
    PlayerPrefs.SetInt("LastScore", Mathf.FloorToInt(score));
    PlayerPrefs.Save();
    Debug.Log("LastScore disimpan: " + Mathf.FloorToInt(score));

    // Kirim ke GameManager (jika ada)
    if (TrafficRacer.GameManager.singeton != null)
    {
        TrafficRacer.GameManager.singeton.SetScore(Mathf.FloorToInt(score));
    }
}
}