using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int gridX;
    public int gridY;

    // Anything that can block movement
    public GameObject entityOnTile;

    // Placeables that enemies can walk on
    public GameObject bombOnTile;

    public Sprite playerTile;
    public Sprite enemyTile;

    // Tile the player is allowed to go onto. False means it's an enemy tile.
    public bool isPlayerTile = true;
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    private BattleGrid grid;
    public SpriteRenderer shadowRenderer;
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
        if (entityOnTile || bombOnTile) {
            shadowRenderer.enabled = true;
        } else {
            shadowRenderer.enabled = false;
        }
    }

    public Vector3 GetTransform() {
        return gameObject.transform.position;
    }

    public void SetEntityOnTile(GameObject entity)
    {
        entityOnTile = entity;   
    }

    public void RemoveEntityOnTile()
    {
        entityOnTile = null;
        Debug.Log("Removing entity from tile " + entityOnTile);
    }

    public void SetGrid(BattleGrid gridIn) {
        grid = gridIn;
    }

    public BattleGrid GetGrid() {
        return grid;
    }
}
