using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BarrelMutation : Mutation
{
    private int range = 3;
    public GameObject projectile;
    public BattleGrid grid;
    public Tile originTile;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void useAbility() {
        // get target tile
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>();
        Debug.Log("chad test grid: " + grid);
        originTile = grid.getPlayerTile();
        Tile targetTile = grid.GetTile(originTile.gridX + range, originTile.gridY);
        Debug.Log("chad test tile: " + targetTile.entityOnTile);
        if (targetTile.entityOnTile == null)
        { 
            Debug.Log("Placing barrel");
            BarrelProjectile bpObject = Instantiate(projectile, targetTile.GetTransform(), Quaternion.identity).GetComponent<BarrelProjectile>();
            bpObject.InitializeBarrel(targetTile, grid);
            targetTile.entityOnTile = gameObject;
        }
    }
}
