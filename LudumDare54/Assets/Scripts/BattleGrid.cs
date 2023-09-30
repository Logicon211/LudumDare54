using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    public Tile[,] grid = new Tile[6,3];
    public GameObject tile;

    private float tileXDistance = 2.5f;
    private float tileYDistance = -1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < grid.GetLength(0); x++) {
            for (int y = 0; y < grid.GetLength(1); y++) {
                float xPos = tileXDistance * x;
                float yPos = tileYDistance * y;
                Tile newTile = Instantiate(tile, new Vector3(xPos, yPos, 0), Quaternion.identity).GetComponent<Tile>();
                newTile.gridX = x;
                newTile.gridY = y;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tile getPlayerTile() {
        // TODO: get player's tile
        return grid[0,0];
    }
}
