using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManagershooter : MonoBehaviour
{
    public static GameManagershooter Instance { get; private set; }

    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button exitButton; // Tambahan tombol keluar
    [SerializeField] private string previousSceneName = "MainMenu"; // Ubah sesuai nama scene sebelumnya

    private Player player;
    private Invaders invaders;
    private MysteryShip mysteryShip;
    private Bunker[] bunkers;

    private void Awake()
    {
        if (Instance != null)
            DestroyImmediate(gameObject);
        else
            Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        FindAllReferences();

        if (playAgainButton != null)
            playAgainButton.onClick.AddListener(PlayAgain);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitToPreviousScene);

        NewGame();
    }

    private void Update()
    {
        if (gameOverUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            PlayAgain();
        }
    }

    private void FindAllReferences()
    {
        player = FindObjectOfType<Player>();
        invaders = FindObjectOfType<Invaders>();
        mysteryShip = FindObjectOfType<MysteryShip>();
        bunkers = FindObjectsOfType<Bunker>();
    }

    private void NewGame()
    {
        Time.timeScale = 1f;
        gameOverUI.SetActive(false);
        NewRound();
    }

    private void NewRound()
    {
        if (invaders != null)
        {
            invaders.ResetInvaders();
            invaders.gameObject.SetActive(true);
        }

        foreach (var bunker in bunkers)
        {
            if (bunker != null)
                bunker.ResetBunker();
        }

        Respawn();
    }

    private void Respawn()
    {
        if (player != null)
        {
            Vector3 position = player.transform.position;
            position.x = 0f;
            player.transform.position = position;
            player.gameObject.SetActive(true);
        }
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);

        if (invaders != null)
            invaders.gameObject.SetActive(false);

        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToPreviousScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(previousSceneName);
    }

    public void OnPlayerKilled(Player player)
    {
        if (player != null)
            player.gameObject.SetActive(false);

        GameOver();
    }

    public void OnInvaderKilled(Invader invader)
    {
        if (invader != null)
            invader.gameObject.SetActive(false);

        if (invaders != null && invaders.GetAliveCount() == 0)
            NewRound();
    }

    public void OnMysteryShipKilled(MysteryShip mysteryShip)
    {
        // Optional
    }

    public void OnBoundaryReached()
    {
        if (invaders != null && invaders.gameObject.activeSelf)
        {
            invaders.gameObject.SetActive(false);
            OnPlayerKilled(player);
        }
    }
}
