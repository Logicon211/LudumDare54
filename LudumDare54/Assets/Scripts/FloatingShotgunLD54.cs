using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingShotgunLD54 : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    public Transform shootPosition;

    private float delay = 0.2f;
    private float shotgunLifeTime = 0.6f;

    private float damage = 2;
    private float projectileSpeed = 5f;
    public int numberOfProjectiles = 8;

    public float accuracy = 8f;
    private bool fired = false;

    private Animator animator;
    private AudioSource AS;
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        AS = this.gameObject.GetComponent<AudioSource>();
    }

    public void setStats(float damageIn, float projectileSpeedIn, int numberOfProjectilesIn) {
        this.damage = damageIn;
        this.projectileSpeed = projectileSpeedIn;
        this.numberOfProjectiles = numberOfProjectilesIn;
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 0f) {
            delay -= Time.deltaTime;
        } else {
            //TODO: Shoot gun
            // projectile.setDamage(laserDamage);
            if(!fired) {
                Debug.Log("Firing shotgun");
                for (int i = 0; i<numberOfProjectiles; i++) {
                    // TODO: Trigger fire animation
                    GameObject projectileLaunched = Instantiate(bullet, shootPosition.position, shootPosition.rotation) as GameObject;
                    projectileLaunched.transform.Rotate(0, 0, Random.Range(-accuracy, accuracy));
                    projectileLaunched.GetComponent<Rigidbody2D>().velocity = projectileLaunched.transform.right * (projectileSpeed + Random.Range(0f, 0.5f));
                    
                    fired = true;
                    animator.SetBool("Shoot", true);
                    AS.Play();
                    // Debug.Log("SHOTGUN SHOOTING AT: " + (projectileSpeed));
                }
            } else {
                animator.SetBool("Shoot", false);
            }
            if(shotgunLifeTime > 0f) {
                shotgunLifeTime -= Time.deltaTime;
            } else {
                Destroy(this.gameObject);
            }
        }
    }
}
