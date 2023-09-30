using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int gridX;
    public int gridY;

    public GameObject entityOnTile;

    public Sprite playerTile;
    public Sprite enemyTile;

    public bool isPlayerTile = true;
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerTile) {
            spriteRenderer.sprite = playerTile;
        } else {
            spriteRenderer.sprite = enemyTile;
        }
    }
}
