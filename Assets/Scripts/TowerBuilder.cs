using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    [Header("Kule Ayarları")]
    public Transform towerPrefab;
    [Header("Seçili Kule")]
    public TowerStatsSO selectedTowerStats;

    // Butonların çağıracağı fonksiyon
    public void SetSelectedTower(TowerStatsSO towerStats)
    {
        selectedTowerStats = towerStats;
        Debug.Log("Yeni Kule Seçildi: " + (towerStats != null ? towerStats.towerName : "Seçim İptal"));
    }
    void Update()
    {
        // Farenin sol tıkı ile kule inşa et
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f;

            Grid<GridObject> grid = MapEditor.Instance.GetGrid();
            
            // Farenin konumundan grid indekslerini (x, y) alıyoruz
            grid.GetXY(mouseWorldPosition, out int x, out int y);
            GridObject clickedObject = grid.GetGridObject(x, y);

            if (clickedObject != null)
            {
                // SADECE hücre tipi Buildable (Yeşil 'B') ise ve Üzeri boşsa kule dik!
                if (clickedObject.type == TileType.Buildable && !clickedObject.isOccupied)
                {Vector3 cellCenter = grid.GetWorldPositionCenter(x, y);
                    Instantiate(selectedTowerStats.towerPrefab, cellCenter, Quaternion.identity);
                    clickedObject.isOccupied = true;
                    Debug.Log("Kule inşa edildi: " + x + ", " + y);
                }
                else
                {
                    Debug.Log("Buraya kule dikilemez veya zaten dolu!");
                }

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            SetSelectedTower(null);
        }
    }
}