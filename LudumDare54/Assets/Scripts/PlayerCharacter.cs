using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Which way to move the character, not used anymore
    enum Direction {Up, Right, Down, Left, None};

    public int health;
    public Vector2Int position = new Vector2Int();
    public GameObject hitEffect;

    public BattleGrid grid;
    public GameManager manager;

    public float MovementCooldown = .05f;
    public float CurrentMovementCooldown = 0f;

    public TMPro.TMP_Text healthText;

    // temp until we get something better
    private bool isDead = false;
    private Tile currentTile;
    public SpriteRenderer playerSprite;
    
    private Animator animator;

    public GameObject nextMutationQueuedIcon;
    private SpriteRenderer nextMutationQueuedSprite;

    private AudioSource AS;
    public AudioClip laserNoise;
    public AudioClip hurtNoise;

    public GameObject shield;
    public TMPro.TMP_Text shieldHPText;
    private float shieldHP = 0;
    public AudioClip shieldDeflect;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Grid");
        GameObject m = GameObject.FindGameObjectWithTag("GameController");

        animator = gameObject.GetComponentInChildren<Animator>();
        playerSprite = gameObject.GetComponentInChildren<SpriteRenderer>();

        if (g != null) {
            Debug.Log("grid not found");
            grid = g.GetComponent<BattleGrid>();
        }
        if (m != null) {
            manager = m.GetComponent<GameManager>();
        }
        yield return new WaitUntil(() => grid.isInitialized);
        position.Set(1, 1);
        currentTile = grid.GetTile(1, 1);
        MovePlayerObject();
        grid.SetPlayer(gameObject.GetComponent<PlayerCharacter>());
        playerSprite.enabled = true;
        nextMutationQueuedSprite = nextMutationQueuedIcon.GetComponent<SpriteRenderer>();
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!manager.IsPaused()) {
            Move();
            Attack();
        }
        if(healthText) {
            healthText.text = "HP: " + health.ToString();
        }
        if(nextMutationQueuedSprite) {
            if (manager.mutationQueue.Count > 0) {
                nextMutationQueuedSprite.sprite = manager.mutationQueue[0].icon;
            } else {
                nextMutationQueuedSprite.sprite = null;
            }
        }

        if(shieldHP >= 1) {
            shield.SetActive(true);
            shieldHP -= Time.deltaTime;
            shieldHPText.text = ((int)shieldHP).ToString();
        } else {
            shield.SetActive(false);
            shieldHP = 0;
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Mouse0)) {
            RegularAttack();
        }
        if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.Mouse1)) {
            SpecialAttack();
        }
    }

    void RegularAttack()
    {
        animator.SetTrigger("BasicAttack");
        for (int x = position.x + 1; x < grid.grid.GetLength(0) ; x++) {
            Tile attackTile = grid.GetTile(x, position.y);
            if (attackTile.entityOnTile != null )
            {
                Debug.Log("Enemy hit");
                EnemyAi enemy = attackTile.entityOnTile.GetComponent<EnemyAi>();
                if(enemy) {
                    enemy.Damage(1);
                }
                RobotBomb bomb = attackTile.entityOnTile.GetComponent<RobotBomb>();
                if(bomb) {
                    bomb.Damage(1);
                }

                if(!bomb && !enemy){
                    continue;
                }

                AS.PlayOneShot(laserNoise);

                Instantiate(hitEffect, new Vector3(Random.Range(-0.4f, 0.4f)+attackTile.GetTransform().x, Random.Range(0f, 0.9f) + attackTile.GetTransform().y, 0), Quaternion.identity);
                //Random.Range(-0.5f, 0.4f)
                break;
            }
        }
    }

    void SpecialAttack()
    {
        if (manager.mutationQueue.Count > 0)
        {
            Debug.Log("Using " + manager.mutationQueue[0].GetName());
            manager.mutationQueue[0].useAbility(gameObject);
            manager.mutationQueue.RemoveAt(0);
            Debug.Log("Mutations left: " + manager.mutationQueue.Count);
        }
        else 
        {
            Debug.Log("Can't use special: No Mutations Left");
        }
    }

    /*
        Fun time movement stuff
    */
    void Move()
    {
        Vector2Int newPosition = GetMovement();
        if (!position.Equals(newPosition) && IsValidMove(newPosition)) {
            position.Set(newPosition.x, newPosition.y);
            MovePlayerObject();
            CurrentMovementCooldown = MovementCooldown;
        }
    }

    Vector2Int GetMovement()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            return position + Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            return position + Vector2Int.right;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            return position + Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            return position + Vector2Int.left;
        }
        return position;
    }

    Vector2Int GetGridBoundaries(BattleGrid grid)
    {
        return new Vector2Int(this.grid.grid.GetLength(0) - 1, this.grid.grid.GetLength(1) - 1);
    }

    bool IsValidMove(Vector2Int newPosition)
    {
        Vector2Int boundaries = grid.getPlayerBoundaries();
        Debug.Log(boundaries.x);
        // Vector2Int boundaries = GetGridBoundaries(grid);
        // Check if its in the boundaries
        if (newPosition.x < 0 || newPosition.x > boundaries.x || newPosition.y < 0 || newPosition.y > boundaries.y ) {
            Debug.Log("Bad Move " + newPosition);
            return false;
        }
        // Check if theres no entity on the new tile
        if (grid.GetTile(newPosition.x, newPosition.y).entityOnTile != null) {
            Debug.Log("Bad Move: Entity on tile");
            return false;
        }
        return true;
    }

    public void MovePlayerObject() {
        Tile tile = grid.GetTile(position.x, position.y);
        transform.position = tile.GetTransform();
        if (currentTile != null) {
                //So we don't remove cracks.
                if(currentTile.entityOnTile == gameObject){
                    currentTile.RemoveEntityOnTile();
                }
        }
        tile.SetEntityOnTile(gameObject);
        currentTile = tile;
    }

    public Vector2Int GetPosition() { return position; }

    // Incoming damage
    public void Damage(int damageTaken)
    {
        if(shieldHP > 0) {
            shieldHP -= damageTaken;
            manager.PlayClip(shieldDeflect);
        } else {
            health -= damageTaken;

            if (health <= 0f && !isDead) {
                Kill();
            }
            else{
                // hit animation or something
                manager.PlayClip(hurtNoise);
            }   
        }
    }

    public void Kill()
    {
        if(!isDead) {
            isDead = true;
            Debug.Log("dead lol");

            if(healthText) {
                healthText.text = "DEAD";
            }
            
        }
        gameObject.SetActive(false);
    }

    public void TurnOnShield(float shieldValue) {
        shieldHP = shieldValue;
    }

    public int GetHealth() 
    {
        return health;
    }

    public void DisableOnDeath()
    {
        //does something
    }

    public Tile GetCurrentTile() {
        return currentTile;
    }

    public BattleGrid GetGrid() {
        return grid;
    }
}
