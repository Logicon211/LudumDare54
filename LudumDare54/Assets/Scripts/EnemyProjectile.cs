using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    public int damage = 1;

    public float startingSpeed = 1f;
    public float acceleration = 0f;

    private Rigidbody2D rigidbody;

    public float timeToLive = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.velocity = -transform.right * startingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if( acceleration != 0f){
            rigidbody.velocity += -1* (Vector2)transform.right * acceleration * Time.deltaTime;
        }

        if(timeToLive > 0f) {
            timeToLive -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("TRIGGERING COLLISION");
        if (other.gameObject.tag == "Player") {
            Debug.Log("HITS PLAYER");

            // Hurt this dude.
            other.gameObject.GetComponent<PlayerCharacter>().Damage(damage);
            Destroy(this.gameObject);
        }
    }

}
