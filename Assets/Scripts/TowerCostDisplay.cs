using UnityEngine;
using TMPro;

/// <summary>
/// Tower'ı seçildiğinde maliyetini gösteren UI component.
/// UI'da tower seçimi sırasında kullanıcıya "Bu tower 100 coin tutuyor" şeklinde bilgi verir.
/// </summary>
public class TowerCostDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Color affordableColor = Color.white;
    [SerializeField] private Color unaffordableColor = Color.red;

    private TowerStatsSO lastSelectedTowerStats;

    private void OnEnable()
    {
        CoinManager.OnCoinChanged += HandleCoinChanged;
    }

    private void OnDisable()
    {
        CoinManager.OnCoinChanged -= HandleCoinChanged;
    }


    private void Start()
    {
        if (costText != null)
        {
            costText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Tower seçildiğinde çağır - UI'da maliyeti göster
    /// </summary>
    public void ShowTowerCost(TowerStatsSO towerStats)
    {
        if (towerStats == null || costText == null)
        {
            costText.gameObject.SetActive(false);
            return;
        }
        lastSelectedTowerStats = towerStats;
        float currentCoins = CoinManager.Instance.GetCurrentCoins();
        bool canAfford = currentCoins >= towerStats.cost;

        costText.text = $"{towerStats.towerName}\nMaliyet: {towerStats.cost}";
        costText.color = canAfford ? affordableColor : unaffordableColor;
        costText.gameObject.SetActive(true);
    }


    private void HandleCoinChanged(float newAmount)
    {
      
        if (lastSelectedTowerStats != null)
        {
            ShowTowerCost(lastSelectedTowerStats);
        }
    }

    /// <summary>
    /// Seçim iptal edildiğinde UI'ı gizle
    /// </summary>
    public void HideTowerCost()
    {
        if (costText != null)
        {
            costText.gameObject.SetActive(false);
        }
    }
}
