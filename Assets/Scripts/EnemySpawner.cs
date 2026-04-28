using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave (Raid) Ayarları")]
    public List<WaveSO> waves; // Oyunun bölümündeki tüm wave'ler sırasıyla buraya eklenecek
    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    [Header("Düşmanın Rotası")]
    public List<Vector2Int> pathCoordinates;

    private void Update()
    {
        // Raid'i başlatmak için şimdilik T (Test) tuşunu kullanıyoruz. 
        // İleride bunu oyun ekranındaki "Next Wave" butonuna bağlarsın.
        if (Input.GetKeyDown(KeyCode.T) && !isSpawning && currentWaveIndex < waves.Count)
        {
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
    }

    private IEnumerator SpawnWave(WaveSO wave)
    {
        isSpawning = true;
        Debug.Log($"=== {wave.waveName} BAŞLADI ===");

        Grid<GridObject> grid = MapEditor.Instance.GetGrid();
        List<Vector3> worldWaypoints = new List<Vector3>();

        foreach (Vector2Int coord in pathCoordinates)
        {
            worldWaypoints.Add(grid.GetWorldPositionCenter(coord.x, coord.y));
        }

        if (worldWaypoints.Count == 0)
        {
            Debug.LogError("Rota bulunamadı! Harita koordinatlarını kontrol et.");
            yield break;
        }

        // Wave içindeki düşman gruplarını (Setup) sırayla dön
        foreach (WaveEnemySetup setup in wave.enemySetups)
        {
            // O gruptan kaç tane istendiyse o kadar yarat
            for (int i = 0; i < setup.count; i++)
            {
                // 1. Düşman Prefab'ını doğrudan SO'nun içinden çekip sahnede yaratıyoruz
                GameObject spawnedEnemy = Instantiate(setup.enemyType.enemyPrefab, worldWaypoints[0], Quaternion.identity);

                // 2. Can ve Stat atamasını scriptlere enjekte ediyoruz
                EnemyHealth healthScript = spawnedEnemy.GetComponent<EnemyHealth>();
                if (healthScript != null)
                {
                    healthScript.enemyStats = setup.enemyType;
                    healthScript.InitializeHealth(); // Canını fullemesi için emir ver
                }

                EnemyPathfinding pathScript = spawnedEnemy.GetComponent<EnemyPathfinding>();
                if (pathScript != null)
                {
                    // Hızı SO üzerinden yolluyoruz
                    pathScript.Setup(worldWaypoints, setup.enemyType.moveSpeed); 
                }

                // 3. Sıradaki düşman doğmadan önce belirtilen saniye kadar bekle
                yield return new WaitForSeconds(setup.spawnDelay);
            }
        }

        Debug.Log($"=== {wave.waveName} BİTTİ ===");
        currentWaveIndex++;
        isSpawning = false;
    }
}