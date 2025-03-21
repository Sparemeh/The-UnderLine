using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField]
    float playerRadius = 0.5f; 

    [SerializeField]
    float blockSize = 1f;    

    public GridGenerator gridManager;
    public PlayerController playerController;

    bool gameEnded = false;

    // Update is called once per frame
    void Update()
    {
        if (gameEnded) return;

        // Iterate through all active grid prefabs from grid manager
        foreach (GameObject grid in gridManager.GetActiveGrids())
        {
            // Iterate through all block prefabs in each active grid prefab
            foreach (Transform block in grid.transform)
            {
                // Check if block is a Powerup.
                if (block.CompareTag("PowerUp"))
                {
                    // Check if player is overlapping powerup
                    if (CircleIntersectsSquare(playerTransform.position, playerRadius, block.position, blockSize))
                    {
                        // Get Powerup component and activate
                        PowerUps powerUp = block.GetComponent<PowerUps>();
                        powerUp.Activate(playerController);
                        Destroy(block.gameObject);
                        continue;
                    }
                }

                // Check if it's a water block.
                bool isWater = block.CompareTag("Water");

                // If the block is water and player is jumping, skip collision check.
                if (isWater && playerController.IsJumping)
                {
                    continue;
                }

                // Check if player is intersecting with the block
                if (CircleIntersectsSquare(playerTransform.position, playerRadius, block.position, blockSize))
                {
                    if (playerController.IsInvincible)
                    {
                        // If player is invincible, delete the colliding block.
                        Destroy(block.gameObject);
                    }
                    else
                    {
                        // Otherwise end game.
                        gameEnded = true;
                        playerController.EndGame();
                        return;
                    }
                }
            }
        }
    }

    // Returns true if a circle intersects a square.
    bool CircleIntersectsSquare(Vector2 circleCenter, float radius, Vector2 squareCenter, float size)
    {
        float halfSize = size / 2f;

        // Determine boundaries of the square.
        float left = squareCenter.x - halfSize;
        float right = squareCenter.x + halfSize;
        float bottom = squareCenter.y - halfSize;
        float top = squareCenter.y + halfSize;

        // Find the closest point on the square to the circle's center.
        float closestX = Mathf.Clamp(circleCenter.x, left, right);
        float closestY = Mathf.Clamp(circleCenter.y, bottom, top);

        // Compute the distance between the circle's center and this closest point.
        float distance = Vector2.Distance(circleCenter, new Vector2(closestX, closestY));

        // return true if the distance is shorter than the radius
        return distance < radius;
    }
}
