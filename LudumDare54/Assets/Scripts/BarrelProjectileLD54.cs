using UnityEngine;
using System.Collections.Generic;

public class BarrelProjectileLD54 : MonoBehaviour {

    public GameObject explosion;
    public int damage = 5;
    bool hasAppeared;
    public SpriteRenderer spriteRenderer;

    private Tile currentTile;
    private BattleGrid grid;

    private bool bombArmed;

  void Start() {
  }

  public void InitializeBarrel(Tile tile, BattleGrid grid)
  {
    Debug.Log("InitializeBarrel");
    currentTile = tile;
    this.grid = grid;
  }

  public void SetShadow() {
    currentTile.bombOnTile = gameObject;
  }

  public void ArmBomb() {
        Debug.Log("bombArmed");
    bombArmed = true;
  }

  void FixedUpdate() {

    if(bombArmed && currentTile.entityOnTile){
        CheckForHit();
        GameObject barrelExplosion = Instantiate(explosion, transform.position, new Quaternion());
        currentTile.bombOnTile = null;
        Destroy(gameObject);
    }

  }

  void CheckForHit() {

    if (currentTile.entityOnTile != null) {
      currentTile.entityOnTile.GetComponent<EnemyAi>().Damage(damage);
    }
  }
}