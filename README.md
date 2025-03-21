# Custom Collision Detection Documentation
Link to video demo: https://drive.google.com/file/d/15Ky2ILVXLWB8fGWubdxEeNm-Sufp4Dua/view?usp=sharing
## Overview
This system performs collision detection between the player and grid blocks a custom algorithm. It checks if the player's hitbox intersects with a block’s area.

- **GridManager**  
  - Maintains a list of active grid prefabs, where each grid is a collection of block prefabs.
  - Provides access to these grids

- **PlayerController**  
  - Manages player state properties such as `IsInvincible` and `IsJumping`, which affect collision outcomes.

- **CustomCollisionManager**  
  - Iterates through active grids and their child blocks.
  - Checks for collisions between the player and each block.
 
 ## Collision Handling Logic

 1. **Iterating Over Active Grids**
   - The collision manager retrieves active grid prefabs from the `GridManager`.
   - For each grid, it loops through every child block.

2. **Handling Special Cases**
   - **Powerups:**  
     - If a block is tagged `"Powerup"` and a collision is detected, the powerup's effect is activated, and the powerup is removed.
   - **Water Blocks:**  
     - If a block is tagged `"Water"` and the player is jumping (`IsJumping` is `true`), the collision check for that block is skipped.

3. **Collision Detection Algorithm**
   - The method `CircleIntersectsSquare` performs the following steps:
     - **Calculate Block Boundaries:**  
       - Determine the half-size of the block.
       - Compute the boundaries (left, right, top, bottom) based on the block’s center.
     - **Find Closest Point:**  
       - Clamp the player’s position to these boundaries to find the closest point on the block.
     - **Distance Check:**  
       - Calculate the distance between the player's center and the closest point.
       - If the distance is less than the player's radius, a collision is detected.

4. **Collision Outcome**
   - **Player Invincible:**  
     - If `IsInvincible` is `true`, the colliding block is destroyed.
   - **Normal Collision:**  
     - If not invincible, a collision triggers game over.
