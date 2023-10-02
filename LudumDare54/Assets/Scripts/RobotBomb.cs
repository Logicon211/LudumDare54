using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Interface;

public class RobotBomb : MonoBehaviour, IDamageable<int>
{
    public float timer = 3f;
    public int damage = 5;

    public int health = 10;

    public TMPro.TMP_Text HPText;
    public TMPro.TMP_Text timertext;
    
    public GameObject explosion;
    // Start is called before the first frame update

    private bool isDead = false;
    private Tile onTile;

    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        if(HPText) {
            HPText.text = health.ToString();
        }
        if(timertext) {
            timertext.text = ((int)timer).ToString();
        }

        BattleGrid grid = onTile.GetGrid();
        // Check adjacent tiles to explode and damage:
        int x = onTile.gridX;
        int y = onTile.gridY;
        if(timer <= 0f) {
            // TODO: write explode code
            // Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);


            Explode(x, y, grid);
            if(x - 1 >=0) {
                Explode(x-1, y, grid);
            }
            if(x + 1 < 6) {
                Explode(x+1, y, grid);
            }
            if(y - 1 >= 0) {
                Explode(x, y-1, grid);
            }
            if(y + 1 < 3) {
                Explode(x, y+1, grid);
            }
            if(x - 1 >=0 && y-1 >=0) {
                Explode(x-1, y-1, grid);
            }
            if(x - 1 >=0 && y+1 <3) {
                Explode(x-1, y+1, grid);
            }
            if(x + 1 <6 && y+1 <3) {
                Explode(x+1, y+1, grid);
            }
            if(x + 1 <6 && y-1 >=0) {
                Explode(x+1, y-1, grid);
            }
            Destroy(gameObject);
        } else {
            // Set Flashing:
            if(x - 1 >=0) {
                SetFlashing(grid.grid[x-1, y], true);
            }
            if(x + 1 < 6) {
                SetFlashing(grid.grid[x+1, y], true);
            }
            if(y - 1 >= 0) {
                SetFlashing(grid.grid[x, y-1], true);
            }
            if(y + 1 < 3) {
                SetFlashing(grid.grid[x, y+1], true);
            }
            if(x - 1 >=0 && y-1 >=0) {
                SetFlashing(grid.grid[x-1, y-1], true);
            }
            if(x - 1 >=0 && y+1 <3) {
                SetFlashing(grid.grid[x-1, y+1], true);
            }
            if(x + 1 <6 && y+1 <3) {
                SetFlashing(grid.grid[x+1, y+1], true);
            }
            if(x + 1 <6 && y-1 >=0) {
                SetFlashing(grid.grid[x+1, y-1], true);
            }
        }

    }

    public void Explode(int x, int y, BattleGrid grid) {
        SetFlashing(grid.grid[x, y], false);
        if(grid.getPlayerTile().Equals(grid.grid[x, y])) {
            gameManager.GetPlayer().Damage(damage);
        }
        Instantiate(explosion, new Vector3(grid.grid[x, y].transform.position.x, grid.grid[x, y].transform.position.y, grid.grid[x, y].transform.position.z), Quaternion.identity);
    }

    public void SetFlashing(Tile tile, bool flashing) {
        tile.enableFlashing = flashing;
    }

    public void Damage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0 && !isDead) {
            Kill();
        }
        else{
            //Instantiate(hitEffect, transform.position, Quaternion.identity);
        }   
    }

    public void Kill()
    {
        if(!isDead) {
            BattleGrid grid = onTile.GetGrid();
            int x = onTile.gridX;
            int y = onTile.gridY;
            // Set Flashing:
            if(x - 1 >=0) {
                SetFlashing(grid.grid[x-1, y], false);
            }
            if(x + 1 < 6) {
                SetFlashing(grid.grid[x+1, y], false);
            }
            if(y - 1 >= 0) {
                SetFlashing(grid.grid[x, y-1], false);
            }
            if(y + 1 < 3) {
                SetFlashing(grid.grid[x, y+1], false);
            }
            if(x - 1 >=0 && y-1 >=0) {
                SetFlashing(grid.grid[x-1, y-1], false);
            }
            if(x - 1 >=0 && y+1 <3) {
                SetFlashing(grid.grid[x-1, y+1], false);
            }
            if(x + 1 <6 && y+1 <3) {
                SetFlashing(grid.grid[x+1, y+1], false);
            }
            if(x + 1 <6 && y-1 >=0) {
                SetFlashing(grid.grid[x+1, y-1], false);
            }

            isDead = true;
            Destroy(gameObject);
        }
    }

    public void SetTile(Tile tile) {
        onTile = tile;
        onTile.SetEntityOnTile(gameObject);
    }


}
