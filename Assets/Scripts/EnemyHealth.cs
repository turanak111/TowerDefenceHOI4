using Microlight.MicroBar;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable 
{
    public EnemyStatsSO enemyStats; 
    private float currentHealth;
    [SerializeField] MicroBar healthBar;

   public void InitializeHealth() 
    {
        currentHealth = enemyStats.maxHealth;
        if(healthBar != null)
        {
                 healthBar.Initialize(currentHealth);
        }
   
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(healthBar != null)
        {
                    healthBar.UpdateBar(healthBar.CurrentValue - amount);
        }

        Debug.Log($"{enemyStats.enemyName} vuruldu! Kalan Can: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{enemyStats.enemyName} öldü!");
        
        // Düşman öldüğünde para ver
        if (enemyStats.rewardMoney > 0)
        {
            CoinManager.Instance.AddCoins(enemyStats.rewardMoney);
            

        }
        
        // Para kazanma mekaniği buraya gelecek
        Destroy(gameObject);
    }
}