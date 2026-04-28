using UnityEngine;

// Hücrenin ne tür bir alan olduğunu belirten kategoriler
public enum TileType 
{
    Empty,      // Boş alan (Ağaç, dağ, tepe vs. Hiçbir şey yapılamaz)
    Path,       // Düşman Yolu (Buradan düşmanlar geçecek)
    Buildable   // Kule İnşa Alanı (Sadece buraya kule dikilebilir)
}

public class GridObject 
{
    private Grid<GridObject> grid;
    public int x { get; private set; }
    public int y { get; private set; }

    // Hücrenin tipini tutan değişken. Varsayılan olarak her yer "Empty" başlar.
    public TileType type = TileType.Empty; 
    public bool isOccupied = false; // Üzerinde kule var mı?

    public GridObject(Grid<GridObject> grid, int x, int y) 
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public override string ToString() 
    {
        // Ekranda sadece koordinat değil, tipin baş harfi de yazsın (Örn: E, P, B)
        return type.ToString().Substring(0,1); 
    }
}