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
    private int health;

    //Player tracker (might not be needed, could just get player position from grid)
    private GameObject player;
    private BattleGrid battleGrid;

    public GameObject healthPickup;
    
    //Prevent us from dying twice
    private bool isDead = false;

    private Rigidbody2D enemyBody;
    public GameObject enemyGameObject;

    private float decisionCooldown; // Hand in on initialization
    private float currentDecisionCooldown = 2.5f; // May need to be tweaked.
    private float attackCooldown; // Hand in on initialization
    private float currentAttackCooldown = 2f; // May need to be tweaked.


    private void Awake() {
        enemyGameObject = GetComponent<GameObject>();
        enemyBody = GetComponent<Rigidbody2D>();
        // audio = GetComponent<AudioSource>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        // RB = this.GetComponent<Rigidbody2D>();
        // //TODO hand in grid correctly
        battleGrid = this.GetComponent<BattleGrid>();
    }
    
    private void Start()
    {
        // Instantiate(poofEffect, transform.position, Quaternion.identity);
        // currentHealth = health;
        // originalXScale = this.transform.localScale.x;
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
        
        var chosenMovement;

        // Build the list of legalMovement options. Pray we never conflict with another enemy.
        var legalMoveList = determineLegalMoves();
        // If player is aligned, then vision was unclear.
            //Choose a random legal option.
        if(alignment == 0){
            var index = random.Next(legalMoveList.Count);
            chosenMovement = legalMoveList[index];
        }

        if(alignment <){
            
        }
        // If player is unaligned, let's move towards the player.
            // If the player is unaligned, and we cannot move towards the player, move either backwards or forwards.

        
        // If the player is aligned, 




        // If that's not zero, let's take that move if it's legal, otherwise continue.


        // If that's zero, lets move forward or backwards if it's legal. Otherwise do nothing. Stretch goal, vibrate? shake? indicate you couldn't move.



        // Determine legal moves
            // determine edges
                // 

        //battleGrid.moveEnemyIntoTile();

        // get player position
        // Reset decision cooldown
        currentDecisionCooldown = decisionCooldown;
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
        Left = -1
    }

}
