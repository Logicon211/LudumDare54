using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemy : EnemyAi
{

    public override void SpawnAttack(){

        Debug.Log("attak spawned.");

        AS.PlayOneShot(attackSound);
        // TELEPORT SOME BOMBS
        Tile bombSpawnTile = battleGrid.getEnemyBombSpawnLocation();
        GameObject bomb = Instantiate(attacks[0], new Vector3(bombSpawnTile.transform.position.x, bombSpawnTile.transform.position.y, bombSpawnTile.transform.position.z), Quaternion.identity);
        bombSpawnTile.entityOnTile = bomb;
    }
}
