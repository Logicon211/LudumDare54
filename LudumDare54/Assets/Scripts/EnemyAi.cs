using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Interface;


public class EnemyAi: MonoBehaviour, IDamageable<int>, IKillable
{


    public Vector2Int gridPosition;

    public int health;

    //Player tracker (might not be needed, could just get player position from grid)
    private GameObject player;
    private BattleGrid battleGrid;
    
    //Prevent us from dying twice
    private bool isDead = false;

    //private Rigidbody2D enemyBody;
    public GameObject enemyGameObject;

    public float decisionCooldown; // Hand in on initialization
    public float currentDecisionCooldown = 2.5f; // May need to be tweaked.
    public float attackCooldown; // Hand in on initialization
    public float currentAttackCooldown = 2f; // May need to be tweaked.

    private Animator animator;
    public List<GameObject> attacks;


    private void Awake() {
        //TODO play spawn animation to hide jankyness

        //enemyGameObject = GetComponent<GameObject>();
        //enemyBody = GetComponent<Rigidbody2D>();
        // audio = GetComponent<AudioSource>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // player = GameObject.FindGameObjectWithTag("Player");
        // // RB = this.GetComponent<Rigidbody2D>();
        // // //TODO hand in grid correctly
        // battleGrid = this.GetComponent<BattleGrid>();
        // var spawnLocation = battleGrid.getEnemySpawnLocation(this);
        // xPosition = spawnLocation.gridX;
        // yPosition = spawnLocation.gridY;
        // enemyGameObject.transform.position = spawnLocation.transform.position;
    }
    
    IEnumerator Start()
    {
        // Instantiate(poofEffect, transform.position, Quaternion.identity);
        // currentHealth = health;
        // originalXScale = this.transform.localScale.x;
                player = GameObject.FindGameObjectWithTag("Player");
        // RB = this.GetComponent<Rigidbody2D>();
        // //TODO hand in grid correctly
        battleGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>();
        yield return new WaitUntil(() => battleGrid.isInitialized);
        var spawnLocation = battleGrid.getEnemySpawnLocation(this);
        gridPosition.x = spawnLocation.gridX;
        gridPosition.y = spawnLocation.gridY;
        enemyGameObject.transform.position = spawnLocation.transform.position;
        animator = enemyGameObject.GetComponent<Animator>();

    }

     private void Update () {

    }

    private void FixedUpdate() {
        //Countdown decision cooldown
        if (currentDecisionCooldown > 0f) {
            currentDecisionCooldown -= Time.deltaTime;
        }

        //Countdown attack cooldown
        if (currentAttackCooldown > 0f) {
            currentAttackCooldown -= Time.deltaTime;
        }


        if( currentDecisionCooldown <= 0){
            Debug.Log("Decision time");
            // If both cooldowns, check player position
            var playerTile = battleGrid.getPlayerTile();
            // If player position lined up
            var alignment = AlignedWithPlayer(playerTile);
            if(alignment == 0){
                Debug.Log("Aligned with player");
                if( currentAttackCooldown <= 0){
                    // If sight is clear
                    if(ClearLineToPlayer(playerTile)){
                        Debug.Log("Attack time");
                        Attack();
                    }
                    else{
                        Debug.Log("Aligned, off cooldown, no line of sight, move time");
                        Move(playerTile, alignment);
                    }
                }
                // else move
                else{
                    Debug.Log("Attack on cooldown, move time");
                    Move(playerTile, alignment);
                }
            // else player position not lined up
            }
            else{
                Debug.Log("Not lined up, move time");
                Move(playerTile, alignment);
            }
        }
    }

    // Incoming damage
    public void Damage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0f && !isDead) {
            Kill();
        }
        else{
            //Instantiate(hitEffect, transform.position, Quaternion.identity);
        }   
    }

    
    public void Kill()
    {
        if(!isDead) {
            isDead = true;
            Destroy(gameObject);
            
        }
    }

    // Negative means the player is lower
    // 0 means we are aligned
    // Positive means the player is higher
    private int AlignedWithPlayer(Tile playerTile){
        var combined = gridPosition.y - playerTile.gridY;
        Debug.Log("Player Y: " + playerTile.gridY + "     Enemy Y: " + gridPosition.y + ". Outcome: " + combined);
        return playerTile.gridY - gridPosition.y;
    }

    private bool ClearLineToPlayer(Tile playerTile){

        // Worried this logic is incorrect, too many changes to x during loop, should calculate it up front.
        // For as many spaces as there are between this character (minus one to not count ourselves) and the player character. 
        for(int x = gridPosition.x-1; x> playerTile.gridX; x--) {
            
            // Get the entity on that tile (could be null)
            var entity = battleGrid.grid[x, gridPosition.y].entityOnTile;
            //If the field has an entity on it.
            if(entity != null){
                // Is it the player?
                if(entity == player){
                    return true;
                }
                return false;
            }
        }
        return true;
    }

    public void Move(Tile playerTile, int alignment)
    {
        Debug.Log("Move Method reached");
        legalMoves chosenMovement = legalMoves.NoLegalMove;

        // Build the list of legalMovement options. Pray we never conflict with another enemy.
        List<legalMoves> legalMoveList = determineLegalMoves();
        

        Debug.Log("We have: " + legalMoveList.Count + " Legal Moves.");

        // We could end up in a corner with no legal moves.
        if(legalMoveList.Count == 0){
            // No action to take, vibrate in place?
        }
        // If player is aligned, then vision was unclear.
        else if(alignment == 0){
            Debug.Log("Aligned with player");
            //Choose a random legal option.
            chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
        }

        // Negative alignment -> player is lower
        else if(alignment < 0){
            Debug.Log("Lower than player");
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
            Debug.Log("Higher than player");
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
            Debug.Log("We had no legal moves");
        }
        
        
        else {
            var newXposition = gridPosition.x;
            var newYposition = gridPosition.y;

            if(chosenMovement == legalMoves.Up){
                newYposition--;
                Debug.Log("Attempting to move Up");
            }
            else if(chosenMovement == legalMoves.Down){
                newYposition++;
                Debug.Log("Attempting to move Down");
            }
            else if(chosenMovement == legalMoves.Right){
                newXposition++;
                Debug.Log("Attempting to move Right");
            }
            else{
                newXposition--;
                Debug.Log("Attempting to move Left");
            }
            
            
            // Take that movement decision
            var potentialTile = battleGrid.moveEnemyIntoTile(this, newXposition, newYposition);
            
            // It didn't succeed for some reason.
            if(potentialTile == null){
                
                Debug.Log("We were unable to move this enemy into a valid tile after having calculated legal tiles because it was occupied. Need to synchronize sooner.");
                
                currentDecisionCooldown = decisionCooldown / 2;
            }
            // It succeeded
            else{
                gridPosition.x = potentialTile.gridX;
                gridPosition.y = potentialTile.gridY;
                enemyGameObject.transform.position = potentialTile.transform.position;
                currentDecisionCooldown = decisionCooldown;
            }
        }
    }

    public void Attack(){
        // Choose a random attack from the (list) of passed in attacks
        animator.SetTrigger("AttackAnimation");

        // Reset both cooldowns
        currentDecisionCooldown = decisionCooldown;
        currentAttackCooldown = attackCooldown;
    }

    public void SpawnAttack(){

        Debug.Log("Skeleton attak spawned.");

        Instantiate(attacks[Random.Range(0, attacks.Count)], transform.position, Quaternion.identity);

    }

    // This method might belong on the BattleGrid instead.
    private List<legalMoves> determineLegalMoves(){
        // Get the boundaries from the battleGrid.
        Vector2 legalBoundaries = battleGrid.getEnemyBoundaries();
        Debug.Log("maximum boundaries: x: " + legalBoundaries.x + " .   y: " + legalBoundaries.y );
        // Get the tiles the player owns.
        var playerArea = battleGrid.playerTileLength;

        List<legalMoves> legalMoveList = new List<legalMoves>();

        // If our position is not the top, we can move up.
        if(gridPosition.y < legalBoundaries.y){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x, gridPosition.y+1].entityOnTile == null){
                legalMoveList.Add(legalMoves.Down);
                Debug.Log("Down is a legal move");
            } 
        }
        // If our position is not the bottom, we can move down.
        if(gridPosition.y > 0){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x, gridPosition.y-1].entityOnTile == null){
                legalMoveList.Add(legalMoves.Up);
                                Debug.Log("Up is a legal move");
            } 
        }
        // If our x position is not at the rightmost enemy boundary, we can move right.
        if(gridPosition.x < legalBoundaries.x){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x+1, gridPosition.y].entityOnTile == null){
                legalMoveList.Add(legalMoves.Right);  
                                Debug.Log("Right is a legal move");
            } 
        }
        // If our x position is not at the playerArea limit.
        if(gridPosition.x != playerArea+1){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x-1, gridPosition.y].entityOnTile == null){
                legalMoveList.Add(legalMoves.Left);
                                Debug.Log("Left is a legal move");
            } 
        }

        return legalMoveList;
    }

    private enum legalMoves {
        Up,
        Right,
        Down,
        Left,
        NoLegalMove
    }

}
