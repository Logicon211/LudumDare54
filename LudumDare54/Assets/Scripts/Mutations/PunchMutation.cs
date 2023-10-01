using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PunchMutation : Mutation
{
    // how many columns ahead
    public int range = 1;
    // how many rows it affects
    public int columnRange = 3;
    public int damage = 5;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2Int GetGridBoundaries(BattleGrid grid)
    {
        return new Vector2Int(grid.grid.GetLength(0) - 1, grid.grid.GetLength(1) - 1);
    }

    bool IsValidTile(BattleGrid grid, Vector2Int newPosition)
    {
        Vector2Int boundaries = GetGridBoundaries(grid);
        // Check if its in the boundaries
        if (newPosition.x < 0 || newPosition.x > boundaries.x || newPosition.y < 0 || newPosition.y > boundaries.y ) {
            Debug.Log("Invalid Tile, wont attack " + newPosition);
            return false;
        }
        return true;
    }

    public override void useAbility(GameObject playerRef) {
        BattleGrid grid = playerRef.GetComponent<PlayerCharacter>().GetGrid();
        Tile originTile = grid.getPlayerTile();

        int originX = originTile.gridX;
        int originY = originTile.gridY;

        // check that the target tiles are valid then damage, go from top to bottom
        for (int y = originY - 1; y <= originY + 1; y++) {
            Vector2Int vec = new Vector2Int(originX + 1, y);
            if (IsValidTile(grid, vec)) {
                grid.GetTile(vec).Damage(damage);
                Instantiate(projectile, grid.GetTile(originX, y).GetTransform(), Quaternion.identity);
            }
        }
    }
}
