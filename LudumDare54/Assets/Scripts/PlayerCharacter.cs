using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    // Which way to move the character
    enum Direction {Up, Right, Down, Left, None};

    public float Health { get; set; }
    public Vector2Int Position = new Vector2Int();

    public BattleGrid grid;

    public float MovementCooldown = .5f;
    public float CurrentMovementCooldown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Grid");
        if (g.Length > 0) {
            grid = g[0].GetComponent<BattleGrid>();
        }
        Position.Set(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        Direction dir = CheckMovement();
        Vector2Int newPosition = NewPosition(dir);
        if (IsValidMove(newPosition)) {
            Position.Set(newPosition.x, newPosition.y);
            CurrentMovementCooldown = MovementCooldown;
        }
    }

    Direction CheckMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (horizontal != 0) {
            return horizontal < 0 ? Direction.Left : Direction.Right;
        }
        if (vertical != 0) {
            return vertical < 0 ? Direction.Up : Direction.Down;
        }
        return Direction.None;
    }

    Vector2Int NewPosition(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Position + Vector2Int.up;
            case Direction.Right: return Position + Vector2Int.right;
            case Direction.Down: return Position + Vector2Int.down;
            case Direction.Left:  return Position + Vector2Int.left;
            default: return Position; 
        }
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
}
