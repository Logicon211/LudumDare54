using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Which way to move the character, not used anymore
    enum Direction {Up, Right, Down, Left, None};

    public float Health { get; set; }
    public Vector2Int Position = new Vector2Int();

    public BattleGrid grid;

    public float MovementCooldown = .05f;
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
        Vector2Int newPosition = GetMovement();
        if (!Position.Equals(newPosition) && IsValidMove(newPosition)) {
            Position.Set(newPosition.x, newPosition.y);
            CurrentMovementCooldown = MovementCooldown;
        }
    }

    Vector2Int GetMovement()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Position + Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Position + Vector2Int.right;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            return Position + Vector2Int.down;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            return Position + Vector2Int.left;
        }
        return Position;
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

    public Vector2Int GetPosition() { return Position; }
}
