using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile2 : MonoBehaviour
{
    // Start is called before the first frame update

    // Should be set when it first instantiates
    private int attackDamage = 3;

    private BoxCollider2D boxCollider;

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
        List<Collider2D> results = new List<Collider2D>();
        boxCollider.OverlapCollider(filter, results);
        foreach(Collider2D collision in results) {
            if (collision) {
                if (collision.gameObject.tag == "Enemy") {
                    EnemyAi enemy = collision.gameObject.GetComponent<EnemyAi>();
                    enemy.Damage(attackDamage);
                }
            }
        }
    }

    public void despawn(){
        Destroy(this.gameObject);
    }

}
