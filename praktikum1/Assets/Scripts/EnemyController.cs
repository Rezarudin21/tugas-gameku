using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TrafficRacer; // Pastikan namespace GameManager dikenali

public class EnemyController : MonoBehaviour
{
    public GameObject gameOverUI; // Drag dari Inspector

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Game Over - Trigger");
            HandleGameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasTriggered && collision.gameObject.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Game Over - Collision");
            HandleGameOver();
        }
    }

   void HandleGameOver()
{
    ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
    if (scoreManager != null && GameManager.singeton != null)
    {
        int finalScore = (int)scoreManager.GetScore();
        GameManager.singeton.SetScore(finalScore);
    }

    SceneManager.LoadScene("GameoverScene"); // Ganti dengan nama scene GameOver kamu
}


}
