using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.tvOS;

public class GridGenerator : MonoBehaviour
{
    public List<GameObject> gridPrefabs;   // List of premade grid prefabs

    [Header("Grid Settings")]

    [SerializeField]
    int gridHeight = 18;

    [SerializeField]
    float gridSpeed = 4f;

    [SerializeField]
    float removeThreshold = -17f;

    [SerializeField]
    float initialBottomY = 11f;

    // List to keep track of currently active grid prefabs
    private List<GameObject> activeGrids = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Spawn two grids initially.
        SpawnGrid(initialBottomY);                    // Bottom grid
        SpawnGrid(initialBottomY + gridHeight + 4);         // Top grid
    }

    // Update is called once per frame
    void Update()
    {
        // Move all active grids downward.
        foreach (GameObject grid in activeGrids)
        {
            grid.transform.position += Vector3.down * gridSpeed * Time.deltaTime;
        }

        // Check if the bottom grid has passed the remove threshold
        if (activeGrids.Count > 0 && activeGrids[0].transform.position.y < removeThreshold)
        {
            // Remove bottom grid
            Destroy(activeGrids[0]);
            activeGrids.RemoveAt(0);

            // Spawn grid prefab
            SpawnGrid(9 + gridHeight);
        }
    }

    // Spawns a random grid at Y position
    void SpawnGrid(float yPosition)
    {
        int randomIndex = UnityEngine.Random.Range(0, gridPrefabs.Count);
        GameObject prefabToSpawn = gridPrefabs[randomIndex];
        GameObject newGrid = Instantiate(prefabToSpawn, new Vector3(1, yPosition, 0), Quaternion.identity, transform);

        // add new grid to list of active grids
        activeGrids.Add(newGrid);
    }
    public List<GameObject> GetActiveGrids()
    {
        return activeGrids;
    }


}
