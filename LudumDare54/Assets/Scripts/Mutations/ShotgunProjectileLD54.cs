using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunProjectileLD54 : MonoBehaviour
{

    public List<GameObject> enemiesHit = new List<GameObject>();
    public int damage = 2;

    public float timeToLive = 5f;

    public int numPenetrations = 0;

    public GameObject hitEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

        void OnBecameInvisible()
        {
            Destroy(this.gameObject);
        }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            if(!enemiesHit.Contains(other.gameObject)) {
                EnemyAi enemy = other.gameObject.GetComponent<EnemyAi>();
                enemy.Damage(damage);
                Debug.Log("Shotgun Projectile hit enemy");
                Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
                if(numPenetrations <= 0) {
                Debug.Log("Shotgun Projectile hit enemy");
                    Destroy(this.gameObject);
                } else {
                    numPenetrations--;
                }
            }
        }
    }
}
