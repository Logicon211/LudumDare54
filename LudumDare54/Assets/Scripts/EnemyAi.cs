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
    protected BattleGrid battleGrid;
    
    //Prevent us from dying twice
    private bool isDead = false;

    //private Rigidbody2D enemyBody;
    // public GameObject enemyGameObject;

    public float decisionCooldown; // Hand in on initialization
    public float currentDecisionCooldown = 2.5f; // May need to be tweaked.
    public float attackCooldown; // Hand in on initialization
    public float currentAttackCooldown = 2f; // May need to be tweaked.
    public string enemyName;

    protected Animator animator;
    public List<GameObject> attacks;

    public TMPro.TMP_Text healthText;

    private GameManager gameManager;

    public float tileHeightOffset = 0f;
    public Transform attackLocation;

    protected AudioSource AS;
    public AudioClip attackSound;
    
    protected SpriteRenderer sprite;
    
    private bool debugLogging = false;

    private float killTimer = 1f;
    private float currentKillTimer = 1f;
    public GameObject deathExplosion;

    private void Awake() {

    }
    
    IEnumerator Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        battleGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<BattleGrid>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        yield return new WaitUntil(() => battleGrid.isInitialized);
        var spawnLocation = battleGrid.getEnemySpawnLocation(this);
        gridPosition.x = spawnLocation.gridX;
        gridPosition.y = spawnLocation.gridY;
        sprite.sortingOrder = 2 + (gridPosition.y * 3);
        gameObject.transform.position = new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y + tileHeightOffset, spawnLocation.transform.position.z);
        // TODO: Create teleport effect on spawn
        animator = gameObject.GetComponent<Animator>();
        AS = gameObject.GetComponent<AudioSource>();
    }

     private void Update () {
        if (isDead) {
            currentKillTimer -= Time.deltaTime;
            if(currentKillTimer <= 0f) {
                Destroy(gameObject);
            }
            return;
        }
         healthText.text = health.ToString();
    }

    protected void FixedUpdate() {
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
            if(debugLogging){
                Debug.Log("Decision time");
            }
            // If both cooldowns, check player position
            var playerTile = battleGrid.getPlayerTile();
            // If player position lined up
            var alignment = AlignedWithPlayer(playerTile);
            if(alignment == 0){
                if(debugLogging){
                    Debug.Log("Aligned with player");
                }
                if( currentAttackCooldown <= 0){
                    // If sight is clear
                    if(debugLogging){
                        Debug.Log("Attack time");
                    }
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
                    if(debugLogging){
                        Debug.Log("Attack on cooldown, move time");
                    }
                    Move(playerTile, alignment);
                }
            // else player position not lined up
            }
            else{
                if(debugLogging){
                    Debug.Log("Not lined up, move time");
                }
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
            gameManager.RemoveEnemyFromList(this.gameObject);
            isDead = true;
            battleGrid.GetTile(gridPosition).entityOnTile = null;
            if (deathExplosion != null) {
                Instantiate(deathExplosion, gameObject.transform.position, Quaternion.identity);
            }
            AddPhysics();
        }
    }

    // Negative means the player is lower
    // 0 means we are aligned
    // Positive means the player is higher
    protected int AlignedWithPlayer(Tile playerTile){
        var combined = gridPosition.y - playerTile.gridY;
        if(debugLogging){
            Debug.Log("Player Y: " + playerTile.gridY + "     Enemy Y: " + gridPosition.y + ". Outcome: " + combined);
        }
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

    protected virtual void Move(Tile playerTile, int alignment)
    {
        if(debugLogging){
            Debug.Log("Move Method reached");
        }
        legalMoves chosenMovement = legalMoves.NoLegalMove;

        // Build the list of legalMovement options. Pray we never conflict with another enemy.
        List<legalMoves> legalMoveList = determineLegalMoves();
        

        if(debugLogging){
            Debug.Log("We have: " + legalMoveList.Count + " Legal Moves.");
        }

        // We could end up in a corner with no legal moves.
        if(legalMoveList.Count == 0){
            // No action to take, vibrate in place?
        }
        // If player is aligned, then vision was unclear.
        else if(alignment == 0){
            if(debugLogging){
                Debug.Log("Aligned with player");
            }
            //Choose a random legal option.
            chosenMovement = legalMoveList[Random.Range(0, legalMoveList.Count)];
        }

        // Negative alignment -> player is lower
        else if(alignment < 0){
            if(debugLogging){
                Debug.Log("Lower than player");
            }
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
            if(debugLogging){
                Debug.Log("Higher than player");
            }

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
            if(debugLogging){
                Debug.Log("We had no legal moves");
            }
        }
        
        
        else {
            var newXposition = gridPosition.x;
            var newYposition = gridPosition.y;

            if(chosenMovement == legalMoves.Up){
                newYposition--;
                if(debugLogging){
                    Debug.Log("Attempting to move Up");
                }
            }
            else if(chosenMovement == legalMoves.Down){
                newYposition++;
                if(debugLogging){
                    Debug.Log("Attempting to move Down");
                }
            }
            else if(chosenMovement == legalMoves.Right){
                newXposition++;
                if(debugLogging){
                    Debug.Log("Attempting to move Right");
                }
            }
            else{
                newXposition--;
                if(debugLogging){
                    Debug.Log("Attempting to move Left");
                }
            }
            
            
            // Take that movement decision
            var potentialTile = battleGrid.moveEnemyIntoTile(this, newXposition, newYposition);
            
            // It didn't succeed for some reason.
            if(potentialTile == null){
                
                if(debugLogging){
                    Debug.Log("We were unable to move this enemy into a valid tile after having calculated legal tiles because it was occupied. Need to synchronize sooner.");
                }

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

    public void Attack(){
        // Choose a random attack from the (list) of passed in attacks
        animator.SetTrigger("AttackAnimation");

        // Reset both cooldowns
        currentDecisionCooldown = decisionCooldown;
        currentAttackCooldown = attackCooldown;
    }

    public virtual void SpawnAttack(){

        if(debugLogging){
            Debug.Log("attak spawned.");
        }

        AS.PlayOneShot(attackSound);
        GameObject projectile = Instantiate(attacks[Random.Range(0, attacks.Count)], attackLocation.position, Quaternion.identity);
        projectile.GetComponent<SpriteRenderer>().sortingOrder = (sprite.sortingOrder + 1);

    }

    // This method might belong on the BattleGrid instead.
    protected List<legalMoves> determineLegalMoves(){
        // Get the boundaries from the battleGrid.
        Vector2 legalBoundaries = battleGrid.getEnemyBoundaries();
        if(debugLogging){
            Debug.Log("maximum boundaries: x: " + legalBoundaries.x + " .   y: " + legalBoundaries.y );
        }
        
        // Get the tiles the player owns.
        var playerArea = battleGrid.playerTileLength;

        List<legalMoves> legalMoveList = new List<legalMoves>();

        // If our position is not the top, we can move up.
        if(gridPosition.y < legalBoundaries.y){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x, gridPosition.y+1].entityOnTile == null){
                legalMoveList.Add(legalMoves.Down);
                if(debugLogging){
                    Debug.Log("Down is a legal move");
                }
            } 
        }
        // If our position is not the bottom, we can move down.
        if(gridPosition.y > 0){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x, gridPosition.y-1].entityOnTile == null){
                legalMoveList.Add(legalMoves.Up);
                if(debugLogging){
                    Debug.Log("Up is a legal move");
                }
            } 
        }
        // If our x position is not at the rightmost enemy boundary, we can move right.
        if(gridPosition.x < legalBoundaries.x){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x+1, gridPosition.y].entityOnTile == null){
                legalMoveList.Add(legalMoves.Right);  
                if(debugLogging){
                    Debug.Log("Right is a legal move");
                }
            } 
        }
        // If our x position is not at the playerArea limit.
        if(gridPosition.x != playerArea+1){
            // And the location is empty
            if(battleGrid.grid[gridPosition.x-1, gridPosition.y].entityOnTile == null){
                legalMoveList.Add(legalMoves.Left);
                if(debugLogging){
                    Debug.Log("Left is a legal move");
                }
            } 
        }

        return legalMoveList;
    }

    protected enum legalMoves {
        Up,
        Right,
        Down,
        Left,
        NoLegalMove
    }

    private void AddPhysics() {
        Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();

        Vector2 vec = new Vector2(75.0f, Random.Range(-50f, 50f));

        // rb.SetRotation(90f);
        // rb.AddForce(transform.right * 2000f);
        rb.isKinematic = true;
        rb.velocity = vec;
        rb.AddTorque(100f);
    }

}
