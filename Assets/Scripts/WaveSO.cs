using System.Collections.Generic;
using UnityEngine;

// Wave içindeki tek bir grubu temsil eden yapı (Örn: 5 tane Orc, 1'er saniye arayla)
[System.Serializable]
public class WaveEnemySetup
{
    public EnemyStatsSO enemyType; // Hangi düşman? (İçinde prefab ve statlar var)
    public int count;              // Kaç adet doğacak?
    public float spawnDelay;       // İki düşman arasında kaç saniye beklenecek?
}

[CreateAssetMenu(fileName = "NewWave", menuName = "Tower Defense/Wave")]
public class WaveSO : ScriptableObject
{
    public string waveName;
    public List<WaveEnemySetup> enemySetups;
}