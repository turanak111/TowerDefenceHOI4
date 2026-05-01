
using UnityEngine;


[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "Tower Defense/Enemy Stats")]
public class EnemyStatsSO : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab; 

    public float maxHealth;
    public float moveSpeed;
    public int rewardMoney; // Öldüğünde vereceği para

}