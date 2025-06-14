using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleSceneInteraction : MonoBehaviour
{
    public string puzzleSceneName = "puzzlegame";    // Nama scene tujuan
    public KeyCode interactionKey = KeyCode.F;       // Tombol interaksi
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            SceneManager.LoadScene(puzzleSceneName);
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
