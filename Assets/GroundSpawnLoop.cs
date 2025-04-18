using System.Collections;
using UnityEngine;

public class GroundSpawnLoop : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab yang akan di-spawn
    public Vector3 spawnAreaMin; // Batas bawah area spawn
    public Vector3 spawnAreaMax; // Batas atas area spawn
    public float spawnInterval = 1f; // Waktu antar spawn
    public float spawnDuration = 60f; // Durasi spawn (1 menit)

    private float elapsedTime = 0f;

    void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    IEnumerator SpawnObjects()
    {
        while (elapsedTime < spawnDuration)
        {
            SpawnObject();
            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
        }
    }

    void SpawnObject()
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            spawnAreaMin.y, // Tetap di tanah
            Random.Range(spawnAreaMin.z, spawnAreaMax.z)
        );

        Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
    }
}
