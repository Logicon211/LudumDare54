using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEnemy : EnemyAi
{
    // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

    public override void SpawnAttack(){

        Debug.Log("attak spawned.");

        GameObject projectile = Instantiate(attacks[Random.Range(0, attacks.Count)], attackLocation.position, Quaternion.identity);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = (sprite.sortingOrder + 1);
    }

    public void StartAttackSound() {
        if(!AS.isPlaying) {
            AS.Play();
        }
    }

    public void StopAttackSound() {
        AS.Stop();
    }
}
