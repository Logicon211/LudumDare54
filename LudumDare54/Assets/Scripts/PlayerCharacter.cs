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

    public float MovementCooldown = .05f;
    public float CurrentMovementCooldown = 0f;

    // temp until we get something better
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Grid");
        if (g.Length > 0) {
            grid = g[0].GetComponent<BattleGrid>();
        }
        position.Set(1, 1);
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
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            // Special Attack
            Debug.Log("Special Pressesd");
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            Damage(10);
        }
    }

    /*
        Fun time movement stuff
    */
    void Move()
    {
        if (CurrentMovementCooldown > 0f) {
            CurrentMovementCooldown -= Time.deltaTime;
            return;
        }
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
            return position + Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return position + Vector2Int.right;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            return position + Vector2Int.down;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            return position + Vector2Int.left;
        }
        return position;
    }

    Vector2Int GetGridBoundaries(BattleGrid grid)
    {
        return new Vector2Int(this.grid.grid.GetLength(0), this.grid.grid.GetLength(1));
    }

    bool IsValidMove(Vector2Int newPosition)
    {
        Vector2Int boundaries = GetGridBoundaries(grid);
        if (newPosition.x < 0 || newPosition.x > boundaries.x || newPosition.y < 0 || newPosition.y > boundaries.y ) {
            Debug.Log("Bad Move " + newPosition);
            return false;
        }
        Debug.Log("Good Move " + newPosition);
        return true;
    }

    public MovePlayerObject() {
        Tile tile = grid.grid[position.x, position.y];
        Debug.log(tile.transform.position);
        transorm.position(new Vector3(, 0f, 0f));
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
}
