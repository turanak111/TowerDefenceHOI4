using UnityEngine;

public class MapEditor : MonoBehaviour
{
    private Grid<GridObject> grid;

    private void Start()
    {
        // Tüm ekranı kaplayacak daha büyük bir grid oluşturuyoruz (Örn: 16x9)
        grid = new Grid<GridObject>(16, 9, 10f, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    private void Update()
    {
        // Mouse'un dünyadaki yerini alıyoruz
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        // GetMouseButtonDown yerine GetMouseButton kullanıyoruz ki basılı tutup "boyayabilelim"
        
        // SOL TIK: Düşman Yolu Çiz (Path)
        if (Input.GetMouseButton(0)) 
        {
            PaintTile(mouseWorldPosition, TileType.Path, Color.red);
        }
        // SAĞ TIK: Kule İnşa Alanı Çiz (Buildable)
        else if (Input.GetMouseButton(1)) 
        {
            PaintTile(mouseWorldPosition, TileType.Buildable, Color.green);
        }
        // ORTA TIK: Silgi (Empty)
        else if (Input.GetMouseButton(2)) 
        {
            PaintTile(mouseWorldPosition, TileType.Empty, Color.white);
        }
    }

    // Seçilen hücreyi boyayan yardımcı fonksiyon
    private void PaintTile(Vector3 worldPosition, TileType newType, Color color)
    {
        grid.GetXY(worldPosition, out int x, out int y);
        GridObject clickedObject = grid.GetGridObject(x, y);

        if (clickedObject != null && clickedObject.type != newType)
        {
            // Hücrenin tipini değiştir
            clickedObject.type = newType;
            
            // Ekrandaki yazıyı ve rengi güncelle (E, P, B olarak görünecek)
            grid.UpdateDebugText(x, y, clickedObject.ToString(), color);
            
            Debug.Log($"Harita Güncellendi: {x},{y} artık {newType}");
        }
    }
}