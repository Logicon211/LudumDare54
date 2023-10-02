using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile2 : MonoBehaviour
{
    // Start is called before the first frame update

    // Should be set when it first instantiates
    public int attackDamage = 4;

    private BoxCollider2D boxCollider;

    public Tile originTile;
    private Tile damageTile1;
    private Tile damageTile2;
    private Tile damageTile3;

private ContactFilter2D filter;

    void Start()
    {
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
        filter = new ContactFilter2D();

        LayerMask mask = LayerMask.GetMask("Enemies");
        filter.SetLayerMask(mask);
        filter.useTriggers = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void damageEnemies(){
        // List<Collider2D> results = new List<Collider2D>();
        // boxCollider.OverlapCollider(filter, results);
        // foreach(Collider2D collision in results) {
        //     if (collision) {
        //         if (collision.gameObject.tag == "Enemy") {
        //             EnemyAi enemy = collision.gameObject.GetComponent<EnemyAi>();
        //             enemy.Damage(attackDamage);
        //         }
        //     }
        // }
        if(damageTile1){
            damageTile1.Damage(attackDamage);
        }
        if(damageTile2){
            damageTile2.Damage(attackDamage);
        }
        if(damageTile3){
            damageTile3.Damage(attackDamage);
        }
    }

    public void SetDamagingTiles(Tile tilein) {
        BattleGrid grid = tilein.GetGrid();
        int x = tilein.gridX;
        int y = tilein.gridY;

        if(x + 1 < 6) {
            damageTile1 = grid.grid[x+1, y];
        }
        if(x + 2 < 6) {
            damageTile2 = grid.grid[x+2, y];
        }
        if(x + 3 < 6) {
            damageTile3 = grid.grid[x+3, y];
        }
    }

    public void despawn(){
        Destroy(this.gameObject);
    }

}
