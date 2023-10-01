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

    public override void useAbility(GameObject player) {
        grid = player.GetComponent<PlayerCharacter>().GetGrid();
        originTile = grid.getPlayerTile();
        Tile targetTile = grid.GetTile(originTile.gridX + range, originTile.gridY);
        if (targetTile.bombOnTile == null)
        {
            Debug.Log("Chad: Placing barrel kill me");
            BarrelProjectile bpObject = Instantiate(projectile, targetTile.GetTransform(), Quaternion.identity).GetComponent<BarrelProjectile>();
            bpObject.InitializeBarrel(targetTile, grid);
            // targetTile.entityOnTile = gameObject;
            // targetTile.bombOnTile = gameObject;
        }
    }
}
