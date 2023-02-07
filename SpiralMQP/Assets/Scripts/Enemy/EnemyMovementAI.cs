using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    // MovementDetailsSO contains movement details such as speed
    [SerializeField] private MovementDetailsSO movementDetails;
    private Enemy enemy;
    private Stack<Vector3> movementSteps = new Stack<Vector3>(); // The vector3 stack that contains the grid path information calculated from the AStar algo
    private Vector3 playerReferencePosition; // The previous position that the enemy was at, for monitoring the enemy position
    private Coroutine moveEnemyRoutine; // Coroutine for the enemy movement
    private float currentEnemyPathRebuildCooldown;
    private WaitForFixedUpdate waitForFixedUpdate;
    [HideInInspector] public float moveSpeed;
    [HideInInspector] public int updateFrameNumber = 1; // Default value. This is set by the enemy spawner
    private bool chasePlayer = false; // If the enemy should chase the player (depends on the enemy player distance)

    private void Awake()
    {
        // Load components
        enemy = GetComponent<Enemy>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        // Create waitforfixed update for use in coroutine
        waitForFixedUpdate = new WaitForFixedUpdate();

        // Reset player reference position
        playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();
    }

    private void Update()
    {
        MoveEnemy();
    }


    /// <summary>
    /// Use AStar pathfinding to build a path to the player - and then move the enemy to each grid location on the path
    /// </summary>
    private void MoveEnemy()
    {
        // Movement cooldown timer
        currentEnemyPathRebuildCooldown -= Time.deltaTime;

        // Check distance to player to see if enemy should start chasing
        if (!chasePlayer && Vector3.Distance(transform.position, GameManager.Instance.GetPlayer().GetPlayerPosition()) < enemy.enemyDetails.chaseDistance)
        {
            chasePlayer = true;
        }

        // If not close enough to chase player then return
        if (!chasePlayer)
            return;

        // Only process AStar path rebuild on certain frames to spread the load between enemies
        if (Time.frameCount % Settings.targetFrameRateToSpreadPathfindingOver != updateFrameNumber) return;
        

        // If the movement cooldown timer reached or player has moved more than required distance
        // Then rebuild the enemy path and move the enemy
        if (currentEnemyPathRebuildCooldown <= 0f || (Vector3.Distance(playerReferencePosition, GameManager.Instance.GetPlayer().GetPlayerPosition()) > Settings.playerMoveDistanceToRebuildPath))
        {
            // Reset path rebuild cooldown timer
            currentEnemyPathRebuildCooldown = Settings.enemyPathRebuildCooldown;

            // Reset player reference position
            playerReferencePosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

            // Move the enemy using AStar pathfinding - Trigger rebuild of path to player
            CreatePath();

            // If a path has been found move the enemy
            if (movementSteps != null)
            {
                // Check if there is already a coroutine running
                if (moveEnemyRoutine != null)
                {
                    // Trigger idle event
                    enemy.idleEvent.CallIdleEvent();
                    StopCoroutine(moveEnemyRoutine);
                }

                // Move enemy along the path using a coroutine
                moveEnemyRoutine = StartCoroutine(MoveEnemyRoutine(movementSteps));

            }
        }
    }


    /// <summary>
    /// Coroutine to move the enemy to the next location on the path
    /// </summary>
    private IEnumerator MoveEnemyRoutine(Stack<Vector3> movementSteps)
    {
        while (movementSteps.Count > 0)
        {
            Vector3 nextPosition = movementSteps.Pop();

            // While not very close, continue to move - when close, move onto the next step
            while (Vector3.Distance(nextPosition, transform.position) > 0.2f)
            {
                // Trigger movement event
                enemy.movementToPositionEvent.CallMovementToPositionEvent(nextPosition, transform.position, moveSpeed, (nextPosition - transform.position).normalized);

                yield return waitForFixedUpdate;  // Moving the enmy using 2D physics so wait until the next fixed update

            }

            yield return waitForFixedUpdate;
        }

        // End of path steps - trigger the enemy idle event
        enemy.idleEvent.CallIdleEvent();

    }


    /// <summary>
    /// Use the AStar static class to create a path for the enemy
    /// </summary>
    private void CreatePath()
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        // Get players position on the grid
        Vector3Int playerGridPosition = GetNearestNonObstaclePlayerPosition(currentRoom);

        // Get enemy position on the grid
        Vector3Int enemyGridPosition = grid.WorldToCell(transform.position);

        // Build a path for the enemy to move on
        movementSteps = AStar.BuildPath(currentRoom, enemyGridPosition, playerGridPosition);

        // Take off first step on path - this is the grid square the enemy is already on
        if (movementSteps != null)
        {
            movementSteps.Pop();
        }
        else
        {
            // Trigger idle event - no path
            enemy.idleEvent.CallIdleEvent();
        }
    }


    /// <summary>
    /// Set the frame number that the enemy path will be recalculated on - to avoid performance spikes
    /// </summary>
    public void SetUpdateFrameNumber(int updateFrameNumber)
    {
        this.updateFrameNumber = updateFrameNumber;
    }


    /// <summary>
    /// Get the nearest position to the player that isn't on an obstacle
    /// </summary>
    private Vector3Int GetNearestNonObstaclePlayerPosition(Room currentRoom)
    {
        // Get the player current world position
        Vector3 playerPosition = GameManager.Instance.GetPlayer().GetPlayerPosition();

        // Get the player current cell position on grid map (the origin is always 0,0 on the default grid map)
        Vector3Int playerCellPosition = currentRoom.instantiatedRoom.grid.WorldToCell(playerPosition);

        // Get the position on our room grid map position which we need to compensate the offset 
        Vector2Int adjustedPlayerCellPositon = new Vector2Int(playerCellPosition.x - currentRoom.templateLowerBounds.x, playerCellPosition.y - currentRoom.templateLowerBounds.y);

        // Try get the obstacle that's on the player position (obstacle type will be marked with a value of zero)
        int obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPositon.x, adjustedPlayerCellPositon.y];

        // If the player isn't on a cell square marked as an obstacle then return that position
        if (obstacle != 0)
        {
            return playerCellPosition;
        }
        // Find a surounding cell that isn't an obstacle - required because with the 'half collision' tiles and tables the player can be on a grid square that is marked as an obstacle
        else
        {
            // Nested for loop to check all 9 cells
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0) continue; // Skip self

                    try
                    {
                        // Check if current cell is an obstacle
                        obstacle = currentRoom.instantiatedRoom.aStarMovementPenalty[adjustedPlayerCellPositon.x + i, adjustedPlayerCellPositon.y + j];
                        if (obstacle != 0)
                        {
                            return new Vector3Int(playerCellPosition.x + i, playerCellPosition.y + j, 0);
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            // Theoretically, the program is not supposed to get here. But shit happens.
            // No non-obstacle cells surrounding the player so just return the player position
            return playerCellPosition;
        }
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif
    #endregion Validation
}
