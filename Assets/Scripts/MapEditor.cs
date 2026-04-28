using UnityEngine;

public class MapEditor : MonoBehaviour
{
    public bool isEditMode = false;
    public static MapEditor Instance { get; private set; }

    private Grid<GridObject> grid;

    private void Awake()
    {
        // Oyun başladığında bu scriptin tek bir örneği olduğundan emin oluyoruz
        Instance = this;
    }


    private void Start()
    {
        // Tüm ekranı kaplayacak daha büyük bir grid oluşturuyoruz (Örn: 16x9)
        grid = new Grid<GridObject>(16, 9, 10f, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
        LoadMap();
        Debug.Log("Sistem Başlatıldı ve Harita Verileri Yüklendi.");
            }

    public Grid<GridObject> GetGrid()
    {
        return grid;
    }
    private void Update()
    {


        // Mouse'un dünyadaki yerini alıyoruz
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;

        // GetMouseButtonDown yerine GetMouseButton kullanıyoruz ki basılı tutup "boyayabilelim"
        if (!isEditMode) return;
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

        if (Input.GetKey(KeyCode.H))
        {
            SaveMap();
        }
        if(Input.GetKey(KeyCode.L))
        {
            LoadMap();
        }
    }

    
    private void SaveMap() {
    GridSaveData saveData = new GridSaveData();
    saveData.width = grid.GetWidth(); // Grid boyutların neyse o
    saveData.height = grid.GetHeight();
    saveData.types = new TileType[saveData.width * saveData.height];

    for (int x = 0; x < saveData.width; x++) {
        for (int y = 0; y < saveData.height; y++) {
            saveData.types[x + y * saveData.width] = grid.GetGridObject(x, y).type;
        }
    }

    string json = JsonUtility.ToJson(saveData);
    System.IO.File.WriteAllText(Application.persistentDataPath + "/map.json", json);
    Debug.Log("Harita Kaydedildi: " + Application.persistentDataPath);
}
private void LoadMap() 
{
    string path = Application.persistentDataPath + "/map.json";
    
    // Önce böyle bir dosya var mı diye kontrol edelim ki oyun çökmesin
    if (System.IO.File.Exists(path)) 
    {
        string json = System.IO.File.ReadAllText(path);
        GridSaveData saveData = JsonUtility.FromJson<GridSaveData>(json);

        // Kaydedilen harita ile şu anki grid boyutları aynı mı diye güvenlik kontrolü yapıyoruz
        if (saveData.width == grid.GetWidth() && saveData.height == grid.GetHeight()) 
        {
            for (int x = 0; x < saveData.width; x++) 
            {
                for (int y = 0; y < saveData.height; y++) 
                {
                    // 1 Boyutlu diziden 2 Boyutlu koordinatlara (x,y) denk gelen indeksi buluyoruz
                    int index = x + y * saveData.width;
                    TileType savedType = saveData.types[index];

                    GridObject gridObject = grid.GetGridObject(x, y);
                    if (gridObject != null) 
                    {
                        // Hücrenin tipini güncelliyoruz
                        gridObject.type = savedType;

                        // Tipe göre rengi belirliyoruz
                        Color color = Color.white; // Varsayılan: Empty
                        if (savedType == TileType.Path) color = Color.red;
                        else if (savedType == TileType.Buildable) color = Color.green;

                        // Ekrandaki yazıyı ve rengi güncelliyoruz
                        grid.UpdateDebugText(x, y, gridObject.ToString(), color);
                    }
                }
            }
            Debug.Log("Harita Başarıyla Yüklendi!");
        } 
        else 
        {
            Debug.LogError("Hata: Kaydedilen harita boyutu mevcut grid ile uyuşmuyor!");
        }
    } 
    else 
    {
        Debug.LogWarning("Kayıtlı harita bulunamadı! Önce haritayı kaydetmelisin.");
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