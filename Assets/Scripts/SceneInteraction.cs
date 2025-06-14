using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInteraction : MonoBehaviour
{
    public string targetSceneName = "shooting-games"; // Nama scene tujuan
    public KeyCode interactionKey = KeyCode.F;      // Tombol interaksi
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            SceneManager.LoadScene(targetSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
