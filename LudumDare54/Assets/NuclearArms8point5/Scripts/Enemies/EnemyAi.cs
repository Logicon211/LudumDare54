using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Interface;


public class EnemyAi: MonoBehaviour, IDamageable<int>, IKillable
{


    public Vector2Int gridPosition;
    //1, 2, 3
    private int xPosition;
    //1, 2, 3
    private int yPosition;
    public int health;

    //Player tracker (might not be needed, could just get player position from grid)
    private GameObject player;
    public BattleGrid battleGrid;
    
    //Prevent us from dying twice
    private bool isDead = false;

    //private Rigidbody2D enemyBody;
    public GameObject enemyGameObject;

    private float decisionCooldown; // Hand in on initialization
    private float currentDecisionCooldown = 2.5f; // May need to be tweaked.
    private float attackCooldown; // Hand in on initialization
    private float currentAttackCooldown = 2f; // May need to be tweaked.


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
    
    private void Start()
    {
        // Instantiate(poofEffect, transform.position, Quaternion.identity);
        // currentHealth = health;
        // originalXScale = this.transform.localScale.x;
                player = GameObject.FindGameObjectWithTag("Player");
        // RB = this.GetComponent<Rigidbody2D>();
        // //TODO hand in grid correctly
        battleGrid = battleGrid.GetComponent<BattleGrid>();
        var spawnLocation = battleGrid.getEnemySpawnLocation(this);
        xPosition = spawnLocation.gridX;
        yPosition = spawnLocation.gridY;
        enemyGameObject.transform.position = spawnLocation.transform.position;
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

            // If both cooldowns, check player position
            var playerTile = battleGrid.getPlayerTile();
            // If player position lined up
            var alignment = AlignedWithPlayer(playerTile);
            if(alignment == 0){
                if( currentAttackCooldown <= 0){
                    // If sight is clear
                    if(ClearLineToPlayer(playerTile)){
                        Attack();
                    }
                    else{
                        Move(playerTile, alignment);
                    }
                }
                // else move
                else{
                    Move(playerTile, alignment);
                }
            // else player position not lined up
            }
            else{
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
            if(legalMoveList.Contains(legalMoves.Down)){
                chosenMovement = legalMoves.Down;
            }
            else{
                //Choose a random legal option.
                chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
            }
        }
        // Positive alignment -> player is higher
        else if(alignment > 0){
            // Move Up if that is a legal move.
            if(legalMoveList.Contains(legalMoves.Up)){
                chosenMovement = legalMoves.Up;
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
                newYposition++;
            }
            else if(chosenMovement == legalMoves.Down){
                newYposition--;
            }
            else if(chosenMovement == legalMoves.Right){
                newXposition++;
            }
            else{
                newXposition++;
            }
            
            
            // Take that movement decision
            var potentialTile = battleGrid.moveEnemyIntoTile(this, newXposition, newYposition);
            
            if(potentialTile == null){
                
                Debug.Log("We were unable to move this enemy into a valid tile after having calculated legal tiles because it was occupied. Need to synchronize sooner.");
                
                currentDecisionCooldown = decisionCooldown / 2;
            }
            else{
                enemyGameObject.transform.position = potentialTile.transform.position;
                currentDecisionCooldown = decisionCooldown;
            }
        }
    }

    public void Attack(){
        // Choose a random attack from the (list) of passed in attacks

        // Reset both cooldowns
        currentDecisionCooldown = decisionCooldown;
        currentAttackCooldown = attackCooldown;
    }

    // This method might belong on the BattleGrid instead.
    private List<legalMoves> determineLegalMoves(){
        // Get the boundaries from the battleGrid.
        Vector2 legalBoundaries = battleGrid.getEnemyBoundaries();
        // Get the tiles the player owns.
        var playerArea = battleGrid.playerTileLength;

        List<legalMoves> legalMoveList = new List<legalMoves>();
        if(gridPosition.y == legalBoundaries.y){
            // Do nothing
        }
        // If our position is not the top, we can move up.
        if(gridPosition.y < legalBoundaries.y){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x, gridPosition.y+1].entityOnTile == null){
                legalMoveList.Add(legalMoves.Up);
            } 
        }
        // If our position is not the bottom, we can move down.
        if(gridPosition.y > 0){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x, gridPosition.y-1].entityOnTile == null){
                legalMoveList.Add(legalMoves.Down);
            } 
        }
        // If our x position is not at the rightmost enemy boundary, we can move right.
        if(gridPosition.x < legalBoundaries.x){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x+1, gridPosition.y].entityOnTile == null){
                legalMoveList.Add(legalMoves.Right);  
            } 
        }
        // If our x position is not at the playerArea limit.
        if(gridPosition.x > playerArea){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x-1, gridPosition.y].entityOnTile == null){
                legalMoveList.Add(legalMoves.Left);
            } 
        }

        return legalMoveList;
    }

    private enum legalMoves : int {
        Up = 1,
        Right = 1,
        Down = -1,
        Left = -1,
        NoLegalMove = 0
    }

}
