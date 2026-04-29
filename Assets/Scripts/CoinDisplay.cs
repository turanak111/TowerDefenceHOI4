using UnityEngine;
using TMPro;

/// <summary>
/// Coin UI gösterimi. CoinManager events'e subscribe olur ve para değişikliklerini gösterir.
/// Canvas'ta Text (TextMeshPro) component'i ile kullanılır.
/// </summary>
public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private bool isSubscribed = false;

    private void Start()
    {
        // CoinManager'ı kontrol et (Start'ta kesinlikle initialize edilmiş olur)
        if (CoinManager.Instance == null)
        {
            Debug.LogError("[CoinDisplay] CoinManager.Instance null! Scene'de CoinManager GameObject'i Hierarchy'de kontrol et.");
            return;
        }

        // Subscribe ol event'lere
        CoinManager.OnCoinChanged += UpdateCoinDisplay;
        isSubscribed = true;
        
        // İlk değeri göster
        UpdateCoinDisplay(CoinManager.Instance.GetCurrentCoins());
    }

    private void OnDestroy()
    {
        // Unsubscribe ol event'lerden (cleanup)
        if (isSubscribed)
        {
            CoinManager.OnCoinChanged -= UpdateCoinDisplay;
        }
    }

    private void UpdateCoinDisplay(float currentCoins)
    {
        if (coinText != null)
        {
            coinText.text = $"💰 {currentCoins}";
        }
    }
}
