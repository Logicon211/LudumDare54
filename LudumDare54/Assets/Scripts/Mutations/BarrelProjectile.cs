using UnityEngine;
using System.Collections.Generic;

public class BarrelProjectile : MonoBehaviour {

    public GameObject explosion;
    public GameObject teleport;
    public float timeToLive;
    public float timeTillExplosion;
    public float markerTimer;
    public int damage = 5;
    public bool timedExplosion = false;
    Animator animator;
    bool hasAppeared;
    SpriteRenderer sprite;

    private Tile currentTile;
    private BattleGrid grid;

  void Start() {
    sprite = gameObject.GetComponent<SpriteRenderer>();
    animator = gameObject.GetComponentInChildren<Animator>();
    timeTillExplosion = timeToLive + markerTimer;
  }

  public void InitializeBarrel(Tile tile, BattleGrid grid)
  {
    currentTile = tile;
    this.grid = grid;
  }

  public void SetShadow() {
    currentTile.bombOnTile = gameObject;
  }

  void FixedUpdate() {
    timeTillExplosion -= Time.deltaTime;
    if (!hasAppeared && timeTillExplosion <= timeToLive - markerTimer) {
        animator.SetBool("ShowBarrel", true);
        hasAppeared = true;
        Instantiate(teleport, transform.position, new Quaternion());
        // currentTile.bombOnTile = gameObject;
    }
    if (( timedExplosion && timeTillExplosion <= 0f) || (hasAppeared && currentTile.entityOnTile)) {
        CheckForHit();
        GameObject barrelExplosion = Instantiate(explosion, transform.position, new Quaternion());
        currentTile.bombOnTile = null;
        Destroy(gameObject);
    }
  }

  void CheckForHit() {
    if (currentTile.entityOnTile) {
      currentTile.Damage(damage);
    }
  }
}