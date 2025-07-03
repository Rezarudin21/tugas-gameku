using UnityEngine;

namespace TrafficRacer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager singeton;

        [HideInInspector] public GameStatus gameStatus = GameStatus.NONE;
        [HideInInspector] public int currentCarIndex = 0;

        // Score pemain
        [HideInInspector] public int lastScore = 0;

        void Awake()
        {
            if (singeton == null)
            {
                singeton = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("GameManager aktif.");
            }
            else if (singeton != this)
            {
                Debug.LogWarning("Duplicate GameManager ditemukan dan dihancurkan.");
                Destroy(gameObject);
            }
        }

        public void SetScore(int score)
        {
            lastScore = score;
        }

        public int GetScore()
        {
            return lastScore;
        }

        /// <summary>
        /// Simpan skor terakhir ke PlayerPrefs, dipanggil ketika game over.
        /// </summary>
        public void SaveScoreToLeaderboard()
        {
            const int maxLeaderboardEntries = 10;

            // Ambil data leaderboard lama
            int[] leaderboard = new int[maxLeaderboardEntries];
            for (int i = 0; i < maxLeaderboardEntries; i++)
            {
                leaderboard[i] = PlayerPrefs.GetInt("HighScore" + i, 0);
            }

            // Tambahkan skor baru dan urutkan
            leaderboard[maxLeaderboardEntries - 1] = lastScore;
            System.Array.Sort(leaderboard);
            System.Array.Reverse(leaderboard);

            // Simpan kembali hanya 10 skor tertinggi
            for (int i = 0; i < maxLeaderboardEntries; i++)
            {
                PlayerPrefs.SetInt("HighScore" + i, leaderboard[i]);
            }

            PlayerPrefs.Save();
        }
    }
}
