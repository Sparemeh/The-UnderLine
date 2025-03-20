using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.tvOS;

public class GridGenerator : MonoBehaviour
{

    public GameObject grid;
    public List<GameObject> gridPrefabs;   // List of premade grid prefabs

    [Header("Grid Settings")]
    [SerializeField]
    int gridWidth = 7;

    [SerializeField]
    int gridHeight = 18;

    [SerializeField]
    float gridSpeed = 4f;

    [SerializeField]
    float cellSize = 0.5f;

    [SerializeField]
    float spawnY = 10f;

    [SerializeField]
    float removeThreshold = -17f;

    [SerializeField]
    float initialBottomY = 11f;      // Y position for the bottom grid

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
            // Remove the bottom grid
            Destroy(activeGrids[0]);
            activeGrids.RemoveAt(0);

            // Determine the Y position for the new grid based on the current top grid
            float topY = activeGrids[activeGrids.Count - 1].transform.position.y;
            SpawnGrid(26f);
        }
    }

    // Spawns a new grid at a specified Y position by selecting a random prefab from the list.
    void SpawnGrid(float yPosition)
    {
        int randomIndex = UnityEngine.Random.Range(0, gridPrefabs.Count);
        GameObject prefabToSpawn = gridPrefabs[randomIndex];
        GameObject newGrid = Instantiate(prefabToSpawn, new Vector3(1, yPosition, 0), Quaternion.identity, transform);
        activeGrids.Add(newGrid);
    }


}
