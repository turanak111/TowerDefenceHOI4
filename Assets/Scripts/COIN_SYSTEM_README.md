# 💰 Coin Sistemi Dokümantasyonu

## Genel Bakış
Bu sistem Tower Defense oyununda para ekonomisini yönetir. Decouple, event-driven mimarisi sayesinde tüm komponentler bağımsız çalışır.

## Mimarisi

### 1. **CoinManager.cs** (Singleton)
Ana para yöneticisi. Oyundaki tüm coin işlemlerinin merkez noktası.

**Public Methods:**
```csharp
float GetCurrentCoins()                    // Güncel coin sayısı
bool TryPurchaseTower(float cost, string towerName)  // Tower satın al
void AddCoins(float amount)                // Para ekle (enemy ölü, vb.)
void RefundCoins(float amount, string reason)  // Para geri ver
void SetStartingCoins(float amount)        // Başlangıç parasını ayarla
```

**Events (Subscribe olarak haberdar ol):**
```csharp
OnCoinChanged   // Herhangi bir coin değişikliği (UI güncellemesi için)
OnCoinEarned    // Para kazanıldı (enemy öldü)
OnCoinSpent     // Para harcandı (tower satın alındı)
```

---

## Bileşenler

### 2. **TowerStatsSO.cs**
Tower istatistikleri Scriptable Object.

**Yeni Field:**
```csharp
[Header("Ekonomi")]
public float cost;  // Tower'ın satın alma maliyeti
```

**Setup:**
1. Unity Asset panelinde sağ tık → Create → Tower Defense → Tower Stats
2. `cost` field'ını istediğin nilai ile doldur
3. Bu ScriptableObject'i Tower Button'a ata

---

### 3. **EnemyStatsSO.cs** (Zaten var)
Enemy istatistikleri. `rewardMoney` field'ı zaten mevcut!

**Mevcut Field:**
```csharp
public int rewardMoney;  // Enemy öldüğünde verdiği para
```

---

## Decouple Mimarı - Veri Akışı

```
┌─────────────────────────────────────────────────────┐
│               CoinManager (Singleton)               │
│  ✓ Para sayısını tutar                              │
│  ✓ Event'ler fırlatır                               │
└────────────────────────────────────────────────────┘
         ↑                        ↑                ↑
         │                        │                │
    [TowerBuilder]         [EnemyHealth]    [UI Components]
    - Para kontrol         - Enemy öldü →    - CoinDisplay
    - Tower satın al       - Para ver        - TowerCostDisplay
```

### Akış:
1. **Tower Satın Alma:**
   - TowerBuilder.cs → `CoinManager.TryPurchaseTower(cost)`
   - Başarılı olursa tower inşa edilir
   - `OnCoinSpent` event'i fırlatılır

2. **Para Kazanma:**
   - EnemyHealth.Die() → `CoinManager.AddCoins(rewardMoney)`
   - `OnCoinEarned` event'i fırlatılır

3. **UI Güncellemesi:**
   - CoinDisplay.cs → `OnCoinChanged` event'e subscribe
   - Para değiştiğinde otomatik güncellenir

---

## Kurulum Adımları

### 1️⃣ Scene Setup
1. Hierarchy'de boş GameObject oluştur → "GameManager" adıyla
2. CoinManager.cs script'ini GameManager'a ekle
3. `DontDestroyOnLoad` özelliği sayesinde scene değişse de kalır

### 2️⃣ UI Setup (Canvas)
1. Canvas oluştur (varsa kullan)
2. Text (TextMeshPro) oluştur → "CoinDisplay" adıyla
3. CoinDisplay.cs script'ini buna ekle
4. Script'teki `coinText` field'ını bu Text'e ata

### 3️⃣ Tower Cost Display (Opsiyonel)
1. Text (TextMeshPro) oluştur → "TowerCostDisplay" adıyla
2. TowerCostDisplay.cs script'ini buna ekle
3. TowerBuilder'ı `SetSelectedTower()` çağrısında bu scripti çağıracak şekilde modifiye et:
   ```csharp
   towerCostDisplay.ShowTowerCost(towerStats);
   ```

### 4️⃣ Tower Setup
1. Her tower için ScriptableObject oluştur
2. `cost` field'ını doldur (örn: 100)
3. Tower Prefab'ı ata
4. Button'a atanan TowerStatsSO'da cost olduğundan emin ol

### 5️⃣ Enemy Setup
1. Her enemy ScriptableObject'inde `rewardMoney` doldur
2. Örn: Zayıf enemy = 10, Güçlü enemy = 50

---

## Kullanım Örnekleri

### Doğrudan Para Ver (Hile/Test)
```csharp
CoinManager.Instance.AddCoins(100);  // 100 para ekle
```

### Para Kontrol Et
```csharp
float currentCoins = CoinManager.Instance.GetCurrentCoins();
if (currentCoins >= 50)
{
    // Kule satın al
}
```

### Event'e Subscribe Ol
```csharp
private void OnEnable()
{
    CoinManager.OnCoinChanged += MyUpdateFunction;
}

private void OnDisable()
{
    CoinManager.OnCoinChanged -= MyUpdateFunction;
}

private void MyUpdateFunction(float coins)
{
    Debug.Log($"Para değişti: {coins}");
}
```

---

## Decouple Faydaları ✨

✅ **TowerBuilder** CoinManager'ı bilmek zorunda değil (sadece TryPurchaseTower çağırır)
✅ **EnemyHealth** UI sistemini bilmek zorunda değil (sadece AddCoins çağırır)
✅ **UI** Tower/Enemy sistem detaylarını bilmek zorunda değil (sadece event'i dinler)
✅ Yeni para mekanizması eklemek çok kolay (Bonus, Multiplier, vb.)
✅ Test etmek kolay - her sistem bağımsız test edilebilir

---

## İleri Özellikleri (Gerekirse Eklenebilir)

- **Wave Bonus:** Her dalga tamamlandığında para ver
- **Multiplier:** Difficulty arttıkça para çarpanı
- **Loan System:** Borç alma mekanizması
- **Tower Refund:** Tower silindiğinde para geri
- **Income Over Time:** Zaman içinde pasif para kazanma

---

## Debug
CoinManager tüm işlemleri Debug.Log ile kaydeder. Console'u açarak akışı takip edebilirsin!

```
[CoinManager] +100 para kazanıldı! Toplam: 100
[CoinManager] Archer satın alındı! Maliyet: 50, Kalan: 50
[CoinManager] Yetersiz para! Gerekli: 100, Mevcut: 50
```
