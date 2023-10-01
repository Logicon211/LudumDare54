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
    void Start()
    {
        
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

            Destroy(gameObject);
        }

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
