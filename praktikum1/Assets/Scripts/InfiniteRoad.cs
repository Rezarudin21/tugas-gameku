using UnityEngine;
using System.Collections.Generic;

public class InfiniteRoad : MonoBehaviour
{
    public float speed = 5f;
    public float roadLength = 20f;
    public float spawnDistance = 30f;
    public Transform player;
    public ScoreManager scoreManager;

    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public int maxEnemiesPerRoad = 2; // Tetap 2 saja
    public float laneOffset;

    private bool hasSpawnedNext = false;
    private float spawnChance = 0.3f; // Nilai awal: 30% kemungkinan spawn tiap musuh

    void Update()
    {
        UpdateSpeed();
        UpdateSpawnChance(); // Hitung kepadatan musuh berdasarkan waktu

        transform.position += Vector3.back * speed * Time.deltaTime;

        float distanceToEnd = transform.position.z + roadLength - player.position.z;

        if (distanceToEnd <= spawnDistance && !hasSpawnedNext)
        {
            SpawnNextRoad();
            hasSpawnedNext = true;
        }

        if (transform.position.z < player.position.z - (roadLength * 2))
        {
            Destroy(gameObject);
        }
    }

    void UpdateSpeed()
    {
        float currentScore = scoreManager.GetScore();

        if (currentScore < 100)
            speed = 5f;
        else if (currentScore < 500)
            speed = 7f;
        else if (currentScore < 1000)
            speed = 9f;
        else if (currentScore < 2500)
            speed = 11f;
        else if (currentScore < 5000)
            speed = 13f;
        else
            speed = 15f;
    }

    void UpdateSpawnChance()
    {
        float timeAlive = Time.timeSinceLevelLoad;

        // Kepadatan meningkat seiring waktu
        if (timeAlive < 30f)
            spawnChance = 0.3f; // awal: 30%
        else if (timeAlive < 60f)
            spawnChance = 0.5f;
        else if (timeAlive < 90f)
            spawnChance = 0.7f;
        else
            spawnChance = 0.9f; // setelah 90 detik, hampir selalu spawn musuh
    }

    void SpawnNextRoad()
    {
        float newZ = FindHighestZPosition() + roadLength;
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, newZ);
        GameObject newRoad = Instantiate(gameObject, newPosition, transform.rotation);

        InfiniteRoad newRoadScript = newRoad.GetComponent<InfiniteRoad>();
        newRoadScript.enabled = false;

        newRoadScript.player = player;
        newRoadScript.scoreManager = scoreManager;
        newRoadScript.enemyPrefab = enemyPrefab;
        newRoadScript.laneOffset = laneOffset;
        newRoadScript.hasSpawnedNext = false;

        foreach (Transform child in newRoad.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                Destroy(child.gameObject);
            }
        }

        newRoadScript.spawnChance = spawnChance;
        newRoadScript.SpawnEnemies();

        newRoadScript.enabled = true;
    }

    float FindHighestZPosition()
    {
        InfiniteRoad[] allRoads = FindObjectsOfType<InfiniteRoad>();
        float maxZ = float.MinValue;

        foreach (InfiniteRoad road in allRoads)
        {
            if (road.transform.position.z > maxZ)
            {
                maxZ = road.transform.position.z;
            }
        }

        return maxZ;
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null) return;

        List<int> availableLanes = new List<int> { -1, 0, 1 }; // kiri, tengah, kanan
        int enemiesSpawned = 0;

        while (enemiesSpawned < maxEnemiesPerRoad && availableLanes.Count > 0)
        {
            float roll = Random.value; // nilai antara 0.0 dan 1.0

            if (roll < spawnChance)
            {
                int randomIndex = Random.Range(0, availableLanes.Count);
                int lane = availableLanes[randomIndex];
                availableLanes.RemoveAt(randomIndex);

                float zPos = transform.position.z + Random.Range(5f, roadLength - 5f);
                Vector3 spawnPos = new Vector3(lane * laneOffset, -1.55f, zPos);

                GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity, this.transform);
                enemy.tag = "Enemy";

                enemiesSpawned++;
            }
            else
            {
                break; // tidak spawn musuh lagi jika gagal roll
            }
        }
    }
}
