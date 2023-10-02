using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemy : EnemyAi
{

    private static readonly object lock_ = new object();


    public void Awake() {
        currentAttackCooldown += Random.Range(-1f, 2f);
        currentDecisionCooldown = Random.Range(-1f, 1f);
    }
    public override void SpawnAttack(){

        Debug.Log("attak spawned.");
        lock(lock_){
            if(!isDead) {
                AS.PlayOneShot(attackSound);
                // TELEPORT SOME BOMBS
                Tile bombSpawnTile = battleGrid.getEnemyBombSpawnLocation();
                if(bombSpawnTile){
                    GameObject bomb = Instantiate(attacks[0], new Vector3(bombSpawnTile.transform.position.x, bombSpawnTile.transform.position.y, bombSpawnTile.transform.position.z), Quaternion.identity);
                    bomb.GetComponent<RobotBomb>().SetTile(bombSpawnTile);
                }
            }
        }
    }

    protected override void FixedUpdate() {
        if (isDead) {
            return;
        }

        //Countdown decision cooldown
        if (currentDecisionCooldown > 0f) {
            currentDecisionCooldown -= Time.deltaTime;
        }

        //Countdown attack cooldown
        if (currentAttackCooldown > 0f) {
            currentAttackCooldown -= Time.deltaTime;
        }


        if( currentDecisionCooldown <= 0){
            // If both cooldowns, check player position
            var playerTile = battleGrid.getPlayerTile();
            // If player position lined up
            var alignment = AlignedWithPlayer(playerTile);

            if( currentAttackCooldown <= 0){

                Attack();

                // if(ClearLineToPlayer(playerTile)){
                //     Debug.Log("Attack time");
                //     Attack();
                // }
                // else{
                //     Debug.Log("Aligned, off cooldown, no line of sight, move time");
                //     Move(playerTile, alignment);
                // }
            }
            // else move
            else{
                Move(playerTile, alignment);
            }
            // else player position not lined up
        }
    }

    protected override void Move(Tile playerTile, int alignment)
    {
        legalMoves chosenMovement = legalMoves.NoLegalMove;

        // Build the list of legalMovement options. Pray we never conflict with another enemy.
        List<legalMoves> legalMoveList = determineLegalMoves();

        // We could end up in a corner with no legal moves.
        if(legalMoveList.Count == 0){
            // No action to take, vibrate in place?
        }
        // If player is aligned, then vision was unclear.
        else if(alignment == 0){
            //Choose a random legal option.
            if(legalMoveList.Contains(legalMoves.Up)){
                chosenMovement = legalMoves.Up;
            } else if(legalMoveList.Contains(legalMoves.Down)){
                chosenMovement = legalMoves.Down;
            }else {
                chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
            }
        }

        // Negative alignment -> player is lower
        else if(alignment < 0){
            chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
        }
        // Positive alignment -> player is higher
        else if(alignment > 0){
            chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
        }

        
        if(chosenMovement == legalMoves.NoLegalMove) {
            // We had no legal moves.
            // TODO vibrate in place to indicate we couldn't move?
            // Decision cooldown halved.
            currentDecisionCooldown = decisionCooldown / 2;
        }
        
        
        else {
            var newXposition = gridPosition.x;
            var newYposition = gridPosition.y;

            if(chosenMovement == legalMoves.Up){
                newYposition--;
            }
            else if(chosenMovement == legalMoves.Down){
                newYposition++;
            }
            else if(chosenMovement == legalMoves.Right){
                newXposition++;
            }
            else{
                newXposition--;
            }
            
            
            // Take that movement decision
            var potentialTile = battleGrid.moveEnemyIntoTile(this, newXposition, newYposition);
            
            // It didn't succeed for some reason.
            if(potentialTile == null){

                currentDecisionCooldown = decisionCooldown / 2;
            }
            // It succeeded
            else{
                gridPosition.x = potentialTile.gridX;
                gridPosition.y = potentialTile.gridY;

                gameObject.transform.position = new Vector3(potentialTile.transform.position.x, potentialTile.transform.position.y + tileHeightOffset, potentialTile.transform.position.z);

                currentDecisionCooldown = decisionCooldown;

                sprite.sortingOrder = 2 + (gridPosition.y * 3);

            }
        }
    }
}
