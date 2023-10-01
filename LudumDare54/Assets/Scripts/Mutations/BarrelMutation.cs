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
        Tile targetTile = grid.GetTile(originTile.gridX + range, originTile.gridY);
        if (targetTile.entityOnTile != null)
        { 
            GameObject proj = Resources.Load("BarrelProjectile") as GameObject;
            BarrelProjectile bpObject = Instantiate(proj, targetTile.GetTransform(), Quaternion.identity).GetComponent<BarrelProjectile>();
            bpObject.InitializeBarrel(targetTile, grid);
        }
    }
}