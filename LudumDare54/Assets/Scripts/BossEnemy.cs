using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : EnemyAi
{
    // Start is called before the first frame update
//     void Start()
//     {
        
//     }


    public List<GameObject> gunAttacks;
    public float movementCooldown = 0.5f;
    public GameObject fistAttack;

    // Update is called once per frame
    protected void FixedUpdate()
    {
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
    
        // Is player position lined up
        var alignment = AlignedWithPlayer(playerTile);

            // If our attack is off cooldown we are doing an attack.
            if( currentAttackCooldown <= 0){
                Attack(alignment);
            }
            else{
                Move(playerTile, alignment);
            }
        // else player position not lined up
        }
    }


    public void Attack(int alignment){
        // Choose a random attack from the (list) of passed in attacks

        //Gun attack if we are lined up.
        if(alignment == 0){
            animator.SetTrigger("GunAttack");
        }



        animator.SetTrigger("OtherAttack");

        // Reset both cooldowns
        currentDecisionCooldown = decisionCooldown;
        currentAttackCooldown = attackCooldown;
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
            chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
        }

        // Negative alignment -> player is lower
        else if(alignment < 0){
            // Move down if that is a legal move.
            if(legalMoveList.Contains(legalMoves.Up)){
                chosenMovement = legalMoves.Up;
            }
            else{
                //Choose a random legal option.
                chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
            }
        }
        // Positive alignment -> player is higher
        else if(alignment > 0){

            // Move Up if that is a legal move.
            if(legalMoveList.Contains(legalMoves.Down)){
                chosenMovement = legalMoves.Down;
            }
            else{
                //Choose a random legal option.
                chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
            }
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

                currentDecisionCooldown = movementCooldown / 2;
            }
            // It succeeded
            else{
                gridPosition.x = potentialTile.gridX;
                gridPosition.y = potentialTile.gridY;

                gameObject.transform.position = new Vector3(potentialTile.transform.position.x, potentialTile.transform.position.y + tileHeightOffset, potentialTile.transform.position.z);

                currentDecisionCooldown = movementCooldown;

                sprite.sortingOrder = 2 + (gridPosition.y * 3);

            }
        }
    }

    public void spawnFistAttack(){

    }


    public void SummonFist(){
        Tile currentPlayerTile = battleGrid.getPlayerTile();
        GameObject fist = Instantiate(fistAttack, currentPlayerTile.transform.position, Quaternion.identity);
       // projectile.getComponent(battleGrid.getPlayerTile;
        fist.GetComponent<fistAttackLD54>().targetTile = currentPlayerTile;
    }


    public void SpawnGunAttack(){

        Debug.Log("attack spawned.");
        // Spawn his three bullet projectiles.
        foreach (GameObject attack in gunAttacks)
        {
            GameObject projectile = Instantiate(attack, attackLocation.position, Quaternion.identity);
            projectile.GetComponent<SpriteRenderer>().sortingOrder = (sprite.sortingOrder + 1);
        }


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
