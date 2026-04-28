using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerStats", menuName = "Tower Defense/Tower Stats")]
public class TowerStatsSO : ScriptableObject
{
    public string towerName;
    
    [Header("Görseller")]
    public GameObject towerPrefab;
    public GameObject bulletPrefab; // Her kulenin atacağı mermi farklı olabilir!
    
    [Header("İstatistikler")]
    public float range;
    public float fireRate;
    public float damage; // Kulenin mermisi ne kadar hasar vuracak?
}