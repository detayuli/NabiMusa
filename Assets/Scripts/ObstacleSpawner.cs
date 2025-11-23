using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Setup")]
    public GameObject[] obstaclePrefabs; // Array berisi prefab Batu, Kayu, Sampah
    public float spawnXPosition = 12f;  // Posisi X di mana obstacle mulai muncul (di luar layar kanan)
    
    [Header("Timing & Speed")]
    public float minSpawnDelay = 1.0f;  // Waktu tunggu minimal antar spawn
    public float maxSpawnDelay = 2.0f;  // Waktu tunggu maksimal antar spawn
    public float obstacleMoveSpeed = 5f; // Kecepatan bergerak ke kiri (Harus cocok dengan script movement obstacle!)
    public float yoffset = -1.5f;
    [Header("Lanes Setup")]
    public float laneHeight = 2.5f;     // Jarak antar jalur (Harus cocok dengan BasketRunnerManager)

    private bool isSpawning = false;

    void Start()
    {
        // Mulai spawning saat game dimulai
        StartSpawning();
    }

    public void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnSequence());
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }

    private IEnumerator SpawnSequence()
    {
        while (isSpawning)
        {
            // Tentukan waktu tunggu acak
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            // Jika game belum over, lakukan spawn
            if (isSpawning) 
            {
                SpawnObstacle();
            }
        }
    }

    private void SpawnObstacle()
    {
        // 1. Pilih Jalur (Lane) acak: -1 (Bawah), 0 (Tengah), atau 1 (Atas)
        int randomLaneIndex = Random.Range(0, 3) - 1; // Menghasilkan -1, 0, atau 1
        float spawnY = (randomLaneIndex * laneHeight) + yoffset;
        
        // 2. Pilih Prefab Obstacle acak
        if (obstaclePrefabs.Length == 0) return;
        GameObject selectedPrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // 3. Tentukan posisi spawn
        Vector3 spawnPosition = new Vector3(spawnXPosition, spawnY, 0);

        // 4. Instantiate dan gerakkan
        GameObject newObstacle = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        // Tambahkan komponen movement ke obstacle yang baru dibuat
        // Ini adalah cara sederhana untuk membuat obstacle bergerak ke kiri
        ObstacleMovement moveComponent = newObstacle.AddComponent<ObstacleMovement>();
        moveComponent.speed = obstacleMoveSpeed; 
        
        // Pastikan obstacle terhapus setelah keluar layar
        Destroy(newObstacle, 10f); // Hapus setelah 10 detik agar tidak menumpuk
    }
}