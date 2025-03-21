using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform playerTransform;

    [SerializeField]
    float playerRadius = 0.5f; 

    [Header("Block Settings")]

    [SerializeField]
    float blockSize = 1f;         // Assumed width/height of each block (square)

    [Header("Grid Manager Reference")]
    public GridGenerator gridManager;      // Reference to your GridManager component

    bool gameEnded = false;

    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        // Iterate through all active grid prefabs from your grid manager.
        foreach (GameObject grid in gridManager.GetActiveGrids())
        {
            // Iterate through all block prefabs in each active grid prefab
            foreach (Transform block in grid.transform)
            {

                // Check if it's a water block.
                bool isWater = block.CompareTag("Water");
                // If the block is water and the player is jumping, skip collision check.
                if (isWater && playerController != null && playerController.IsJumping)
                {
                    continue;
                }

                // Check if the player is intersecting with the block
                if (CircleIntersectsSquare(playerTransform.position, playerRadius, block.position, blockSize))
                {
                    gameEnded = true;
                    playerController.EndGame();
                    return; 
                }
            }
        }
    }



    // Returns true if a circle (center, radius) intersects a square (center, size).
    // Assumes the square is axis-aligned.
    bool CircleIntersectsSquare(Vector2 circleCenter, float radius, Vector2 squareCenter, float size)
    {
        float halfSize = size / 2f;
        // Determine the boundaries of the square.
        float left = squareCenter.x - halfSize;
        float right = squareCenter.x + halfSize;
        float bottom = squareCenter.y - halfSize;
        float top = squareCenter.y + halfSize;

        // Find the closest point on the square to the circle's center.
        float closestX = Mathf.Clamp(circleCenter.x, left, right);
        float closestY = Mathf.Clamp(circleCenter.y, bottom, top);

        // Compute the distance between the circle's center and this closest point.
        float distance = Vector2.Distance(circleCenter, new Vector2(closestX, closestY));


        return distance < radius;
    }
}
