using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float damage;
    private float explosionRadius;
    public float speed = 15f;

    public void Setup(Transform target, float damageAmount, float explosionRadius = 0f)
    {
        this.target = target;
        this.damage = damageAmount;
        this.explosionRadius = explosionRadius;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 moveDirection = (target.position - transform.position).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // Eğer patlama çapı 0 ise, sadece tek hedefi vur
        if (explosionRadius == 0f)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }
        else
        {
            // Alan hasarı: Patlama çapı içindeki tüm düşmanları vur
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            for (int i = 0; i < enemies.Length; i++)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemies[i].transform.position);
                
                // Patlama alanı içindeki düşmanları vur
                if (distanceToEnemy <= explosionRadius)
                {
                    IDamageable damageable = enemies[i].GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damage);
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}