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
            
            BarrelProjectileLD54 bpObject = Instantiate(projectile, new Vector3(targetTile.GetTransform().x, targetTile.GetTransform().y + 1, targetTile.GetTransform().z), Quaternion.identity).GetComponent<BarrelProjectileLD54>();
            bpObject.InitializeBarrel(targetTile, grid);

            bpObject.GetComponent<SpriteRenderer>().sortingOrder = 4 + (targetTile.gridY * 3);
            // targetTile.entityOnTile = gameObject;
            // targetTile.bombOnTile = gameObject;
        }
    }

    public void spawnBarrel(GameObject player){

    }

}
