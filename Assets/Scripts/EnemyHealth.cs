using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
    public EnemyStatsSO enemyStats; 
    private float currentHealth;

   public void InitializeHealth() 
    {
        currentHealth = enemyStats.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{enemyStats.enemyName} vuruldu! Kalan Can: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{enemyStats.enemyName} öldü!");
        // Para kazanma mekaniği buraya gelecek
        Destroy(gameObject);
    }
}