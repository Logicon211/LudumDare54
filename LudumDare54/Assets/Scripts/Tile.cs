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
    public GameObject flashingTile;
    public bool enableFlashing = false;
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
            // This logic should be moved to the SetEntityOnTile method.
            if(entityOnTile) {
                if(entityOnTile.GetComponent<tileCracks>() == null){
                    shadowRenderer.enabled = true;
                }
                // Some terrible hack to maybe stop the phantom shadow
                if(entityOnTile.GetComponent<PlayerCharacter>() != null) {
                    if(!entityOnTile.GetComponent<PlayerCharacter>().GetCurrentTile().Equals(this)) {
                        RemoveEntityOnTile();
                    }
                }
            }

        } else {
            shadowRenderer.enabled = false;
        }

        if(enableFlashing) {
            flashingTile.SetActive(true);
        } else {
            flashingTile.SetActive(false);
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
        // Debug.Log("Removing entity from tile " + entityOnTile);
    }

    public void SetGrid(BattleGrid gridIn) {
        grid = gridIn;
    }

    public BattleGrid GetGrid() {
        return grid;
    }

    public void Damage(int damage) {
        if (entityOnTile) {
            if(entityOnTile.GetComponent<EnemyAi>()){
                entityOnTile.GetComponent<EnemyAi>().Damage(damage);
            } else if(entityOnTile.GetComponent<RobotBomb>()){
                Debug.Log("ROBOT BOMB HIT WITH SOMETHING");
                entityOnTile.GetComponent<RobotBomb>().Damage(damage);
            }
        }
    }
}
