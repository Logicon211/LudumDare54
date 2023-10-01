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
        if(HPText) {
            HPText.text = health.ToString();
        }
        if(timertext) {
            timertext.text = ((int)timer).ToString();
        }

        if(timer <= 0f) {
            // TODO: write explode code
            Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);

            BattleGrid grid = onTile.GetGrid();
            // Check adjacent tiles to explode and damage:
            int x = onTile.gridX;
            int y = onTile.gridY;

            Explode(x, y, grid);
            if(x - 1 >=0) {
                Explode(x-1, y, grid);
            }
            if(x + 1<= 6) {
                Explode(x+1, y, grid);
            }
            if(y - 1 >= 0) {
                Explode(x, y-1, grid);
            }
            if(y + 1 <= 3) {
                Explode(x, y+1, grid);
            }
            if(x - 1 >=0 && y-1 >=0) {
                Explode(x-1, y-1, grid);
            }
            if(x - 1 >=0 && y+1 >=3) {
                Explode(x-1, y+1, grid);
            }
            if(x + 1 >=6 && y+1 >=3) {
                Explode(x+1, y+1, grid);
            }
            if(x + 1 >=6 && y-1 >=0) {
                Explode(x+1, y-1, grid);
            }
            Destroy(gameObject);
        }

    }

    public void Explode(int x, int y, BattleGrid grid) {
        if(grid.getPlayerTile().Equals(grid.grid[x, y])) {
            gameManager.GetPlayer().Damage(damage);
        }
        Instantiate(explosion, new Vector3(grid.grid[x, y].transform.position.x, grid.grid[x, y].transform.position.y, grid.grid[x, y].transform.position.z), Quaternion.identity);
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
            isDead = true;
            Destroy(gameObject);
        }
    }

    public void SetTile(Tile tile) {
        onTile = tile;
        onTile.SetEntityOnTile(gameObject);
    }


}
