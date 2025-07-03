using UnityEngine;
using UnityEngine.UI;

public class PlayerFallDeath : MonoBehaviour
{
    [Header("Fall Death Settings")]
    [SerializeField] private float deathHeight = -10f;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private AudioClip fallDeathSound;
    private AudioSource audioSource;
    private bool hasDied = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    void Update()
    {
        if (!hasDied && transform.position.y < deathHeight)
        {
            HandleDeath();
        }
    }

    public void ForceDeath()
    {
        if (!hasDied)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        hasDied = true;

        // Play sound
        if (fallDeathSound != null)
        {
            audioSource.PlayOneShot(fallDeathSound);
        }

        // Show Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Stop the game
        Time.timeScale = 0f;

        // Optional: show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public bool IsDead()
    {
        return hasDied;
    }
}
