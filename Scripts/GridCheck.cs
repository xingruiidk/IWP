using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCheck : MonoBehaviour
{
    public static GridCheck instance;
    public Vector2 gridDimensions = new Vector2(10, 10); 
    public float cellSize = 1f;
    public LayerMask collidableLayer; 
    public Color emptyCellColor = Color.green;
    public Color occupiedCellColor = Color.red;
    public bool drawDebug = true;
    private List<Vector3> emptyCells = new List<Vector3>();
    void Start()
    {
        instance = this;
        CheckGrid();
    }
    void Update()
    {
    }

    private void CheckGrid()
    {
        Vector3 startPoint = transform.position - new Vector3(gridDimensions.x, 0, gridDimensions.y) * cellSize * 0.5f;

        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int y = 0; y < gridDimensions.y; y++)
            {
                Vector3 cellCenter = startPoint + new Vector3(x * cellSize, 0, y * cellSize);
                Collider[] colliders = Physics.OverlapBox(cellCenter, Vector3.one * (cellSize * 0.5f), Quaternion.identity, collidableLayer);

                if (colliders.Length == 0)
                {
                    emptyCells.Add(cellCenter);

                    if (drawDebug)
                    {
                        Debug.DrawLine(cellCenter + Vector3.left * cellSize / 2, cellCenter + Vector3.right * cellSize / 2, emptyCellColor);
                        Debug.DrawLine(cellCenter + Vector3.forward * cellSize / 2, cellCenter + Vector3.back * cellSize / 2, emptyCellColor);
                    }
                }
                else if (drawDebug)
                {
                    Debug.DrawLine(cellCenter + Vector3.left * cellSize / 2, cellCenter + Vector3.right * cellSize / 2, occupiedCellColor);
                    Debug.DrawLine(cellCenter + Vector3.forward * cellSize / 2, cellCenter + Vector3.back * cellSize / 2, occupiedCellColor);
                }
            }
        }
    }

    public List<Vector3> GetRandomEmptyCells(int count)
    {
        if (emptyCells.Count == 0)
        {
            Debug.LogWarning("No empty cells available!");
            return new List<Vector3>();
        }

        // Get up to 'count' unique random positions from the empty cells list
        List<Vector3> randomPositions = new List<Vector3>();
        HashSet<int> chosenIndices = new HashSet<int>();
        System.Random random = new System.Random();

        while (randomPositions.Count < count && chosenIndices.Count < emptyCells.Count)
        {
            int index = random.Next(0, emptyCells.Count);
            if (chosenIndices.Add(index))
            {
                randomPositions.Add(emptyCells[index]);
            }
        }

        return randomPositions;
    }
    private void DrawWireCube(Vector3 center, Vector3 size, Color color)
    {
        Vector3 halfSize = size * 0.5f;
        Vector3[] corners = new Vector3[8];
        corners[0] = center + new Vector3(-halfSize.x, 0, -halfSize.z);
        corners[1] = center + new Vector3(-halfSize.x, 0, halfSize.z);
        corners[2] = center + new Vector3(halfSize.x, 0, halfSize.z);
        corners[3] = center + new Vector3(halfSize.x, 0, -halfSize.z);

        corners[4] = center + new Vector3(-halfSize.x, size.y, -halfSize.z);
        corners[5] = center + new Vector3(-halfSize.x, size.y, halfSize.z);
        corners[6] = center + new Vector3(halfSize.x, size.y, halfSize.z);
        corners[7] = center + new Vector3(halfSize.x, size.y, -halfSize.z);

        // Draw bottom square
        Debug.DrawLine(corners[0], corners[1], color);
        Debug.DrawLine(corners[1], corners[2], color);
        Debug.DrawLine(corners[2], corners[3], color);
        Debug.DrawLine(corners[3], corners[0], color);

        // Draw top square
        Debug.DrawLine(corners[4], corners[5], color);
        Debug.DrawLine(corners[5], corners[6], color);
        Debug.DrawLine(corners[6], corners[7], color);
        Debug.DrawLine(corners[7], corners[4], color);

        // Draw vertical lines
        Debug.DrawLine(corners[0], corners[4], color);
        Debug.DrawLine(corners[1], corners[5], color);
        Debug.DrawLine(corners[2], corners[6], color);
        Debug.DrawLine(corners[3], corners[7], color);
    }
}
