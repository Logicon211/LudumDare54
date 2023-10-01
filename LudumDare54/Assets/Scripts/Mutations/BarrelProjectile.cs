using UnityEngine;
using System.Collections.Generic;

public class BarrelProjectile : MonoBehaviour {

    public GameObject explosion;
    public GameObject teleport;
    public float timeToLive;
    public float timeTillExplosion;
    public float markerTimer;
    Player player;
    Barrel barrel;
    Animator animator;
    bool hasAppeared;
    SpriteRenderer sprite;

    private Tile currentTile;
    private BattleGrid grid;

  void StartBarrel() {
    barrel = GameObject.FindObjectOfType<Barrel>();
    sprite = gameObject.GetComponent<SpriteRenderer>();
    animator = gameObject.GetComponentInChildren<Animator>();
    timeTillExplosion = timeToLive + markerTimer;
  }

  public void InitializeBarrel(Tile tile, BattleGrid grid)
  {
    currentTile = tile;
    this.grid = grid;
  }

  void FixedUpdate() {
    timeTillExplosion -= Time.deltaTime;
    if (!hasAppeared && timeTillExplosion <= timeToLive - markerTimer) {
        animator.SetBool("ShowBarrel", true);
        hasAppeared = true;
        Instantiate(teleport, transform.position, new Quaternion(), barrel.gameArea.transform);
    }
    if (timeTillExplosion <= 0f) {
        CheckForHit();
        GameObject barrelExplosion = Instantiate(explosion, transform.position, new Quaternion(), barrel.gameArea.transform);
        if (barrel.GetLevel() > 2) {
            barrelExplosion.gameObject.transform.localScale *= 1.7f; 
        }
        Destroy(gameObject);
    }
  }

  void CheckForHit() {

  }

}