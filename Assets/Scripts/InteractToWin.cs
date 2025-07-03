using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractToWin : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactionDistance = 2f;
    public KeyCode interactKey = KeyCode.F;
    public GameObject winUI;
    public string nextSceneName;

    [Header("UI Buttons")]
    public Button exitButton; // <- Tambahkan ini

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (winUI != null)
            winUI.SetActive(false);

        if (exitButton != null)
            exitButton.onClick.AddListener(QuitGame); // <- Event listener
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactionDistance && Input.GetKeyDown(interactKey))
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        if (winUI != null)
            winUI.SetActive(true);

        Debug.Log("You Win!");

        Time.timeScale = 0f;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }

    void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // agar berhenti saat play di editor
#endif
    }
}
