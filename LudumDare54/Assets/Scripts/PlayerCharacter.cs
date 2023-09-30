using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Which way to move the character, not used anymore
    enum Direction {Up, Right, Down, Left, None};

    public int health;
    public Vector2Int position = new Vector2Int();

    public BattleGrid grid;
    public GameManager manager;

    public float MovementCooldown = .05f;
    public float CurrentMovementCooldown = 0f;

    // temp until we get something better
    private bool isDead = false;
    private Tile currentTile;

    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("Grid");
        GameObject m = GameObject.FindGameObjectWithTag("GameManager");
        if (g != null) {
            grid = g.GetComponent<BattleGrid>();
        }
        position.Set(1, 1);
        currentTile = grid.GetTile(1, 1);
        MovePlayerObject();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            // Regular Attack
            Debug.Log("Attack Pressed");
            RegularAttack();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            // Special Attack
            Debug.Log("Special Pressesd");
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            Damage(10);
        }
    }

    void RegularAttack()
    {
        for (int x = position.x + 1; x < grid.grid.GetLength(0) ; x++) {
            Tile attackTile = grid.GetTile(x, position.y);
            if (attackTile.entityOnTile != null )
            {
                Debug.Log("Enemy hit");
                break;
            }
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return position + Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return position + Vector2Int.right;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            return position + Vector2Int.up;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
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
        Debug.Log("Good Move " + newPosition);
        return true;
    }

    public void MovePlayerObject() {
        Debug.Log("attempting to get tile at " + position.x + "," + position.y);
        Tile tile = grid.GetTile(position.x, position.y);
        transform.position = tile.GetTransform();
        tile.SetEntityOnTile(gameObject);
        if (currentTile != null) {
            Debug.Log("wtf please jkust work");
            currentTile.RemoveEntityOnTile();
        }
        currentTile = tile;
    }

    public Vector2Int GetPosition() { return position; }

    // Incoming damage
    public void Damage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0f && !isDead) {
            Kill();
        }
        else{
            // hit animation or something
        }   
    }

    public void Kill()
    {
        if(!isDead) {
            isDead = true;
            Debug.Log("dead lol");
            
        }
    }

    public int GetHealth() 
    {
        return health;
    }
}
