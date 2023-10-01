using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemy : EnemyAi
{

    public override void SpawnAttack(){

        Debug.Log("attak spawned.");

        AS.PlayOneShot(attackSound);
        // TELEPORT SOME BOMBS
        // Instantiate(attacks[Random.Range(0, attacks.Count)], attackLocation.position, Quaternion.identity);

    }
}
