using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float damage; 
    public float speed = 15f;

    public void Setup(Transform target, float damageAmount)
    {
        this.target = target;
        this.damage = damageAmount;
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
        IDamageable damageable = target.GetComponent<IDamageable>();
        
        if (damageable != null)
        {
            damageable.TakeDamage(damage); 
        }

        Destroy(gameObject);
    }
}