using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{

    // Which way to move the character
    enum Direction {Up, Right, Down, Left, None};

    public float Health { get; set; }
    public Vector2 Position { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(CheckMovement());
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

}
