using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [Header("Kule Ayarları")]
    public Transform towerPrefab;
    [Header("Seçili Kule")]
    public TowerStatsSO selectedTowerStats;

    private bool isRemovingTower = false;

    // Butonların çağıracağı fonksiyon (Kule seçme)
    public void SetSelectedTower(TowerStatsSO towerStats)
    {
        selectedTowerStats = towerStats;
        isRemovingTower = false; 
        Debug.Log("Yeni Kule Seçildi: " + (towerStats != null ? towerStats.towerName : "Seçim İptal"));
    }

  
    public void SetRemovingTower()
    {
        isRemovingTower = !isRemovingTower; 
        
        if (isRemovingTower)
        {
            selectedTowerStats = null;
            Debug.Log("Silme Modu AKTİF!");
        }
        else
        {
            Debug.Log("Silme Modu KAPALI!");
        }
    }

    void Update()
    {
        // Farenin sol tıkı
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;

            Grid<GridObject> grid = MapEditor.Instance.GetGrid();
            
            grid.GetXY(mouseWorldPosition, out int x, out int y);
            GridObject clickedObject = grid.GetGridObject(x, y);

            if (clickedObject != null)
            {
                // DURUM 1: SİLME MODU AKTİFSE
                if(isRemovingTower)
                {
                    // Tıklanan yer doluysa ve gerçekten bir kule referansı varsa
                    if (clickedObject.isOccupied && clickedObject.placedTower != null)
                    {
                        // Sahnedeki kule objesini yok et
                        Destroy(clickedObject.placedTower.gameObject);
                        
                        // Grid hücresini tekrar boş ve inşa edilebilir duruma getir
                        clickedObject.placedTower = null;
                        clickedObject.isOccupied = false;
                        
                        Debug.Log("Kule başarıyla silindi: " + x + ", " + y);
                    }
                }
                // DURUM 2: İNŞA MODU AKTİFSE
                else if (clickedObject.type == TileType.Buildable && !clickedObject.isOccupied)
                {
                    if(selectedTowerStats == null) return;

                    Vector3 cellCenter = grid.GetWorldPositionCenter(x, y);
                    
                    // Instantiate edilen objeyi GamebObject/Transform olarak yakala
                    Transform newTower = Instantiate(selectedTowerStats.towerPrefab, cellCenter, Quaternion.identity).transform;
                    
                    // Yakalanan bu objeyi grid hücresinin içine kaydet
                    clickedObject.placedTower = newTower;
                    clickedObject.isOccupied = true;
                    
                    Debug.Log("Kule inşa edildi: " + x + ", " + y);
                }
                else
                {
                    Debug.Log("Buraya işlem yapılamaz!");
                }
            }
        }
        
        // Sağ tık ile her şeyi (seçimi ve silme modunu) iptal et
        if (Input.GetMouseButtonDown(1))
        {
            SetSelectedTower(null);
            isRemovingTower = false;
            Debug.Log("Tüm eylemler iptal edildi.");
        }
    }
}