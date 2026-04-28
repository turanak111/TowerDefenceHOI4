using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Kule Verileri")]
    public TowerStatsSO towerStats; // Tüm istatistikler ve mermi prefabı burada!
    
    [Header("Referanslar")]
    public Transform target;       
    public Transform firePoint;        

    private float fireCountdown = 0f;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        // Menzili artık SO'dan okuyoruz
        if (Vector3.Distance(transform.position, target.position) > towerStats.range)
        {
            target = null;
            return;
        }

        if (fireCountdown <= 0f)
        {
            Shoot();
            // Atış hızını SO'dan okuyoruz
            fireCountdown = 1f / towerStats.fireRate; 
        }

        fireCountdown -= Time.deltaTime;
    }

    void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject enemy = enemies[i];
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // Menzil kontrolünü SO'dan yapıyoruz
        if (nearestEnemy != null && shortestDistance <= towerStats.range)
        {
            target = nearestEnemy.transform;
        }
    }

    void Shoot()
    {
        // Mermiyi SO içindeki prefab'dan Instantiate ediyoruz
        GameObject projObj = Instantiate(towerStats.bulletPrefab, firePoint.position, Quaternion.identity);
        
        Projectile projectile = projObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            // Mermiye hem hedefi hem de kulenin hasarını gönderiyoruz
            projectile.Setup(target, towerStats.damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Editörde seçildiğinde menzili SO'dan okuyarak çiz (SO boşsa hata vermemesi için null kontrolü)
        if (towerStats != null) 
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, towerStats.range);
        }
    }
}