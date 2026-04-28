using System;
using UnityEngine;
using CodeMonkey.Utils;

public class Grid<TGridObject>
{
    private int width;
    private int height;
    private float cellSize;
    private TGridObject[,] gridArray; 
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridArray = new TGridObject[width, height];
        debugTextArray = new TextMesh[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);

                debugTextArray[x, y] = UtilsClass.CreateWorldText(
                    gridArray[x, y].ToString(),
                    null,
                    GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f,
                    100,
                    Color.white,
                    TextAnchor.MiddleCenter
                );
                
                debugTextArray[x, y].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // Küçültme

                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }

    public Vector3 GetWorldPositionCenter(int x, int y)
    {
        return GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    // İlgili hücrenin ekrandaki yazısını ve rengini güncelleyen fonksiyon
    public void UpdateDebugText(int x, int y, string text, Color color)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            debugTextArray[x, y].text = text;
            debugTextArray[x, y].color = color;
        }
    }
}