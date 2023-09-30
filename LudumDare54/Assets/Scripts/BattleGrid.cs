using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{

    private static int gridXLength = 6;
    private static int gridYLength = 3;

    public Tile[,] grid = new Tile[gridXLength,gridYLength];
    public GameObject tile;

    private float tileXDistance = 2.9f;
    private float tileYDistance = -1.5f;

    public float middle;

    // Number of tiles the plater currently owns. Enemies own 6 minus this value.
    public int playerTileLength = 2; //Gives the player 0, 1, 2. Could be changed to be fed into the script to allow for different arena starts?
    private Vector2Int playerGridBoundaries;
    private Vector2Int enemyGridBoundaries;

    public bool isInitialized = false;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                float xPos = tileXDistance * x;
                float yPos = tileYDistance * y;
                
                Tile newTile = Instantiate(tile, new Vector3(xPos, yPos, 0), Quaternion.identity, this.gameObject.transform).GetComponent<Tile>();
                
                newTile.gridX = x;
                newTile.gridY = y;
                grid[x, y] = newTile;
                // 0, 1, 2
                if(x > playerTileLength) {
                    newTile.isPlayerTile = false;
                }
            }
            middle = tileXDistance * gridXLength /2;
        }
        // Calculate player and enemy grid boundaries.
        playerGridBoundaries = new Vector2Int(playerTileLength, this.grid.GetLength(1) - 1);
        enemyGridBoundaries = new Vector2Int(gridXLength-1, this.grid.GetLength(1) - 1);
        isInitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile getPlayerTile() {
        // TODO: get player's tile
        return grid[0,0];
    }

    public Tile GetTile(int x, int y) {
        return grid[x, y];
    }

    public void SetPlayerTileLength(int playerTileLength){
        this.playerTileLength = playerTileLength;

        // Re-calculate player and enemy grid boundaries.
        // Player limits
        // X -> length of player area
        // Y -> length Y of grid.
        playerGridBoundaries = new Vector2Int(playerTileLength, this.grid.GetLength(1) - 1);

        // Enemy limits
        // They need to look at playerTileLength to determine their left limit. 
        // X -> length of player area
        // Y -> length Y of grid.
        enemyGridBoundaries = new Vector2Int(gridXLength-1, this.grid.GetLength(1) - 1);

    }

    public Vector2Int getPlayerBoundaries(){
        return playerGridBoundaries;
    }

    public Vector2Int getEnemyBoundaries(){
        return enemyGridBoundaries;
    }   

    // Attempt to move an enemy into a tile, returns true if it succeeded?
    private readonly object lock_ = new object();
    public Tile moveEnemyIntoTile(EnemyAi entity, int x, int y){
            // We could synchronize at a different location if this ends up locking the game up. IE: Create a method on tiles to update the entity on it, and synchronize that.
        lock(lock_){
            Debug.Log("Attempting to move enemy to: x: " + x + ".   y: " + y);
            Tile potentialTile = grid[x,y];
            // If the tile is empty
            if(potentialTile.entityOnTile == null){
                // Remove the entity from it's old tile, add it to it's new tile.
                Debug.Log("Moving this entity onto a null tile.");

                // We need to null out the previous tile we were on.
                // Not sure if this if check is necessary, scared about the possibility.
                // If the entity on the previousTile was us, then null it out.
                if(grid[entity.gridPosition.x,entity.gridPosition.y].entityOnTile == entity.enemyGameObject){
                    Debug.Log("Freeing up old tile.");
                    grid[entity.gridPosition.x,entity.gridPosition.y].entityOnTile = null;
                }
                else{
                    Debug.Log("We tried to null out an entity on a tile that was not this entity.");
                }
                // Set up to the new tile.
                potentialTile.entityOnTile = entity.enemyGameObject;
                return potentialTile;
            }
            else{
                return null;
            }
        }
    }


    public Tile getEnemySpawnLocation(EnemyAi ai){
        lock(lock_){

            while(true){
                var xPos = Random.Range(playerTileLength+1, gridXLength-1);
                var yPos = Random.Range(0, 3);
                Debug.Log("Attempting to spawn enemy at: x: " + xPos + ".   y: " + yPos);
                Tile potentialTile = grid[xPos, yPos];
                if (potentialTile.entityOnTile == null){
                    potentialTile.entityOnTile = ai.enemyGameObject;
                    return potentialTile;
                }
            }
        }
    }
}
