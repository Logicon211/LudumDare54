using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Interface;


public class EnemyAi: MonoBehaviour, IDamageable<int>, IKillable
{
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

    private void Awake() {
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
        
        //Countdown attack cooldown

        //if both cooldowns, check player position
        var playerPos = battleGrid.getPlayerTile();
            //If player position lined up
                var alignment = AlignedWithPlayer(playerPos);
                if(alignment == 0){
                    // If sight is clear
                    VisionToPlayer(playerPos);
                        //Attack, reset both cooldowns
    
                }
            //else move

                //Reset decision cooldown
        // else if decision cooldown
            //if player position not lined up
                //move
            //else
                //Do nothing, reset decision cooldown but halved?
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

    private int AlignedWithPlayer(Tile playerTile){
        return playerTile.gridY - yPosition;
    }

    private bool VisionToPlayer(Tile playerTile){
        for(int x = xPosition-1; x> playerTile.gridX; x--) {
            if(battleGrid.grid[x-1, yPosition].entityOnTile != null){
                return false;
            }
        }

        return true;
    }

    public void Move(Tile playerTile)
    {
        // Determine difference between player and us.        
        int yMove = AlignedWithPlayer(playerTile);

        // If that's not zero, let's take that move if it's legal, otherwise continue.


        // If that's zero, lets move forward or backwards if it's legal. Otherwise do nothing. Stretch goal, vibrate? shake? indicate you couldn't move.



        // Determine legal moves
            // determine edges
                // 


        //


        // get player position
    }


}
