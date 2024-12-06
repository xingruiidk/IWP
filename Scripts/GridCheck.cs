using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCheck : MonoBehaviour
{
    public static GridCheck instance;
    public Vector2 gridDimensions; 
    public float cellSize;
    public LayerMask collidableLayer; 
    public Color emptyCellColor = Color.green;
    public Color occupiedCellColor = Color.red;
    public bool drawDebug = true;
    private List<Vector3> emptyCells = new List<Vector3>();
    void Start()
    {
        instance = this;
    }
    void Update()
    {
    }

    public void CheckGrid()
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
                    //Debug.Log(emptyCells.Count);
                    if (drawDebug)
                    {
                        //Debug.DrawLine(cellCenter + Vector3.left * cellSize / 2, cellCenter + Vector3.right * cellSize / 2, emptyCellColor);
                        //Debug.DrawLine(cellCenter + Vector3.forward * cellSize / 2, cellCenter + Vector3.back * cellSize / 2, emptyCellColor);
                    }
                }
                else if (drawDebug)
                {
                    //Debug.DrawLine(cellCenter + Vector3.left * cellSize / 2, cellCenter + Vector3.right * cellSize / 2, occupiedCellColor);
                    //Debug.DrawLine(cellCenter + Vector3.forward * cellSize / 2, cellCenter + Vector3.back * cellSize / 2, occupiedCellColor);
                }
            }
        }
    }

    public List<Vector3> GetRandomEmptyCells(int count)
    {
        if (emptyCells.Count == 0)
        {
            Debug.Log("0 empty cell");
            return new List<Vector3>();
        }
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
}
