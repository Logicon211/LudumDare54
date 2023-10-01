using UnityEngine;
using System.Collections.Generic;

public class PunchProjectile : MonoBehaviour {
  public float timeToLive = .5f;
  float currentTimeAlive;
  Stats stats;
  Punch punch;

  BoxCollider2D projectileBox;

  Player player;

  private List<GameObject> enemiesHit = new List<GameObject>();
  void Start() {
    currentTimeAlive = 0f;
    punch = GameObject.FindObjectOfType<Punch>();
    projectileBox = gameObject.GetComponent<BoxCollider2D>();
    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
  }

  void FixedUpdate() {
    currentTimeAlive += Time.deltaTime;
    if (currentTimeAlive >= timeToLive) {
      Destroy(gameObject);
    }
  }

}