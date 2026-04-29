using UnityEngine;
using System;

/// <summary>
/// Singleton para yöneticisi. Oyundaki tüm coin işlemlerinin merkez noktası.
/// Decouple yapı için events kullanır.
/// </summary>
public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private float currentCoins;
    
    [SerializeField] private float startingCoins = 100f; // Inspector'dan düzenlenir

    // Events - Sistemlerin coin değişikliklerinden haberdar olmak için subscribe olacakları
    public static event Action<float> OnCoinChanged; // Coin sayısı değişti
    public static event Action<float> OnCoinEarned;  // Para kazanıldı (enemy öldü)
    public static event Action<float> OnCoinSpent;   // Para harcandı (tower satın alındı)

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Başlangıçta verilen miktar
        currentCoins = startingCoins;
        OnCoinChanged?.Invoke(currentCoins);
    }

    /// <summary>
    /// Güncel coin sayısını döndür
    /// </summary>
    public float GetCurrentCoins()
    {
        return currentCoins;
    }

    /// <summary>
    /// Doğrudan coin ekle (hile, start para, vb.)
    /// </summary>
    public void AddCoins(float amount)
    {
        if (amount <= 0) return;

        currentCoins += amount;
        OnCoinChanged?.Invoke(currentCoins);
        OnCoinEarned?.Invoke(amount);

        Debug.Log($"[CoinManager] +{amount} para kazanıldı! Toplam: {currentCoins}");
    }

    /// <summary>
    /// Kule satın almayı dene. Başarılı olursa true döndür.
    /// </summary>
    public bool TryPurchaseTower(float cost, string towerName = "Tower")
    {
        if (cost < 0)
        {
            Debug.LogWarning("[CoinManager] Negatif maliyet!");
            return false;
        }

        if (currentCoins >= cost)
        {
            currentCoins -= cost;
            OnCoinChanged?.Invoke(currentCoins);
            OnCoinSpent?.Invoke(cost);

            Debug.Log($"[CoinManager] {towerName} satın alındı! Maliyet: {cost}, Kalan: {currentCoins}");
            return true;
        }
        else
        {
            Debug.Log($"[CoinManager] Yetersiz para! Gerekli: {cost}, Mevcut: {currentCoins}");
            return false;
        }
    }

    /// <summary>
    /// Para reddet (tower silme, vb. işlemlerde geri para)
    /// </summary>
    public void RefundCoins(float amount, string reason = "Refund")
    {
        if (amount <= 0) return;

        currentCoins += amount;
        OnCoinChanged?.Invoke(currentCoins);

        Debug.Log($"[CoinManager] {reason} - {amount} para geri alındı! Toplam: {currentCoins}");
    }

    /// <summary>
    /// Başlangıç parasını ayarla (debug/test için)
    /// </summary>
    public void SetStartingCoins(float amount)
    {
        currentCoins = Mathf.Max(0, amount);
        OnCoinChanged?.Invoke(currentCoins);
        Debug.Log($"[CoinManager] Başlangıç parası: {currentCoins}");
    }
}
